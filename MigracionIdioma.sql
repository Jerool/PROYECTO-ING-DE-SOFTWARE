-- ──────────────────────────────────────────────────────────────────
-- Migración: agregar idioma preferido por usuario.
--
-- Permite que cada usuario tenga su idioma guardado. Al loguearse, la app
-- carga ese idioma automáticamente y los textos quedan en la lengua que
-- el usuario usó la última vez.
--
-- Códigos válidos: 'es' (Español) y 'en' (English). Valor por defecto: 'es'.
--
-- Ejecutar UNA SOLA VEZ en SSMS.
-- ──────────────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT 1 FROM sys.columns
               WHERE Name = N'Idioma' AND Object_ID = Object_ID(N'Usuario'))
BEGIN
    ALTER TABLE Usuario ADD Idioma NVARCHAR(5) NOT NULL DEFAULT 'es';
END
GO
