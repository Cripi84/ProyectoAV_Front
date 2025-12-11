<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="G_User.aspx.cs" Inherits="proyectoFrontend_V3.G_User" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Gestionar Usuarios</title>
    <link rel="stylesheet" href="\Estilos\Styles.css">
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
                    <button type="button" class="dropdown-item dropdown-logout" onclick="window.location.href='CerrarSesion.aspx'">
                        Cerrar Sesión
                    </button>
                </div>
            </div>
            <h1 class="topbar-title">Usuarios</h1>
        </div>
        
        <div class="main-content">
            <table class="table-users">
                <thead>
                    <tr>
                        <th>Usuario</th>
                        <th>Email</th>
                        <th>Rol</th>
                        <th>Estado</th>
                        <th>Acciones</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptUsuarios" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("NombreUsuario") %> <%# Eval("ApellidoUsuario") %></td>
                                <td><%# Eval("Email") %></td>
                                <td><%# Eval("Rol") %></td>
                                <td>
                                    <span class='<%# Convert.ToInt32(Eval("ID_Estado")) == 1 ? "badge-activo" : "badge-inactivo" %>'>
                                        <%# Eval("Estado") %>
                                    </span>
                                </td>
                                <td>
                                    <button type="button" class="btn-edit" onclick="editarUsuario(<%# Eval("ID_Usuario") %>)">Editar</button>
                                    <button type="button" class="btn-delete" onclick="desactivarUsuario(<%# Eval("ID_Usuario") %>)">
                                        <%# Convert.ToInt32(Eval("ID_Estado")) == 1 ? "Desactivar" : "Activar" %>
                                    </button>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    
                    <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-vacio" Visible="false"></asp:Label>
                </tbody>
            </table>
        </div>
        
        <button type="button" class="fab-add" onclick="window.location.href='Registro_Management.aspx'">+</button>
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

        function editarUsuario(id) {
            alert('Editar usuario ID: ' + id + ' - Próximamente');
        }

        function desactivarUsuario(id) {
            alert('Cambiar estado usuario ID: ' + id + ' - Próximamente');
        }
    </script>
</body>
</html>