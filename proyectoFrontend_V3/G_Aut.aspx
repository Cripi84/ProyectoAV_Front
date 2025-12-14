<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="G_Aut.aspx.cs" Inherits="proyectoFrontend_V3.G_Aut" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Gestionar Autores</title>
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
                        <p class="profile-name">Nombre Usuario</p>
                        <p class="profile-email">correo@ejemplo.com</p>
                    </div>
                    <button type="button" class="dropdown-item" onclick="window.location.href='Perfil.aspx'">
                        Mi Perfil
                    </button>
                    <button class="dropdown-item dropdown-logout" onclick="cerrarSesion()">
                    Cerrar Sesión
                </button>
                </div>
            </div>
            <h1 class="topbar-title">Autores</h1>
        </div>
        
        <div class="main-content">
            <table class="table-authors">
                <thead>
                    <tr>
                        <th>Nombre</th>
                        <th>Nacionalidad</th>
                        <th>Acciones</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptAutores" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("NombreAutor") %> <%# Eval("ApellidoAutor") %></td>
                                <td><%# Eval("Nacionalidad") %></td>
                                <td>
                                    <button type="button" class="btn-edit" onclick="editarAutor(<%# Eval("ID_Autor") %>)">Editar</button>
                                    <button type="button" class="btn-delete" onclick="eliminarAutor(<%# Eval("ID_Autor") %>, '<%# Eval("NombreAutor") %> <%# Eval("ApellidoAutor") %>')">Eliminar</button>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    
                    <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-vacio" Visible="false"></asp:Label>
                </tbody>
            </table>
        </div>
        
        <button type="button" class="fab-add" onclick="window.location.href='Form_Autor.aspx'">+</button>
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

        function editarAutor(id) {
            window.location.href = 'Form_Autor.aspx?id=' + id;
        }

        function eliminarAutor(id, nombre) {
            if (confirm('¿Está seguro de eliminar al autor: ' + nombre + '?')) {
                window.location.href = 'G_Aut.aspx?action=delete&id=' + id;
            }
        }
    </script>
</body>
</html>