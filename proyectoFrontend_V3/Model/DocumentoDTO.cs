using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyectoFrontend_V3.Model
{
    public class DocumentoDTO
    {
        public int ID_Documento { get; set; }
        public string Titulo { get; set; }
        public string Foto { get; set; }
        public int ID_Categoria { get; set; }
        public string NombreCategoria { get; set; }
        public string RutaFisica { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public int ID_UsuarioSubida { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime FechaSubida { get; set; }
    }
}