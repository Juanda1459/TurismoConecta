using System.Net.Http.Json;
using TurismoConecta.web.Client.Models;

namespace TurismoConecta.web.Client.Services
{
    public class UsuarioApiService
    {
        private readonly HttpClient _http;

        public UsuarioApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<UsuarioAdminItemDto>> ListarUsuariosAsync(CancellationToken ct = default)
        {
            try
            {
                var resultado = await _http.GetFromJsonAsync<List<UsuarioAdminItemDto>>("api/usuarios", ct);
                return resultado ?? new List<UsuarioAdminItemDto>();
            }
            catch
            {
                return new List<UsuarioAdminItemDto>();
            }
        }

        public async Task<List<RolInfo>> ListarRolesAsync(CancellationToken ct = default)
        {
            try
            {
                var resultado = await _http.GetFromJsonAsync<List<RolInfo>>("api/usuarios/roles", ct);
                return resultado ?? new List<RolInfo>();
            }
            catch
            {
                return new List<RolInfo>
                {
                    new() { Nombre = "Turista", Descripcion = "Viajero y explorador" },
                    new() { Nombre = "AdminComercio", Descripcion = "Dueño de negocio local" },
                    new() { Nombre = "AdminMunicipio", Descripcion = "Gestor turístico municipal" },
                    new() { Nombre = "AdminGeneral", Descripcion = "Superadministrador" }
                };
            }
        }

        public async Task<(bool Exito, string? Error)> CambiarRolAsync(int idUsuario, string nombreRol, CancellationToken ct = default)
        {
            try
            {
                var body = new CambioRolRequest { IdUsuario = idUsuario, NombreRol = nombreRol };
                var respuesta = await _http.PutAsJsonAsync("api/usuarios/asignar-rol", body, ct);
                if (respuesta.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                var err = await respuesta.Content.ReadAsStringAsync(ct);
                return (false, err);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Exito, string? Error)> ActualizarUsuarioAsync(int idUsuario, UsuarioAdminEdicionRequest dto, CancellationToken ct = default)
        {
            try
            {
                var respuesta = await _http.PutAsJsonAsync($"api/usuarios/{idUsuario}", dto, ct);
                if (respuesta.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                var err = await respuesta.Content.ReadAsStringAsync(ct);
                return (false, err);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Exito, string? Error)> EliminarUsuarioAsync(int idUsuario, CancellationToken ct = default)
        {
            try
            {
                var respuesta = await _http.DeleteAsync($"api/usuarios/{idUsuario}", ct);
                if (respuesta.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                var err = await respuesta.Content.ReadAsStringAsync(ct);
                return (false, err);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Exito, string? Error)> CambiarEstadoAsync(int idUsuario, bool activo, CancellationToken ct = default)
        {
            try
            {
                var respuesta = await _http.PutAsJsonAsync($"api/usuarios/{idUsuario}/estado", activo, ct);
                if (respuesta.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                var err = await respuesta.Content.ReadAsStringAsync(ct);
                return (false, err);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Exito, string? Error)> CrearUsuarioAsync(UsuarioAdminCrearRequest dto, CancellationToken ct = default)
        {
            try
            {
                var regDto = new
                {
                    nombre = dto.Nombre,
                    apellido = dto.Apellido,
                    email = dto.Email,
                    password = dto.Password,
                    telefono = dto.Telefono
                };

                var respuesta = await _http.PostAsJsonAsync("api/auth/register", regDto, ct);
                if (!respuesta.IsSuccessStatusCode)
                {
                    var err = await respuesta.Content.ReadAsStringAsync(ct);
                    return (false, err);
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<PerfilResponse?> ObtenerPerfilAsync(CancellationToken ct = default)
        {
            try
            {
                return await _http.GetFromJsonAsync<PerfilResponse>("api/usuarios/perfil", ct);
            }
            catch
            {
                return null;
            }
        }
    }
}
