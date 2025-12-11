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
            if (!IsPostBack)
            {
                // Verificar si viene un ID por QueryString (modo edición)
                string idParam = Request.QueryString["id"];

                if (!string.IsNullOrEmpty(idParam) && int.TryParse(idParam, out int idAutor))
                {
                    // Modo EDITAR - cargar datos del autor
                    CargarDatosAutor(idAutor);
                }
            }
        }

        private void CargarDatosAutor(int idAutor)
        {
            try
            {
                var cliente = new WS_UsersSoapClient();
                var respuesta = cliente.ObtenerAutorPorID(idAutor);

                if (respuesta.CodigoError == 1 && respuesta.Autores != null && respuesta.Autores.Length > 0)
                {
                    var autor = respuesta.Autores[0];

                    // Cargar datos en los campos usando JavaScript
                    string script = $@"
                document.getElementById('idAutor').value = '{autor.ID_Autor}';
                document.getElementById('nombreAutor').value = '{autor.NombreAutor.Replace("'", "\\'")}';
                document.getElementById('apellidoAutor').value = '{autor.ApellidoAutor.Replace("'", "\\'")}';
                document.getElementById('nacionalidad').value = '{autor.Nacionalidad.Replace("'", "\\'")}';
                document.getElementById('fechaNacimiento').value = '{autor.FechaNacimiento:yyyy-MM-dd}';
            ";

                    if (autor.FechaDefuncion.HasValue)
                    {
                        script += $"document.getElementById('fechaDefuncion').value = '{autor.FechaDefuncion.Value:yyyy-MM-dd}';";
                    }

                    ClientScript.RegisterStartupScript(this.GetType(), "cargarDatos", script, true);
                }
                else
                {
                    MostrarAlerta("No se pudo cargar el autor");
                    Response.Redirect("G_Aut.aspx");
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error al cargar datos: " + ex.Message);
                Response.Redirect("G_Aut.aspx");
            }
        }

        protected void btnGuardarAutor_Click(object sender, EventArgs e)
        {
            // Verificar si es modo CREAR o EDITAR
            string idAutorStr = Request.Form["idAutor"];

            if (!string.IsNullOrEmpty(idAutorStr) && int.TryParse(idAutorStr, out int idAutor) && idAutor > 0)
            {
                // Modo EDITAR
                ActualizarAutor(idAutor);
            }
            else
            {
                // Modo CREAR
                CrearAutor();
            }
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

        private void ActualizarAutor(int idAutor)
        {
            string nombreAutor = Request.Form["nombreAutor"];
            string apellidoAutor = Request.Form["apellidoAutor"];
            string nacionalidad = Request.Form["nacionalidad"];
            string fechaNacimientoStr = Request.Form["fechaNacimiento"];
            string fechaDefuncionStr = Request.Form["fechaDefuncion"];

            // Validaciones frontend (mismas que CrearAutor)
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
                Autor autorActualizado = new Autor
                {
                    ID_Autor = idAutor,
                    NombreAutor = nombreAutor.Trim(),
                    ApellidoAutor = apellidoAutor.Trim(),
                    Nacionalidad = nacionalidad.Trim(),
                    FechaNacimiento = fechaNacimiento,
                    FechaDefuncion = fechaDefuncion
                };

                var respuesta = cliente.ActualizarAutor(autorActualizado);

                if (respuesta.CodigoError == 1)
                {
                    // Autor actualizado exitosamente
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "redirect",
                        "alert('" + respuesta.Mensaje.Replace("'", "\\'") + "'); window.location='G_Aut.aspx';",
                        true
                    );
                }
                else
                {
                    // Error al actualizar
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
