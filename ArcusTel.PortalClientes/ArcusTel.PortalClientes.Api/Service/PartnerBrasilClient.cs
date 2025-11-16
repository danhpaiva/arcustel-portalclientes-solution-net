using ArcusTel.PortalClientes.Api.DTO;
using ArcusTel.PortalClientes.Api.Interface;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ArcusTel.PortalClientes.Api.Service;

public class PartnerBrasilClient : IPartnerBrasilClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private readonly IMemoryCache _cache;
    private const string TOKEN_CACHE_KEY = "PartnerBrasil_Jwt";

    public PartnerBrasilClient(HttpClient http, IConfiguration config, IMemoryCache cache)
    {
        _http = http;
        _config = config;
        _cache = cache;
    }

    private async Task EnsureAuth(CancellationToken ct)
    {
        if (_cache.TryGetValue<string>(TOKEN_CACHE_KEY, out var token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return;
        }

        var creds = new { username = _config["Partners:PartnerBrasil:Username"], password = _config["Partners:PartnerBrasil:Password"] };
        var res = await _http.PostAsJsonAsync("/api/auth/login", creds, ct);
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadAsStringAsync(ct);
        var body = JsonSerializer.Deserialize<PartnerBrasilAuthResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (body?.Token == null) throw new Exception("PartnerBrasil: token null");
        _cache.Set(TOKEN_CACHE_KEY, body.Token, TimeSpan.FromMinutes(9));
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", body.Token);
    }

    public async Task<PartnerBrasilActivationResponse?> ActivateDidAsync(string e164Number, string requestedBy, CancellationToken ct = default)
    {
        await EnsureAuth(ct);
        var payload = new { didNumber = e164Number };
        var res = await _http.PostAsJsonAsync("/api/DidActivation/request", payload, ct);
        var raw = await res.Content.ReadAsStringAsync(ct);
        if (!res.IsSuccessStatusCode)
        {
            // optional: return a wrapper with error info
            return null;
        }
        return JsonSerializer.Deserialize<PartnerBrasilActivationResponse>(raw, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<PartnerBrasilActivationResponse?> GetStatusByNumberAsync(string e164Number, CancellationToken ct = default)
    {
        await EnsureAuth(ct);
        // Assuming partner has GET by id or query: adapt if different
        var res = await _http.GetAsync($"/api/DidActivation/request-by-number?didNumber={Uri.EscapeDataString(e164Number)}", ct);
        if (!res.IsSuccessStatusCode) return null;
        var raw = await res.Content.ReadAsStringAsync(ct);
        return JsonSerializer.Deserialize<PartnerBrasilActivationResponse>(raw, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}