-- ──────────────────────────────────────────────────────────────────
-- Migración: patrón Composite para gestión de permisos.
--
-- Modelo:
--   Patente  → Leaf  (permiso individual, ej. "GestionUsuarios.Crear")
--   Familia  → Composite (agrupa Patentes y/o otras Familias)
--   Rol      → Composite (agrupa Patentes y/o Familias) — la tabla Roles ya existía
--
-- Tablas de relación M:N:
--   FamiliaPatente   → Familia ←→ Patente
--   FamiliaIntegrada → Familia ←→ Familia (subfamilias / anidamiento)
--   RolPatente       → Rol ←→ Patente
--   RolFamilia       → Rol ←→ Familia
--
-- Reglas que cubre el modelo:
--   - Una familia puede tener N patentes y N subfamilias (anidamiento).
--   - Un rol puede tener N patentes y N familias.
--   - La unicidad de filas en las tablas relación (PK compuesta) evita duplicados.
--
-- Las validaciones de "familia duplicada por composición" y "rol en uso por algún
-- usuario" se hacen en el BLL, no en la base.
--
-- Ejecutar UNA SOLA VEZ.
-- ──────────────────────────────────────────────────────────────────

-- ─── Patente (hoja del Composite) ─────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Patente')
BEGIN
    CREATE TABLE Patente (
        Id      INT IDENTITY(1,1) PRIMARY KEY,
        Nombre  NVARCHAR(100) NOT NULL UNIQUE,
        DataKey NVARCHAR(100) NOT NULL UNIQUE   -- código único, ej. "Usuarios.Crear"
    );
END
GO

-- ─── Familia (composite) ──────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Familia')
BEGIN
    CREATE TABLE Familia (
        Id     INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL UNIQUE
    );
END
GO

-- ─── Relaciones M:N ───────────────────────────────────────────────

-- Familia ←→ Patente
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'FamiliaPatente')
BEGIN
    CREATE TABLE FamiliaPatente (
        IdFamilia INT NOT NULL,
        IdPatente INT NOT NULL,
        CONSTRAINT PK_FamiliaPatente PRIMARY KEY (IdFamilia, IdPatente),
        CONSTRAINT FK_FamiliaPatente_Familia FOREIGN KEY (IdFamilia) REFERENCES Familia(Id) ON DELETE CASCADE,
        CONSTRAINT FK_FamiliaPatente_Patente FOREIGN KEY (IdPatente) REFERENCES Patente(Id) ON DELETE CASCADE
    );
END
GO

-- Familia ←→ Subfamilia (anidamiento)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'FamiliaIntegrada')
BEGIN
    CREATE TABLE FamiliaIntegrada (
        IdFamiliaPadre INT NOT NULL,
        IdFamiliaHija  INT NOT NULL,
        CONSTRAINT PK_FamiliaIntegrada PRIMARY KEY (IdFamiliaPadre, IdFamiliaHija),
        -- Sin ON DELETE CASCADE acá: si lo ponemos, SQL Server detecta ciclos.
        CONSTRAINT FK_FamiliaIntegrada_Padre FOREIGN KEY (IdFamiliaPadre) REFERENCES Familia(Id),
        CONSTRAINT FK_FamiliaIntegrada_Hija  FOREIGN KEY (IdFamiliaHija)  REFERENCES Familia(Id),
        CONSTRAINT CK_FamiliaIntegrada_NoAutoref CHECK (IdFamiliaPadre <> IdFamiliaHija)
    );
END
GO

-- Rol ←→ Patente
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'RolPatente')
BEGIN
    CREATE TABLE RolPatente (
        IdRol     INT NOT NULL,
        IdPatente INT NOT NULL,
        CONSTRAINT PK_RolPatente PRIMARY KEY (IdRol, IdPatente),
        CONSTRAINT FK_RolPatente_Rol     FOREIGN KEY (IdRol)     REFERENCES Roles(Id)   ON DELETE CASCADE,
        CONSTRAINT FK_RolPatente_Patente FOREIGN KEY (IdPatente) REFERENCES Patente(Id) ON DELETE CASCADE
    );
END
GO

-- Rol ←→ Familia
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'RolFamilia')
BEGIN
    CREATE TABLE RolFamilia (
        IdRol     INT NOT NULL,
        IdFamilia INT NOT NULL,
        CONSTRAINT PK_RolFamilia PRIMARY KEY (IdRol, IdFamilia),
        CONSTRAINT FK_RolFamilia_Rol     FOREIGN KEY (IdRol)     REFERENCES Roles(Id)   ON DELETE CASCADE,
        CONSTRAINT FK_RolFamilia_Familia FOREIGN KEY (IdFamilia) REFERENCES Familia(Id) ON DELETE CASCADE
    );
END
GO

-- ─── Catálogo inicial de Patentes ─────────────────────────────────
-- Estas son las acciones puntuales del sistema. Agregá las que correspondan
-- a tu app. Conviene un patrón "Modulo.Accion" para que sea legible.
INSERT INTO Patente (Nombre, DataKey)
SELECT v.Nombre, v.DataKey FROM (VALUES
    ('Gestión Usuarios - Ver',         'Usuarios.Ver'),
    ('Gestión Usuarios - Crear',       'Usuarios.Crear'),
    ('Gestión Usuarios - Modificar',   'Usuarios.Modificar'),
    ('Gestión Usuarios - Desbloquear', 'Usuarios.Desbloquear'),
    ('Gestión Usuarios - Activar',     'Usuarios.Activar'),
    ('Bitácora - Ver',                 'Bitacora.Ver'),
    ('Bitácora - Exportar PDF',        'Bitacora.ExportarPDF'),
    ('Permisos - Gestionar',           'Permisos.Gestionar')
) AS v(Nombre, DataKey)
WHERE NOT EXISTS (SELECT 1 FROM Patente p WHERE p.DataKey = v.DataKey);
GO

-- Verificación rápida (opcional):
-- SELECT * FROM Patente;
-- SELECT * FROM Familia;
-- SELECT * FROM Roles;
