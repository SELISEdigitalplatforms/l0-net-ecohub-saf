namespace SeliseBlocks.Ecohub.Saf;

/// <summary>
/// Interface for interacting with the SAF API. 
/// Provides methods to manage member public keys and retrieve receiver information.
/// </summary>
public interface ISafPkiApiService
{
    /// <summary>
    /// Uploads a member's public key to the SAF system.
    /// </summary>
    /// <param name="request">The upload request containing BearerToken, Payload.Version, Payload.Key, and Payload.ExpireInDays.</param>
    /// <returns>A <see cref="SafMemberPublicKeysResponse"/> with the uploaded key details, including IsSuccess, Data, and Error.</returns>
    Task<SafMemberPublicKeysResponse> UploadMemberPublicKey(SafMemberPublicKeyUploadRequest request);

    /// <summary>
    /// Verifies a member's decrypted public key.
    /// </summary>
    /// <param name="request">The verification request containing BearerToken, KeyId, and Payload.DecryptedContent.</param>
    /// <returns>A <see cref="SafMemberVerifyDecryptedKeyResponse"/> with verification status, including IsSuccess, Data, and Error.</returns>
    Task<SafMemberVerifyDecryptedKeyResponse> VerifyMemberDecryptedPublicKey(SafMemberVerifyDecryptedKeyRequest request);

    /// <summary>
    /// Activates a member's public key.
    /// </summary>
    /// <param name="bearerToken">Authentication token.</param>
    /// <param name="keyId">Key identifier.</param>
    /// <returns>A <see cref="SafDynamicResponse"/> indicating activation success or failure, including IsSuccess, Data, and Error.</returns>
    Task<SafDynamicResponse> ActivateMemberPublicKey(string bearerToken, string keyId);

    /// <summary>
    /// Deactivates a member's public key.
    /// </summary>
    /// <param name="bearerToken">Authentication token.</param>
    /// <param name="keyId">Key identifier.</param>
    /// <returns>A <see cref="SafValidationResponse"/> indicating deactivation result, including IsSuccess and Error.</returns>
    Task<SafValidationResponse> DeactivatePublicKey(string bearerToken, string keyId);

    /// <summary>
    /// Deletes an inactive member's public key.
    /// </summary>
    /// <param name="bearerToken">Authentication token.</param>
    /// <param name="keyId">Key identifier.</param>
    /// <returns>A <see cref="SafValidationResponse"/> indicating deletion result, including IsSuccess and Error.</returns>
    Task<SafValidationResponse> DeleteInactivePublicKey(string bearerToken, string keyId);

    /// <summary>
    /// Retrieves a member's public key(s) by their IDP number.
    /// </summary>
    /// <param name="bearerToken">Authentication token.</param>
    /// <param name="idpNumber">Member's IDP number.</param>
    /// <returns>A <see cref="SafMemberPublicKeysResponse"/> containing the member's public key details, including IsSuccess, Data, and Error.</returns>
    Task<SafMemberPublicKeysResponse> GetMemberPublicKey(string bearerToken, string idpNumber);

    /// <summary>
    /// Retrieves a member's encrypted public key.
    /// </summary>
    /// <param name="bearerToken">Authentication token.</param>
    /// <param name="keyId">Key identifier.</param>
    /// <returns>A <see cref="SafMemberGetEncryptedKeyResponse"/> containing the encrypted key, including IsSuccess, Data, and Error.</returns>
    Task<SafMemberGetEncryptedKeyResponse> GetMemberEncryptedPublicKey(string bearerToken, string keyId);

    /// <summary>
    /// Retrieves all public keys for the authenticated member.
    /// </summary>
    /// <param name="bearerToken">Authentication token.</param>
    /// <returns>A <see cref="SafMemberPublicKeysResponse"/> containing all public keys, including IsSuccess, Data, and Error.</returns>
    Task<SafMemberPublicKeysResponse> GetAllKeys(string bearerToken);

    /// <summary>
    /// Retrieves details for a specific public key by key ID.
    /// </summary>
    /// <param name="bearerToken">Authentication token.</param>
    /// <param name="keyId">Key identifier.</param>
    /// <returns>A <see cref="SafMemberPublicKeyResponse"/> containing the key details, including IsSuccess, Data, and Error.</returns>
    Task<SafMemberPublicKeyResponse> GetKeyDetails(string bearerToken, string keyId);

    /// <summary>
    /// Retrieves public keys for a member by key type.
    /// </summary>
    /// <param name="bearerToken">Authentication token.</param>
    /// <param name="idpNumber">Member's IDP number.</param>
    /// <param name="keyType">The type of key to filter by.</param>
    /// <returns>A <see cref="SafMemberPublicKeysResponse"/> containing the filtered keys, including IsSuccess, Data, and Error.</returns>
    Task<SafMemberPublicKeysResponse> GetKeysByKeyType(string bearerToken, string idpNumber, string keyType);
}