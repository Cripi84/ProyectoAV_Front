<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Form_Autor.aspx.cs" Inherits="proyectoFrontend_V3.Form_Autor" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Autor</title>
    <link rel="stylesheet" href="\Estilos\Styles.css">
</head>
<body>
    <form id="formAutor" runat="server">
        <div class="login-container">
            <div class="login-box">
                <h1 class="login-title" id="titulo">Gestión de Autor</h1>
                <div class="login-form">
                    <!-- Campo oculto para ID (0 = nuevo, >0 = editar) -->
                    <input type="hidden" id="idAutor" name="idAutor" value="0" />
                    
                    <label for="nombreAutor" class="login-label">Nombre</label>
                    <input type="text" id="nombreAutor" name="nombreAutor" class="login-input" 
                           placeholder="Nombre del autor" required />
                    
                    <label for="apellidoAutor" class="login-label">Apellido</label>
                    <input type="text" id="apellidoAutor" name="apellidoAutor" class="login-input" 
                           placeholder="Apellido del autor" required />
                    
                    <label for="nacionalidad" class="login-label">Nacionalidad</label>
                    <input type="text" id="nacionalidad" name="nacionalidad" class="login-input" 
                           placeholder="Nacionalidad" required />
                    
                    <label for="fechaNacimiento" class="login-label">Fecha de nacimiento</label>
                    <input type="date" id="fechaNacimiento" name="fechaNacimiento" class="login-input" required />
                    
                    <label for="fechaDefuncion" class="login-label">Fecha de defunción (opcional)</label>
                    <input type="date" id="fechaDefuncion" name="fechaDefuncion" class="login-input" />
                    
                    <asp:Button ID="btnGuardarAutor" runat="server"
                        CssClass="btn-login" Text="Guardar Autor" 
                        OnClick="btnGuardarAutor_Click" />
                </div>
                <p class="register-link"><a href="G_Aut.aspx">Volver a autores</a></p>
            </div>
        </div>
    </form>
    
    <script>
        // Cambiar el título y texto del botón según el modo
        window.addEventListener('DOMContentLoaded', function() {
            const idAutor = document.getElementById('idAutor').value;
            const titulo = document.getElementById('titulo');
            const boton = document.getElementById('<%= btnGuardarAutor.ClientID %>');
            
            if (idAutor && idAutor !== '0') {
                titulo.textContent = 'Editar Autor';
                boton.value = 'Actualizar Autor';
            } else {
                titulo.textContent = 'Nuevo Autor';
                boton.value = 'Guardar Autor';
            }
        });
    </script>
</body>
</html>