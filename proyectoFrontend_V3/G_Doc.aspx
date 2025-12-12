<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="G_Doc.aspx.cs" Inherits="proyectoFrontend_V3.G_Doc" %>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Gestionar Documentos</title>
    <link rel="stylesheet" href="\Estilos\Styles.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="js/Documentos.js"></script>
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
                <button class="dropdown-item dropdown-logout" onclick="window.location.href='Logout.aspx'">
                    Cerrar Sesión
                </button>
            </div>
        </div>
        <h1 class="topbar-title">Gestionar Documentos</h1>
    </div>
    
    <!-- Filtros (Opcional) -->
    <div class="filter-section" style="padding: 20px; background: white; margin: 20px;">
        <label for="filtroCategoria">Filtrar por Categoría:</label>
        <select id="filtroCategoria" onchange="filtrarPorCategoria()">
            <option value="">Todas las categorías</option>
            <option value="1">Categoría 1</option>
            <option value="2">Categoría 2</option>
            <option value="3">Categoría 3</option>
        </select>
    </div>
    
    <div class="main-content">
        <table class="table-docs">
            <thead>
                <tr>
                    <th>Título</th>
                    <th>Categoría</th>
                    <th>Autores</th>
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
    </script>
</body>
</html>