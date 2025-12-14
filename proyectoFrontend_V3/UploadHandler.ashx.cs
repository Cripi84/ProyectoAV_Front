using proyectoFrontend_V3.ServicioUsuarios;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ProyectoAV_Back
{
    public class UploadHandler : IHttpHandler, IRequiresSessionState
    {
        private readonly string[] _allowedDocExt = { ".pdf", ".docx", ".doc", ".txt" };
        private readonly string[] _allowedImgExt = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

        private const long MAX_DOC_BYTES = 10 * 1024 * 1024;
        private const long MAX_IMG_BYTES = 5 * 1024 * 1024;
        private const string CNX = "CnxProyecto";

        public void ProcessRequest(HttpContext context)
        {
            try
            {
             



                // Validar sesión
                if (context.Session?["UsuarioID"] == null)
                {
                    ResponderError(context, 401, "Usuario no autenticado.");
                    context.Response.ContentType = "application/json";
                    context.Response.Write("{\"success\":false, \"error\":\"Usuario no autenticado.\"}");
                    return;
                }

               
                if (context.Request.Files.Count != 2)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    context.Response.Write(
                        "{\"success\":false, \"error\":\"Se requieren exactamente 2 archivos.\"}"
                    );
                    return;
                }


                // En lugar de buscar por nombre
                HttpPostedFile archivoDoc = context.Request.Files.Count > 0 ? context.Request.Files[0] : null;
                HttpPostedFile archivoFoto = context.Request.Files.Count > 1 ? context.Request.Files[1] : null;

                // Validar archivo de documento
                if (archivoDoc == null || archivoDoc.ContentLength == 0)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    context.Response.Write("{\"success\":false, \"error\":\"Debe seleccionar un archivo de documento.\"}");
                    return;
                }

                // Validar archivo de foto
                if (archivoFoto == null || archivoFoto.ContentLength == 0)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    context.Response.Write("{\"success\":false, \"error\":\"La imagen de portada es obligatoria.\"}");
                    return;
                }

                // Validar tamaño del documento
                if (archivoDoc.ContentLength > MAX_DOC_BYTES)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    context.Response.Write("{\"success\":false, \"error\":\"Archivo de documento excede el tamaño máximo de 10 MB.\"}");
                    return;
                }

                // Validar tamaño de la imagen
                if (archivoFoto.ContentLength > MAX_IMG_BYTES)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    context.Response.Write("{\"success\":false, \"error\":\"Imagen de portada excede el tamaño máximo de 5 MB.\"}");
                    return;
                }

                // Validar extensiones
                string extensionDoc = Path.GetExtension(archivoDoc.FileName).ToLowerInvariant();
                string extensionFoto = Path.GetExtension(archivoFoto.FileName).ToLowerInvariant();

                if (!_allowedDocExt.Contains(extensionDoc))
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    context.Response.Write("{\"success\":false, \"error\":\"Formato del documento no válido. Permitidos: PDF, DOCX, DOC, TXT.\"}");
                    return;
                }

                if (!_allowedImgExt.Contains(extensionFoto))
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    context.Response.Write("{\"success\":false, \"error\":\"Formato de imagen no válido. Permitidos: JPG, PNG, GIF, BMP.\"}");
                    return;
                }

                string titulo = context.Request.Form["titulo"];
                string idCategoriaStr = context.Request.Form["ddlCategoria"];
                string idAutorStr = context.Request.Form["ddlAutor"];
                string sinopsis = context.Request.Form["sinopsis"];
                string fechaStr = context.Request.Form["fechaPublicacion"];

                if (string.IsNullOrWhiteSpace(titulo) ||
                    string.IsNullOrWhiteSpace(idCategoriaStr) ||
                    string.IsNullOrWhiteSpace(idAutorStr) ||
                    string.IsNullOrWhiteSpace(sinopsis))
                {
                    ResponderError(context, 400, "Todos los campos son obligatorios.");
                    return;
                }

                if (!int.TryParse(idCategoriaStr, out int idCategoria))
                {
                    ResponderError(context, 400, "Categoría inválida.");
                    return;
                }

                if (!DateTime.TryParse(fechaStr, out DateTime fechaPublicacion) || fechaPublicacion > DateTime.Now)
                {
                    ResponderError(context, 400, "Fecha de publicación inválida.");
                    return;
                }

                string nombreCategoria = ObtenerNombreCategoria(idCategoria);
                if (string.IsNullOrEmpty(nombreCategoria))
                {
                    ResponderError(context, 400, "No se pudo obtener la categoría.");
                    return;
                }

                // Carpetas
                string carpetaCategoria = context.Server.MapPath($"~/Categorias/{nombreCategoria}");
                Directory.CreateDirectory(carpetaCategoria);

                string carpetaFotos = context.Server.MapPath("~/Fotos/");
                Directory.CreateDirectory(carpetaFotos);

                // Nombres únicos 
                string nombreDocUnico = $"{Guid.NewGuid()}{extensionDoc}";
                string nombreFotoUnico = $"{Guid.NewGuid()}{extensionFoto}";

                archivoDoc.SaveAs(Path.Combine(carpetaCategoria, nombreDocUnico));
                archivoFoto.SaveAs(Path.Combine(carpetaFotos, nombreFotoUnico));

                string rutaRelativa = $"{nombreCategoria}/{nombreDocUnico}";
                int idUsuario = Convert.ToInt32(context.Session["UsuarioID"]);

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings[CNX].ConnectionString))
                using (SqlCommand cmd = new SqlCommand("sp_Documentos_Insertar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Titulo", titulo);
                    cmd.Parameters.AddWithValue("@Foto", nombreFotoUnico);
                    cmd.Parameters.AddWithValue("@ID_Categoria", idCategoria);
                    cmd.Parameters.AddWithValue("@Sinopsis", sinopsis);
                    cmd.Parameters.AddWithValue("@RutaFisica", rutaRelativa);
                    cmd.Parameters.AddWithValue("@FechaPublicacion", fechaPublicacion);
                    cmd.Parameters.AddWithValue("@ID_UsuarioSubida", idUsuario);
                    cmd.Parameters.AddWithValue("@Autores", idAutorStr);

                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read() && Convert.ToInt32(dr["Exito"]) == 1)
                        {
                            context.Response.ContentType = "application/json";
                            context.Response.Write("{\"success\":true,\"mensaje\":\"Documento guardado correctamente\"}");
                        }
                        else
                        {
                            ResponderError(context, 500, dr["Mensaje"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ResponderError(context, 500, ex.Message);
            }
        }

        private void ResponderError(HttpContext ctx, int code, string msg)
        {
            ctx.Response.StatusCode = code;
            ctx.Response.ContentType = "application/json";
            ctx.Response.Write("{\"success\":false,\"error\":\"" +
                HttpUtility.JavaScriptStringEncode(msg) + "\"}");
        }

        private string ObtenerNombreCategoria(int idCategoria)
        {
            try
            {
                var ws = new WS_UsersSoapClient();
                var r = ws.ObtenerCategoriaPorID(idCategoria);

                if (r.CodigoError == 1 && r.Categorias?.Length > 0)
                    return r.Categorias[0].NombreCategoria;

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public bool IsReusable => false;
    }
}
