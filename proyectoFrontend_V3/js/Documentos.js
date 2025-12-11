'use strict';

const URL_BACKEND = 'https://localhost:44360/WS_Users.asmx/'
const HANDLERS_URL = 'https://localhost:44360/'

// CARGAR AL INICIAR LA PÁGINA
window.addEventListener('load', function () {
    // Si estamos en G_Doc.aspx
    if (document.querySelector('.table-docs')) {
        cargarDocumentos();
    }

    // Si estamos en Form_Doc.aspx
    if (document.getElementById('categoria')) {
        cargarCategorias();
    }
});

function cargarDocumentos() {
    fetch(HANDLERS_URL + 'ListHandler.ashx')
        .then(response => response.json())
        .then(documentos => {
            let tbody = document.querySelector('.table-docs tbody');
            tbody.innerHTML = '';

            if (documentos.length === 0) {
                tbody.innerHTML = '<tr><td colspan="4">No hay documentos registrados</td></tr>';
                return;
            }

            documentos.forEach(doc => {
                let tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${doc.Titulo}</td>
                    <td>${doc.NombreCategoria}</td>
                    <td>
                        <button class="btn-view" onclick="verAutores(${doc.ID_Documento})">Ver Autores</button>
                    </td>
                    <td>
                        <button class="btn-download" onclick="descargarDocumento(${doc.ID_Documento})">Descargar</button>
                        <button class="btn-edit" onclick="editarDocumento(${doc.ID_Documento})">Editar</button>
                        <button class="btn-delete" onclick="eliminarDocumento(${doc.ID_Documento}, '${doc.Titulo}')">Eliminar</button>
                    </td>
                `;
                tbody.appendChild(tr);
            });
        })
        .catch(error => {
            console.error('Error al cargar documentos:', error);
            alert('Error al cargar documentos' + error);
        });
}


// CARGAR CATEGORÍAS EN DROPDOWN (Form_Doc.aspx)

function cargarCategorias() {
    // Llamar a tu WS de categorías existente
    fetch(URL_BACKEND + 'ObtenerCategorias', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' }
    })
        .then(response => response.json())
        .then(data => {
            let categorias = data.d;
            let select = document.getElementById('categoria');
            select.innerHTML = '<option value="">Seleccione una categoría</option>';

            categorias.forEach(cat => {
                let option = document.createElement('option');
                option.value = cat.ID_Categoria;
                option.textContent = cat.NombreCategoria;
                option.setAttribute('data-nombre', cat.NombreCategoria);
                select.appendChild(option);
            });
        })
        .catch(error => {
            console.error('Error al cargar categorías:', error);
        });
}

// GUARDAR DOCUMENTO (Form_Doc.aspx)

function guardarDocumento() {
    let titulo = document.getElementById('titulo').value;
    let idCategoria = document.getElementById('categoria').value;
    let nombreCategoria = document.getElementById('categoria').selectedOptions[0].getAttribute('data-nombre');
    let sinopsis = document.getElementById('sinopsis').value;
    let fechaPublicacion = document.getElementById('fechaPublicacion').value;
    let fotoInput = document.getElementById('fotoDocumento');
    let archivoInput = document.getElementById('archivoDocumento');

    // Validaciones
    if (!titulo) {
        alert('Ingrese el título del documento');
        return;
    }
    if (!idCategoria) {
        alert('Seleccione una categoría');
        return;
    }
    if (!archivoInput.files[0]) {
        alert('Seleccione el archivo del documento');
        return;
    }

    // Crear FormData para enviar archivos
    let formData = new FormData();
    formData.append('titulo', titulo);
    formData.append('idCategoria', idCategoria);
    formData.append('nombreCategoria', nombreCategoria);
    formData.append('sinopsis', sinopsis);
    formData.append('fechaPublicacion', fechaPublicacion);

    // Agregar archivos
    if (archivoInput.files[0]) {
        formData.append('archivoDoc', archivoInput.files[0]);
    }
    if (fotoInput.files[0]) {
        formData.append('archivoFoto', fotoInput.files[0]);
    }

    // Subir archivos primero
    fetch(HANDLERS_URL + 'UploadHandler.ashx', {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            if (data.error) {
                alert('Error: ' + data.error);
                return;
            }

            // Ahora registrar en BD
            registrarEnBD(titulo, data.foto, idCategoria, data.rutaFisica, fechaPublicacion);
        })
        .catch(error => {
            console.error('Error al subir archivo:', error);
            alert('Error al subir archivo');
        });
}

// Registrar documento en base de datos
function registrarEnBD(titulo, foto, idCategoria, rutaFisica, fechaPublicacion) {
    let documento = {
        Titulo: titulo,
        Foto: foto,
        ID_Categoria: parseInt(idCategoria),
        RutaFisica: rutaFisica,
        FechaPublicacion: fechaPublicacion ? new Date(fechaPublicacion) : null,
        ID_UsuarioSubida: 1  // Se obtiene de la sesión en el backend
    };

    fetch(URL_BACKEND + 'InsertarDocumento', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ doc: documento })
    })
        .then(response => response.json())
        .then(data => {
            let idDocumento = data.d;
            alert('Documento registrado exitosamente');
            limpiarFormulario();
            setTimeout(() => {
                window.location.href = 'G_Doc.aspx';
            }, 1000);
        })
        .catch(error => {
            console.error('Error al registrar en BD:', error);
            alert('Error al registrar documento en BD');
        });
}


// DESCARGAR DOCUMENTO

function descargarDocumento(idDocumento) {
    window.location.href = HANDLERS_URL + 'DownloadHandler.ashx?id=' + idDocumento;
}


// ELIMINAR DOCUMENTO

function eliminarDocumento(idDocumento, titulo) {
    if (!confirm(`¿Está seguro de eliminar el documento "${titulo}"?`)) {
        return;
    }

    fetch(HANDLERS_URL + 'DeleteHandler.ashx?id=' + idDocumento, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(data => {
            if (data.error) {
                alert('Error: ' + data.error);
            } else {
                alert(data.mensaje);
                cargarDocumentos();  // Recargar lista
            }
        })
        .catch(error => {
            console.error('Error al eliminar:', error);
            alert('Error al eliminar documento');
        });
}


// EDITAR DOCUMENTO (Opcional)

function editarDocumento(idDocumento) {
    window.location.href = 'Form_Doc.aspx?id=' + idDocumento;
}


// VER AUTORES (Futuro)

function verAutores(idDocumento) {
    // Por implementar: mostrar modal con autores del documento
    alert('Funcionalidad de autores por implementar');
}


// LIMPIAR FORMULARIO
function limpiarFormulario() {
    document.getElementById('titulo').value = '';
    document.getElementById('categoria').value = '';
    document.getElementById('sinopsis').value = '';
    document.getElementById('fechaPublicacion').value = '';
    document.getElementById('fotoDocumento').value = '';
    document.getElementById('archivoDocumento').value = '';
}