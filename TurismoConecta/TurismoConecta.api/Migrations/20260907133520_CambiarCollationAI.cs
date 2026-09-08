using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurismoConecta.api.Migrations
{
    /// <inheritdoc />
    public partial class CambiarCollationAI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    idCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    icono = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__8A3D240C145FC97D", x => x.idCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Departamento",
                columns: table => new
                {
                    idDepartamento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    pais = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "Colombia"),
                    descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Departam__C225F98D7F570F44", x => x.idDepartamento);
                });

            migrationBuilder.CreateTable(
                name: "Etiqueta",
                columns: table => new
                {
                    idEtiqueta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etiqueta", x => x.idEtiqueta);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    idRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Rol__3C872F76177276D9", x => x.idRol);
                });

            migrationBuilder.CreateTable(
                name: "TiposNotificacion",
                columns: table => new
                {
                    idTipoNotificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TiposNot__6F70CA63E933ECF1", x => x.idTipoNotificacion);
                });

            migrationBuilder.CreateTable(
                name: "Municipio",
                columns: table => new
                {
                    idMunicipio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idDepartamento = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    historia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    clima = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    fechasRelevantes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    imagenUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    latitud = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    longitud = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    fechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Municipi__FD10E40022C53579", x => x.idMunicipio);
                    table.ForeignKey(
                        name: "FK_Municipios_Departamentos",
                        column: x => x.idDepartamento,
                        principalTable: "Departamento",
                        principalColumn: "idDepartamento");
                });

            migrationBuilder.CreateTable(
                name: "MunicipioEtiqueta",
                columns: table => new
                {
                    idMunicipioEtiqueta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idMunicipio = table.Column<int>(type: "int", nullable: false),
                    idEtiqueta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipioEtiqueta", x => x.idMunicipioEtiqueta);
                    table.ForeignKey(
                        name: "FK_MunicipioEtiqueta_Etiqueta",
                        column: x => x.idEtiqueta,
                        principalTable: "Etiqueta",
                        principalColumn: "idEtiqueta");
                    table.ForeignKey(
                        name: "FK_MunicipioEtiqueta_Municipio",
                        column: x => x.idMunicipio,
                        principalTable: "Municipio",
                        principalColumn: "idMunicipio");
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idRol = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    passwordHash = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    municipioAsignadoId = table.Column<int>(type: "int", nullable: true),
                    fechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    emailConfirmado = table.Column<bool>(type: "bit", nullable: false),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    PasswordResetToken = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PasswordResetExpira = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuario__645723A6F4DEE701", x => x.idUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Municipios",
                        column: x => x.municipioAsignadoId,
                        principalTable: "Municipio",
                        principalColumn: "idMunicipio");
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles",
                        column: x => x.idRol,
                        principalTable: "Rol",
                        principalColumn: "idRol");
                });

            migrationBuilder.CreateTable(
                name: "Itinerario",
                columns: table => new
                {
                    idItinerario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    fechaInicio = table.Column<DateOnly>(type: "date", nullable: true),
                    fechaFin = table.Column<DateOnly>(type: "date", nullable: true),
                    compartido = table.Column<bool>(type: "bit", nullable: false),
                    codigoCompartir = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    fechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())"),
                    observaciones = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Itinerar__B201E2D6E29D1263", x => x.idItinerario);
                    table.ForeignKey(
                        name: "FK_Itinerarios_Usuarios",
                        column: x => x.idUsuario,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Negocio",
                columns: table => new
                {
                    idNegocio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idMunicipio = table.Column<int>(type: "int", nullable: false),
                    idCategoria = table.Column<int>(type: "int", nullable: false),
                    idUsuario = table.Column<int>(type: "int", nullable: true),
                    nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    direccion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    telefono = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    horario = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    imagenPrincipalUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    latitud = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    longitud = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pendiente")
                        .Annotation("Relational:DefaultConstraintName", "DF_Negocio_estado"),
                    fechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fechaAprobacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Negocio", x => x.idNegocio);
                    table.ForeignKey(
                        name: "FK_Negocio_Categoria",
                        column: x => x.idCategoria,
                        principalTable: "Categoria",
                        principalColumn: "idCategoria");
                    table.ForeignKey(
                        name: "FK_Negocio_Municipio",
                        column: x => x.idMunicipio,
                        principalTable: "Municipio",
                        principalColumn: "idMunicipio");
                    table.ForeignKey(
                        name: "FK_Negocio_Usuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Notificacion",
                columns: table => new
                {
                    idNotificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    idTipoNotificacion = table.Column<int>(type: "int", nullable: false),
                    mensaje = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    leida = table.Column<bool>(type: "bit", nullable: false),
                    fechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__AFE1D7E4960044C0", x => x.idNotificacion);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Tipos",
                        column: x => x.idTipoNotificacion,
                        principalTable: "TiposNotificacion",
                        principalColumn: "idTipoNotificacion");
                    table.ForeignKey(
                        name: "FK_Notificaciones_Usuarios",
                        column: x => x.idUsuario,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario");
                });

            migrationBuilder.CreateTable(
                name: "ItinerarioDetalle",
                columns: table => new
                {
                    idItinerarioDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idItinerario = table.Column<int>(type: "int", nullable: false),
                    idMunicipio = table.Column<int>(type: "int", nullable: false),
                    diaNumero = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    orden = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    fechaVisita = table.Column<DateOnly>(type: "date", nullable: true),
                    distanciaKm = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    tiempoEstimadoMin = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Itinerar__CCC310BCE5DA3F0B", x => x.idItinerarioDetalle);
                    table.ForeignKey(
                        name: "FK_ItinerarioDetalle_Itinerarios",
                        column: x => x.idItinerario,
                        principalTable: "Itinerario",
                        principalColumn: "idItinerario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItinerarioDetalle_Municipios",
                        column: x => x.idMunicipio,
                        principalTable: "Municipio",
                        principalColumn: "idMunicipio");
                });

            migrationBuilder.CreateTable(
                name: "Favorito",
                columns: table => new
                {
                    idFavorito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    idMunicipio = table.Column<int>(type: "int", nullable: true),
                    idNegocio = table.Column<int>(type: "int", nullable: true),
                    fechaGuardado = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Favorito_fechaGuardado")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorito", x => x.idFavorito);
                    table.ForeignKey(
                        name: "FK_Favorito_Municipio",
                        column: x => x.idMunicipio,
                        principalTable: "Municipio",
                        principalColumn: "idMunicipio");
                    table.ForeignKey(
                        name: "FK_Favorito_Negocio",
                        column: x => x.idNegocio,
                        principalTable: "Negocio",
                        principalColumn: "idNegocio");
                    table.ForeignKey(
                        name: "FK_Favorito_Usuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario");
                });

            migrationBuilder.CreateTable(
                name: "GaleriaNegocio",
                columns: table => new
                {
                    idGaleria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idNegocio = table.Column<int>(type: "int", nullable: false),
                    imagenUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GaleriaN__F48A5B5461E947ED", x => x.idGaleria);
                    table.ForeignKey(
                        name: "FK_GaleriaNegocio_Negocio",
                        column: x => x.idNegocio,
                        principalTable: "Negocio",
                        principalColumn: "idNegocio");
                });

            migrationBuilder.CreateTable(
                name: "NegocioEtiqueta",
                columns: table => new
                {
                    idNegocioEtiqueta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idEtiqueta = table.Column<int>(type: "int", nullable: false),
                    idNegocio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NegocioEtiqueta", x => x.idNegocioEtiqueta);
                    table.ForeignKey(
                        name: "FK_NegocioEtiqueta_Etiqueta",
                        column: x => x.idEtiqueta,
                        principalTable: "Etiqueta",
                        principalColumn: "idEtiqueta");
                    table.ForeignKey(
                        name: "FK_NegocioEtiqueta_Negocio",
                        column: x => x.idNegocio,
                        principalTable: "Negocio",
                        principalColumn: "idNegocio");
                });

            migrationBuilder.CreateTable(
                name: "Reseña",
                columns: table => new
                {
                    idReseña = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    idMunicipio = table.Column<int>(type: "int", nullable: true),
                    idNegocio = table.Column<int>(type: "int", nullable: true),
                    calificacion = table.Column<byte>(type: "tinyint", nullable: false),
                    comentario = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    respuesta = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    fechaRespuesta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    moderada = table.Column<bool>(type: "bit", nullable: false),
                    fechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reseña", x => x.idReseña);
                    table.ForeignKey(
                        name: "FK_Reseña_Municipio",
                        column: x => x.idMunicipio,
                        principalTable: "Municipio",
                        principalColumn: "idMunicipio");
                    table.ForeignKey(
                        name: "FK_Reseña_Negocio",
                        column: x => x.idNegocio,
                        principalTable: "Negocio",
                        principalColumn: "idNegocio");
                    table.ForeignKey(
                        name: "FK_Reseña_Usuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Categori__72AFBCC62B055AE0",
                table: "Categoria",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorito_idMunicipio",
                table: "Favorito",
                column: "idMunicipio");

            migrationBuilder.CreateIndex(
                name: "IX_Favorito_idNegocio",
                table: "Favorito",
                column: "idNegocio");

            migrationBuilder.CreateIndex(
                name: "IX_Favorito_idUsuario",
                table: "Favorito",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_GaleriaNegocio_idNegocio",
                table: "GaleriaNegocio",
                column: "idNegocio");

            migrationBuilder.CreateIndex(
                name: "IX_Itinerario_idUsuario",
                table: "Itinerario",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_ItinerarioDetalle_idItinerario",
                table: "ItinerarioDetalle",
                column: "idItinerario");

            migrationBuilder.CreateIndex(
                name: "IX_ItinerarioDetalle_idMunicipio",
                table: "ItinerarioDetalle",
                column: "idMunicipio");

            migrationBuilder.CreateIndex(
                name: "IX_Municipio_idDepartamento",
                table: "Municipio",
                column: "idDepartamento");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipioEtiqueta_idEtiqueta",
                table: "MunicipioEtiqueta",
                column: "idEtiqueta");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipioEtiqueta_idMunicipio",
                table: "MunicipioEtiqueta",
                column: "idMunicipio");

            migrationBuilder.CreateIndex(
                name: "IX_Negocio_idCategoria",
                table: "Negocio",
                column: "idCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Negocio_idMunicipio",
                table: "Negocio",
                column: "idMunicipio");

            migrationBuilder.CreateIndex(
                name: "IX_Negocio_idUsuario",
                table: "Negocio",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_NegocioEtiqueta_idEtiqueta",
                table: "NegocioEtiqueta",
                column: "idEtiqueta");

            migrationBuilder.CreateIndex(
                name: "IX_NegocioEtiqueta_idNegocio",
                table: "NegocioEtiqueta",
                column: "idNegocio");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacion_idTipoNotificacion",
                table: "Notificacion",
                column: "idTipoNotificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacion_idUsuario",
                table: "Notificacion",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Reseña_idMunicipio",
                table: "Reseña",
                column: "idMunicipio");

            migrationBuilder.CreateIndex(
                name: "IX_Reseña_idNegocio",
                table: "Reseña",
                column: "idNegocio");

            migrationBuilder.CreateIndex(
                name: "IX_Reseña_idUsuario",
                table: "Reseña",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "UQ__Rol__72AFBCC66E08C31C",
                table: "Rol",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__TiposNot__40F9A206DA8B2BA4",
                table: "TiposNotificacion",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_idRol",
                table: "Usuario",
                column: "idRol");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_municipioAsignadoId",
                table: "Usuario",
                column: "municipioAsignadoId");

            migrationBuilder.CreateIndex(
                name: "UQ__Usuario__AB6E6164279433C1",
                table: "Usuario",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorito");

            migrationBuilder.DropTable(
                name: "GaleriaNegocio");

            migrationBuilder.DropTable(
                name: "ItinerarioDetalle");

            migrationBuilder.DropTable(
                name: "MunicipioEtiqueta");

            migrationBuilder.DropTable(
                name: "NegocioEtiqueta");

            migrationBuilder.DropTable(
                name: "Notificacion");

            migrationBuilder.DropTable(
                name: "Reseña");

            migrationBuilder.DropTable(
                name: "Itinerario");

            migrationBuilder.DropTable(
                name: "Etiqueta");

            migrationBuilder.DropTable(
                name: "TiposNotificacion");

            migrationBuilder.DropTable(
                name: "Negocio");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Municipio");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "Departamento");
        }
    }
}
