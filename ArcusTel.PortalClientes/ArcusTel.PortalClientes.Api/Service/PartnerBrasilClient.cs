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

        Console.WriteLine("[PartnerBrasilClient] Authenticating...");
        var res = await _http.PostAsJsonAsync("/api/auth/login", creds, ct);
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadAsStringAsync(ct);

        var body = JsonSerializer.Deserialize<PartnerBrasilAuthResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (body?.Token == null) throw new Exception("PartnerBrasil: token null");

        _cache.Set(TOKEN_CACHE_KEY, body.Token, TimeSpan.FromMinutes(9));
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", body.Token);
        Console.WriteLine("[PartnerBrasilClient] Authentication successful.");
    }

    public async Task<PartnerBrasilActivationResponse?> ActivateDidAsync(string e164Number, string requestedBy, CancellationToken ct = default)
    {
        await EnsureAuth(ct);

        var payload = new { didNumber = e164Number };

        Console.WriteLine("[PartnerBrasilClient] POST /api/DidActivation/request");
        Console.WriteLine($"[PartnerBrasilClient] Payload: {JsonSerializer.Serialize(payload)}");

        HttpResponseMessage res;
        try
        {
            res = await _http.PostAsJsonAsync("/api/DidActivation/request", payload, ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PartnerBrasilClient] Exception during POST: {ex}");
            throw;
        }

        var raw = await res.Content.ReadAsStringAsync(ct);
        Console.WriteLine($"[PartnerBrasilClient] StatusCode: {res.StatusCode}");
        Console.WriteLine($"[PartnerBrasilClient] Response body: {raw}");

        if (!res.IsSuccessStatusCode)
        {
            Console.WriteLine("[PartnerBrasilClient] ActivateDidAsync returning null due to non-success status code.");
            return null;
        }

        try
        {
            var result = JsonSerializer.Deserialize<PartnerBrasilActivationResponse>(raw, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (result == null) Console.WriteLine("[PartnerBrasilClient] Deserialized result is null!");
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PartnerBrasilClient] Exception during deserialization: {ex}");
            throw;
        }
    }

    public async Task<PartnerBrasilActivationResponse?> GetStatusByNumberAsync(string e164Number, CancellationToken ct = default)
    {
        await EnsureAuth(ct);

        Console.WriteLine($"[PartnerBrasilClient] GET /api/DidActivation/request-by-number?didNumber={e164Number}");

        HttpResponseMessage res;
        try
        {
            res = await _http.GetAsync($"/api/DidActivation/request-by-number?didNumber={Uri.EscapeDataString(e164Number)}", ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PartnerBrasilClient] Exception during GET: {ex}");
            throw;
        }

        var raw = await res.Content.ReadAsStringAsync(ct);
        Console.WriteLine($"[PartnerBrasilClient] StatusCode: {res.StatusCode}");
        Console.WriteLine($"[PartnerBrasilClient] Response body: {raw}");

        if (!res.IsSuccessStatusCode)
        {
            Console.WriteLine("[PartnerBrasilClient] GetStatusByNumberAsync returning null due to non-success status code.");
            return null;
        }

        try
        {
            var result = JsonSerializer.Deserialize<PartnerBrasilActivationResponse>(raw, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (result == null) Console.WriteLine("[PartnerBrasilClient] Deserialized result is null!");
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PartnerBrasilClient] Exception during deserialization: {ex}");
            throw;
        }
    }
}