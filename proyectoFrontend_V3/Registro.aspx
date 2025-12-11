<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="proyectoFrontend_V3.Registro" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Registro</title>
    <link rel="stylesheet" href="\Estilos\Styles.css">
</head>
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="login-box">
                <h1 class="login-title">Registro de Usuario</h1>
                <div class="login-form">
                    <label for="nombre" class="login-label">Nombre</label>
                    <input type="text" id="nombre" name="nombre" class="login-input" placeholder="Ingrese su nombre" required>
                    
                    <label for="apellido" class="login-label">Apellido</label>
                    <input type="text" id="apellido" name="apellido" class="login-input" placeholder="Ingrese su apellido" required>
                    
                    <label for="nickname" class="login-label">Nickname</label>
                    <input type="text" id="nickname" name="nickname" class="login-input" placeholder="Ingrese su nickname" required>
                    
                    <label for="correo" class="login-label">Correo</label>
                    <input type="email" id="correo" name="correo" class="login-input" placeholder="Ingrese su correo" required>
                    
                    <label for="password" class="login-label">Contraseña</label>
                    <input type="password" id="password" name="password" class="login-input" placeholder="Ingrese su contraseña" required>
                    
                    <asp:Button ID="btnRegistro" runat="server" CssClass="btn-login"
                        Text="Registrarse" OnClick="btnRegistro_Click" />
                </div>
                <p class="register-link">¿Ya tienes cuenta? <a href="LogIn.aspx">Inicia sesión aquí</a></p>
            </div>
        </div>
    </form>
</body>
</html>
