using proyectoFrontend_V3.ServicioUsuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace proyectoFrontend_V3
{
    public partial class Form_Cat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnGuardarCategoria_Click(object sender, EventArgs e)
        {
            CrearCategoria();
        }

        private void CrearCategoria()
        {
            string nombreCategoria = Request.Form["nombreCategoria"];

            // Validación frontend
            if (string.IsNullOrWhiteSpace(nombreCategoria))
            {
                MostrarAlerta("Por favor ingrese el nombre de la categoría");
                return;
            }

            if (nombreCategoria.Length < 3)
            {
                MostrarAlerta("El nombre de la categoría debe tener al menos 3 caracteres");
                return;
            }

            if (nombreCategoria.Length > 100)
            {
                MostrarAlerta("El nombre de la categoría no puede exceder 100 caracteres");
                return;
            }

            try
            {
                var cliente = new WS_UsersSoapClient(); // O el cliente de tu servicio
                Categoria nuevaCategoria = new Categoria
                {
                    NombreCategoria = nombreCategoria.Trim()
                };

                var respuesta = cliente.CrearCategoria(nuevaCategoria);

                if (respuesta.CodigoError == 1)
                {
                    // Categoría creada exitosamente
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "redirect",
                        "alert('" + respuesta.Mensaje.Replace("'", "\\'") + "'); window.location='G_Cat.aspx';",
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