using proyectoFrontend_V3.ServicioUsuarios;
using System;
using System.Web.UI.WebControls;

namespace proyectoFrontend_V3
{
    public partial class Form_Doc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCategorias();
                CargarAutores();
            }
        }

        private void CargarCategorias()
        {
            try
            {
                var cliente = new WS_UsersSoapClient();
                var respuesta = cliente.ObtenerCategorias();

                if (respuesta.CodigoError == 1 && respuesta.Categorias != null && respuesta.Categorias.Length > 0)
                {
                    ddlCategoria.DataSource = respuesta.Categorias;
                    ddlCategoria.DataTextField = "NombreCategoria";
                    ddlCategoria.DataValueField = "ID_Categoria";
                    ddlCategoria.DataBind();

                    // Agregar opción inicial
                    ddlCategoria.Items.Insert(0, new ListItem("Seleccione una categoría", ""));
                }
                else
                {
                    ddlCategoria.Items.Clear();
                    ddlCategoria.Items.Add(new ListItem("No hay categorías disponibles", ""));
                }
            }
            catch (Exception ex)
            {
                // Log o mostrar error
                ddlCategoria.Items.Clear();
                ddlCategoria.Items.Add(new ListItem("Error al cargar categorías", ""));
                // Opcional: mostrar mensaje de error en un label
                System.Diagnostics.Debug.WriteLine("Error cargando categorías: " + ex.Message);
            }
        }

        private void CargarAutores()
        {
            try
            {
                var cliente = new WS_UsersSoapClient();
                var respuesta = cliente.ObtenerAutores();

                if (respuesta.CodigoError == 1 && respuesta.Autores != null && respuesta.Autores.Length > 0)
                {
                    ddlAutor.DataSource = respuesta.Autores;
                    // Concatenar nombre y apellido
                    ddlAutor.DataTextField = "NombreAutor"; // Si tienes esta propiedad
                    // O si no existe, puedes usar el evento ItemDataBound
                    ddlAutor.DataValueField = "ID_Autor";
                    ddlAutor.DataBind();

                    // Agregar opción inicial
                    ddlAutor.Items.Insert(0, new ListItem("Seleccione un autor", ""));
                }
                else
                {
                    ddlAutor.Items.Clear();
                    ddlAutor.Items.Add(new ListItem("No hay autores disponibles", ""));
                }
            }
            catch (Exception ex)
            {
                ddlAutor.Items.Clear();
                ddlAutor.Items.Add(new ListItem("Error al cargar autores", ""));
                System.Diagnostics.Debug.WriteLine("Error cargando autores: " + ex.Message);
            }
        }
    }
}