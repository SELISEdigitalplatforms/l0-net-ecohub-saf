using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

/// <summary>
/// Represents a request to obtain a bearer token from the SAF API.
/// This class contains the request URL and the body of the request, which includes the necessary parameters for authentication.
/// The body of the request should be of type SafAccessTokenRequestBody, which contains properties such as grant type, client ID, client secret, and scope.
/// </summary>
public class SafBearerTokenRequest
{
    /// <summary>
    /// The request URL should be the endpoint for obtaining a bearer token.
    /// </summary>
    public string RequestUrl { get; set; } = string.Empty;
    /// <summary>
    /// The bearer token should be the token obtained from the SAF API after successful authentication.
    /// This token is used to authorize requests to the SAF API.
    /// </summary>
    public SafAccessTokenRequestBody Body { get; set; }
}

/// <summary>
/// Represents the body of the request to obtain a bearer token from the SAF API.
/// This class contains the necessary parameters for authentication, such as grant type, client ID, client secret, and scope.
/// </summary>
public class SafAccessTokenRequestBody
{
    [Required(ErrorMessage = "GrantType is required")]
    [JsonPropertyName("grant_type")]
    public string GrantType { get; set; } = string.Empty;

    [Required(ErrorMessage = "ClientId is required")]
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; } = string.Empty;

    [Required(ErrorMessage = "ClientSecret is required")]
    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; set; } = string.Empty;

    [JsonPropertyName("scope")]
    public string Scope { get; set; } = string.Empty;
}
