using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
            string ruta = context.Request.QueryString["ruta"];

            if (string.IsNullOrEmpty(ruta))
            {
                context.Response.StatusCode = 400;
                context.Response.Write("Ruta inválida");
                return;
            }

            // Ruta base donde guardas los archivos
            string basePath = context.Server.MapPath("~/Categorias/");
            string rutaCompleta = Path.Combine(basePath, ruta);

            if (!File.Exists(rutaCompleta))
            {
                context.Response.StatusCode = 404;
                context.Response.Write("Archivo no existe + ruta=" + rutaCompleta);
                Debug.WriteLine("Archivo no existe + ruta=" + rutaCompleta);
                return;
            }

            context.Response.Clear();
            context.Response.ContentType = "application/octet-stream";
            context.Response.AddHeader(
                "Content-Disposition",
                "attachment; filename=" + Path.GetFileName(rutaCompleta)
            );

            context.Response.TransmitFile(rutaCompleta);
            context.Response.End();
        }
        public bool IsReusable
        {
            get { return false; }
        }
    }
}