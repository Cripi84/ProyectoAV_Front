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
           //Obtener ID del documento
            string idDocParam = context.Request.QueryString["id"];

            if (string.IsNullOrEmpty(idDocParam))
            {
                context.Response.StatusCode = 400;
                context.Response.Write("Falta el parámetro 'id'.");
                return;
            }

            int idDocumento = Convert.ToInt32(idDocParam);

            try
            {
                // Consultar la ruta del archivo en la BD
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
                        rutaFisica = reader.IsDBNull(5) ? null : reader.GetString(5);
                        titulo = reader.GetString(1);
                    }
                }

                if (string.IsNullOrEmpty(rutaFisica))
                {
                    context.Response.StatusCode = 404;
                    context.Response.Write("Documento no encontrado en la base de datos.");
                    return;
                }

                // Construir la ruta completa del archivo
                string filePath = context.Server.MapPath("~/Biblioteca/Categoria/" + rutaFisica);

                if (File.Exists(filePath))
                {
                    // Obtener nombre del archivo
                    string fileName = Path.GetFileName(filePath);

                    // Configurar la descarga
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                    context.Response.WriteFile(filePath);
                    context.Response.End();
                }
                else
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "text/html";
                    context.Response.Write("<script>alert('¡Archivo físico no encontrado!');</script>");
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write("Error al descargar: " + ex.Message);
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}