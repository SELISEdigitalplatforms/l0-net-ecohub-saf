namespace SeliseBlocks.Ecohub.Saf;

/// <summary>
/// Interface for interacting with the SAF API. 
/// Provides methods to retrieve receiver information and member public keys.
/// </summary>
public interface ISafApiService
{
    /// <summary>
    /// Asynchronously retrieves receiver information from the SAF API.
    /// </summary>
    /// <param name="request">
    /// The request object containing the bearer token and payload for obtaining receiver information.
    /// The payload includes details such as:
    /// - <see cref="SafReceiversRequestPayload.LicenceKey"/>: The licence key for authentication.
    /// - <see cref="SafReceiversRequestPayload.Password"/>: The password for authentication.
    /// - <see cref="SafReceiversRequestPayload.RequestId"/>: A unique identifier for the request.
    /// - <see cref="SafReceiversRequestPayload.RequestTime"/>: The timestamp of the request.
    /// - <see cref="SafReceiversRequestPayload.UserAgent"/>: Information about the user agent making the request.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a collection of 
    /// <see cref="SafReceiversResponse"/> objects, which include information about the receivers.
    /// </returns>
    /// <exception cref="HttpRequestException">
    /// Thrown if there is an issue with the HTTP request, such as a network error or invalid response.
    /// </exception>
    /// <exception cref="JsonException">
    /// Thrown if there is an issue deserializing the response from the SAF API.
    /// </exception>
    Task<IEnumerable<SafReceiversResponse>> GetReceiversAsync(SafReceiversRequest request);


    /// <summary>
    /// Asynchronously retrieves the public key of a member from the SAF API.
    /// </summary>
    /// <param name="bearerToken">
    /// The bearer token obtained from the SAF API after successful authentication. 
    /// This token is used to authorize the request to retrieve the member's public key.
    /// </param>
    /// <param name="idpNumber">
    /// The IDP number of the member whose public key is being requested. 
    /// This number uniquely identifies the member in the SAF API.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a 
    /// <see cref="SafMemberPublicKeyResponse"/> object, which includes the public key and related metadata of the member.
    /// </returns>
    /// <exception cref="HttpRequestException">
    /// Thrown if there is an issue with the HTTP request, such as a network error or invalid response.
    /// </exception>
    /// <exception cref="JsonException">
    /// Thrown if there is an issue deserializing the response from the SAF API.
    /// </exception>
    Task<SafMemberPublicKeyResponse> GetMemberPublicKey(string bearerToken, string idpNumber);

    /// <summary>
    /// Asynchronously retrieves receiver information from the SAF API.
    /// </summary>
    /// <param name="request">
    /// The request object containing the bearer token and payload for obtaining receiver information.
    /// The payload includes details such as:
    /// - <see cref="SafMemberPublicKeyUploadRequestPayload.Version"/>: Version of the public key.
    /// - <see cref="SafMemberPublicKeyUploadRequestPayload.Key"/>: The public key to be uploaded.
    /// - <see cref="SafMemberPublicKeyUploadRequestPayload.ExpireInDays"/>: Expire in days of the public key.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a collection of 
    /// <see cref="SafReceiversResponse"/> objects, which include information about the receivers.
    /// </returns>
    /// <exception cref="HttpRequestException">
    /// Thrown if there is an issue with the HTTP request, such as a network error or invalid response.
    /// </exception>
    /// <exception cref="JsonException">
    /// Thrown if there is an issue deserializing the response from the SAF API.
    /// </exception>
    Task<SafMemberPublicKeyResponse> UploadMemberPublicKey(SafMemberPublicKeyUploadRequest request);

    /// <summary>
    /// Asynchronously retrieves the public key of a member from the SAF API.
    /// </summary>
    /// <param name="bearerToken">
    /// The bearer token obtained from the SAF API after successful authentication. 
    /// This token is used to authorize the request to retrieve the member's public key.
    /// </param>
    /// <param name="keyId">
    /// The Key ID of the member whose public key is being requested. 
    /// This number uniquely identifies the member in the SAF API.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a 
    /// <see cref="SafMemberGetEncryptedKeyResponse"/> object, which includes the public key and related metadata of the member.
    /// </returns>
    /// <exception cref="errorCode">
    /// Thrown if there is an issue with the key, such as key version already exist.
    /// </exception>
    /// <exception cref="errorMessage">
    /// Thrown with detailed message if there is an issue with the given key.
    /// </exception>
    Task<SafMemberGetEncryptedKeyResponse> GetMemberEncryptedPublicKey(string bearerToken, string keyId);

    /// <summary>
    /// Asynchronously retrieves the public key of a member from the SAF API.
    /// </summary>
    /// <param name="bearerToken">
    /// The bearer token obtained from the SAF API after successful authentication. 
    /// This token is used to authorize the request to retrieve the member's public key.
    /// </param>
    /// <param name="keyId">
    /// The Key ID of the member whose public key is being requested. 
    /// This number uniquely identifies the member in the SAF API.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a 
    /// <see cref="SafMemberVerifyDecryptedKeyResponse"/> object, which includes the verification status (Success/Fail).
    /// </returns>
    /// <exception cref="errorCode">
    /// Thrown if there is an issue with the key, such as key version already exist.
    /// </exception>
    /// <exception cref="errorMessage">
    /// Thrown with detailed message if there is an issue with the given key.
    /// </exception>
    Task<SafMemberVerifyDecryptedKeyResponse> VerifyMemberDecryptedPublicKey(string bearerToken, string keyId);
}
