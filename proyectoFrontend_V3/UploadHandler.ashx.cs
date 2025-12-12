using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ProyectoAV_Back
{
    /// <summary>
    /// Summary description for Handler2
    /// </summary>
    public class UploadHandler : IHttpHandler
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
                // Obtener parámetros
                string nombreCategoria = context.Request.Form["nombreCategoria"];
                int idCategoria = Convert.ToInt32(context.Request.Form["idCategoria"]);

                // Archivo principal (PDF, DOCX, etc.)
                HttpPostedFile archivoDoc = context.Request.Files["archivoDoc"];

                // Foto de portada (opcional)
                HttpPostedFile archivoFoto = context.Request.Files["archivoFoto"];

                // Crear carpeta de categoría si no existe
                string carpetaCategoria = context.Server.MapPath("~/Biblioteca/Categorias/" + nombreCategoria);
                if (!Directory.Exists(carpetaCategoria))
                {
                    Directory.CreateDirectory(carpetaCategoria);
                }

                // Guardar documento principal
                string nombreDoc = Path.GetFileName(archivoDoc.FileName);
                string rutaDoc = Path.Combine(carpetaCategoria, nombreDoc);
                archivoDoc.SaveAs(rutaDoc);

                // Guardar foto si existe
                string nombreFoto = null;
                if (archivoFoto != null && archivoFoto.ContentLength > 0)
                {
                    string carpetaFotos = context.Server.MapPath("~/Fotos/Documentos");
                    if (!Directory.Exists(carpetaFotos))
                    {
                        Directory.CreateDirectory(carpetaFotos);
                    }

                    nombreFoto = "doc_" + DateTime.Now.Ticks + Path.GetExtension(archivoFoto.FileName);
                    string rutaFoto = Path.Combine(carpetaFotos, nombreFoto);
                    archivoFoto.SaveAs(rutaFoto);
                }

                // Ruta relativa para guardar en BD
                string rutaRelativa = nombreCategoria + "/" + nombreDoc;

                // Respuesta exitosa
                context.Response.ContentType = "application/json";
                context.Response.Write("{\"mensaje\": \"Archivo subido exitosamente\", \"rutaFisica\": \"" + rutaRelativa + "\", \"foto\": \"" + nombreFoto + "\"}");
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