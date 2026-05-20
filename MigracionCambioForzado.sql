-- ──────────────────────────────────────────────────────────────────
-- Migración: forzar cambio de contraseña en el primer login.
--
-- Cuando el admin crea un usuario nuevo, le asigna una contraseña por defecto
-- (nombre + últimos 3 del DNI). Esa contraseña la conocen tanto el admin como
-- el usuario nuevo, así que NO es segura: el admin podría loguearse con la
-- cuenta del usuario.
--
-- Solución: marcamos al usuario con DebeCambiarContrasena = 1 al crearlo. En
-- el primer login el sistema lo manda directo al formulario de cambio de
-- contraseña, y solo después puede operar normal. Una vez cambiada, el flag
-- queda en 0.
--
-- Lo mismo aplica cuando el admin DESBLOQUEA a un usuario (reseteamos la
-- contraseña a la por defecto), así que también ponemos el flag a 1 ahí.
--
-- Ejecutar UNA SOLA VEZ en SSMS.
-- ──────────────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT 1 FROM sys.columns
               WHERE Name = N'DebeCambiarContrasena' AND Object_ID = Object_ID(N'Usuario'))
BEGIN
    ALTER TABLE Usuario ADD DebeCambiarContrasena BIT NOT NULL DEFAULT 0;
END
GO

-- Si querés que los usuarios existentes también tengan que cambiar la
-- contraseña la próxima vez que entren, descomentá esto:
-- UPDATE Usuario SET DebeCambiarContrasena = 1 WHERE UserName <> 'admin';
