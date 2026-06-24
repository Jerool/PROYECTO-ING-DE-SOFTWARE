-- ============================================================
-- Migración: Patente para cambiar idioma
-- ----------------------------------------------------------------
-- Agrega la patente que controla la visibilidad del menú "Idioma"
-- en FRMMenuPrincipalAdmin.
--
--   - Sesion.CambiarIdioma → puede ver y usar el menú Idioma
--
-- Migración: se la asigna a TODOS los roles existentes para que
-- ningún usuario pierda la capacidad de cambiar idioma al migrar.
-- Después podés sacarla manualmente desde Gestión de Permisos.
--
-- Idempotente: se puede ejecutar varias veces sin error.
-- ============================================================

USE [Gestion Usuario]
GO

-- Paso 1: Crear la patente
IF NOT EXISTS (SELECT 1 FROM Patente WHERE DataKey = 'Sesion.CambiarIdioma')
    INSERT INTO Patente (Nombre, DataKey) VALUES ('Cambiar Idioma', 'Sesion.CambiarIdioma');

GO

-- Paso 2: Asignar a todos los roles existentes
INSERT INTO RolPatente (IdRol, IdPatente)
SELECT r.Id, p.Id
FROM Roles r
CROSS JOIN Patente p
WHERE p.DataKey = 'Sesion.CambiarIdioma'
  AND NOT EXISTS (
        SELECT 1
        FROM RolPatente rp
        WHERE rp.IdRol = r.Id AND rp.IdPatente = p.Id
  );

GO

-- ============================================================
-- IMPORTANTE: después de ejecutar este script, andá a
-- FRMIntegridad y dale "Recalcular" para que los DVH/DVV
-- queden coherentes con las nuevas filas.
-- ============================================================
