<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home_User.aspx.cs" Inherits="proyectoFrontend_V3.Home_User" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Home Usuario</title>
    <link rel="stylesheet" href="\Estilos\Styles.css">
     <script src="js/Cookies.js"></script>
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
        <h1 class="topbar-title">Biblioteca Digital</h1>
    </div>
    
    <div class="main-content">
        <div class="search-bar">
            <input type="text" class="search-input" placeholder="Buscar documento...">
            <select class="search-category">
                <option value="">Todas las categorías</option>
                <option value="1">Categoría 1</option>
                <option value="2">Categoría 2</option>
            </select>
            <button class="btn-search">Buscar</button>
        </div>
        
        <h2 class="section-title">Documentos Disponibles</h2>
        
        <div class="document-list">
            <div class="document-card">
                <h3 class="document-title">Un Cuento</h3>
                <p class="document-category">Categoría: Autobiografía</p>
                <button class="btn-view" onclick="window.location.href='Perfil_Doc.aspx'">
                    Ver Detalle
                </button>
            </div>
            <div class="document-card">
                <h3 class="document-title">Historia de Vida</h3>
                <p class="document-category">Categoría: Biografía</p>
                <button class="btn-view" onclick="window.location.href='Perfil_Doc.aspx'">
                    Ver Detalle
                </button>
            </div>
            <div class="document-card">
                <h3 class="document-title">Memorias</h3>
                <p class="document-category">Categoría: Autobiografía</p>
                <button class="btn-view" onclick="window.location.href='Perfil_Doc.aspx'">
                    Ver Detalle
                </button>
            </div>
        </div>
    </div>
    
    <script>
        function toggleDropdown() {
            document.getElementById("dropdownMenu").classList.toggle("show");
        }

        // Cerrar el dropdown si se hace clic fuera de él
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

