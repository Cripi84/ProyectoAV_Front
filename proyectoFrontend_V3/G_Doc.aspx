<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="G_Doc.aspx.cs" Inherits="proyectoFrontend_V3.G_Doc" %>


<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Gestionar Documentos</title>
    <link rel="stylesheet" href="\Estilos\Styles.css">
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
                <!-- Se llena dinámicamente con JavaScript -->
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
