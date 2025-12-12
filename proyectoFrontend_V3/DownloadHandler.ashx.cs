using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace ProyectoAV_Back
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class DownloadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            // Obtener ID del documento desde la URL
            string idDocParam = context.Request.QueryString["id"];

            // Validar que se haya enviado el ID
            if (string.IsNullOrEmpty(idDocParam))
            {
                EnviarError(context, 400, "Falta el parámetro 'id'");
                return;
            }

            // Validar que el ID sea un número válido
            int idDocumento;
            if (!int.TryParse(idDocParam, out idDocumento))
            {
                EnviarError(context, 400, "El ID de documento es inválido");
                return;
            }

            try
            {
                // Consultar información del documento en la base de datos
                string connectionString = ConfigurationManager.ConnectionStrings["CnxProyecto"].ConnectionString;
                string rutaFisica = null;
                string titulo = null;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Documentos_ObtenerPorID", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Documento", idDocumento);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Columnas según tu SP: ID, Titulo, Foto, ID_Categoria, NombreCategoria, RutaFisica, etc.
                        titulo = reader.GetString(1);           // Columna 1: Titulo
                        rutaFisica = reader.IsDBNull(5) ? null : reader.GetString(5);  // Columna 5: RutaFisica
                    }
                    else
                    {
                        EnviarError(context, 404, "Documento no encontrado en la base de datos");
                        return;
                    }
                }

                // Validar que exista la ruta física
                if (string.IsNullOrEmpty(rutaFisica))
                {
                    EnviarError(context, 404, "El documento no tiene un archivo asociado");
                    return;
                }

                // Construir la ruta completa del archivo en el servidor
                string filePath = context.Server.MapPath("~/Categoria/" + rutaFisica);

                // Verificar que el archivo existe físicamente
                if (!File.Exists(filePath))
                {
                    EnviarError(context, 404, "El archivo físico no existe en el servidor");
                    return;
                }

                // Obtener información del archivo
                FileInfo fileInfo = new FileInfo(filePath);
                string fileName = fileInfo.Name;
                string extension = fileInfo.Extension.ToLower();

                // Determinar el tipo MIME según la extensión
                string contentType = ObtenerContentType(extension);

                // Configurar la respuesta HTTP para la descarga
                context.Response.Clear();
                context.Response.ContentType = contentType;
                context.Response.AddHeader("Content-Disposition",
                    "attachment; filename=\"" + fileName + "\"");
                context.Response.AddHeader("Content-Length", fileInfo.Length.ToString());

                // Enviar el archivo al cliente
                context.Response.TransmitFile(filePath);
                context.Response.Flush();
                context.Response.End();
            }
            catch (SqlException sqlEx)
            {
                EnviarError(context, 500, "Error de base de datos: " + sqlEx.Message);
            }
            catch (IOException ioEx)
            {
                EnviarError(context, 500, "Error al leer el archivo: " + ioEx.Message);
            }
            catch (Exception ex)
            {
                EnviarError(context, 500, "Error al descargar: " + ex.Message);
            }
        }

        private void EnviarError(HttpContext context, int statusCode, string mensaje)
        {
            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            context.Response.Write("{\"error\": \"" +
                mensaje.Replace("\"", "'").Replace("\r", "").Replace("\n", "") + "\"}");
            context.Response.End();
        }

        private string ObtenerContentType(string extension)
        {
            switch (extension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".doc":
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xls":
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".ppt":
                case ".pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".txt":
                    return "text/plain";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                default:
                    return "application/octet-stream";
            }
        }
        public bool IsReusable
        {
            get { return false; }
        }
    }
}