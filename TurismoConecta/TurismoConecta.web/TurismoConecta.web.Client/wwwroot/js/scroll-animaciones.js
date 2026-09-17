window.activarAnimacionScroll = () => {
    const observador = new IntersectionObserver((entradas) => {
        entradas.forEach(entrada => {
            if (entrada.isIntersecting) {
                entrada.target.classList.add('visible');
                observador.unobserve(entrada.target);
            }
        });
    }, { threshold: 0.15 });

    document.querySelectorAll('.tarjeta-animada').forEach(el => observador.observe(el));
    window.activarZoomMapa = () => {
        const mapa = document.querySelector(".map-story");

        if (!mapa) {
            return;
        }

        const actualizarMapa = () => {
            const rect = mapa.getBoundingClientRect();
            const altura = window.innerHeight;

            const progreso = Math.max(
                0,
                Math.min(1, (altura - rect.top) / (altura + rect.height))
            );

            mapa.style.setProperty("--map-progress", progreso.toString());
        };

        actualizarMapa();

        window.addEventListener("scroll", actualizarMapa, {
            passive: true
        });
    };
};