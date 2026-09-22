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
            scrollWheelZoom: true,
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

            if (zoom < 6.5) {
                this.actualizarZona("Colombia");
            } else if (zoom < 9) {
                this.actualizarZona("Boyacá");
            } else {
                this.actualizarZona("Municipios de Boyacá");
            }
        });

        this.mostrarMunicipios();
        this.resaltarBoyaca();
        this.mapaInicializado = true;
    },

        marcadores: [],

    mostrarMunicipios: function () {
        this.marcadores.forEach((marcador) => this.mapa.removeLayer(marcador));
        this.marcadores = [];

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

            this.marcadores.push(marcador);
        });
    },

    /**
     * Descarga el GeoJSON de Colombia y dibuja un polígono dorado
     * sobre el departamento de Boyacá para resaltarlo en el mapa.
     */
    resaltarBoyaca: function () {
        const url =
            'https://gist.githubusercontent.com/john-guerra/43c7656821069d00dcbc/raw/' +
            'be6a6e239cd5b5b803c6e7c2ec405b793a9064dd/Colombia.geo.json';

        fetch(url)
            .then(function (r) { return r.json(); })
            .then(function (data) {
                // El GeoJSON de Colombia tiene la propiedad NOMBRE_DPT en mayúsculas
                const boyacaGeoJson = {
                    type: 'FeatureCollection',
                    features: data.features.filter(function (f) {
                        const nombre = (f.properties.NOMBRE_DPT || f.properties.name || '').toUpperCase();
                        return nombre.includes('BOYAC');
                    })
                };

                if (boyacaGeoJson.features.length === 0) {
                    console.warn('No se encontró el departamento de Boyacá en el GeoJSON.');
                    return;
                }

                L.geoJSON(boyacaGeoJson, {
                    style: {
                        color: '#D9A441',       // borde dorado (paleta del proyecto)
                        weight: 2.5,
                        fillColor: '#D9A441',   // relleno dorado
                        fillOpacity: 0.18,
                        dashArray: null
                    },
                    onEachFeature: function (feature, layer) {
                        layer.bindTooltip('Boyacá', {
                            permanent: false,
                            direction: 'center',
                            className: 'boyaca-tooltip'
                        });
                    }
                }).addTo(window.TurismoConectaMapa.mapa);
            })
            .catch(function (err) {
                console.warn('No se pudo cargar el GeoJSON de Boyacá:', err);
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
            [5.65, -73.30],
            8.5,
            {
                animate: true,
                duration: 2
            }
        );
        this.actualizarZona("Boyacá");
    }
};

window.TurismoConectaMapa.activarScroll = function () {
    let enBoyaca = false;
    let animando = false;

    const actualizarZoom = () => {
        if (!window.TurismoConectaMapa.mapa || animando) {
            return;
        }

        const scrollY = window.scrollY || window.pageYOffset;

        // Cuando el usuario baja haciendo scroll (> 80px), vuela con animación hacia Boyacá
        if (scrollY > 80 && !enBoyaca) {
            enBoyaca = true;
            animando = true;

            window.TurismoConectaMapa.mapa.flyTo([5.65, -73.30], 8.5, {
                animate: true,
                duration: 1.8
            });

            window.TurismoConectaMapa.actualizarZona("Boyacá");

            setTimeout(() => {
                animando = false;
            }, 1900);
        }
        // Cuando vuelve a subir al inicio (< 40px), regresa al mapa general de Colombia
        else if (scrollY < 40 && enBoyaca) {
            enBoyaca = false;
            animando = true;

            window.TurismoConectaMapa.mapa.flyTo([4.5709, -74.2973], 5.0, {
                animate: true,
                duration: 1.5
            });

            window.TurismoConectaMapa.actualizarZona("Colombia");

            setTimeout(() => {
                animando = false;
            }, 1600);
        }
    };

    window.addEventListener("scroll", actualizarZoom, { passive: true });

    // También al hacer clic en el indicador "Desliza para acercarte a Boyacá"
    const indicador = document.querySelector(".hero-mapa-indicador");
    if (indicador) {
        indicador.style.cursor = "pointer";
        indicador.addEventListener("click", () => {
            window.TurismoConectaMapa.acercarABoyaca();
        });
    }
};