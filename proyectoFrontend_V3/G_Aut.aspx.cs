using proyectoFrontend_V3.ServicioUsuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace proyectoFrontend_V3
{
    public partial class G_Aut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verificar si viene acción de eliminar
                string action = Request.QueryString["action"];
                string idParam = Request.QueryString["id"];

                if (action == "delete" && !string.IsNullOrEmpty(idParam))
                {
                    if (int.TryParse(idParam, out int idAutor))
                    {
                        EliminarAutor(idAutor);
                    }
                }

                CargarAutores();
            }
        }

        private void CargarAutores()
        {
            try
            {
                var cliente = new WS_UsersSoapClient();
                var respuesta = cliente.ObtenerAutores();

                if (respuesta.CodigoError == 1 && respuesta.Autores != null && respuesta.Autores.Length > 0)
                {
                    // Enlazar los datos al Repeater
                    rptAutores.DataSource = respuesta.Autores;
                    rptAutores.DataBind();
                    lblMensaje.Visible = false;
                }
                else
                {
                    // No hay autores o hubo un error
                    rptAutores.DataSource = null;
                    rptAutores.DataBind();
                    lblMensaje.Text = respuesta.Mensaje ?? "No hay autores registrados";
                    lblMensaje.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar autores: " + ex.Message;
                lblMensaje.Visible = true;
            }
        }

        private void EliminarAutor(int idAutor)
        {
            try
            {
                var cliente = new WS_UsersSoapClient();
                var respuesta = cliente.EliminarAutor(idAutor);

                if (respuesta.CodigoError == 1)
                {
                    // Éxito - mostrar mensaje y recargar
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "alert",
                        $"alert('{respuesta.Mensaje.Replace("'", "\\'")}');",
                        true
                    );
                }
                else if (respuesta.CodigoError == -2)
                {
                    // Autor con documentos asociados
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "alert",
                        $"alert('⚠️ {respuesta.Mensaje.Replace("'", "\\'")}');",
                        true
                    );
                }
                else if (respuesta.CodigoError == -3)
                {
                    // Intento de eliminar autor "Desconocido"
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "alert",
                        $"alert(' {respuesta.Mensaje.Replace("'", "\\'")}');",
                        true
                    );
                }
                else
                {
                    // Otro error
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "alert",
                        $"alert(' Error: {respuesta.Mensaje.Replace("'", "\\'")}');",
                        true
                    );
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "alert",
                    $"alert(' Error al eliminar: {ex.Message.Replace("'", "\\'")}');",
                    true
                );
            }
        }
    }
}