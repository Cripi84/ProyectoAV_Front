using proyectoFrontend_V3.ServicioUsuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace proyectoFrontend_V3
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            IniciarSesion();
        }

        private void IniciarSesion()
        {
            string loginInput = Request.Form["login"];
            string password = Request.Form["password"];

            // VALIDACIONES
            if (string.IsNullOrWhiteSpace(loginInput))
            {
                MostrarAlerta("Por favor ingrese su usuario o correo");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MostrarAlerta("Por favor ingrese su contraseña");
                return;
            }

            try
            {
                var cliente = new WS_UsersSoapClient();
                var respuesta = cliente.IniciarSesion(loginInput, password);

                // DEBUG: Verificar qué está llegando
                if (respuesta == null)
                {
                    MostrarAlerta("La respuesta del servidor es nula");
                    return;
                }

                // Verificar CodigoError
                MostrarAlerta("Código: " + respuesta.CodigoError + " | Mensaje: " +
                             (respuesta.Mensaje ?? "Sin mensaje"));

                if (respuesta.CodigoError == 1 && respuesta.Usuario != null)
                {
                    // Login exitoso
                    Session["UsuarioID"] = respuesta.Usuario.ID_Usuario;
                    Session["UsuarioNombre"] = respuesta.Usuario.NombreUsuario;
                    Session["UsuarioRol"] = respuesta.Usuario.ID_Rol;

                    // Redirigir según rol
                    if (respuesta.Usuario.ID_Rol == 3)
                    {
                        Response.Redirect("Home_User.aspx");
                    }
                    else
                    {
                        Response.Redirect("Home_Management.aspx");
                    }
                }
                else
                {
                    MostrarAlerta(respuesta.Mensaje ?? "Error desconocido");
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error: " + ex.Message);
            }
        }

        private void MostrarAlerta(string mensaje)
        {
            ClientScript.RegisterStartupScript(
                this.GetType(),
                "alert",
                "alert('" + mensaje.Replace("'", "\\'") + "');",
                true
            );
        }
    }
}