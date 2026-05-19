-- ──────────────────────────────────────────────────────────────────
-- Migración: convertir EVENTOS.Modulo (varchar) y EVENTOS.Evento (varchar)
-- en EVENTOS.IdModulo (FK a Modulo) y EVENTOS.IdTipoEvento (FK a TipoEvento)
-- + columna Detalle para la parte dinámica (ej. "Intento 2/3").
--
-- Pasos:
--   1) Crea las tablas Modulo y TipoEvento si no existen y completa los catálogos.
--   2) Agrega columnas IdModulo, IdTipoEvento, Detalle (todas NULL al inicio).
--   3) Mapea Modulo (texto) -> IdModulo.
--   4) Parsea Evento (texto) -> IdTipoEvento + Detalle (cada caso a mano).
--   5) Borra las columnas viejas.
--   6) Pone NOT NULL y crea las FKs.
--
-- Ejecutar UNA SOLA VEZ. Si tu base ya tiene IdModulo / IdTipoEvento, saltear.
-- ──────────────────────────────────────────────────────────────────

-- ─── Paso 1: tablas catálogo + datos base ─────────────────────────

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Modulo')
BEGIN
    CREATE TABLE Modulo (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(50) NOT NULL UNIQUE
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TipoEvento')
BEGIN
    CREATE TABLE TipoEvento (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL UNIQUE
    );
END
GO

-- Insertamos los módulos que usa el código (si no están).
INSERT INTO Modulo (Nombre)
SELECT v.Nombre FROM (VALUES
    ('Login'),
    ('Gestión Usuario'),
    ('Contraseña'),
    ('Usuario')
) AS v(Nombre)
WHERE NOT EXISTS (SELECT 1 FROM Modulo m WHERE m.Nombre = v.Nombre);
GO

-- Insertamos los tipos de evento que usa el código (si no están).
INSERT INTO TipoEvento (Nombre)
SELECT v.Nombre FROM (VALUES
    ('Intento de login con sesión ya activa'),
    ('Usuario inexistente'),
    ('Usuario bloqueado'),
    ('Usuario inactivo'),
    ('Contraseña incorrecta'),
    ('Usuario bloqueado por intentos fallidos'),
    ('Login exitoso'),
    ('Usuario desbloqueado'),
    ('Usuario activado'),
    ('Usuario desactivado'),
    ('Email modificado'),
    ('Rol modificado'),
    ('Usuario creado'),
    ('Contraseña cambiada exitosamente'),
    ('Logout realizado')
) AS v(Nombre)
WHERE NOT EXISTS (SELECT 1 FROM TipoEvento t WHERE t.Nombre = v.Nombre);
GO

-- ─── Paso 2: agregar columnas nuevas ──────────────────────────────

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IdModulo' AND Object_ID = Object_ID(N'EVENTOS'))
    ALTER TABLE EVENTOS ADD IdModulo INT NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IdTipoEvento' AND Object_ID = Object_ID(N'EVENTOS'))
    ALTER TABLE EVENTOS ADD IdTipoEvento INT NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'Detalle' AND Object_ID = Object_ID(N'EVENTOS'))
    ALTER TABLE EVENTOS ADD Detalle NVARCHAR(500) NULL;
GO

-- ─── Paso 3: mapear Modulo (texto) -> IdModulo ────────────────────

UPDATE EVENTOS
SET IdModulo = (SELECT M.Id FROM Modulo M WHERE M.Nombre = EVENTOS.Modulo);
GO

-- Verificación opcional: que no queden filas sin mapear.
-- SELECT * FROM EVENTOS WHERE IdModulo IS NULL;

-- ─── Paso 4: parsear Evento (texto) -> IdTipoEvento + Detalle ─────
-- Cada caso del histórico se mapea a mano. Si tu base tiene formatos
-- distintos, ajustá las condiciones.

-- 4.a) Eventos sin detalle (texto exacto coincide con TipoEvento.Nombre)
UPDATE EVENTOS
SET IdTipoEvento = T.Id, Detalle = NULL
FROM EVENTOS E
INNER JOIN TipoEvento T ON T.Nombre = E.Evento
WHERE E.IdTipoEvento IS NULL;
GO

-- 4.b) "Contraseña incorrecta. Intento X/Y"
UPDATE EVENTOS
SET IdTipoEvento = (SELECT Id FROM TipoEvento WHERE Nombre = 'Contraseña incorrecta'),
    Detalle = LTRIM(SUBSTRING(Evento, LEN('Contraseña incorrecta.') + 1, 500))
WHERE Evento LIKE 'Contraseña incorrecta%' AND IdTipoEvento IS NULL;
GO

-- 4.c) "Usuario bloqueado por 3 intentos fallidos"
UPDATE EVENTOS
SET IdTipoEvento = (SELECT Id FROM TipoEvento WHERE Nombre = 'Usuario bloqueado por intentos fallidos'),
    Detalle = Evento
WHERE Evento LIKE 'Usuario bloqueado por%intentos%' AND IdTipoEvento IS NULL;
GO

-- 4.d) "Rol modificado a X"
UPDATE EVENTOS
SET IdTipoEvento = (SELECT Id FROM TipoEvento WHERE Nombre = 'Rol modificado'),
    Detalle = Evento
WHERE Evento LIKE 'Rol modificado a%' AND IdTipoEvento IS NULL;
GO

-- 4.e) "Usuario creado: X"
UPDATE EVENTOS
SET IdTipoEvento = (SELECT Id FROM TipoEvento WHERE Nombre = 'Usuario creado'),
    Detalle = Evento
WHERE Evento LIKE 'Usuario creado%' AND IdTipoEvento IS NULL;
GO

-- 4.f) "Usuario X desbloqueado y contraseña reseteada"
UPDATE EVENTOS
SET IdTipoEvento = (SELECT Id FROM TipoEvento WHERE Nombre = 'Usuario desbloqueado'),
    Detalle = Evento
WHERE Evento LIKE 'Usuario % desbloqueado%' AND IdTipoEvento IS NULL;
GO

-- 4.g) Fallback: lo que quedó sin clasificar va a "Usuario creado" como Detalle libre,
-- o bien dejamos NULL y revisamos a mano. Acá lo dejamos NULL para no inventar datos.
-- SELECT * FROM EVENTOS WHERE IdTipoEvento IS NULL;  -- revisar manualmente si hay filas.

-- ─── Paso 5: borrar columnas viejas ───────────────────────────────

IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'Modulo' AND Object_ID = Object_ID(N'EVENTOS'))
    ALTER TABLE EVENTOS DROP COLUMN Modulo;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'Evento' AND Object_ID = Object_ID(N'EVENTOS'))
    ALTER TABLE EVENTOS DROP COLUMN Evento;
GO

-- ─── Paso 6: NOT NULL + foreign keys ──────────────────────────────

ALTER TABLE EVENTOS ALTER COLUMN IdModulo INT NOT NULL;
GO

ALTER TABLE EVENTOS ALTER COLUMN IdTipoEvento INT NOT NULL;
GO

ALTER TABLE EVENTOS
ADD CONSTRAINT FK_Eventos_Modulo
    FOREIGN KEY (IdModulo) REFERENCES Modulo(Id);
GO

ALTER TABLE EVENTOS
ADD CONSTRAINT FK_Eventos_TipoEvento
    FOREIGN KEY (IdTipoEvento) REFERENCES TipoEvento(Id);
GO
