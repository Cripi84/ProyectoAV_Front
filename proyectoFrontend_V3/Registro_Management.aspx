<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro_Management.aspx.cs" Inherits="proyectoFrontend_V3.Registro_Management" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Registro de Usuario - Administración</title>
    <link rel="stylesheet" href="\Estilos\Styles.css">
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="login-box">
                <h1 class="login-title">Registro de Usuario</h1>
                <div class="login-form">
                    
                    <label for="nombre" class="login-label">Nombre</label>
                    <input type="text" id="nombre" name="nombre" class="login-input" placeholder="Ingrese el nombre" required>
                    
                    <label for="apellido" class="login-label">Apellido</label>
                    <input type="text" id="apellido" name="apellido" class="login-input" placeholder="Ingrese el apellido" required>
                    
                    <label for="nickname" class="login-label">Nickname</label>
                    <input type="text" id="nickname" name="nickname" class="login-input" placeholder="Ingrese el nickname" required>
                    
                    <label for="correo" class="login-label">Correo</label>
                    <input type="email" id="correo" name="correo" class="login-input" placeholder="Ingrese el correo" required>
                    
                    <label for="password" class="login-label">Contraseña</label>
                    <input type="password" id="password" name="password" class="login-input" placeholder="Ingrese la contraseña" required>
                    
                    <label for="rol" class="login-label">Rol</label>
                    <select id="rol" name="rol" class="login-input" required>
                        <option value="">Seleccione un rol</option>
                        <option value="1">Administrador</option>
                        <option value="2">Bibliotecario</option>
                        <option value="3">Usuario</option>
                    </select>
                    
                    <label for="estado" class="login-label">Estado</label>
                    <select id="estado" name="estado" class="login-input" required>
                        <option value="">Seleccione un estado</option>
                        <option value="1">Activo</option>
                        <option value="2">Inactivo</option>
                    </select>
                    
                    <asp:Button ID="btnRegistrarAdmin" runat="server" CssClass="btn-login"
                        Text="Registrar Usuario" OnClick="btnRegistrarAdmin_Click" />
                </div>
                <p class="register-link"><a href="G_User.aspx">Volver</a></p>
            </div>
        </div>
    </form>
</body>
</html>