using proyectoFrontend_V3.ServicioUsuarios;
using System;
using System.Web.UI.WebControls;

namespace proyectoFrontend_V3
{
    public partial class G_User : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarUsuarios();

                // Mostrar mensaje si viene de Form_Usuarios
                if (Request.QueryString["msg"] != null)
                {
                    string mensaje = Request.QueryString["msg"];
                    if (mensaje == "editado")
                    {
                        lblMensaje.Text = "Usuario actualizado correctamente";
                        lblMensaje.CssClass = "mensaje-exito";
                        lblMensaje.Visible = true;
                    }
                }
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
                    rptUsuarios.DataSource = respuesta.Usuarios;
                    rptUsuarios.DataBind();
                }
                else
                {
                    rptUsuarios.DataSource = null;
                    rptUsuarios.DataBind();
                    lblMensaje.Text = respuesta.Mensaje ?? "No hay usuarios registrados";
                    lblMensaje.CssClass = "mensaje-info";
                    lblMensaje.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar usuarios: " + ex.Message;
                lblMensaje.CssClass = "mensaje-error";
                lblMensaje.Visible = true;
            }
        }

        protected void btnCambiarEstado_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string[] args = btn.CommandArgument.Split(',');
            int idUsuario = Convert.ToInt32(args[0]);
            int estadoActual = Convert.ToInt32(args[1]);

            if (estadoActual == 1)
            {
                DesactivarUsuario(idUsuario);
            }
            else
            {
                ReactivarUsuario(idUsuario);
            }
        }

        private void DesactivarUsuario(int idUsuario)
        {
            try
            {
                var cliente = new WS_UsersSoapClient();
                var respuesta = cliente.EliminarUsuario(idUsuario);

                if (respuesta.CodigoError == 1)
                {
                    lblMensaje.Text = respuesta.Mensaje;
                    lblMensaje.CssClass = "mensaje-exito";
                    lblMensaje.Visible = true;
                    CargarUsuarios();
                }
                else
                {
                    lblMensaje.Text = respuesta.Mensaje ?? "No se pudo desactivar el usuario";
                    lblMensaje.CssClass = "mensaje-error";
                    lblMensaje.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al desactivar usuario: " + ex.Message;
                lblMensaje.CssClass = "mensaje-error";
                lblMensaje.Visible = true;
            }
        }

        private void ReactivarUsuario(int idUsuario)
        {
            try
            {
                var cliente = new WS_UsersSoapClient();
                var respuesta = cliente.ReactivarUsuario(idUsuario);

                if (respuesta.CodigoError == 1)
                {
                    lblMensaje.Text = respuesta.Mensaje;
                    lblMensaje.CssClass = "mensaje-exito";
                    lblMensaje.Visible = true;
                    CargarUsuarios();
                }
                else
                {
                    lblMensaje.Text = respuesta.Mensaje ?? "No se pudo reactivar el usuario";
                    lblMensaje.CssClass = "mensaje-error";
                    lblMensaje.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al reactivar usuario: " + ex.Message;
                lblMensaje.CssClass = "mensaje-error";
                lblMensaje.Visible = true;
            }
        }
    }
}