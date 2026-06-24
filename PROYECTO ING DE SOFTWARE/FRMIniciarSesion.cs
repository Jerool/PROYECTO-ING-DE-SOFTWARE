using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BLL.BLLUsuario_GV42;

namespace PROYECTO_ING_DE_SOFTWARE
{

    public partial class FRMIniciarSesion : Form, IObservadorIdioma_GV42
    {
        private readonly BLLUsuario_GV42 _bllUsuario;

        public FRMIniciarSesion()
        {
            InitializeComponent();
            _bllUsuario = new BLLUsuario_GV42();

            IdiomaManager_GV42.Instancia.Suscribir(this);
            this.FormClosed += (s, e) => IdiomaManager_GV42.Instancia.Desuscribir(this);

            ActualizarIdioma();
        }

        public void ActualizarIdioma()
        {
            this.Text = IdiomaManager_GV42.T("login.titulo");
            if (lblTitulo != null) lblTitulo.Text = IdiomaManager_GV42.T("login.titulo");
            if (lblSubtitulo != null) lblSubtitulo.Text = IdiomaManager_GV42.T("login.subtitulo");
            if (label1 != null) label1.Text = IdiomaManager_GV42.T("login.login");
            if (label2 != null) label2.Text = IdiomaManager_GV42.T("login.contrasena");
            if (btnIngresar != null) btnIngresar.Text = IdiomaManager_GV42.T("login.btnIngresar");
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string login = txtLogIn.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(contrasena))
            {
                MessageBox.Show(IdiomaManager_GV42.T("general.completarCampos"),
                                IdiomaManager_GV42.T("general.advertencia"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Validaciones_GV42.EsLoginValido(login))
            {
                MessageBox.Show(Validaciones_GV42.MENSAJE_LOGIN,
                                IdiomaManager_GV42.T("general.advertencia"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogIn.Focus();
                return;
            }

            ResultadoLogin resultado = _bllUsuario.IntentarLogin(login, contrasena);

            switch (resultado)
            {
                case ResultadoLogin.Exitoso:
                    AbrirFormularioSegunRol();
                    break;
                case ResultadoLogin.UsuarioBloqueado:
                case ResultadoLogin.BloqueadoPorIntentos:
                    MessageBox.Show(IdiomaManager_GV42.T("login.bloqueado"),
                                    IdiomaManager_GV42.T("general.accesoDenegado"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case ResultadoLogin.UsuarioInactivo:
                    MessageBox.Show(IdiomaManager_GV42.T("login.inactivo"),
                                    IdiomaManager_GV42.T("general.accesoDenegado"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case ResultadoLogin.ContrasenaIncorrecta:
                    MessageBox.Show(IdiomaManager_GV42.T("login.contrasenaIncorrecta"),
                                    IdiomaManager_GV42.T("general.error"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case ResultadoLogin.UsuarioInexistente:
                    MessageBox.Show(IdiomaManager_GV42.T("login.usuarioInexistente"),
                                    IdiomaManager_GV42.T("general.error"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case ResultadoLogin.SesionActiva:
                    MessageBox.Show(IdiomaManager_GV42.T("login.sesionActiva"),
                                    IdiomaManager_GV42.T("general.advertencia"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case ResultadoLogin.Error:
                    MessageBox.Show(IdiomaManager_GV42.T("login.errorUsuario"),
                                    IdiomaManager_GV42.T("general.error"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void AbrirFormularioSegunRol()
        {
            Usuario_GV42 actual = SessionManager_GV42.Instancia.ObtenerUsuarioActual();

            if (!VerificarIntegridad(actual))
            {
                BLLUsuario_GV42.CerrarSesión();
                return;
            }

            if (actual != null && actual.DebeCambiarContrasena)
            {
                MessageBox.Show(IdiomaManager_GV42.T("login.cambioRequeridoMensaje"),
                                IdiomaManager_GV42.T("login.cambioRequeridoTitulo"),
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                FRMCambiarContrasenia cambio = new FRMCambiarContrasenia(primerLogin: true);
                cambio.Show();
                this.Hide();
                return;
            }

            Form formulario = new FRMMenuPrincipalAdmin();
            formulario.Show();
            this.Hide();
        }

        private bool VerificarIntegridad(Usuario_GV42 actual)
        {
            try
            {
                var bllInt = new BLLIntegridad_GV42();
                ResultadoIntegridad res = bllInt.Verificar();

                if (res.EsIntegra)
                {
                    try { bllInt.IniciarBackupsProgramados(); } catch { }
                    return true;
                }

                BLLBitacora_GV42.Instancia.RegistrarEvento(
                    actual.Login, "Admin", "Integridad comprometida",
                    string.Join(", ", res.TablasComprometidas), "Alta");

                var bllPermisos = new BLLPermisos_GV42();
                Rol_GV42 rolCompleto = bllPermisos.ObtenerArbolRol(actual.Rol.Id);
                var dataKeys = rolCompleto != null
                    ? rolCompleto.ObtenerPatentes().Select(p => p.DataKey ?? string.Empty).ToList()
                    : new List<string>();

                bool puedeRecalcular = dataKeys.Contains("Integridad.Recalcular");
                bool puedeRestaurar  = dataKeys.Contains("Integridad.Restore");

                if (!puedeRecalcular && !puedeRestaurar)
                {
                    MessageBox.Show(
                        IdiomaManager_GV42.T("integridad.sistemaInactivoMensaje"),
                        IdiomaManager_GV42.T("integridad.sistemaInactivoTitulo"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                using (var frm = new FRMIntegridad(res, puedeRecalcular, puedeRestaurar))
                {
                    DialogResult dr = frm.ShowDialog(this);

                    if (frm.SeRestauroBackup)
                    {
                        MessageBox.Show(
                            IdiomaManager_GV42.T("integridad.cerrandoAppBackup"),
                            IdiomaManager_GV42.T("integridad.titulo"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Exit();
                        return false;
                    }

                    if (frm.SeRecalcularon)
                    {
                        BLLBitacora_GV42.Instancia.RegistrarEvento(
                            actual.Login, "Admin", "Integridad recalculada",
                            "Admin aceptó los cambios externos como válidos.", "Alta");
                        try { bllInt.IniciarBackupsProgramados(); } catch { }
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    IdiomaManager_GV42.T("integridad.errorVerificacion") + "\n\n" + ex.Message,
                    IdiomaManager_GV42.T("general.error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

    }
}
