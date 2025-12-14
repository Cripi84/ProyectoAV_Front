'use strict'

$(function () {
    $.ajaxSetup({ xhrFields: { withCredentials: true } });

    const $tbl = $(".table-docs tbody");

    function escapeHtml(text) {
        if (text == null) return "";
        return $('<div />').text(text).html();
    }

    function showAlert(msg, type) {
        alert(msg);
    }

    // ===================== CARGAR DOCUMENTOS =====================
    function loadDocumentos(idCategoria) {
        const url = idCategoria
            ? '/ListHandler?categoria=' + idCategoria
            : '/ListHandler.ashx';

        $.ajax({
            url: url,
            method: 'GET',
            dataType: 'json',
            success: function (documentos) {
                renderTable(documentos);
            },
            error: function (xhr, status, err) {
                console.error("LoadDocumentos ERROR", xhr.status, xhr.responseText);
                showAlert("Error al cargar documentos (" + xhr.status + ")");
                renderTable([]);
            }
        });
    }

    // ===================== RENDERIZAR TABLA =====================
    function renderTable(items) {
        $tbl.empty();
        if (!items || items.length === 0) {
            $tbl.append('<tr><td colspan="4" class="text-center">No hay documentos</td></tr>');
            return;
        }

        items.forEach(doc => {
            const tr = $('<tr>');
            tr.append(`<td>${escapeHtml(doc.Titulo)}</td>`);
            tr.append(`<td>${escapeHtml(doc.NombreCategoria)}</td>`);
            tr.append(`<td>${escapeHtml(doc.NombreUsuario)}</td>`);

            const actions = $(`
                <td>
                    <button class="btn-download" data-id="${doc.ID_Documento}" data-titulo="${escapeHtml(doc.Titulo)}">
                        Descargar
                    </button>
                    <button class="btn-edit" data-id="${doc.ID_Documento}">
                        Editar
                    </button>
                    <button class="btn-delete" data-id="${doc.ID_Documento}">
                        Eliminar
                    </button>
                </td>
            `);
            tr.append(actions);
            $tbl.append(tr);
        });
    }

    // ===================== DESCARGAR DOCUMENTO =====================
    $tbl.on('click', '.btn-download', function () {
        const id = $(this).data('id');
        window.open('DownloadHandler.ashx?id=' + id, '_blank');
    });

    // ===================== EDITAR DOCUMENTO =====================
    $tbl.on('click', '.btn-edit', function () {
        const id = $(this).data('id');
        window.location.href = 'Form_Doc.aspx?id=' + id;
    });

    // ===================== ELIMINAR DOCUMENTO =====================
    $tbl.on('click', '.btn-delete', function () {
        const id = $(this).data('id');
        if (!confirm('¿Eliminar este documento? Esta acción no se puede deshacer.')) return;

        $.ajax({
            url: 'G_Doc.aspx/DeleteHandler?id=' + id,
            method: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res && res.mensaje) {
                    showAlert(res.mensaje);
                    loadDocumentos();
                } else {
                    showAlert("Documento eliminado");
                    loadDocumentos();
                }
            },
            error: function (xhr) {
                const msg = xhr.responseJSON && xhr.responseJSON.error
                    ? xhr.responseJSON.error
                    : 'Error al eliminar';
                showAlert("Error: " + msg);
            }
        });
    });

    // ===================== SUBIR DOCUMENTO (Para Form_Doc.aspx) =====================
    window.subirDocumento = function () {

        const titulo = $('#titulo').val().trim();
        const idCategoria = $('#ddlCategoria').val(); // Cambiado a ddlCategoria
        const nombreCategoria = $("#ddlCategoria option:selected").text().trim(); // Cambiado a ddlCategoria
        const archivoDoc = $('#archivoDocumento')[0].files[0];
        const archivoFoto = $('#fotoDocumento')[0].files[0];

        if (!titulo || !idCategoria || !archivoDoc) {
            alert('Por favor complete todos los campos requeridos');
            return;
        }

        const formData = new FormData();
        formData.append('titulo', titulo);
        formData.append('idCategoria', idCategoria);
        formData.append('nombreCategoria', nombreCategoria);
        formData.append('archivoDoc', archivoDoc);

        if (archivoFoto) {
            formData.append('archivoFoto', archivoFoto);
        }

        $.ajax({
            url: 'UploadHandler.ashx',
            method: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (res) {
                alert(res.mensaje || 'Documento subido exitosamente');
                window.location.href = 'G_Doc.aspx';
            },
            error: function (xhr) {
                const msg = xhr.responseJSON?.error || 'Error al subir documento';
                alert("Error: " + msg);
            }
        });
    };

    // ===================== FILTRAR POR CATEGORÍA =====================
    window.filtrarPorCategoria = function () {
        const idCategoria = $('#filtroCategoria').val();
        loadDocumentos(idCategoria || null);
    };

    // ===================== CARGAR AL INICIO =====================
    loadDocumentos();

    // *** IMPORTANTE: Cargar categorías y autores si estamos en Form_Doc.aspx ***
    if ($('#categoria').length > 0) {
        cargarCategorias();
    }
    if ($('#autor').length > 0) {
        cargarAutores();
    }
});

function cargarCategorias() {
    console.log("Cargando categorías..."); // Para debug

    $.ajax({
        url: "/WS_Users.asmx/ObtenerCategorias",
        method: "POST",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (resp) {
            console.log("Respuesta categorías:", resp); // Para debug

            const categorias = resp.d.Categorias;
            const $sel = $("#categoria");

            $sel.empty();
            $sel.append('<option value="">Seleccione una categoría</option>');

            categorias.forEach(c => {
                $sel.append(`
                    <option value="${c.ID_Categoria}">
                        ${c.NombreCategoria}
                    </option>
                `);
            });

            console.log("Categorías cargadas:", categorias.length);
        },
        error: function (xhr) {
            console.error("Error cargando categorías", xhr.responseText);
            alert("Error al cargar categorías");
        }
    });
}

function cargarAutores() {
    console.log("Cargando autores..."); // Para debug

    $.ajax({
        url: "/WS_Users.asmx/ObtenerAutores",
        method: "POST",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (resp) {
            console.log("Respuesta autores:", resp); // Para debug

            const autores = resp.d.Autores;
            const $sel = $("#autor");

            $sel.empty();
            $sel.append('<option value="">Seleccione un autor</option>');

            autores.forEach(a => {
                $sel.append(`
                    <option value="${a.ID_Autor}">
                        ${a.NombreAutor} ${a.ApellidoAutor}
                    </option>
                `);
            });

            console.log("Autores cargados:", autores.length);
        },
        error: function (xhr) {
            console.error("Error cargando autores", xhr.responseText);
            alert("Error al cargar autores");
        }
    });
}