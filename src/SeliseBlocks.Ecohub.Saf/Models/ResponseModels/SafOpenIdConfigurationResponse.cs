using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

public class SafOpenIdConfigurationResponse
{
    [JsonPropertyName("token_endpoint")]
    public string TokenEndpoint { get; set; } = string.Empty;
    [JsonPropertyName("token_endpoint_auth_methods_supported")]
    public string[] TokenEndpointAuthMethodsSupported { get; set; }
    [JsonPropertyName("jwks_uri")]
    public string[] JwksUri { get; set; }
    [JsonPropertyName("response_modes_supported")]
    public string[] ResponseModesSupported { get; set; }
    [JsonPropertyName("subject_types_supported")]
    public string[] SubjectTypesSupported { get; set; }
    [JsonPropertyName("id_token_signing_alg_values_supported")]
    public string[] IdTokenSigningAlgValuesSupported { get; set; }
    [JsonPropertyName("response_types_supported")]
    public string[] ResponseTypesSupported { get; set; }
    [JsonPropertyName("scopes_supported")]
    public string[] ScopesSupported { get; set; }
    [JsonPropertyName("issuer")]
    public string Issuer { get; set; } = string.Empty;
    [JsonPropertyName("request_uri_parameter_supported")]
    public bool RequestUriParameterSupported { get; set; }
    [JsonPropertyName("userinfo_endpoint")]
    public string UserinfoEndpoint { get; set; } = string.Empty;
    [JsonPropertyName("authorization_endpoint")]
    public string AuthorizationEndpoint { get; set; } = string.Empty;
    [JsonPropertyName("device_authorization_endpoint")]
    public string DeviceAuthorizationEndpoint { get; set; } = string.Empty;
    [JsonPropertyName("http_logout_supported")]
    public bool HttpLogoutSupported { get; set; }
    [JsonPropertyName("frontchannel_logout_supported")]
    public bool FrontChannelLogoutSupported { get; set; }
    [JsonPropertyName("end_session_endpoint")]
    public string EndSessionEndpoint { get; set; } = string.Empty;
    [JsonPropertyName("claims_supported")]
    public string[] ClaimsSupported { get; set; }
    [JsonPropertyName("kerberos_endpoint")]
    public string KerberosEndpoint { get; set; } = string.Empty;
    [JsonPropertyName("tenant_region_scope")]
    public string TenantRegionScope { get; set; } = string.Empty;
    [JsonPropertyName("cloud_instance_name")]
    public string CloudInstanceName { get; set; } = string.Empty;
    [JsonPropertyName("cloud_graph_host_name")]
    public string CloudGraphHostName { get; set; } = string.Empty;
    [JsonPropertyName("msgraph_host")]
    public string MsGraphHost { get; set; } = string.Empty;
    [JsonPropertyName("rbac_url")]
    public string RbacUrl { get; set; } = string.Empty;

}
