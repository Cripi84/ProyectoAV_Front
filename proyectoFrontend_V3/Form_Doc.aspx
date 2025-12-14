<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Form_Doc.aspx.cs" Inherits="proyectoFrontend_V3.Form_Doc" %>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Documento</title>
    <link rel="stylesheet" href="/Estilos/Styles.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
</head>
<body>
    <form id="formDocumento" runat="server">
    <div class="login-container">
        <div class="login-box">
            <h1 class="login-title">Gestión de Documento</h1>
            <div class="login-form">
                <label class="login-label">Título</label>
                <input type="text" id="txtTitulo" name="titulo" class="login-input" required />
                
                <label class="login-label">Categoría</label>
                <asp:DropDownList ID="ddlCategoria" runat="server"
                                  CssClass="login-input"
                                  ClientIDMode="Static"
                                  name="ddlCategoria"
                                  required="required">
                </asp:DropDownList>
                
                <label class="login-label">Autor</label>
                <asp:DropDownList ID="ddlAutor" runat="server"
                                  CssClass="login-input"
                                  ClientIDMode="Static"
                                  name="ddlAutor"
                                  required="required">
                </asp:DropDownList>
                
                <label class="login-label">Sinopsis</label>
                <textarea id="txtSinopsis" name="sinopsis" class="login-input" rows="4" required></textarea>
                
                <label class="login-label">Fecha de publicación</label>
                <input type="date" id="txtFechaPublicacion" name="fechaPublicacion" class="login-input" required />
                
                <label class="login-label">Imagen de portada <span style="color:red;">*</span></label>
                <input type="file" id="archivoFoto" name="archivoFoto" class="login-input" 
                       accept=".jpg,.jpeg,.png,.gif,.bmp" required />
                
                <label class="login-label">Archivo del documento <span style="color:red;">*</span></label>
                <input type="file" id="archivoDoc" name="archivoDoc" class="login-input"
                       accept=".pdf,.doc,.docx,.txt" required />
                
                <button type="button" id="btnGuardar" class="btn-login">
                    Guardar Documento
                </button>
                <button type="button" class="btn-secondary"
                        onclick="window.location.href='G_Doc.aspx'">
                    Cancelar
                </button>
                
                <div id="mensaje" style="margin-top: 15px; display: none;"></div>
            </div>
        </div>
    </div>
    </form>

    <script>
        $(document).ready(function () {
            var today = new Date().toISOString().split('T')[0];
            $('#txtFechaPublicacion').attr('max', today);

            $("#btnGuardar").on('click', function (e) {
                e.preventDefault();

                const archivoDoc = document.getElementById('archivoDoc');
                const archivoFoto = document.getElementById('archivoFoto');

                if (!archivoDoc.files || archivoDoc.files.length === 0) {
                    alert('Selecciona un archivo de documento.');
                    return;
                }
                if (!archivoFoto.files || archivoFoto.files.length === 0) {
                    alert('Selecciona una imagen de portada.');
                    return;
                }

                const fd = new FormData();
                fd.append('titulo', $("#txtTitulo").val());
                fd.append('ddlCategoria', $("#ddlCategoria").val());
                fd.append('ddlAutor', $("#ddlAutor").val());
                fd.append('sinopsis', $("#txtSinopsis").val());
                fd.append('fechaPublicacion', $("#txtFechaPublicacion").val());
                fd.append('archivoDoc', archivoDoc.files[0]);
                fd.append('archivoFoto', archivoFoto.files[0]);

                $("#btnGuardar").prop('disabled', true).text('Guardando...');

                fetch('UploadHandler.ashx', { method: 'POST', body: fd, credentials: 'same-origin' })
                    .then(r => {
                        if (!r.ok) throw new Error('HTTP ' + r.status);
                        return r.json();
                    })
                    .then(res => {
                        if (res && res.success) {
                            alert(res.mensaje || 'Documento guardado correctamente');
                            window.location.href = 'G_Doc.aspx';
                        } else {
                            alert('Error: ' + (res && res.error ? res.error : 'Error desconocido'));
                            $("#btnGuardar").prop('disabled', false).text('Guardar Documento');
                        }
                    })
                    .catch(err => {
                        alert('Error al guardar el documento');
                        console.error(err);
                        $("#btnGuardar").prop('disabled', false).text('Guardar Documento');
                    });
            });
        });
    </script>

    <style>
        .btn-login:disabled {
            opacity: 0.6;
            cursor: not-allowed;
        }
    </style>
</body>
</html>