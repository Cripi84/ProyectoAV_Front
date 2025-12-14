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

    // ===================== CARGAR CATEGORÍAS PARA FILTRO =====================
    //function cargarCategoriasParaFiltro() {
    //    $.ajax({
    //        url: "/WS_Users.asmx/ObtenerCategorias",
    //        method: "POST",
    //        data: "{}",
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        success: function (resp) {
    //            const categorias = resp.d.Categorias;
    //            const $sel = $("#filtroCategoria");

    //            $sel.empty();
    //            $sel.append('<option value="">Todas las categorías</option>');

    //            categorias.forEach(c => {
    //                $sel.append(`
    //                    <option value="${c.ID_Categoria}">
    //                        ${escapeHtml(c.NombreCategoria)}
    //                    </option>
    //                `);
    //            });
    //        },
    //        error: function (xhr) {
    //            console.error("Error cargando categorías para filtro", xhr.responseText);
    //        }
    //    });
    //}

    // ===================== CARGAR DOCUMENTOS =====================
    function loadDocumentos() {
        const url = 'G_Doc/ListHandler.ashx';
        $.ajax({
            url: url,
            method: 'GET',
            dataType: 'json',
            success: function (documentos) {
                console.log("Respuesta del servidor:", documentos);
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
                    <button class="btn-edit" data-id="${doc.ID_Documento}">
                        Modificar
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
            url: 'DeleteHandler.ashx?id=' + id,
            method: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res && res.mensaje) {
                    showAlert(res.mensaje);
                } else {
                    showAlert("Documento eliminado");
                }
                loadDocumentos();
            },
            error: function (xhr) {
                const msg = xhr.responseJSON && xhr.responseJSON.error
                    ? xhr.responseJSON.error
                    : 'Error al eliminar';
                showAlert("Error: " + msg);
            }
        });
    });

    // ===================== FILTRAR POR CATEGORÍA =====================
    window.filtrarPorCategoria = function () {
        const idCategoria = $('#filtroCategoria').val();
        loadDocumentos(idCategoria || null);
    };

    // ===================== CARGAR AL INICIO =====================
    //cargarCategoriasParaFiltro();
    loadDocumentos();

    // *** IMPORTANTE: Cargar categorías y autores si estamos en Form_Doc.aspx ***
    if ($('#categoria').length > 0) {
        cargarCategorias();
    }
    if ($('#autor').length > 0) {
        cargarAutores();
    }
});

// ===================== FUNCIONES PARA Form_Doc.aspx =====================
function cargarCategorias() {
    console.log("Cargando categorías para formulario...");

    $.ajax({
        url: "/WS_Users.asmx/ObtenerCategorias",
        method: "POST",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (resp) {
            console.log("Respuesta categorías:", resp);

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
    console.log("Cargando autores para formulario...");

    $.ajax({
        url: "/WS_Users.asmx/ObtenerAutores",
        method: "POST",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (resp) {
            console.log("Respuesta autores:", resp);

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