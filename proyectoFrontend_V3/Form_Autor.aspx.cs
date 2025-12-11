using proyectoFrontend_V3.ServicioUsuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace proyectoFrontend_V3
{
    public partial class Form_Autor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnGuardarAutor_Click(object sender, EventArgs e)
        {
            CrearAutor();
        }

        private void CrearAutor()
        {
            string nombreAutor = Request.Form["nombreAutor"];
            string apellidoAutor = Request.Form["apellidoAutor"];
            string nacionalidad = Request.Form["nacionalidad"];
            string fechaNacimientoStr = Request.Form["fechaNacimiento"];
            string fechaDefuncionStr = Request.Form["fechaDefuncion"];

            // Validaciones frontend
            if (string.IsNullOrWhiteSpace(nombreAutor))
            {
                MostrarAlerta("Por favor ingrese el nombre del autor");
                return;
            }

            if (string.IsNullOrWhiteSpace(apellidoAutor))
            {
                MostrarAlerta("Por favor ingrese el apellido del autor");
                return;
            }

            if (string.IsNullOrWhiteSpace(nacionalidad))
            {
                MostrarAlerta("Por favor ingrese la nacionalidad");
                return;
            }

            if (string.IsNullOrWhiteSpace(fechaNacimientoStr))
            {
                MostrarAlerta("Por favor ingrese la fecha de nacimiento");
                return;
            }

            DateTime fechaNacimiento;
            if (!DateTime.TryParse(fechaNacimientoStr, out fechaNacimiento))
            {
                MostrarAlerta("Fecha de nacimiento inválida");
                return;
            }

            // Validar fecha de defunción (opcional)
            DateTime? fechaDefuncion = null;
            if (!string.IsNullOrWhiteSpace(fechaDefuncionStr))
            {
                DateTime tempFecha;
                if (DateTime.TryParse(fechaDefuncionStr, out tempFecha))
                {
                    fechaDefuncion = tempFecha;
                }
                else
                {
                    MostrarAlerta("Fecha de defunción inválida");
                    return;
                }
            }

            try
            {
                var cliente = new WS_UsersSoapClient();
                Autor nuevoAutor = new Autor
                {
                    NombreAutor = nombreAutor.Trim(),
                    ApellidoAutor = apellidoAutor.Trim(),
                    Nacionalidad = nacionalidad.Trim(),
                    FechaNacimiento = fechaNacimiento,
                    FechaDefuncion = fechaDefuncion
                };

                var respuesta = cliente.CrearAutor(nuevoAutor);

                if (respuesta.CodigoError == 1)
                {
                    // Autor creado exitosamente
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "redirect",
                        "alert('" + respuesta.Mensaje.Replace("'", "\\'") + "'); window.location='G_Aut.aspx';",
                        true
                    );
                }
                else
                {
                    // Error al crear
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