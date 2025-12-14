function descargarArchivo(rutaFisica) {

    fetch('DownloadHandler.ashx?ruta=' + encodeURIComponent(rutaFisica))
        .then(response => {
            if (!response.ok) {
                throw new Error("Error en la descarga");
            }
            return response.blob();
        })
        .then(blob => {

            // Obtener nombre del archivo desde la ruta
            const nombreArchivo = rutaFisica.split('/').pop();

            const url = window.URL.createObjectURL(blob);
            const a = document.createElement("a");
            a.href = url;
            a.download = nombreArchivo;

            document.body.appendChild(a);
            a.click();

            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);
        })
        .catch(error => {
            alert("Error: archivo no existe o no se pudo descargar");
            console.error(error);
        });
}
