<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="G_Doc.aspx.cs" Inherits="proyectoFrontend_V3.G_Doc" %>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Gestionar Documentos</title>
    <link rel="stylesheet" href="/Estilos/Styles.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
</head>
<body>
    <div class="topbar">
        <div class="menu-container">
            <button class="btn-menu" onclick="toggleDropdown()">☰</button>
            <div class="dropdown-menu" id="dropdownMenu">
                <div class="dropdown-header">
                    <p class="profile-name">Nombre Usuario</p>
                    <p class="profile-email">correo@ejemplo.com</p>
                </div>
                <button class="dropdown-item" onclick="window.location.href='Perfil.aspx'">
                    Mi Perfil
                </button>
                <button class="dropdown-item dropdown-logout" onclick="cerrarSesion()">
                    Cerrar Sesión
                </button>
            </div>
        </div>
        <h1 class="topbar-title">Gestionar Documentos</h1>
    </div>
    
    <!-- Filtros -->
    <div class="filter-section" style="padding: 20px; background: white; margin: 20px;">
        <label for="filtroCategoria">Filtrar por Categoría:</label>
        <select id="filtroCategoria">
            <option value="">Cargando categorías...</option>
        </select>
    </div>
    
    <div class="main-content">
        <table class="table-docs">
            <thead>
                <tr>
                    <th>Título</th>
                    <th>Categoría</th>
                    <th>Usuario</th>
                    <th>Acciones</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td colspan="4" style="text-align: center;">Cargando documentos...</td>
                </tr>
            </tbody>
        </table>
    </div>
    
    <button class="fab-add" onclick="window.location.href='Form_Doc.aspx'">+</button>
    
    <script>
        'use strict';

        $(function () {
            $.ajaxSetup({ xhrFields: { withCredentials: true } });

            const $tbl = $(".table-docs tbody");

            // ===================== FUNCIONES AUXILIARES =====================
            function escapeHtml(text) {
                if (text == null) return "";
                return $('<div />').text(text).html();
            }

            function showAlert(msg, type) {
                alert(msg);
            }

            // ===================== CARGAR CATEGORÍAS =====================
            function cargarCategorias() {
                $.ajax({
                    type: "POST",
                    url: "/WS_Users.asmx/ObtenerCategorias", // Ajusta la ruta según tu proyecto
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        const categorias = response.d.Categorias;
                        const select = $('#filtroCategoria');

                        select.empty();
                        select.append('<option value="">Todas las categorías</option>');

                        if (categorias && categorias.length > 0) {
                            $.each(categorias, function (i, cat) {
                                select.append(
                                    $('<option></option>')
                                        .val(cat.ID_Categoria)
                                        .text(cat.NombreCategoria)
                                );
                            });
                        } else {
                            select.append('<option value="">No hay categorías disponibles</option>');
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error("Error cargando categorías:", error);
                        const select = $('#filtroCategoria');
                        select.empty();
                        select.append('<option value="">Error al cargar categorías</option>');
                    }
                });
            }

            // ===================== CARGAR DOCUMENTOS =====================
            function loadDocumentos(idCategoria) {
                let url = '/ListHandler.ashx';

                if (idCategoria) {
                    url += '?categoria=' + idCategoria;
                }

                $.ajax({
                    url: url,
                    method: 'GET',
                    dataType: 'json',
                    success: function (documentos) {
                        console.log("Documentos cargados:", documentos);
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
                    $tbl.append('<tr><td colspan="4" style="text-align: center;">No hay documentos</td></tr>');
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

            // ===================== FILTRAR POR CATEGORÍA =====================
            $('#filtroCategoria').on('change', function () {
                const idCategoria = $(this).val();
                loadDocumentos(idCategoria);
            });

            // ===================== DESCARGAR DOCUMENTO =====================
            $tbl.on('click', '.btn-download', function () {
                const id = $(this).data('id');
                window.open('/DownloadHandler.ashx?id=' + id, '_blank');
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
                    url: '/DeleteHandler.ashx?id=' + id,
                    method: 'GET',
                    dataType: 'json',
                    success: function (res) {
                        if (res && res.mensaje) {
                            showAlert(res.mensaje);
                        } else {
                            showAlert("Documento eliminado correctamente");
                        }
                        loadDocumentos(); // Recargar la tabla
                    },
                    error: function (xhr) {
                        const msg = xhr.responseJSON && xhr.responseJSON.error
                            ? xhr.responseJSON.error
                            : 'Error al eliminar';
                        showAlert("Error: " + msg);
                    }
                });
            });

            // ===================== CARGAR AL INICIO =====================
            cargarCategorias(); // Cargar categorías primero
            loadDocumentos();   // Luego cargar documentos
        });

        // ===================== FUNCIONES DEL MENÚ =====================
        function toggleDropdown() {
            document.getElementById("dropdownMenu").classList.toggle("show");
        }

        window.onclick = function (event) {
            if (!event.target.matches('.btn-menu')) {
                var dropdowns = document.getElementsByClassName("dropdown-menu");
                for (var i = 0; i < dropdowns.length; i++) {
                    var openDropdown = dropdowns[i];
                    if (openDropdown.classList.contains('show')) {
                        openDropdown.classList.remove('show');
                    }
                }
            }
        }

        function eraseCookie(name) {
            document.cookie = `${encodeURIComponent(
                name
            )}=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/; SameSite=Lax`;
        }

        function cerrarSesion() {
            eraseCookie('recordarUsuario');
            eraseCookie('recordarPassword');
            window.location.href = 'Login.aspx';
        }
    </script>
</body>
</html>