using ArcusTel.PortalClientes.Api.DTO;
using ArcusTel.PortalClientes.Api.Interface;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ArcusTel.PortalClientes.Api.Service;

public class PartnerBrasilClient : IPartnerClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private readonly IMemoryCache _cache;
    private readonly ILogger<PartnerBrasilClient> _log;

    public PartnerBrasilClient(HttpClient http, IConfiguration config, IMemoryCache cache, ILogger<PartnerBrasilClient> log)
    {
        _http = http;
        _config = config;
        _cache = cache;
        _log = log;
    }

    private string AuthCacheKey => "PartnerBrasil_Jwt";

    public async Task AuthenticateAsync(CancellationToken ct = default)
    {
        if (_cache.TryGetValue<string>(AuthCacheKey, out var tok)) return;

        var creds = new { username = _config["PartnerBrasil:Username"], password = _config["PartnerBrasil:Password"] };
        var res = await _http.PostAsJsonAsync("/api/auth/login", creds, ct);
        res.EnsureSuccessStatusCode();
        var body = await res.Content.ReadFromJsonAsync<PartnerBrasilAuthResponse>(cancellationToken: ct);
        if (body?.Token == null) throw new Exception("PartnerBrasil returned no token");
        // cache token (no expiry info provided — set sliding expiration short)
        _cache.Set(AuthCacheKey, body.Token, TimeSpan.FromMinutes(9));
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", body.Token);
    }

    public async Task<(string rawResponse, object parsed)> ActivateDidAsync(string e164Number, string requestedBy, CancellationToken ct = default)
    {
        await AuthenticateAsync(ct);
        var payload = new { didNumber = e164Number };
        var res = await _http.PostAsJsonAsync("/api/DidActivation/request", payload, ct);
        var raw = await res.Content.ReadAsStringAsync(ct);
        object parsed = null;
        try { parsed = JsonSerializer.Deserialize<PartnerBrasilActivationResponse>(raw); } catch { parsed = raw; }
        return (raw, parsed!);
    }

    public async Task<(string rawResponse, object parsed)> GetStatusAsync(string externalIdOrNumber, CancellationToken ct = default)
    {
        await AuthenticateAsync(ct);
        // Implement according to partner spec (example GET /api/DidActivation/{id} or query by number)
        var res = await _http.GetAsync($"/api/DidActivation/{externalIdOrNumber}", ct);
        var raw = await res.Content.ReadAsStringAsync(ct);
        object parsed = null;
        try { parsed = JsonSerializer.Deserialize<PartnerBrasilActivationResponse>(raw); } catch { parsed = raw; }
        return (raw, parsed!);
    }
}

