<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Form_Cat.aspx.cs" Inherits="proyectoFrontend_V3.Form_Cat" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Categoría</title>
    <link rel="stylesheet" href="\Estilos\Styles.css">
</head>
<body>
    <form id="formCategoria" runat="server">
        <div class="login-container">
            <div class="login-box">
                <h1 class="login-title">Gestión de Categoría</h1>
                <div class="login-form">
                    <label for="nombreCategoria" class="login-label">Nombre de la categoría</label>
                    <input type="text" id="nombreCategoria" name="nombreCategoria" class="login-input"
                           placeholder="Ej. Ciencia ficción" required />
                    
                    <asp:Button ID="btnGuardarCategoria" runat="server"
                        CssClass="btn-login" Text="Guardar Categoría"
                        OnClick="btnGuardarCategoria_Click" />
                </div>
                <p class="register-link"><a href="G_Cat.aspx">Volver a categorías</a></p>
            </div>
        </div>
    </form>
</body>
</html>