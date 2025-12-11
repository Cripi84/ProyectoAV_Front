<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Perfil_Doc.aspx.cs" Inherits="proyectoFrontend_V3.Perfil_Doc" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Detalle del Documento</title>
    <link rel="stylesheet" href="\Estilos\Styles.css">
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
                <button class="dropdown-item dropdown-logout" onclick="window.location.href='LogIn.aspx'">
                    Cerrar Sesión
                </button>
            </div>
        </div>
        <h1 class="topbar-title">Detalle del Documento</h1>
    </div>
    
    <div class="main-content">
        <div class="detail-box">
            <h2 class="detail-title">El arte de la guerra</h2>
            <p class="detail-item"><strong>Categoría:</strong> Ficción</p>
            <p class="detail-item"><strong>Autor(es):</strong> Autor: Lucho Diaz, Colaboradores: Marc Casadó, Graham Hanssen.</p>
            <p class="detail-item"><strong>Sinópsis:</strong> Hello World!</p>
            <p class="detail-item"><strong>Publicación:</strong> 24/24/24</p>
            <button class="btn-download">Descargar Documento</button>
        </div>
    </div>
    
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
