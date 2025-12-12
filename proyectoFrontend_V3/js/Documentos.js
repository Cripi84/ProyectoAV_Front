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
            ? 'G_Doc.aspx/ListHandler?categoria=' + idCategoria
            : 'G_Doc.aspx/ListHandler.ashx';

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
        const idCategoria = $('#idCategoria').val();
        const nombreCategoria = $('#nombreCategoria').val().trim();
        const archivoDoc = $('#archivoDoc')[0].files[0];
        const archivoFoto = $('#archivoFoto')[0].files[0];

        if (!titulo || !idCategoria || !nombreCategoria || !archivoDoc) {
            showAlert('Por favor complete todos los campos requeridos');
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
            url: 'G_Doc.aspx/UploadHandler',
            method: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (res) {
                showAlert(res.mensaje || 'Documento subido exitosamente');
                $('#titulo').val('');
                $('#archivoDoc').val('');
                $('#archivoFoto').val('');
                window.location.href = 'G_Doc.aspx';
            },
            error: function (xhr) {
                const msg = xhr.responseJSON && xhr.responseJSON.error
                    ? xhr.responseJSON.error
                    : 'Error al subir documento';
                showAlert("Error: " + msg);
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
});