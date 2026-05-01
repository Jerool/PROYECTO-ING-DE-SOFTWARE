using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLL
{
    
    public class GeneradorPdf_GV42
    {
        // ── Constantes de layout ──
        // Página tamaño Letter en orientación apaisada (landscape): 792 x 612 puntos.
        // El apaisado nos da más espacio horizontal para la tabla.
        private const int ANCHO_PAGINA = 792;
        private const int ALTO_PAGINA = 612;
        private const int MARGEN = 40;
        private const int ALTO_FILA = 18;
        private const int FILAS_POR_PAGINA = 23;

        // Buffer interno donde armamos todo el PDF antes de escribirlo a disco.
        // Lo necesitamos como MemoryStream para poder calcular las posiciones
        // (offsets) exactas de cada objeto, requeridos por la tabla xref.
        private MemoryStream _buffer;

        // Lista paralela: en _offsetsObjetos[i] guardamos la posición en bytes
        // donde empieza el objeto número (i+1). El xref los necesita.
        private List<long> _offsetsObjetos;

        /// <summary>
        /// Genera un PDF con una tabla a partir de los datos provistos.
        /// </summary>
        /// <param name="ruta">Ruta absoluta donde guardar el archivo .pdf</param>
        /// <param name="titulo">Título grande arriba (ej. "Bitácora de Eventos")</param>
        /// <param name="subtitulo">Línea de detalle (ej. "Generado el ...")</param>
        /// <param name="headers">Nombres de las columnas de la tabla</param>
        /// <param name="anchosProporcionales">Proporción de ancho de cada columna (suman 1.0)</param>
        /// <param name="filas">Lista de filas; cada fila es un array con un valor por columna</param>
        public void Generar(string ruta, string titulo, string subtitulo,
                            string[] headers, float[] anchosProporcionales,
                            List<string[]> filas)
        {
            _buffer = new MemoryStream();
            _offsetsObjetos = new List<long>();

            // Cuántas páginas necesitamos según la cantidad de filas.
            int totalPaginas = Math.Max(1, (int)Math.Ceiling((double)filas.Count / FILAS_POR_PAGINA));

            // Asignación de IDs de objetos en orden:
            //   1            = Catalog (la raíz del documento)
            //   2            = Pages (lista de páginas)
            //   3..2+P       = una página por cada bloque de filas
            //   3+P          = Font Helvetica
            //   4+P          = Font Helvetica-Bold
            //   5+P..4+2P    = Content streams (las "instrucciones de dibujo" de cada página)
            int idCatalog = 1;
            int idPages = 2;
            int primerIdPagina = 3;
            int idFont = primerIdPagina + totalPaginas;
            int idFontBold = idFont + 1;
            int primerIdContents = idFontBold + 1;

            // ── 1) Cabecera del archivo PDF ──
            EscribirHeader();

            // ── 2) Objeto Catalog: el "punto de entrada" del documento ──
            EscribirObjeto(idCatalog, $"<</Type/Catalog/Pages {idPages} 0 R>>");

            // ── 3) Objeto Pages: lista todas las páginas ──
            string kidsStr = string.Join(" ",
                Enumerable.Range(0, totalPaginas).Select(i => $"{primerIdPagina + i} 0 R"));
            EscribirObjeto(idPages, $"<</Type/Pages/Kids[{kidsStr}]/Count {totalPaginas}>>");

            // ── 4) Una página por cada bloque ──
            for (int p = 0; p < totalPaginas; p++)
            {
                int idPage = primerIdPagina + p;
                int idContents = primerIdContents + p;
                EscribirObjeto(idPage,
                    $"<</Type/Page/Parent {idPages} 0 R" +
                    $"/MediaBox[0 0 {ANCHO_PAGINA} {ALTO_PAGINA}]" +
                    $"/Resources<</Font<</F1 {idFont} 0 R/F2 {idFontBold} 0 R>>>>" +
                    $"/Contents {idContents} 0 R>>");
            }

            // ── 5) Fuentes (built-in del PDF, no hay que embeber archivos) ──
            EscribirObjeto(idFont,
                "<</Type/Font/Subtype/Type1/BaseFont/Helvetica/Encoding/WinAnsiEncoding>>");
            EscribirObjeto(idFontBold,
                "<</Type/Font/Subtype/Type1/BaseFont/Helvetica-Bold/Encoding/WinAnsiEncoding>>");

            // ── 6) Calculamos anchos absolutos de columnas ──
            float anchoUtil = ANCHO_PAGINA - 2 * MARGEN;
            float[] anchos = anchosProporcionales.Select(pct => pct * anchoUtil).ToArray();

            // ── 7) Generamos el "content stream" de cada página ──
            for (int p = 0; p < totalPaginas; p++)
            {
                int idContents = primerIdContents + p;
                int filaInicio = p * FILAS_POR_PAGINA;
                int filaFin = Math.Min(filaInicio + FILAS_POR_PAGINA, filas.Count);

                StringBuilder content = new StringBuilder();

                int yActual = ALTO_PAGINA - MARGEN;

                // ── Título grande de la página ──
                content.Append("BT\n");
                content.Append("0.05 0.28 0.63 rg\n");                // Color azul oscuro #0D47A1
                content.Append("/F2 18 Tf\n");                         // Helvetica-Bold 18pt
                content.AppendFormat("1 0 0 1 {0} {1} Tm\n", MARGEN, yActual - 18);
                content.AppendFormat("({0}) Tj\n", EscaparTexto(titulo));
                content.Append("ET\n");
                yActual -= 30;

                // ── Subtítulo (gris, más chico) ──
                content.Append("BT\n");
                content.Append("0.4 0.4 0.4 rg\n");                    // Color gris
                content.Append("/F1 9 Tf\n");
                content.AppendFormat("1 0 0 1 {0} {1} Tm\n", MARGEN, yActual);
                string subtitFinal = subtitulo + (totalPaginas > 1 ? $" - Pagina {p + 1} de {totalPaginas}" : "");
                content.AppendFormat("({0}) Tj\n", EscaparTexto(subtitFinal));
                content.Append("ET\n");
                yActual -= 22;

                // ── Cabecera de la tabla con fondo azul ──
                int yHeaderTop = yActual;
                int yHeaderBot = yHeaderTop - ALTO_FILA;
                content.Append("0.05 0.28 0.63 rg\n");                 // Fondo azul oscuro
                content.AppendFormat("{0} {1} {2} {3} re\n", MARGEN, yHeaderBot, anchoUtil, ALTO_FILA);
                content.Append("f\n");                                 // Rellenar rectángulo

                // Texto blanco del header
                content.Append("BT\n");
                content.Append("1 1 1 rg\n");                          // Color blanco
                content.Append("/F2 10 Tf\n");
                float xCol = MARGEN;
                for (int h = 0; h < headers.Length; h++)
                {
                    content.AppendFormat("1 0 0 1 {0} {1} Tm\n", xCol + 4, yHeaderBot + 5);
                    content.AppendFormat("({0}) Tj\n", EscaparTexto(headers[h]));
                    xCol += anchos[h];
                }
                content.Append("ET\n");
                yActual = yHeaderBot;

                // ── Filas de datos ──
                bool zebra = false;
                for (int f = filaInicio; f < filaFin; f++)
                {
                    int yFilaTop = yActual;
                    int yFilaBot = yFilaTop - ALTO_FILA;

                    // Fondo cebrado (filas alternadas con celeste muy claro)
                    if (zebra)
                    {
                        content.Append("0.89 0.95 0.99 rg\n");          // Celeste muy suave
                        content.AppendFormat("{0} {1} {2} {3} re\n", MARGEN, yFilaBot, anchoUtil, ALTO_FILA);
                        content.Append("f\n");
                    }
                    zebra = !zebra;

                    // Texto de las celdas
                    string[] valores = filas[f];
                    content.Append("BT\n");
                    content.Append("0 0 0 rg\n");                       // Texto negro
                    content.Append("/F1 9 Tf\n");
                    xCol = MARGEN;
                    for (int c = 0; c < headers.Length; c++)
                    {
                        string val = c < valores.Length ? (valores[c] ?? "") : "";
                        // Truncamos si no entra en la celda (estimación: 1 char ≈ 5pt en Helvetica 9).
                        int maxChars = (int)(anchos[c] / 5);
                        if (val.Length > maxChars && maxChars > 1)
                            val = val.Substring(0, maxChars - 1) + ".";

                        content.AppendFormat("1 0 0 1 {0} {1} Tm\n", xCol + 4, yFilaBot + 5);
                        content.AppendFormat("({0}) Tj\n", EscaparTexto(val));
                        xCol += anchos[c];
                    }
                    content.Append("ET\n");

                    yActual = yFilaBot;
                }

                // ── Bordes de la tabla (líneas finas) ──
                content.Append("0.7 0.85 0.95 RG\n");                  // Color de líneas (celeste)
                content.Append("0.5 w\n");                             // Grosor de línea
                // Línea inferior de toda la tabla
                content.AppendFormat("{0} {1} m\n", MARGEN, yActual);
                content.AppendFormat("{0} {1} l\n", MARGEN + anchoUtil, yActual);
                content.Append("S\n");

                EscribirStreamObjeto(idContents, content.ToString());
            }

            
            long xrefOffset = _buffer.Position;
            EscribirXref();
            EscribirTrailer(idCatalog, xrefOffset);

            
            File.WriteAllBytes(ruta, _buffer.ToArray());
        }

        

        // Escribe el header del PDF: versión + comentario binario.
        // El comentario binario indica a las herramientas que el archivo es binario
        // (necesario porque algunos sistemas tratan a los PDFs como texto si no lo ven).
        private void EscribirHeader()
        {
            EscribirBytes("%PDF-1.4\n");
            byte[] binario = { (byte)'%', 0xE2, 0xE3, 0xCF, 0xD3, (byte)'\n' };
            _buffer.Write(binario, 0, binario.Length);
        }

        // Escribe un objeto PDF "no-stream" (típicamente un diccionario).
        // Guardamos el offset donde arranca para usarlo después en el xref.
        private void EscribirObjeto(int id, string contenido)
        {
            while (_offsetsObjetos.Count < id) _offsetsObjetos.Add(0);
            _offsetsObjetos[id - 1] = _buffer.Position;
            EscribirBytes($"{id} 0 obj\n{contenido}\nendobj\n");
        }

        // Escribe un objeto que contiene un "stream" (las instrucciones de dibujo).
        // El diccionario externo tiene que declarar la longitud exacta del stream.
        private void EscribirStreamObjeto(int id, string contenidoStream)
        {
            byte[] streamBytes = Encoding.GetEncoding(1252).GetBytes(contenidoStream);

            while (_offsetsObjetos.Count < id) _offsetsObjetos.Add(0);
            _offsetsObjetos[id - 1] = _buffer.Position;

            EscribirBytes($"{id} 0 obj\n");
            EscribirBytes($"<</Length {streamBytes.Length}>>\nstream\n");
            _buffer.Write(streamBytes, 0, streamBytes.Length);
            EscribirBytes("\nendstream\nendobj\n");
        }

        // Escribe la "tabla de referencias cruzadas" (xref).
        // Indica la posición exacta en bytes de cada objeto del PDF.
        // Formato: cada entrada es exactamente 20 bytes (10 dígitos + " " + 5 dígitos + " " + 'n' + '\n').
        private void EscribirXref()
        {
            int total = _offsetsObjetos.Count + 1;
            EscribirBytes("xref\n");
            EscribirBytes($"0 {total}\n");
            EscribirBytes("0000000000 65535 f \n");                 // entrada 0 (siempre así)
            foreach (var off in _offsetsObjetos)
            {
                EscribirBytes($"{off.ToString("D10")} 00000 n \n");
            }
        }

        // Escribe el trailer: información final del documento.
        // /Size = cantidad de objetos + 1.
        // /Root = el Catalog (objeto raíz).
        // startxref = posición donde empieza el xref.
        private void EscribirTrailer(int idCatalog, long xrefOffset)
        {
            int total = _offsetsObjetos.Count + 1;
            EscribirBytes($"trailer\n<</Size {total}/Root {idCatalog} 0 R>>\n");
            EscribirBytes($"startxref\n{xrefOffset}\n");
            EscribirBytes("%%EOF\n");
        }

        // Helper para escribir un string al buffer usando Windows-1252
        // (que soporta acentos castellanos sin problemas).
        private void EscribirBytes(string s)
        {
            byte[] bytes = Encoding.GetEncoding(1252).GetBytes(s);
            _buffer.Write(bytes, 0, bytes.Length);
        }

        // En PDF, dentro de un literal "(...)" hay que escapar los siguientes caracteres:
        //   \  (backslash)
        //   (
        //   )
        // Si no escapamos, el parser PDF se confunde con los paréntesis del texto vs los del literal.
        private string EscaparTexto(string s)
        {
            if (s == null) return "";
            return s
                .Replace("\\", "\\\\")
                .Replace("(", "\\(")
                .Replace(")", "\\)");
        }
    }
}
