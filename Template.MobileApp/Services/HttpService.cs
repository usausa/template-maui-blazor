namespace Template.MobileApp.Services;

using Rester;

//--------------------------------------------------------------------------------
// Models
//--------------------------------------------------------------------------------

public class AccountLoginRequest
{
    public string Id { get; set; } = default!;
}

public class AccountLoginResponse
{
    public string Token { get; set; } = default!;
}

public sealed class ServerTimeResponse
{
    public DateTime DateTime { get; set; }
}

public class SecretMessageResponse
{
    public string Message { get; set; } = default!;
}

public sealed class DataListEntry
{
    public long Id { get; set; }

    public string Name { get; set; } = default!;
}

#pragma warning disable CA1819
public sealed class DataListResponse
{
    public DataListEntry[] Entries { get; set; } = default!;

    public int Total { get; set; }
}
#pragma warning restore CA1819

//--------------------------------------------------------------------------------
// Service
//--------------------------------------------------------------------------------

public sealed class HttpService
{
    private readonly IHttpClientFactory httpClientFactory;

    public HttpService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    //--------------------------------------------------------------------------------
    // Account
    //--------------------------------------------------------------------------------

    public ValueTask<IRestResponse<AccountLoginResponse>> PostAccountLoginAsync(AccountLoginRequest request, CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(ApiNames.Default);
        return client.PostAsync<AccountLoginResponse>("api/account/login", request, cancel: cancellationToken);
    }

    //--------------------------------------------------------------------------------
    // Basic
    //--------------------------------------------------------------------------------

    public ValueTask<IRestResponse<ServerTimeResponse>> GetServerTimeAsync(CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(ApiNames.Default);
        return client.GetAsync<ServerTimeResponse>("api/server/time", cancel: cancellationToken);
    }

    //--------------------------------------------------------------------------------
    // Data
    //--------------------------------------------------------------------------------

    public ValueTask<IRestResponse<DataListResponse>> GetDataListAsync(CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(ApiNames.Default);
        return client.GetAsync<DataListResponse>("api/data/list", cancel: cancellationToken);
    }

    //--------------------------------------------------------------------------------
    // Secret
    //--------------------------------------------------------------------------------

    public ValueTask<IRestResponse<SecretMessageResponse>> GetSecretMessageAsync(CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(ApiNames.Default);
        return client.GetAsync<SecretMessageResponse>("api/secret/message", cancel: cancellationToken);
    }

    //--------------------------------------------------------------------------------
    // Test
    //--------------------------------------------------------------------------------

    public ValueTask<IRestResponse> GetTestErrorAsync(int code, CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(ApiNames.Default);
        return client.SendAsync(HttpMethod.Get, $"api/test/error/{code}", cancel: cancellationToken);
    }

    public ValueTask<IRestResponse> GetTestDelayAsync(int timeout, CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(ApiNames.Default);
        return client.SendAsync(HttpMethod.Get, $"api/test/delay/{timeout}", cancel: cancellationToken);
    }
}
