using proyectoFrontend_V3.ServicioUsuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace proyectoFrontend_V3
{
    public partial class Form_User : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string idUsuario = Request.QueryString["id"];

                if (!string.IsNullOrEmpty(idUsuario))
                {
                    hfIdUsuario.Value = idUsuario;
                    CargarDatosUsuario(Convert.ToInt32(idUsuario));
                }
                else
                {
                    // Si no hay ID, redirigir a la lista
                    Response.Redirect("G_User.aspx");
                }
            }
        }

        private void CargarDatosUsuario(int idUsuario)
        {
            try
            {
                var cliente = new WS_UsersSoapClient();
                var respuesta = cliente.ObtenerUsuarios();

                if (respuesta.CodigoError == 1 && respuesta.Usuarios != null)
                {
                    // Buscar el usuario específico
                    foreach (var usuario in respuesta.Usuarios)
                    {
                        if (usuario.ID_Usuario == idUsuario)
                        {
                            txtNombre.Text = usuario.NombreUsuario;
                            txtApellido.Text = usuario.ApellidoUsuario;
                            txtAlias.Text = usuario.Alias;
                            txtEmail.Text = usuario.Email;
                            ddlRol.SelectedValue = usuario.ID_Rol.ToString();
                            break;
                        }
                    }
                }
                else
                {
                    lblMensaje.Text = "No se pudo cargar la información del usuario";
                    lblMensaje.CssClass = "mensaje-error";
                    lblMensaje.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar usuario: " + ex.Message;
                lblMensaje.CssClass = "mensaje-error";
                lblMensaje.Visible = true;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int idUsuario = Convert.ToInt32(hfIdUsuario.Value);
                string clave = string.IsNullOrEmpty(txtClave.Text) ? null : txtClave.Text;

                var cliente = new WS_UsersSoapClient();

                var usuario = new User
                {
                    ID_Usuario = idUsuario,
                    NombreUsuario = txtNombre.Text,
                    ApellidoUsuario = txtApellido.Text,
                    Alias = txtAlias.Text,
                    Email = txtEmail.Text,
                    ID_Rol = Convert.ToInt32(ddlRol.SelectedValue),
                    Clave = clave
                };

                var respuesta = cliente.EditarUsuario(usuario);

                if (respuesta.CodigoError == 1)
                {
                    // Redirigir a G_User con mensaje de éxito
                    Response.Redirect("G_User.aspx?msg=editado");
                }
                else
                {
                    lblMensaje.Text = respuesta.Mensaje ?? "No se pudo actualizar el usuario";
                    lblMensaje.CssClass = "mensaje-error";
                    lblMensaje.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al guardar usuario: " + ex.Message;
                lblMensaje.CssClass = "mensaje-error";
                lblMensaje.Visible = true;
            }
        }
    }
}
