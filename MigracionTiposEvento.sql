-- ════════════════════════════════════════════════════════════════════════
-- Migración: catálogo completo de TipoEvento (incluye Integridad)
-- ════════════════════════════════════════════════════════════════════════
-- DALBitacora.Guardar valida que el tipo de evento exista en TipoEvento
-- antes de insertar el registro en EVENTOS. Cada nombre que el código C#
-- use al llamar Auditar(...) tiene que estar en esta tabla.
--
-- Es IDEMPOTENTE: solo inserta los que faltan.
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
    ('Rol modificado'),
    -- ── Eventos del módulo de Integridad (DVH/DVV) ──
    ('Integridad comprometida'),
    ('Integridad recalculada')
) AS v(Nombre)
WHERE NOT EXISTS (SELECT 1 FROM TipoEvento t WHERE t.Nombre = v.Nombre);
GO

-- Verificación opcional:
-- SELECT * FROM TipoEvento ORDER BY Nombre;
