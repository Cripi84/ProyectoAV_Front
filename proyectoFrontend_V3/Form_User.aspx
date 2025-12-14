<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Form_User.aspx.cs" Inherits="proyectoFrontend_V3.Form_User" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Editar Usuario</title>
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
            <h1 class="topbar-title">Editar Usuario</h1>
        </div>
        
        <div class="main-content">
            <div class="form-container">
                <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-info" Visible="false"></asp:Label>
                
                <asp:HiddenField ID="hfIdUsuario" runat="server" />
                
                <div class="form-group">
                    <label>Nombre:</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" required></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label>Apellido:</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" required></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label>Alias:</label>
                    <asp:TextBox ID="txtAlias" runat="server" CssClass="form-control" required></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label>Email:</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" required></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <label>Rol:</label>
                    <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-control">
                        <asp:ListItem Value="1">Administrador</asp:ListItem>
                        <asp:ListItem Value="2">Usuario</asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label>Nueva Contraseña (dejar en blanco para mantener la actual):</label>
                    <asp:TextBox ID="txtClave" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                </div>
                
                <div class="form-buttons">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cambios" 
                        CssClass="btn-primary" OnClick="btnGuardar_Click" />
                    <button type="button" class="btn-secondary" onclick="window.location.href='G_User.aspx'">Cancelar</button>
                </div>
            </div>
        </div>
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
