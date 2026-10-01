namespace TurismoConecta.web.Client.Services;

/// <summary>
/// Guarda la dirección de la API (leída de wwwroot/appsettings.json)
/// y construye URLs completas para imágenes y archivos que sirve la API.
/// </summary>
public class ApiConfig
{
    public string BaseUrl { get; }

    public ApiConfig(string baseUrl)
    {
        BaseUrl = baseUrl.TrimEnd('/');
    }

    public string Url(string ruta)
    {
        if (ruta.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            ruta.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
            ruta.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
        {
            return ruta;
        }

        return $"{BaseUrl}/{ruta.TrimStart('/')}";
    }
}
