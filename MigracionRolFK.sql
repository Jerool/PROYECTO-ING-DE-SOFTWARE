-- ──────────────────────────────────────────────────────────────────
-- Migración: convertir Usuario.Rol (varchar) en Usuario.IdRol (FK a Roles)
--
-- Pasos:
--   1) Agrega columna IdRol (temporalmente NULL).
--   2) Mapea los nombres actuales ("Admin"/"Usuario") a sus Id usando la tabla Roles.
--   3) Borra la columna Rol vieja.
--   4) Marca IdRol como NOT NULL y crea la FK.
--
-- Ejecutar UNA SOLA VEZ. Si tu base ya tiene IdRol, saltear este script.
-- ──────────────────────────────────────────────────────────────────

-- Paso 1: agregar la columna nueva
ALTER TABLE Usuario ADD IdRol INT NULL;
GO

-- Paso 2: mapear los nombres existentes a sus Ids
UPDATE Usuario
SET IdRol = (SELECT R.Id FROM Roles R WHERE R.Nombre = Usuario.Rol);
GO

-- Verificación: que no haya quedado nadie con IdRol nulo (rol no mapeado)
-- Si esto devuelve filas, revisar manualmente esos usuarios antes de seguir.
-- SELECT * FROM Usuario WHERE IdRol IS NULL;

-- Paso 3: borrar la columna vieja
ALTER TABLE Usuario DROP COLUMN Rol;
GO

-- Paso 4: hacer NOT NULL y agregar la foreign key
ALTER TABLE Usuario ALTER COLUMN IdRol INT NOT NULL;
GO

ALTER TABLE Usuario
ADD CONSTRAINT FK_Usuario_Roles
    FOREIGN KEY (IdRol) REFERENCES Roles(Id);
GO
