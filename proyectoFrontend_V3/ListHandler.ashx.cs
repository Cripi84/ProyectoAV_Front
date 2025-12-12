using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;


namespace ProyectoAV_Back
{
    /// <summary>
    /// Summary description for Handler4
    /// </summary>
    public class ListHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                // Obtener parámetro opcional de categoría
                string idCategoriaParam = context.Request.QueryString["categoria"];

                List<Documento> documentos = new List<Documento>();
                string connectionString = ConfigurationManager.ConnectionStrings["CnxProyecto"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd;

                    // Si se especifica categoría
                    if (!string.IsNullOrEmpty(idCategoriaParam))
                    {
                        cmd = new SqlCommand("sp_Documentos_ListarPorCategoria", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_Categoria", Convert.ToInt32(idCategoriaParam));
                    }
                    else
                    {
                        // Listar todos
                        cmd = new SqlCommand("sp_Documentos_Listar", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                    }

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        documentos.Add(new Documento
                        {
                            ID_Documento = reader.GetInt32(0),
                            Titulo = reader.GetString(1),
                            Foto = reader.IsDBNull(2) ? null : reader.GetString(2),
                            ID_Categoria = reader.GetInt32(3),
                            NombreCategoria = reader.GetString(4),
                            RutaFisica = reader.IsDBNull(5) ? null : reader.GetString(5),
                            FechaPublicacion = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                            ID_UsuarioSubida = reader.GetInt32(7),
                            NombreUsuario = reader.GetString(8),
                            FechaSubida = reader.GetDateTime(9)
                        });
                    }
                }

                // Retornar JSON
                context.Response.ContentType = "application/json";
                context.Response.Write(new JavaScriptSerializer().Serialize(documentos));
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