<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="G_Cat.aspx.cs" Inherits="proyectoFrontend_V3.G_Cat" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Gestionar Categorías</title>
    <link rel="stylesheet" href="\Estilos\Styles.css">
    <script src="js/Cookies.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="topbar">
            <div class="menu-container">
                <button type="button" class="btn-menu" onclick="toggleDropdown()">☰</button>
                <div class="dropdown-menu" id="dropdownMenu">
                    <div class="dropdown-header">
                        <p class="profile-name">
                            <asp:Label ID="lblUsuarioNombre" runat="server"></asp:Label>
                        </p>
                        <p class="profile-email">
                            <asp:Label ID="lblUsuarioEmail" runat="server"></asp:Label>
                        </p>
                    </div>
                    <button type="button" class="dropdown-item" onclick="window.location.href='Perfil.aspx'">
                        Mi Perfil
                    </button>
                    <button class="dropdown-item dropdown-logout" onclick="cerrarSesion()">
                    Cerrar Sesión
                </button>
                </div>
            </div>
            <h1 class="topbar-title">Categorías</h1>
        </div>
        
        <div class="main-content">
            <table class="table-categories">
                <thead>
                    <tr>
                        <th>Nombre</th>
                        <th>Acciones</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptCategorias" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("NombreCategoria") %></td>
                                <td>
                                    <button type="button" class="btn-edit" onclick="editarCategoria(<%# Eval("ID_Categoria") %>)">
                                        Editar
                                    </button>
                                    <button type="button" class="btn-delete" onclick="eliminarCategoria(<%# Eval("ID_Categoria") %>, '<%# Eval("NombreCategoria") %>')">
                                        Eliminar
                                    </button>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    
                    <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-vacio" Visible="false"></asp:Label>
                </tbody>
            </table>
        </div>
        
        <button type="button" class="fab-add" onclick="window.location.href='Form_Cat.aspx'">+</button>
    </form>
    
    <script>
        function toggleDropdown() {
            document.getElementById("dropdownMenu").classList.toggle("show");
        }

        window.onclick = function (event) {
            if (!event.target.matches('.btn-menu')) {
                var dropdowns = document.getElementsByClassName("dropdown-menu");
                for (var i = 0; i < dropdowns.length; i++) {
                    var openDropdown = dropdowns[i];
                    if (openDropdown.classList.contains('show')) {
                        openDropdown.classList.remove('show');
                    }
                }
            }
        }

        function editarCategoria(id) {
            window.location.href = 'Form_Cat.aspx?id=' + id;
        }

        function eliminarCategoria(id, nombre) {
            if (confirm('¿Está seguro que desea eliminar la categoría "' + nombre + '"?\n\nEsta acción NO se puede deshacer.\nSi hay documentos asociados, no se podrá eliminar.')) {
                window.location.href = 'G_Cat.aspx?accion=eliminar&id=' + id;
            }
        }
    </script>
</body>
</html>
