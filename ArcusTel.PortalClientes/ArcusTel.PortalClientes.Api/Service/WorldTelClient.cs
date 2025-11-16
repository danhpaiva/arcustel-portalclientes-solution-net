using ArcusTel.PortalClientes.Api.DTO;
using ArcusTel.PortalClientes.Api.Interface;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ArcusTel.PortalClientes.Api.Service;

public class WorldTelClient : IWorldTelClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private readonly IMemoryCache _cache;
    private const string CACHE_KEY = "WorldTel_Token";

    public WorldTelClient(HttpClient http, IConfiguration config, IMemoryCache cache)
    {
        _http = http;
        _config = config;
        _cache = cache;
    }

    private async Task EnsureAuth(CancellationToken ct)
    {
        if (_cache.TryGetValue<string>(CACHE_KEY, out var token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return;
        }

        var creds = new { username = _config["Partners:WorldTel:Username"], password = _config["Partners:WorldTel:Password"] };
        var res = await _http.PostAsJsonAsync("/api/Auth/login", creds, ct);
        res.EnsureSuccessStatusCode();
        var raw = await res.Content.ReadAsStringAsync(ct);
        var body = JsonSerializer.Deserialize<WorldTelAuthResponse>(raw, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (body?.Token == null) throw new Exception("WorldTel: token null");
        var expire = body.ExpiresAt;
        var ttl = expire - DateTimeOffset.UtcNow;
        if (ttl <= TimeSpan.Zero) ttl = TimeSpan.FromMinutes(9);
        _cache.Set(CACHE_KEY, body.Token, ttl);
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", body.Token);
    }

    public async Task<WorldTelActivationResponse?> ActivateDidAsync(string e164Number, string requestedBy, CancellationToken ct = default)
    {
        await EnsureAuth(ct);

        var payload = new { e164Number, createdBy = requestedBy };

        // --- LOG: antes da requisição ---
        Console.WriteLine($"[WorldTelClient] POST /api/InternationalDids/from-number");
        Console.WriteLine($"[WorldTelClient] Payload: {JsonSerializer.Serialize(payload)}");

        HttpResponseMessage res;
        try
        {
            res = await _http.PostAsJsonAsync("/api/InternationalDids/from-number", payload, ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WorldTelClient] Exception during POST: {ex}");
            throw;
        }

        // --- LOG: após a requisição ---
        Console.WriteLine($"[WorldTelClient] StatusCode: {res.StatusCode}");
        var raw = await res.Content.ReadAsStringAsync(ct);
        Console.WriteLine($"[WorldTelClient] Response body: {raw}");

        if (!res.IsSuccessStatusCode)
        {
            Console.WriteLine("[WorldTelClient] ActivateDidAsync returning null due to non-success status code.");
            return null;
        }

        try
        {
            var result = JsonSerializer.Deserialize<WorldTelActivationResponse>(raw,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result == null)
                Console.WriteLine("[WorldTelClient] Deserialized result is null!");

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WorldTelClient] Exception during deserialization: {ex}");
            throw;
        }
    }


    public async Task<WorldTelActivationResponse?> GetStatusByDidIdAsync(string didId, CancellationToken ct = default)
    {
        await EnsureAuth(ct);

        var res = await _http.GetAsync($"/api/InternationalDids/{Uri.EscapeDataString(didId)}", ct);
        if (!res.IsSuccessStatusCode)
            return null;

        var raw = await res.Content.ReadAsStringAsync(ct);

        return JsonSerializer.Deserialize<WorldTelActivationResponse>(raw,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

}
