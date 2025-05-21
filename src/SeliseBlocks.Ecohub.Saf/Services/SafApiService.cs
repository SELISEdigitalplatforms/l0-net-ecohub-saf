using SeliseBlocks.Ecohub.Saf.Helpers;

namespace SeliseBlocks.Ecohub.Saf.Services;

public class SafApiService : ISafApiService
{
    private readonly IHttpRequestGateway _httpRequestGateway;

    public SafApiService(IHttpRequestGateway httpRequestGateway)
    {
        _httpRequestGateway = httpRequestGateway;
    }

    public async Task<SafReceiversResponse> GetReceiversAsync(SafReceiversRequest request)
    {
        var validation = request.Validate();
        if (!validation.IsSuccess)
        {
            return validation.MapToResponse<IEnumerable<SafReceiver>, SafReceiversResponse>();
        }
        var response = await _httpRequestGateway.PostAsync<SafReceiversRequestPayload, IEnumerable<SafReceiver>>(
            endpoint: SafDriverConstant.GetReceiversEndpoint,
            requestBody: request.Payload,
            headers: null,
            bearerToken: request.BearerToken);

        return response.MapToDerivedResponse<IEnumerable<SafReceiver>, SafReceiversResponse>();
    }

    public async Task<SafMemberPublicKeyResponse> GetMemberPublicKey(string bearerToken, string idpNumber)
    {
        var response = new SafMemberPublicKeyResponse();

        if (string.IsNullOrEmpty(bearerToken))
        {
            response.Error = new SafError
            {
                ErrorCode = "ValidationError",
                ErrorMessage = "bearerToken cannot be null or empty."
            };
            return response;
        }
        if (string.IsNullOrEmpty(idpNumber))
        {
            response.Error = new SafError
            {
                ErrorCode = "ValidationError",
                ErrorMessage = "idpNumber cannot be null or empty."
            };
            return response;
        }

        var endpoint = SafDriverConstant.GetMemberPublicKeyEndpoint.Replace("{idpNumber}", idpNumber);
        var safResponse = await _httpRequestGateway.GetAsync<SafMemberPublicKey>(
            endpoint: endpoint,
            headers: null,
            bearerToken: bearerToken);

        response = safResponse.MapToDerivedResponse<SafMemberPublicKey, SafMemberPublicKeyResponse>();

        return response;
    }

    public async Task<SafMemberPublicKeyResponse> UploadMemberPublicKey(SafMemberPublicKeyUploadRequest request)
    {
        var validation = request.Validate();
        if (!validation.IsSuccess)
        {
            return validation.MapToResponse<SafMemberPublicKey, SafMemberPublicKeyResponse>();
        }

        var response = await _httpRequestGateway.PostAsync<SafMemberPublicKeyUploadRequestPayload, SafMemberPublicKey>(
            endpoint: SafDriverConstant.UploadMemberPublicKeyEndpoint,
            requestBody: request.Payload,
            headers: null,
            bearerToken: request.BearerToken);

        return response.MapToDerivedResponse<SafMemberPublicKey, SafMemberPublicKeyResponse>();
    }

    public async Task<SafMemberGetEncryptedKeyResponse> GetMemberEncryptedPublicKey(string bearerToken, string keyId)
    {
        var response = new SafMemberGetEncryptedKeyResponse();

        if (string.IsNullOrEmpty(bearerToken))
        {
            response.Error = new SafError
            {
                ErrorCode = "ValidationError",
                ErrorMessage = "bearerToken cannot be null or empty."
            };
            return response;
        }
        if (string.IsNullOrEmpty(keyId))
        {
            response.Error = new SafError
            {
                ErrorCode = "ValidationError",
                ErrorMessage = "keyId cannot be null or empty."
            };
            return response;
        }

        var endpoint = SafDriverConstant.GetEncryptedPublicKeyEndpoint.Replace("{keyId}", keyId);
        var safResponse = await _httpRequestGateway.GetAsync<SafMemberGetEncryptedKey>(
            endpoint: endpoint,
            headers: null,
            bearerToken: bearerToken);

        response = safResponse.MapToDerivedResponse<SafMemberGetEncryptedKey, SafMemberGetEncryptedKeyResponse>();
        return response;
    }

    public async Task<SafMemberVerifyDecryptedKeyResponse> VerifyMemberDecryptedPublicKey(SafMemberVerifyDecryptedKeyRequest request)
    {
        var validation = request.Validate();
        if (!validation.IsSuccess)
        {
            return validation.MapToResponse<SafMemberVerifyDecryptedKey, SafMemberVerifyDecryptedKeyResponse>();
        }

        try
        {
            var endpoint = SafDriverConstant.VerifyDecryptedPublicKeyEndpoint.Replace("{keyId}", request.KeyId);
            var response = await _httpRequestGateway.PostAsync<SafMemberVerifyDecryptedKeyRequest, SafMemberVerifyDecryptedKey>(
                endpoint: endpoint,
                requestBody: request,
                headers: null,
                bearerToken: request.BearerToken);

            return response.MapToDerivedResponse<SafMemberVerifyDecryptedKey, SafMemberVerifyDecryptedKeyResponse>();
        }
        catch (Exception ex)
        {
            return new SafMemberVerifyDecryptedKeyResponse
            {
                Error = new SafError
                {
                    ErrorCode = "Failed",
                    ErrorMessage = ex.Message
                }
            };
        }
    }


    public async Task<SafCommonResponse> ActivateMemberPublicKey(string bearerToken, string keyId)
    {
        var response = new SafCommonResponse();

        if (string.IsNullOrEmpty(bearerToken))
        {
            response.Error = new SafError
            {
                ErrorCode = "ValidationError",
                ErrorMessage = "bearerToken cannot be null or empty."
            };
            return response;
        }
        if (string.IsNullOrEmpty(keyId))
        {
            response.Error = new SafError
            {
                ErrorCode = "ValidationError",
                ErrorMessage = "keyId cannot be null or empty."
            };
            return response;
        }

        try
        {
            var endpoint = SafDriverConstant.ActivatePublicKeyEndpoint.Replace("{keyId}", keyId);
            var safResponse = await _httpRequestGateway.PostAsync<object, dynamic>(
                endpoint: endpoint,
                requestBody: null,
                headers: null,
                bearerToken: bearerToken);
            response = safResponse.MapToDerivedResponse<dynamic, SafCommonResponse>();
            return response;
        }
        catch (Exception ex)
        {
            return new SafCommonResponse
            {
                Error = new SafError
                {
                    ErrorCode = "Failed",
                    ErrorMessage = ex.Message
                }
            };
        }
    }
}
