using proyectoFrontend_V3.ServicioUsuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace proyectoFrontend_V3
{
    public partial class G_User : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Mostrar datos del usuario logueado en el dropdown
                //lblUsuarioNombre.Text = UsuarioNombre + " " + Session["UsuarioApellido"];
                //lblUsuarioEmail.Text = Session["UsuarioEmail"]?.ToString();

                // Cargar la lista de usuarios
                CargarUsuarios();
            }
        }

        private void CargarUsuarios()
        {
            try
            {
                var cliente = new WS_UsersSoapClient();
                var respuesta = cliente.ObtenerUsuarios();

                if (respuesta.CodigoError == 1 && respuesta.Usuarios != null && respuesta.Usuarios.Length > 0)
                {
                    // Enlazar los datos al Repeater
                    rptUsuarios.DataSource = respuesta.Usuarios;
                    rptUsuarios.DataBind();

                    lblMensaje.Visible = false;
                }
                else
                {
                    // No hay usuarios o hubo un error
                    rptUsuarios.DataSource = null;
                    rptUsuarios.DataBind();

                    lblMensaje.Text = respuesta.Mensaje ?? "No hay usuarios registrados";
                    lblMensaje.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar usuarios: " + ex.Message;
                lblMensaje.Visible = true;
            }
        }
    }
}