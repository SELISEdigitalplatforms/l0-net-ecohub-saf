using SeliseBlocks.Ecohub.Saf.Helpers;

namespace SeliseBlocks.Ecohub.Saf.Services;

public class SafApiService : ISafApiService
{
    private readonly IHttpRequestGateway _httpRequestGateway;

    public SafApiService(IHttpRequestGateway httpRequestGateway)
    {
        _httpRequestGateway = httpRequestGateway;
    }

    public async Task<IEnumerable<SafReceiversResponse>> GetReceiversAsync(SafReceiversRequest request)
    {
        request.Validate();
        var response = await _httpRequestGateway.PostAsync<SafReceiversRequestPayload, IEnumerable<SafReceiversResponse>>(
            endpoint: SafDriverConstant.GetReceiversEndpoint,
            request: request.Payload,
            headers: null,
            bearerToken: request.BearerToken);

        return response;
    }

    /// <summary>
    /// Asynchronously retrieves the public key of a member from the SAF API.
    /// This method sends a request to the SAF API to get the public key of a member based on the provided IDP number.
    /// The request should include the necessary authentication details and the IDP number of the member whose public key is being requested.
    /// </summary>
    /// <param name="bearerToken">
    /// The bearer token obtained from the SAF API after successful authentication.
    /// This token is used to authorize the request to retrieve the member's public key.
    /// 
    /// </param>
    /// <param name="idpNumber"></param>
    /// <returns></returns>
    public async Task<SafMemberPublicKeyResponse> GetMemberPublicKey(string bearerToken, string idpNumber)
    {
        if (string.IsNullOrEmpty(bearerToken))
        {
            throw new ArgumentException("bearerToken cannot be null or empty.", nameof(bearerToken));
        }
        if (string.IsNullOrEmpty(idpNumber))
        {
            throw new ArgumentException("idpNumber cannot be null or empty.", nameof(idpNumber));
        }
        var endpoint = SafDriverConstant.GetMemberPublicKeyEndpoint.Replace("{idpNumber}", idpNumber);
        var response = await _httpRequestGateway.GetAsync<SafMemberPublicKeyResponse>(
            endpoint: endpoint,
            headers: null,
            bearerToken: bearerToken);

        return response;
    }

    /// <summary>
    /// Asynchronously uploads the public key of a member to the SAF API.
    /// This method sends a request to the SAF API to upload a member's public key along with its metadata.
    /// The request should include the necessary authentication details and the payload containing the public key information.
    /// </summary>
    /// <param name="request">
    /// The request object containing the bearer token and payload for uploading the member's public key.
    /// The payload includes details such as:
    /// - The public key to be uploaded.
    /// - version of the public key.
    /// - expire in days of the public key.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a 
    /// <see cref="SafMemberPublicKeyResponse"/> object, which includes the details of the uploaded public key.
    /// </returns>
    public async Task<SafMemberPublicKeyResponse> UploadMemberPublicKey(SafMemberPublicKeyUploadRequest request) 
    {
        request.Validate();

        var response = await _httpRequestGateway.PostAsync<SafMemberPublicKeyUploadRequest, SafMemberPublicKeyResponse>(
            endpoint: SafDriverConstant.UploadMemberPublicKeyEndpoint,
            request: request,
            headers: null,
            bearerToken: request.BearerToken);

        return response;
    }

    /// <summary>
    /// Asynchronously retrieves the encrypted public key of a member from the SAF API.
    /// This method sends a request to the SAF API to get the encrypted public key of a member based on the provided key ID.
    /// The request should include the necessary authentication details and the key ID of the member whose encrypted public key is being requested.
    /// </summary>
    /// <param name="bearerToken">
    /// The bearer token obtained from the SAF API after successful authentication.
    /// This token is used to authorize the request to retrieve the member's public key.
    /// 
    /// </param>
    /// <param name="keyId">
    /// The key ID of the member whose encrypted public key is being requested.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a 
    /// <see cref="SafMemberGetEncryptedKeyResponse"/> object, which includes the provided key ID and the encrypted public key.
    /// </returns>
    public async Task<SafMemberGetEncryptedKeyResponse> GetMemberEncryptedPublicKey(string bearerToken, string keyId) 
    {
        if (string.IsNullOrEmpty(bearerToken))
        {
            throw new ArgumentException("bearerToken cannot be null or empty.", nameof(bearerToken));
        }
        if (string.IsNullOrEmpty(keyId))
        {
            throw new ArgumentException("keyId cannot be null or empty.", nameof(keyId));
        }

        var endpoint = SafDriverConstant.GetEncryptedPublicKeyEndpoint.Replace("{keyId}", keyId);
        var response = await _httpRequestGateway.GetAsync<SafMemberGetEncryptedKeyResponse>(
            endpoint: endpoint,
            headers: null,
            bearerToken: bearerToken);

        return response;
    }
}
