using proyectoFrontend_V3.ServicioUsuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace proyectoFrontend_V3
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private void RegistrarUsuario()
        {
            try
            {
                var cliente = new WS_UsersSoapClient();
                UserPublic nuevo = new UserPublic
                {
                    NombreUsuario = Request.Form["nombre"],
                    ApellidoUsuario = Request.Form["apellido"],
                    Alias = Request.Form["nickname"],
                    Email = Request.Form["correo"],
                    Clave = Request.Form["password"]
                };

                var respuesta = cliente.RegistrarPublico(nuevo);

                if (respuesta.CodigoError == 1)
                {
                    // Registro exitoso - redirigir al login
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "alert",
                        "alert('" + respuesta.Mensaje.Replace("'", "\\'") + "'); window.location='LogIn.aspx';",
                        true
                    );
                }
                else
                {
                    // Error en el registro
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "alert",
                        "alert('" + respuesta.Mensaje.Replace("'", "\\'") + "');",
                        true
                    );
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "alert",
                    "alert('Error: " + ex.Message.Replace("'", "\\'") + "');",
                    true
                );
            }
        }
    }
}