using proyectoFrontend_V3.ServicioUsuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace proyectoFrontend_V3
{
    public partial class G_Cat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verificar si viene de eliminar
                if (Request.QueryString["accion"] == "eliminar")
                {
                    int idCategoria;
                    if (int.TryParse(Request.QueryString["id"], out idCategoria))
                    {
                        EliminarCategoria(idCategoria);
                    }
                }

                // Mostrar datos del usuario logueado (opcional)
                if (Session["UsuarioNombre"] != null)
                {
                    lblUsuarioNombre.Text = Session["UsuarioNombre"].ToString() + " " + Session["UsuarioApellido"]?.ToString();
                }
                if (Session["UsuarioEmail"] != null)
                {
                    lblUsuarioEmail.Text = Session["UsuarioEmail"].ToString();
                }

                // Cargar categorías
                //CargarCategorias();
            }
        }

        private void EliminarCategoria(int idCategoria)
        {
            try
            {
                var cliente = new WS_UsersSoapClient();
                var respuesta = cliente.EliminarCategoria(idCategoria);

                if (respuesta.CodigoError == 1)
                {
                    // Eliminación exitosa
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "success",
                        "alert('" + respuesta.Mensaje.Replace("'", "\\'") + "');",
                        true
                    );
                }
                else
                {
                    // Error al eliminar (probablemente hay documentos asociados)
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "error",
                        "alert('" + respuesta.Mensaje.Replace("'", "\\'") + "');",
                        true
                    );
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "error",
                    "alert('Error: " + ex.Message.Replace("'", "\\'") + "');",
                    true
                );
            }
        }

        //private void CargarCategorias()
        //{
        //    try
        //    {
        //        var cliente = new WS_UsersSoapClient();
        //        var respuesta = cliente.ObtenerCategorias();

        //        if (respuesta.CodigoError == 1 && respuesta.Categorias != null && respuesta.Categorias.Length > 0)
        //        {
        //            // Enlazar los datos al Repeater
        //            rptCategorias.DataSource = respuesta.Categorias;
        //            rptCategorias.DataBind();

        //            lblMensaje.Visible = false;
        //        }
        //        else
        //        {
        //            // No hay categorías
        //            rptCategorias.DataSource = null;
        //            rptCategorias.DataBind();

        //            lblMensaje.Text = "No hay categorías registradas";
        //            lblMensaje.Visible = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMensaje.Text = "Error al cargar categorías: " + ex.Message;
        //        lblMensaje.Visible = true;
        //    }
        //}
    }
}