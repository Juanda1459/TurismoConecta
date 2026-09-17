window.TurismoConectaMapa = {
    mapa: null,
    municipios: [],
    referenciaDotNet: null,
    mapaInicializado: false,

    inicializar: function (municipios, referenciaDotNet) {
        this.municipios = municipios || [];
        this.referenciaDotNet = referenciaDotNet;

        if (this.mapaInicializado) {
            return;
        }

        const contenedor = document.getElementById("mapa-colombia");

        if (!contenedor) {
            return;
        }

        this.mapa = L.map(contenedor, {
            zoomControl: true,
            scrollWheelZoom: false,
            attributionControl: true
        });

        L.tileLayer(
            "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png",
            {
                maxZoom: 19,
                attribution:
                    '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
            }
        ).addTo(this.mapa);

        this.mapa.setView([4.5709, -74.2973], 5);

        this.mapa.on("zoomend", () => {
            const zoom = this.mapa.getZoom();

            if (zoom < 7) {
                this.actualizarZona("Colombia");
            } else if (zoom < 9) {
                this.actualizarZona("Boyacá");
            } else {
                this.actualizarZona("Municipios de Boyacá");
            }
        });

        this.mostrarMunicipios();
        this.mapaInicializado = true;
    },

    mostrarMunicipios: function () {
        this.municipios.forEach((municipio) => {
            const marcador = L.marker([
                municipio.latitud,
                municipio.longitud
            ]);

            marcador
                .addTo(this.mapa)
                .bindPopup(`
                    <div class="popup-municipio">
                        <strong>${municipio.nombre}</strong>
                        <button
                            onclick="window.TurismoConectaMapa.abrirMunicipio(${municipio.id})">
                            Ver municipio
                        </button>
                    </div>
                `);
        });
    },

    actualizarZona: function (zona) {
        if (this.referenciaDotNet) {
            this.referenciaDotNet.invokeMethodAsync(
                "ActualizarZonaMapa",
                zona
            );
        }
    },

    abrirMunicipio: function (idMunicipio) {
        if (this.referenciaDotNet) {
            this.referenciaDotNet.invokeMethodAsync(
                "AbrirMunicipioDesdeMapa",
                idMunicipio
            );
        }
    },

    acercarABoyaca: function () {
        if (!this.mapa) {
            return;
        }

        this.mapa.flyTo(
            [5.4545, -73.3625],
            8,
            {
                animate: true,
                duration: 2
            }
        );
    }
};

window.TurismoConectaMapa.activarScroll = function () {
    const seccion = document.querySelector(".mapa-story-section");

    if (!seccion) {
        return;
    }

    let acercamientoRealizado = false;

    const actualizarZoom = () => {
        const rect = seccion.getBoundingClientRect();
        const alturaVentana = window.innerHeight;

        const progreso = Math.max(
            0,
            Math.min(
                1,
                (alturaVentana - rect.top) /
                (alturaVentana + rect.height)
            )
        );

        const zoomColombia = 5;
        const zoomBoyaca = 8;

        const zoomActual =
            zoomColombia + (zoomBoyaca - zoomColombia) * progreso;

        if (window.TurismoConectaMapa.mapa) {
            window.TurismoConectaMapa.mapa.setZoom(
                zoomActual,
                {
                    animate: false
                }
            );

            const latitud =
                4.5709 + (5.4545 - 4.5709) * progreso;

            const longitud =
                -74.2973 + (-73.3625 + 74.2973) * progreso;

            window.TurismoConectaMapa.mapa.panTo(
                [latitud, longitud],
                {
                    animate: false
                }
            );
        }

        if (
            progreso > 0.55 &&
            !acercamientoRealizado
        ) {
            acercamientoRealizado = true;

            if (window.TurismoConectaMapa.mapa) {
                window.TurismoConectaMapa.mapa.flyTo(
                    [5.4545, -73.3625],
                    8,
                    {
                        animate: true,
                        duration: 1.5
                    }
                );
            }
        }
    };

    actualizarZoom();

    window.addEventListener(
        "scroll",
        actualizarZoom,
        {
            passive: true
        }
    );
};