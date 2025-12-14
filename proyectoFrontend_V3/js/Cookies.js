// ===================== Utilidades de Cookies =====================
function setCookie(name, value, days = 7) {
    const d = new Date();
    d.setTime(d.getTime() + days * 24 * 60 * 60 * 1000);
    document.cookie = `${encodeURIComponent(name)}=${encodeURIComponent(
        value
    )}; expires=${d.toUTCString()}; path=/; SameSite=Lax`;
    // Si usas HTTPS, añade "; Secure" al final
}

function getCookie(name) {
    const key = encodeURIComponent(name) + "=";
    const parts = document.cookie.split("; ");
    for (const part of parts) {
        if (part.startsWith(key)) {
            return decodeURIComponent(part.substring(key.length));
        }
    }
    return null;
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


// ===================== Autorrelleno desde cookies =====================
function cargarCredencialesGuardadas() {
    const usuarioGuardado = getCookie("recordarUsuario");
    const passwordGuardada = getCookie("recordarPassword");

    if (usuarioGuardado) {
        document.getElementById("login").value = usuarioGuardado;
        document.getElementById("recordar").checked = true;
    }

    if (passwordGuardada) {
        document.getElementById("password").value = passwordGuardada;
    }
}

// ===================== Guardar credenciales al hacer submit =====================
function guardarCredencialesSiRecordar() {
    const recordar = document.getElementById("recordar").checked;
    const usuario = document.getElementById("login").value;
    const password = document.getElementById("password").value;

    if (recordar) {
        // Guardar en cookies por 7 días
        setCookie("recordarUsuario", usuario, 7);
        // ADVERTENCIA: Guardar contraseñas en cookies NO es seguro
        // En producción, considera solo guardar el usuario
        setCookie("recordarPassword", password, 7);
    } else {
        // Eliminar cookies si desmarcó "recordar"
        eraseCookie("recordarUsuario");
        eraseCookie("recordarPassword");
    }
}

// ===================== Función para limpiar cookies manualmente =====================
function limpiarCookiesRecordar() {
    eraseCookie("recordarUsuario");
    eraseCookie("recordarPassword");
    document.getElementById("login").value = "";
    document.getElementById("password").value = "";
    document.getElementById("recordar").checked = false;
    alert("Cookies de 'Recordar credenciales' eliminadas.");
}

// ===================== Inicialización =====================
document.addEventListener("DOMContentLoaded", function () {
    // Cargar credenciales guardadas al cargar la página
    cargarCredencialesGuardadas();

    // Interceptar el submit del formulario para guardar cookies
    const form = document.getElementById("form1");
    if (form) {
        form.addEventListener("submit", function (e) {
            // Guardar credenciales antes de enviar el form
            guardarCredencialesSiRecordar();
            // No prevenir el submit, dejar que ASP.NET lo maneje
        });
    }

    // Agregar evento al checkbox de recordar
    const checkRecordar = document.getElementById("recordar");
    if (checkRecordar) {
        checkRecordar.addEventListener("change", function () {
            if (!this.checked) {
                // Si desmarca, preguntar si quiere eliminar cookies
                if (getCookie("recordarUsuario")) {
                    if (confirm("¿Desea eliminar las credenciales guardadas?")) {
                        limpiarCookiesRecordar();
                    }
                }
            }
        });
    }
});
