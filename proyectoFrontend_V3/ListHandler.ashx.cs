using proyectoFrontend_V3.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Script.Serialization;

namespace ProyectoAV_Back
{
    public class ListHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            try
            {
                // Obtener filtro de categoría si existe
                string idCategoriaParam = context.Request.QueryString["NombreCategoria"];
                int? idCategoria = null;

                if (!string.IsNullOrEmpty(idCategoriaParam) && int.TryParse(idCategoriaParam, out int catId))
                {
                    idCategoria = catId;
                }

                List<DocumentoDTO> documentos = new List<DocumentoDTO>();

                string connectionString = ConfigurationManager.ConnectionStrings["CnxProyecto"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Documentos_Listar", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        DocumentoDTO doc = new DocumentoDTO
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
                        };

                        documentos.Add(doc);
                    }
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(documentos);
                context.Response.Write(json);
            }
            catch (SqlException sqlEx)
            {
                context.Response.StatusCode = 500;
                context.Response.Write("{\"error\": \"Error de base de datos: " +
                    sqlEx.Message.Replace("\"", "'") + "\"}");
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write("{\"error\": \"Error general: " +
                    ex.Message.Replace("\"", "'") + "\"}");
            }
        }

        public bool IsReusable => false;
    }
}
