using System.Security.Cryptography;
using SeliseBlocks.Ecohub.Saf.Helpers;

namespace SeliseBlocks.Ecohub.Saf.Services;

public class SafPkiApiService : ISafPkiApiService
{
    private readonly IHttpRequestGateway _httpRequestGateway;

    public SafPkiApiService(IHttpRequestGateway httpRequestGateway)
    {
        _httpRequestGateway = httpRequestGateway;
    }

    public async Task<SafMemberPublicKeyResponse> UploadMemberPublicKey(SafMemberPublicKeyUploadRequest request)
    {
        var validation = request.Validate();
        if (!validation.IsSuccess)
        {
            return validation.MapToResponse<SafMemberPublicKey, SafMemberPublicKeyResponse>();
        }

        var response = await _httpRequestGateway.PostAsync<IEnumerable<SafMemberPublicKeyUploadRequestPayload>, SafMemberPublicKey>(
            endpoint: SafDriverConstant.UploadMemberPublicKeyEndpoint,
            requestBody: request.Payload,
            headers: null,
            bearerToken: request.BearerToken);

        return response.MapToDerivedResponse<SafMemberPublicKey, SafMemberPublicKeyResponse>();
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

    public async Task<SafDynamicResponse> ActivateMemberPublicKey(string bearerToken, string keyId)
    {
        var response = new SafDynamicResponse();

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
            response = safResponse.MapToDerivedResponse<dynamic, SafDynamicResponse>();
            return response;
        }
        catch (Exception ex)
        {
            return new SafDynamicResponse
            {
                Error = new SafError
                {
                    ErrorCode = "Failed",
                    ErrorMessage = ex.Message
                }
            };
        }
    }

    public async Task<SafValidationResponse> DeactivatePublicKey(string bearerToken, string keyId)
    {
        var response = new SafValidationResponse();

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
            var endpoint = SafDriverConstant.DeactivatePublicKeyEndpoint.Replace("{keyId}", keyId);
            var safResponse = await _httpRequestGateway.PostAsync<object, dynamic>(
                endpoint: endpoint,
                requestBody: null,
                headers: null,
                bearerToken: bearerToken);
            response = safResponse;
            return response;
        }
        catch (Exception ex)
        {
            return new SafDynamicResponse
            {
                Error = new SafError
                {
                    ErrorCode = "Failed",
                    ErrorMessage = ex.Message
                }
            };
        }
    }

    public async Task<SafValidationResponse> DeleteInactivePublicKey(string bearerToken, string keyId)
    {
        var response = new SafValidationResponse();

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
            var endpoint = SafDriverConstant.DeleteInactivePublicKeyEndpoint.Replace("{keyId}", keyId);
            var safResponse = await _httpRequestGateway.DeleteAsync<object, dynamic>(
                endpoint: endpoint,
                requestBody: null,
                headers: null,
                bearerToken: bearerToken);
            response = safResponse;
            return response;
        }
        catch (Exception ex)
        {
            return new SafDynamicResponse
            {
                Error = new SafError
                {
                    ErrorCode = "Failed",
                    ErrorMessage = ex.Message
                }
            };
        }
    }

    public async Task<SafMemberPublicKeysResponse> GetMemberPublicKey(string bearerToken, string idpNumber)
    {
        var response = new SafMemberPublicKeysResponse();

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
        var safResponse = await _httpRequestGateway.GetAsync<IEnumerable<SafMemberPublicKey>>(
            endpoint: endpoint,
            headers: null,
            bearerToken: bearerToken);

        response = safResponse.MapToDerivedResponse<SafMemberPublicKey, SafMemberPublicKeysResponse>();

        return response;
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

    public async Task<SafMemberPublicKeysResponse> GetAllKeys(string bearerToken)
    {
        var response = new SafMemberPublicKeysResponse();

        if (string.IsNullOrEmpty(bearerToken))
        {
            response.Error = new SafError
            {
                ErrorCode = "ValidationError",
                ErrorMessage = "bearerToken cannot be null or empty."
            };
            return response;
        }

        var endpoint = SafDriverConstant.GetAllPublicKeyEndpoint;
        var safResponse = await _httpRequestGateway.GetAsync<IEnumerable<SafMemberPublicKey>>(
            endpoint: endpoint,
            headers: null,
            bearerToken: bearerToken);

        response = safResponse.MapToDerivedResponse<SafMemberPublicKey, SafMemberPublicKeysResponse>();

        return response;
    }

    public async Task<SafMemberPublicKeysResponse> GetKeysByKeyType(string bearerToken, string idpNumber, string keyType)
    {
        var response = new SafMemberPublicKeysResponse();

        if (string.IsNullOrEmpty(bearerToken))
        {
            response.Error = new SafError
            {
                ErrorCode = "ValidationError",
                ErrorMessage = "bearerToken cannot be null or empty."
            };
            return response;
        }

        var endpoint = SafDriverConstant.GetPublicKeyByKeyTypeEndpoint.Replace("{idpNumber}", idpNumber);
        endpoint = endpoint.Replace("{keyType}", keyType);
        var safResponse = await _httpRequestGateway.GetAsync<IEnumerable<SafMemberPublicKey>>(
            endpoint: endpoint,
            headers: null,
            bearerToken: bearerToken);

        response = safResponse.MapToDerivedResponse<SafMemberPublicKey, SafMemberPublicKeysResponse>();

        return response;
    }

    public async Task<SafMemberPublicKeyResponse> GetKeyDetails(string bearerToken, string keyId)
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

        var endpoint = SafDriverConstant.GetEncryptedPublicKeyEndpoint.Replace("{keyId}", keyId);
        var safResponse = await _httpRequestGateway.GetAsync<SafMemberPublicKey>(
            endpoint: endpoint,
            headers: null,
            bearerToken: bearerToken);

        response = safResponse.MapToDerivedResponse<SafMemberPublicKey, SafMemberPublicKeyResponse>();

        return response;
    }

}
