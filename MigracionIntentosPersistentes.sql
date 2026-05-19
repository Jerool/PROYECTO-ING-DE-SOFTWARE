-- ──────────────────────────────────────────────────────────────────
-- Migración: persistir intentos de login fallidos en Usuario.
--
-- ¿Por qué? Hoy el contador vive en memoria (singleton). Al cerrar la
-- app los intentos se pierden y un atacante puede hacer 2 intentos,
-- cerrar, abrir, hacer 2 más, y nunca llegar al bloqueo.
--
-- Con estas columnas el contador queda en la base. Además guardamos
-- cuándo fue el último intento fallido para poder "expirar" el contador
-- después de una ventana de tiempo (1 hora) y resetearlo solo.
--
-- Ejecutar UNA SOLA VEZ.
-- ──────────────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT 1 FROM sys.columns
               WHERE Name = N'IntentosFallidos' AND Object_ID = Object_ID(N'Usuario'))
BEGIN
    ALTER TABLE Usuario ADD IntentosFallidos INT NOT NULL DEFAULT 0;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns
               WHERE Name = N'UltimoIntentoFallido' AND Object_ID = Object_ID(N'Usuario'))
BEGIN
    ALTER TABLE Usuario ADD UltimoIntentoFallido DATETIME NULL;
END
GO

-- Verificación rápida (opcional):
-- SELECT DNI, UserName, Bloqueo, IntentosFallidos, UltimoIntentoFallido FROM Usuario;
