<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="G_User.aspx.cs" Inherits="proyectoFrontend_V3.G_User" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Gestionar Usuarios</title>
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
            <h1 class="topbar-title">Usuarios</h1>
        </div>
        
        <div class="main-content">
            <!-- Mensaje de feedback -->
            <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-info" Visible="false"></asp:Label>
            
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
                                    <button type="button" class="btn-edit" 
                                            onclick="window.location.href='Form_Usuer.aspx?id=<%# Eval("ID_Usuario") %>'">
                                        Editar
                                    </button>
                                    <asp:Button ID="btnCambiarEstado" runat="server" 
                                        CssClass='<%# Convert.ToInt32(Eval("ID_Estado")) == 1 ? "btn-delete" : "btn-activate" %>'
                                        Text='<%# Convert.ToInt32(Eval("ID_Estado")) == 1 ? "Desactivar" : "Activar" %>'
                                        CommandArgument='<%# Eval("ID_Usuario") + "," + Eval("ID_Estado") %>'
                                        OnClick="btnCambiarEstado_Click"
                                        OnClientClick="return confirm('¿Está seguro de cambiar el estado de este usuario?');" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
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
    </script>
</body>
</html>