<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Form_Doc.aspx.cs" Inherits="proyectoFrontend_V3.Form_Doc" %>

<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Documento</title>
    <link rel="stylesheet" href="\Estilos\Styles.css">
    <script src="js/Documentos.js"></script>
</head>
<body>
    <form id="formDocumento" runat="server">
        <div class="login-container">
            <div class="login-box">
                <h1 class="login-title">Gestión de Documento</h1>
                <div class="login-form">
                    <label class="login-label">Título</label>
                    <input type="text" id="titulo" class="login-input" />
                    
                    <label class="login-label">Categoría</label>
                    <select id="categoria" class="login-input">
                        <option value="">Seleccione una categoría</option>
                    </select>
                    
                    <label class="login-label">Sinopsis</label>
                    <textarea id="sinopsis" class="login-input" rows="4"
                              placeholder="Breve descripción"></textarea>
                    
                    <label class="login-label">Fecha de publicación</label>
                    <input type="date" id="fechaPublicacion" class="login-input" />
                    
                    <label class="login-label">Imagen de portada</label>
                    <input type="file" id="fotoDocumento" class="login-input" accept="image/*" />
                    
                    <label class="login-label">Archivo del documento</label>
                    <input type="file" id="archivoDocumento" class="login-input"
                           accept=".pdf,.doc,.docx,.epub" />
                    
                    <!-- Cambiar a button type="button" para usar JavaScript -->
                    <button type="button" class="btn-login" onclick="guardarDocumento()">
                        Guardar Documento
                    </button>
                    
                    <button type="button" class="btn-secondary" onclick="window.location.href='G_Doc.aspx'">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </form>
</body>
</html>