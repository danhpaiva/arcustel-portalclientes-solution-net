using ArcusTel.PortalClientes.Api.DTO;
using ArcusTel.PortalClientes.Api.Interface;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ArcusTel.PortalClientes.Api.Service;

public class WorldTelClient : IPartnerClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private readonly IMemoryCache _cache;
    private readonly ILogger<WorldTelClient> _log;

    public WorldTelClient(HttpClient http, IConfiguration config, IMemoryCache cache, ILogger<WorldTelClient> log)
    {
        _http = http; _config = config; _cache = cache; _log = log;
    }

    private record TokenInfo(string Token, DateTimeOffset ExpiresAt);
    private const string CacheKey = "WorldTel_Token";

    public async Task AuthenticateAsync(CancellationToken ct = default)
    {
        if (_cache.TryGetValue<TokenInfo>(CacheKey, out var info) && info.ExpiresAt > DateTimeOffset.UtcNow.AddSeconds(30))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", info.Token);
            return;
        }

        var creds = new { username = _config["WorldTel:Username"], password = _config["WorldTel:Password"] };
        var res = await _http.PostAsJsonAsync("/api/Auth/login", creds, ct);
        res.EnsureSuccessStatusCode();
        var body = await res.Content.ReadFromJsonAsync<WorldTelAuthResponse>(cancellationToken: ct);
        if (body == null) throw new Exception("WorldTel auth failed");
        var expires = body.ExpiresAt;
        _cache.Set(CacheKey, new TokenInfo(body.Token, expires), expires - DateTimeOffset.UtcNow);
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", body.Token);
    }

    public async Task<(string rawResponse, object parsed)> ActivateDidAsync(string e164Number, string requestedBy, CancellationToken ct = default)
    {
        await AuthenticateAsync(ct);
        var payload = new { e164Number = e164Number, createdBy = requestedBy };
        var res = await _http.PostAsJsonAsync("/api/InternationalDids/from-number", payload, ct);
        var raw = await res.Content.ReadAsStringAsync(ct);
        object parsed = null;
        try { parsed = JsonSerializer.Deserialize<WorldTelActivationResponse>(raw); } catch { parsed = raw; }
        return (raw, parsed!);
    }

    public async Task<(string rawResponse, object parsed)> GetStatusAsync(string externalIdOrNumber, CancellationToken ct = default)
    {
        await AuthenticateAsync(ct);
        // Example: GET /api/InternationalDids/{id} or search by didId
        var res = await _http.GetAsync($"/api/InternationalDids/{externalIdOrNumber}", ct);
        var raw = await res.Content.ReadAsStringAsync(ct);
        object parsed = null;
        try { parsed = JsonSerializer.Deserialize<WorldTelActivationResponse>(raw); } catch { parsed = raw; }
        return (raw, parsed!);
    }
}

