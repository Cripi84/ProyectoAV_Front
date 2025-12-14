using Newtonsoft.Json;
using proyectoFrontend_V3.Model;
using proyectoFrontend_V3.ServicioUsuarios;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace proyectoFrontend_V3
{
    public partial class G_Doc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var client = new WS_UsersSoapClient();
                var json = client.ListarDocumentos();
                ResponseDocumento response =
    JsonConvert.DeserializeObject<ResponseDocumento>(json);





                var html = new StringBuilder();

                if (response.Documentos != null)
                {
                    foreach (var doc in response.Documentos)
                    {
                        html.Append("<tr>");
                        html.AppendFormat("<td>{0}</td>", doc.Titulo);
                        html.AppendFormat("<td>{0}</td>", doc.NombreCategoria);
                        html.AppendFormat("<td>{0}</td>", doc.NombreUsuario);
                        html.AppendFormat(
    "<td><button onclick=\"descargarArchivo('{0}')\">Descargar</button></td>",
    doc.RutaFisica.Replace("\\", "/")
);

                        html.Append("</tr>");
                    }


                }
                else
                {
                    html.Append("<tr>");
                    html.Append(" <td colspan=\'4\' style=\'text-align: center;\'>No hay documentos...</td>");
                    html.Append("<tr>");
                    tbody.Text = html.ToString();
                    return;
                }

                tbody.Text = html.ToString();

            }
        }
    }
}