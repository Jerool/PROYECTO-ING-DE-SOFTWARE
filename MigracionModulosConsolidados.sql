-- ════════════════════════════════════════════════════════════════════════
-- Migración: consolidar módulos del catálogo a solo "Usuario" y "Admin"
-- ════════════════════════════════════════════════════════════════════════
--
-- Antes: existían varios módulos ("Login", "Contraseña", "Gestión Usuario",
-- "Usuario", etc.) cada uno como una fila distinta en la tabla Modulo.
-- Después: solo quedan "Usuario" (acciones del propio usuario logueado:
-- login, logout, cambio de contraseña) y "Admin" (acciones administrativas
-- sobre otros usuarios y sobre el sistema de permisos).
--
-- Esta migración:
--   1. Garantiza que existan los dos módulos definitivos.
--   2. Reasigna IdModulo de los registros existentes en EVENTOS según el
--      nombre del módulo viejo.
--   3. Elimina los módulos huérfanos (sin ningún evento que los referencie).
--
-- IMPORTANTE: ejecutar UNA sola vez. Es idempotente — corre varias veces
-- sin romper nada, pero después de la primera no hay nada que migrar.
-- ════════════════════════════════════════════════════════════════════════

-- ─── 1) Crear los módulos definitivos si no existen ────────────────────
IF NOT EXISTS (SELECT 1 FROM Modulo WHERE Nombre = 'Usuario')
    INSERT INTO Modulo (Nombre) VALUES ('Usuario');

IF NOT EXISTS (SELECT 1 FROM Modulo WHERE Nombre = 'Admin')
    INSERT INTO Modulo (Nombre) VALUES ('Admin');
GO

-- ─── 2) Reasignar IdModulo de los EVENTOS existentes ───────────────────
-- Todos los eventos asociados a módulos "del usuario" pasan a "Usuario".
DECLARE @IdUsuario INT = (SELECT Id FROM Modulo WHERE Nombre = 'Usuario');
DECLARE @IdAdmin   INT = (SELECT Id FROM Modulo WHERE Nombre = 'Admin');

UPDATE E
SET    E.IdModulo = @IdUsuario
FROM   EVENTOS E
INNER  JOIN Modulo M ON M.Id = E.IdModulo
WHERE  M.Nombre IN ('Login', 'Contraseña', 'Usuario')
  AND  E.IdModulo <> @IdUsuario;

-- Todos los eventos administrativos pasan a "Admin".
UPDATE E
SET    E.IdModulo = @IdAdmin
FROM   EVENTOS E
INNER  JOIN Modulo M ON M.Id = E.IdModulo
WHERE  M.Nombre IN ('Gestión Usuario', 'Gestion Usuario',
                    'Permisos', 'Bitacora', 'Bitácora')
  AND  E.IdModulo <> @IdAdmin;
GO

-- ─── 3) Eliminar módulos huérfanos del catálogo ────────────────────────
-- Borramos cualquier fila de Modulo que (a) no sea Usuario ni Admin y
-- (b) no tenga ningún evento que la referencie.
DELETE M
FROM   Modulo M
WHERE  M.Nombre NOT IN ('Usuario', 'Admin')
  AND  NOT EXISTS (SELECT 1 FROM EVENTOS E WHERE E.IdModulo = M.Id);
GO

-- ─── Verificación (opcional, comentado) ────────────────────────────────
-- SELECT * FROM Modulo;
-- SELECT M.Nombre AS Modulo, COUNT(*) AS Cantidad
-- FROM EVENTOS E
-- INNER JOIN Modulo M ON M.Id = E.IdModulo
-- GROUP BY M.Nombre;
