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
    /// Summary description for Handler2
    /// </summary>
    public class UploadHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            // Verificar que haya archivos
            if (context.Request.Files.Count == 0)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                context.Response.Write("{\"error\": \"No se seleccionó ningún archivo\"}");
                return;
            }

            try
            {
                // Obtener datos del formulario
                string titulo = context.Request.Form["titulo"];
                string idCategoriaStr = context.Request.Form["idCategoria"];
                string nombreCategoria = context.Request.Form["nombreCategoria"];

                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(titulo))
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    context.Response.Write("{\"error\": \"El título es requerido\"}");
                    return;
                }

                if (string.IsNullOrWhiteSpace(idCategoriaStr))
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    context.Response.Write("{\"error\": \"La categoría es requerida\"}");
                    return;
                }

                int idCategoria = Convert.ToInt32(idCategoriaStr);

                // Obtener ID del usuario desde la sesión
                if (context.Session["UsuarioID"] == null)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    context.Response.Write("{\"error\": \"Usuario no autenticado\"}");
                    return;
                }

               

                // Archivo principal (PDF, DOCX, etc.)
                HttpPostedFile archivoDoc = context.Request.Files["archivoDoc"];

                if (archivoDoc == null || archivoDoc.ContentLength == 0)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    context.Response.Write("{\"error\": \"Debe seleccionar un archivo de documento\"}");
                    return;
                }

                // Foto de portada (opcional)
                HttpPostedFile archivoFoto = context.Request.Files["archivoFoto"];

                if (archivoFoto == null || archivoFoto.ContentLength == 0)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    context.Response.Write("{\"error\": \"Debe seleccionar una foto\"}");
                    return;
                }
                    

                // Crear carpeta de categoría si no existe
                string carpetaCategoria = context.Server.MapPath("~/Categorias/" + nombreCategoria);
                if (!Directory.Exists(carpetaCategoria))
                {
                    Directory.CreateDirectory(carpetaCategoria);
                }

                // Generar nombre único para el documento
                string extension = Path.GetExtension(archivoDoc.FileName);
                string nombreDocUnico = "doc_" + DateTime.Now.Ticks + extension;
                string rutaDoc = Path.Combine(carpetaCategoria, nombreDocUnico);

                // Guardar documento principal
                archivoDoc.SaveAs(rutaDoc);

                // Guardar foto si existe
                string nombreFoto = null;
                if (archivoFoto != null && archivoFoto.ContentLength > 0)
                {
                    string carpetaFotos = context.Server.MapPath("~/Fotos/");
                    if (!Directory.Exists(carpetaFotos))
                    {
                        Directory.CreateDirectory(carpetaFotos);
                    }

                    string extensionFoto = Path.GetExtension(archivoFoto.FileName);
                    nombreFoto = "portada_" + DateTime.Now.Ticks + extensionFoto;
                    string rutaFoto = Path.Combine(carpetaFotos, nombreFoto);
                    archivoFoto.SaveAs(rutaFoto);
                }

                // Ruta relativa para guardar en BD
                string rutaRelativa = nombreCategoria + "/" + nombreDocUnico;

                // Insertar en la base de datos
                int idUsuario = Convert.ToInt32(context.Session["UsuarioID"]);
                string connectionString = ConfigurationManager.ConnectionStrings["CnxProyecto"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_Documentos_Insertar", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Titulo", titulo);
                    cmd.Parameters.AddWithValue("@Foto", (object)nombreFoto ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ID_Categoria", idCategoria);
                    cmd.Parameters.AddWithValue("@RutaFisica", rutaRelativa);
                    cmd.Parameters.AddWithValue("@ID_UsuarioSubida", idUsuario);

                    // Parámetro de salida para el ID generado
                    SqlParameter outputId = new SqlParameter("@ID_Documento", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputId);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    int idDocumentoGenerado = Convert.ToInt32(outputId.Value);

                    // Respuesta exitosa
                    context.Response.ContentType = "application/json";
                    context.Response.Write("{\"mensaje\": \"Documento subido exitosamente\", \"idDocumento\": " + idDocumentoGenerado + ", \"rutaFisica\": \"" + rutaRelativa + "\", \"foto\": \"" + nombreFoto + "\"}");
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                context.Response.Write("{\"error\": \"" + ex.Message.Replace("\"", "'") + "\"}");
            }
        }


        public bool IsReusable
        {
            get { return false; }
        }
    }
}