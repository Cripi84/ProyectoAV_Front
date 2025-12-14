<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="G_Doc.aspx.cs" Inherits="proyectoFrontend_V3.G_Doc" %>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Gestionar Documentos</title>
    <link rel="stylesheet" href="/Estilos/Styles.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="js/Cookies.js"></script>
    <script src="js/Funciones.js"></script>
   <%-- <script src="js/Documentos.js"></script>--%>
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
        <select id="filtroCategoria" onchange="filtrarPorCategoria()">
            <option value="">Todas las categorías</option>
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
                <asp:Literal ID="tbody" runat="server"></asp:Literal>
            </tbody>
        </table>
    </div>
    <button class="fab-add" onclick="window.location.href='Form_Doc.aspx'">+</button>
</body>
</html>