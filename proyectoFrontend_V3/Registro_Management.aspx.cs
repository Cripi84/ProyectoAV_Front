using proyectoFrontend_V3.ServicioUsuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace proyectoFrontend_V3
{
    public partial class Registro_Management : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Verificar que el usuario sea administrador
            //if (Session["UsuarioRol"] == null || Convert.ToInt32(Session["UsuarioRol"]) != 1)
            //{
            //    Response.Redirect("LogIn.aspx");
            //}
        }

        protected void btnRegistrarAdmin_Click(object sender, EventArgs e)
        {
            RegistrarUsuarioAdmin();
        }

        private void RegistrarUsuarioAdmin()
        {
            // Validaciones frontend
            string nombre = Request.Form["nombre"];
            string apellido = Request.Form["apellido"];
            string nickname = Request.Form["nickname"];
            string correo = Request.Form["correo"];
            string password = Request.Form["password"];
            string rolStr = Request.Form["rol"];
            string estadoStr = Request.Form["estado"];

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MostrarAlerta("Por favor ingrese el nombre");
                return;
            }

            if (string.IsNullOrWhiteSpace(apellido))
            {
                MostrarAlerta("Por favor ingrese el apellido");
                return;
            }

            if (string.IsNullOrWhiteSpace(nickname))
            {
                MostrarAlerta("Por favor ingrese el nickname");
                return;
            }

            if (string.IsNullOrWhiteSpace(correo))
            {
                MostrarAlerta("Por favor ingrese el correo");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MostrarAlerta("Por favor ingrese la contraseña");
                return;
            }

            if (password.Length < 6)
            {
                MostrarAlerta("La contraseña debe tener al menos 6 caracteres");
                return;
            }

            if (string.IsNullOrWhiteSpace(rolStr))
            {
                MostrarAlerta("Por favor seleccione un rol");
                return;
            }

            if (string.IsNullOrWhiteSpace(estadoStr))
            {
                MostrarAlerta("Por favor seleccione un estado");
                return;
            }

            int rol, estado;
            if (!int.TryParse(rolStr, out rol))
            {
                MostrarAlerta("Rol inválido");
                return;
            }

            if (!int.TryParse(estadoStr, out estado))
            {
                MostrarAlerta("Estado inválido");
                return;
            }

            try
            {
                var cliente = new WS_UsersSoapClient();
                User nuevoUsuario = new User
                {
                    NombreUsuario = nombre,
                    ApellidoUsuario = apellido,
                    Alias = nickname,
                    Email = correo,
                    Clave = password,
                    ID_Rol = rol,
                    ID_Estado = estado
                };

                var respuesta = cliente.RegistrarAdmin(nuevoUsuario);

                if (respuesta.CodigoError == 1)
                {
                    // Registro exitoso
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "redirect",
                        "alert('" + respuesta.Mensaje.Replace("'", "\\'") + "'); window.location='G_User.aspx';",
                        true
                    );
                }
                else
                {
                    // Error en el registro
                    MostrarAlerta(respuesta.Mensaje);
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error al conectar con el servidor: " + ex.Message);
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
