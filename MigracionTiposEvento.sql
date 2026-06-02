-- ════════════════════════════════════════════════════════════════════════
-- Migración: catálogo completo de TipoEvento
-- ════════════════════════════════════════════════════════════════════════
--
-- DALBitacora.Guardar valida que el tipo de evento exista en la tabla
-- TipoEvento antes de insertar el registro en EVENTOS (ResolverIdTipoEvento
-- lanza excepción si no encuentra). Por eso, cada vez que el código usa un
-- string nuevo en Auditar(...), tenemos que asegurarnos de que esté en
-- TipoEvento. Este script garantiza eso.
--
-- Es IDEMPOTENTE: corre cuantas veces quieras; solo inserta los faltantes.
-- ════════════════════════════════════════════════════════════════════════

INSERT INTO TipoEvento (Nombre)
SELECT v.Nombre FROM (VALUES
    -- ── Acciones del usuario logueado (módulo "Usuario") ──
    ('Login exitoso'),
    ('Contraseña incorrecta'),
    ('Usuario inexistente'),
    ('Usuario bloqueado'),
    ('Usuario inactivo'),
    ('Usuario bloqueado por intentos fallidos'),
    ('Intento de login con sesión ya activa'),
    ('Contraseña cambiada exitosamente'),
    ('Logout realizado'),
    ('Idioma cambiado'),
    -- ── Acciones administrativas (módulo "Admin") ──
    ('Usuario creado'),
    ('Usuario activado'),
    ('Usuario desactivado'),
    ('Usuario desbloqueado'),
    ('Email modificado'),
    ('Rol modificado')
) AS v(Nombre)
WHERE NOT EXISTS (SELECT 1 FROM TipoEvento t WHERE t.Nombre = v.Nombre);
GO

-- ─── Verificación (opcional) ───────────────────────────────────────────
-- SELECT * FROM TipoEvento ORDER BY Nombre;
