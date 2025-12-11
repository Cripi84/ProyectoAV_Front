<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home_Management.aspx.cs" Inherits="proyectoFrontend_V3.Home_Management" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Administrador</title>
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
        <h1 class="topbar-title">Panel de Administración</h1>
    </div>
    
    <div class="main-content">
        <h2 class="section-title">Gestión del Sistema</h2>
        <div class="admin-options">
            <div class="admin-card" onclick="window.location.href='G_Doc.aspx'">
                <h3 class="admin-card-title">Gestionar Documentos</h3>
                <p class="admin-card-desc">Creación y administración de documentos.</p>
            </div>
            <div class="admin-card" onclick="window.location.href='G_Cat.aspx'">
                <h3 class="admin-card-title">Categorías</h3>
                <p class="admin-card-desc">Creación y administración de categorías.</p>
            </div>
            <div class="admin-card" onclick="window.location.href='G_Aut.aspx'">
                <h3 class="admin-card-title">Autores</h3>
                <p class="admin-card-desc">Creación y administración de autores.</p>
            </div>
            <div class="admin-card" onclick="window.location.href='G_User.aspx'">
                <h3 class="admin-card-title">Usuarios</h3>
                <p class="admin-card-desc">Administración de usuarios.</p>
            </div>
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

