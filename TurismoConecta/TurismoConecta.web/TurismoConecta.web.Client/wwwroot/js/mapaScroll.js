let mapa;

// Se llama una sola vez al cargar la página
export function iniciarMapa(centroColombia, zoomColombia) {
    mapa = L.map('mapa-scroll', {
        zoomControl: false,
        dragging: false,       // el usuario no mueve el mapa a mano, solo el scroll lo controla
        scrollWheelZoom: false
    }).setView(centroColombia, zoomColombia);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap'
    }).addTo(mapa);
}

// Se llama cada vez que cambia el progreso del scroll (0 = Colombia, 1 = totalmente en el municipio)
export function actualizarZoom(progreso, centroColombia, zoomColombia, centroDestino, zoomDestino) {
    if (!mapa) return;

    // Interpolación lineal simple entre el punto de Colombia y el punto del municipio
    const lat = centroColombia[0] + (centroDestino[0] - centroColombia[0]) * progreso;
    const lng = centroColombia[1] + (centroDestino[1] - centroColombia[1]) * progreso;
    const zoom = zoomColombia + (zoomDestino - zoomColombia) * progreso;

    mapa.setView([lat, lng], zoom, { animate: false }); // animate:false porque YA estamos animando con el scroll
}

// Detecta el progreso del scroll dentro del contenedor y llama de vuelta a Blazor
export function observarScroll(dotNetRef, idContenedor) {
    const contenedor = document.getElementById(idContenedor);

    window.addEventListener('scroll', () => {
        const rect = contenedor.getBoundingClientRect();
        const alturaTotal = contenedor.offsetHeight - window.innerHeight;
        // progreso: 0 cuando el contenedor empieza a verse, 1 cuando termina de hacer scroll dentro de él
        let progreso = -rect.top / alturaTotal;
        progreso = Math.max(0, Math.min(1, progreso)); // clamp entre 0 y 1

        dotNetRef.invokeMethodAsync('ActualizarProgreso', progreso);
    });
}