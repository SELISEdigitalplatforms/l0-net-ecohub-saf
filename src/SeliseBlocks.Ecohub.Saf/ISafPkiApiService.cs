namespace SeliseBlocks.Ecohub.Saf;

/// <summary>
/// Interface for interacting with the SAF API. 
/// Provides methods to manage member public keys and retrieve receiver information.
/// </summary>
public interface ISafPkiApiService
{



    /// <summary>
    /// Uploads a member's public key.
    /// </summary>
    /// <param name="request">
    /// Upload request containing:
    /// <list type="bullet">
    ///   <item><description><c>BearerToken</c>: Authentication token</description></item>
    ///   <item><description><c>Payload.Version</c>: Key version</description></item>
    ///   <item><description><c>Payload.Key</c>: Public key data</description></item>
    ///   <item><description><c>Payload.ExpireInDays</c>: Key expiration period</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// A <see cref="SafMemberPublicKeyResponse"/> with the uploaded key details
    /// </returns>
    Task<SafMemberPublicKeyResponse> UploadMemberPublicKey(SafMemberPublicKeyUploadRequest request);

    /// <summary>
    /// Verifies a member's decrypted public key.
    /// </summary>
    /// <param name="request">
    /// Verification request containing:
    /// <list type="bullet">
    ///   <item><description><c>BearerToken</c>: Authentication token</description></item>
    ///   <item><description><c>KeyId</c>: Key identifier</description></item>
    ///   <item><description><c>Payload.DecryptedContent</c>: Decrypted key content</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// A <see cref="SafMemberVerifyDecryptedKeyResponse"/> with verification status
    /// </returns>
    Task<SafMemberVerifyDecryptedKeyResponse> VerifyMemberDecryptedPublicKey(SafMemberVerifyDecryptedKeyRequest request);

    /// <summary>
    /// Activates a member's public key.
    /// </summary>
    /// <param name="bearerToken">Authentication token</param>
    /// <param name="keyId">Key identifier</param>
    /// <returns>
    /// A <see cref="SafDynamicResponse"/> indicating activation success or failure
    Task<SafDynamicResponse> ActivateMemberPublicKey(string bearerToken, string keyId);

    Task<SafValidationResponse> DeactivatePublicKey(string bearerToken, string keyId);
    Task<SafValidationResponse> DeleteInactivePublicKey(string bearerToken, string keyId);


    /// <summary>
    /// Retrieves a member's public key by their IDP number.
    /// </summary>
    /// <param name="bearerToken">Authentication token</param>
    /// <param name="idpNumber">Member's IDP number</param>
    /// <returns>
    /// A <see cref="SafMemberPublicKeyResponse"/> containing the member's public key details
    /// </returns>
    Task<SafMemberPublicKeysResponse> GetMemberPublicKey(string bearerToken, string idpNumber);

    /// <summary>
    /// Retrieves a member's encrypted public key.
    /// </summary>
    /// <param name="bearerToken">Authentication token</param>
    /// <param name="keyId">Key identifier</param>
    /// <returns>
    /// A <see cref="SafMemberGetEncryptedKeyResponse"/> containing the encrypted key
    /// </returns>
    Task<SafMemberGetEncryptedKeyResponse> GetMemberEncryptedPublicKey(string bearerToken, string keyId);

    Task<SafMemberPublicKeysResponse> GetAllKeys(string bearerToken);
    Task<SafMemberPublicKeyResponse> GetKeyDetails(string bearerToken, string keyId);
    Task<SafMemberPublicKeysResponse> GetKeysByKeyType(string bearerToken, string idpNumber, string keyType);
}