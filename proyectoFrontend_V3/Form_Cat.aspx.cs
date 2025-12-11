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
            if (!IsPostBack)
            {
                // Verificar si viene un ID por QueryString (modo edición)
                string idParam = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(idParam) && int.TryParse(idParam, out int idCategoria))
                {
                    // Modo EDITAR - cargar datos de la categoría
                    CargarDatosCategoria(idCategoria);
                }
            }
        }

        private void CargarDatosCategoria(int idCategoria)
        {
            try
            {
                var cliente = new WS_UsersSoapClient();
                var respuesta = cliente.ObtenerCategoriaPorID(idCategoria);

                if (respuesta.CodigoError == 1 && respuesta.Categorias != null && respuesta.Categorias.Length > 0)
                {
                    var categoria = respuesta.Categorias[0];

                    // Cargar datos en los campos usando JavaScript
                    string script = $@"
                document.getElementById('idCategoria').value = '{categoria.ID_Categoria}';
                document.getElementById('nombreCategoria').value = '{categoria.NombreCategoria.Replace("'", "\\'")}';
            ";

                    ClientScript.RegisterStartupScript(this.GetType(), "cargarDatos", script, true);
                }
                else
                {
                    MostrarAlerta("No se pudo cargar la categoría");
                    Response.Redirect("G_Cat.aspx");
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error al cargar datos: " + ex.Message);
                Response.Redirect("G_Cat.aspx");
            }
        }

        protected void btnGuardarCategoria_Click(object sender, EventArgs e)
        {
            // Verificar si es modo CREAR o EDITAR
            string idCategoriaStr = Request.Form["idCategoria"];

            if (!string.IsNullOrEmpty(idCategoriaStr) && int.TryParse(idCategoriaStr, out int idCategoria) && idCategoria > 0)
            {
                // Modo EDITAR
                ActualizarCategoria(idCategoria);
            }
            else
            {
                // Modo CREAR
                CrearCategoria();
            }
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
                var cliente = new WS_UsersSoapClient();
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

        private void ActualizarCategoria(int idCategoria)
        {
            string nombreCategoria = Request.Form["nombreCategoria"];

            // Validaciones frontend (mismas que CrearCategoria)
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
                var cliente = new WS_UsersSoapClient();
                Categoria categoriaActualizada = new Categoria
                {
                    ID_Categoria = idCategoria,
                    NombreCategoria = nombreCategoria.Trim()
                };

                var respuesta = cliente.ActualizarCategoria(categoriaActualizada);

                if (respuesta.CodigoError == 1)
                {
                    // Categoría actualizada exitosamente
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "redirect",
                        "alert('" + respuesta.Mensaje.Replace("'", "\\'") + "'); window.location='G_Cat.aspx';",
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
