using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ProyectoAV_Back
{
    /// <summary>
    /// Summary description for Handler3
    /// </summary>
    public class DeleteHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            // Obtener ID del documento
            string idDocParam = context.Request.QueryString["id"];

            if (string.IsNullOrEmpty(idDocParam))
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                context.Response.Write("{\"error\": \"Falta el parámetro 'id'\"}");
                return;
            }

            int idDocumento = Convert.ToInt32(idDocParam);

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CnxProyecto"].ConnectionString;
                string rutaFisica = null;
                string foto = null;

                // Consultar rutas del archivo en la BD
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Documentos_ObtenerRutas", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Documento", idDocumento);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        rutaFisica = reader.IsDBNull(0) ? null : reader.GetString(0);
                        foto = reader.IsDBNull(1) ? null : reader.GetString(1);
                    }
                }

                // Eliminar archivo físico del documento
                if (!string.IsNullOrEmpty(rutaFisica))
                {
                    string filePath = context.Server.MapPath("~/Biblioteca/Categoria/" + rutaFisica);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                // Eliminar foto si existe
                if (!string.IsNullOrEmpty(foto))
                {
                    string fotoPath = context.Server.MapPath("~/Fotos/Documentos/" + foto);
                    if (File.Exists(fotoPath))
                    {
                        File.Delete(fotoPath);
                    }
                }

                // Eliminar registro de la BD
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Documentos_Eliminar", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Documento", idDocumento);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    int filasAfectadas = 0;
                    if (reader.Read())
                    {
                        filasAfectadas = reader.GetInt32(0);
                    }

                    if (filasAfectadas > 0)
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.Write("{\"mensaje\": \"Documento eliminado exitosamente\"}");
                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                        context.Response.ContentType = "application/json";
                        context.Response.Write("{\"error\": \"Documento no encontrado\"}");
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                context.Response.Write("{\"error\": \"" + ex.Message + "\"}");
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}