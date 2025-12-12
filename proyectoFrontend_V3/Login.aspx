<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="proyectoFrontend_V3.login" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Iniciar Sesión</title>
    <link rel="stylesheet" href="\Estilos\Styles.css">
    <script src="js/Cookies.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="login-box">
                <h1 class="login-title">Iniciar Sesión</h1>
                <div class="login-form">
                    <label for="login" class="login-label">Usuario o Correo</label>
                    <input type="text" id="login" name="login" class="login-input" 
                           placeholder="Ingrese su usuario o correo" required>
                    
                    <label for="password" class="login-label">Contraseña</label>
                    <input type="password" id="password" name="password" class="login-input" 
                           placeholder="Ingrese su contraseña" required>
                    
                    <div class="remember-container">
                        <input type="checkbox" id="recordar" name="recordar" value="true">
                        <label for="recordar">Recordar credenciales</label>
                    </div>
                    
                    <asp:Button ID="btnLogin" runat="server" CssClass="btn-login"
                        Text="Iniciar Sesión" OnClick="btnLogin_Click" />
                </div>
                <p class="register-link">¿No tienes cuenta? <a href="Registro.aspx">Regístrate aquí</a></p>
            </div>
        </div>
    </form>
</body>
</html>
