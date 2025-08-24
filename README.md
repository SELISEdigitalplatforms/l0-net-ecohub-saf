
# SeliseBlocks.Ecohub.SAF

[![NuGet Version](https://img.shields.io/nuget/v/SeliseBlocks.Ecohub.SAF?style=flat-square)](https://www.nuget.org/packages/SeliseBlocks.Ecohub.SAF/1.0.0-beta.4)

## Overview

`SeliseBlocks.Ecohub.SAF` is a .NET library for integrating with the SAF (Standard API Framework) platform. It provides comprehensive services for:

- Authentication and token management
- Public key operations
- Event handling via REST or Kafka

## Table of Contents

- [Installation](#installation)
- [Getting Started](#getting-started)
  - [Register Dependencies](#register-dependencies)
- [Features and Usage](#features-and-usage)
  - [1. Authentication](#1-authentication-isafauthservice)
  - [2. General API Interactions](#2-general-api-interactions-isafgeneralapiservice)
  - [3. Public Key Operations](#3-public-key-operations-isafpkiapiservice)
  - [4. REST Proxy Event Handling](#4-rest-proxy-event-handling-isafrestproxyeventhandler)
  - [5. Kafka Event Handling](#5-kafka-event-handling-isafkafkaeventhandler)
- [Response Structure](#response-structure)
- [Error Handling](#error-handling)
- [Dependencies](#dependencies)

## Installation

Install the NuGet package:

```sh
dotnet add package SeliseBlocks.Ecohub.SAF --version 8.0.0-beta.1
```

Or via the NuGet Package Manager:

```sh
Install-Package SeliseBlocks.Ecohub.SAF -Version 8.0.0-beta.1
```

## Getting Started

### Register Dependencies

Before using the library, register its services in your application's dependency injection container:

```csharp
services.AddSafDriverServices("https://your-saf-api-base-url");
```

This registers:

- `ISafAuthService`
- `ISafGeneralApiService`
- `ISafPkiApiService`
- `ISafRestProxyEventHandler`
- `ISafKafkaEventHandler`

---

## Features and Usage

### 1. Authentication (`ISafAuthService`)

The `ISafAuthService` interface provides methods for authenticating with the SAF platform and obtaining bearer tokens.

#### EnrolTechUserAsync

```csharp
Task<SafTechUserEnrolmentResponse> EnrolTechUserAsync(SafTechUserEnrolmentRequest request);
```

Enrolls a technical user in the SAF system and retrieves credentials for API access.

**Parameters:**

- `SafTechUserEnrolmentRequest request`:
  - `Iak` (string, required): The IAK identifier for the technical user.
  - `IdpUserId` (string, required): The IDP user identifier.
  - `LicenceKey` (string, required): The license key for the SAF system.
  - `Password` (string, required): The password for the technical user.
  - `RequestId` (string, optional): Unique identifier for the request.
  - `RequestTime` (string, optional): Timestamp of the request (ISO 8601 format recommended).
  - `UserAgent` (SafUserAgent, optional): Information about the user agent making the request.

**Response:**

- Returns a `SafTechUserEnrolmentResponse` object:
  - `IsSuccess` (bool): Indicates if the enrollment was successful.
  - `Data` (SafTechUserEnrolmentData): Contains enrollment details, such as:
    - `Certificate` (string): The technical user's certificate (PEM or base64).
    - `OAuth2Credentials` (object): OAuth2 credentials for API access.
    - Additional technical user information as provided by SAF.
  - `Error` (SafError): Error details if the enrollment failed.

**Example:**

```csharp
var enrolRequest = new SafTechUserEnrolmentRequest
{
    Iak = "your-iak",
    IdpUserId = "your-idp-user-id",
    LicenceKey = "your-licence-key",
    Password = "your-password",
    RequestId = Guid.NewGuid().ToString(),
    RequestTime = DateTime.UtcNow.ToString("o"),
    UserAgent = new SafUserAgent { Name = "MyApp", Version = "1.0.0" }
};
var enrolResponse = await authService.EnrolTechUserAsync(enrolRequest);
```

#### GetOpenIdConfigurationAsync

```csharp
Task<SafOpenIdConfigurationResponse> GetOpenIdConfigurationAsync(Uri openIdUrl);
```

Retrieves the OpenID configuration from the specified URL.

**Parameters:**

- `openIdUrl` (Uri, required): The URL of the OpenID configuration endpoint (usually ends with "/.well-known/openid-configuration").

**Response:**

- Returns a `SafOpenIdConfigurationResponse` object:
  - `IsSuccess` (bool): Operation status.
  - `Data` (SafOpenIdConfiguration): The OpenID configuration details, including:
    - `TokenEndpoint` (string): The OAuth2 token endpoint URL.
    - `TokenEndpointAuthMethodsSupported` (string[]): Supported authentication methods for the token endpoint.
    - `JwksUri` (string): The JWKS (JSON Web Key Set) URI.
    - `ResponseModesSupported` (string[]): Supported response modes.
    - `SubjectTypesSupported` (string[]): Supported subject types.
    - `IdTokenSigningAlgValuesSupported` (string[]): Supported ID token signing algorithms.
    - `ResponseTypesSupported` (string[]): Supported response types.
    - `ScopesSupported` (string[]): Supported scopes.
    - `Issuer` (string): The issuer identifier.
    - `RequestUriParameterSupported` (bool): Whether request_uri parameter is supported.
    - `UserinfoEndpoint` (string): The userinfo endpoint URL.
    - `AuthorizationEndpoint` (string): The authorization endpoint URL.
    - `DeviceAuthorizationEndpoint` (string): The device authorization endpoint URL.
    - `HttpLogoutSupported` (bool): Whether HTTP logout is supported.
    - `FrontChannelLogoutSupported` (bool): Whether front-channel logout is supported.
    - `EndSessionEndpoint` (string): The end session endpoint URL.
    - `ClaimsSupported` (string[]): Supported claims.
    - `KerberosEndpoint` (string): The Kerberos endpoint URL.
    - `TenantRegionScope` (string): The tenant region scope.
    - `CloudInstanceName` (string): The cloud instance name.
    - `CloudGraphHostName` (string): The cloud graph host name.
    - `MsGraphHost` (string): The Microsoft Graph host name.
    - `RbacUrl` (string): The RBAC URL.
  - `Error` (SafError): Error details if the operation failed.

**Example:**

```csharp
var openIdUrl = new Uri("https://your-saf-api.com/.well-known/openid-configuration");
var response = await authService.GetOpenIdConfigurationAsync(openIdUrl);
```

#### GetBearerToken

```csharp
Task<SafBearerTokenResponse> GetBearerToken(SafBearerTokenRequest request);
```

Obtains a bearer token from the SAF API for authentication.

**Parameters:**

- `SafBearerTokenRequest request`:
  - `RequestUrl` (string, required): The endpoint URL for obtaining a bearer token.
  - `Body` (SafAccessTokenRequestBody, required):
    - `GrantType` (string, required): The OAuth2 grant type (e.g., "client_credentials").
    - `ClientId` (string, required): The client ID for authentication.
    - `ClientSecret` (string, required): The client secret for authentication.
    - `Scope` (string, optional): The requested scope.

**Response:**

- Returns a `SafBearerTokenResponse` object:
  - `IsSuccess` (bool): Operation status.
  - `Data` (SafBearerToken): Contains the access token, token type, expiration, etc.
  - `Error` (SafError): Error details if the operation failed.

**Example:**

```csharp
var request = new SafBearerTokenRequest
{
    RequestUrl = "https://your-saf-api.com/oauth2/token",
    Body = new SafAccessTokenRequestBody
    {
        GrantType = "client_credentials",
        ClientId = "your-client-id",
        ClientSecret = "your-client-secret",
        Scope = "your-scope"
    }
};
var response = await authService.GetBearerToken(request);
```

### 2. General API Interactions (`ISafGeneralApiService`)

The `ISafGeneralApiService` interface provides methods to interact with the SAF API, including retrieving receiver information.

#### GetReceiversAsync

```csharp
Task<SafReceiversResponse> GetReceiversAsync(SafReceiversRequest request);
```

Retrieves receiver information from the SAF API.

**Parameters:**

- `SafReceiversRequest request`:
  - `BearerToken` (string, required): Authentication token.
  - `Payload` (SafReceiversRequestPayload, required):
    - `LicenceKey` (string, required): The licence key.
    - `Password` (string, required): The password.
    - `RequestId` (string): Unique request identifier.
    - `RequestTime` (string): Request timestamp.
    - `UserAgent` (SafUserAgent): User agent information. The `SafUserAgent` object has the following properties:
      - `Name` (string): The name of the user agent (e.g., browser or client name).
      - `Version` (string): The version of the user agent.

**Response:**

- Returns a `SafReceiversResponse` object:
  - `IsSuccess` (bool): Operation status.
  - `Data` (IEnumerable<SafReceiver>): Collection of receiver objects if successful. Each `SafReceiver` contains:
    - `Idp` (IEnumerable<string>): List of IDP identifiers associated with the receiver.
    - `CompanyName` (string): The company name of the receiver.
    - `MemberType` (string): The member type of the receiver.
    - `SupportedProcesses` (IEnumerable<SafSupportedProcess>): List of supported processes. Each `SafSupportedProcess` contains:
      - `ProcessName` (string): Name of the supported process.
      - `ProcessVersion` (string): Version of the supported process.
  - `Error` (SafError): Error details if the operation failed.

**Example:**

```csharp
var request = new SafReceiversRequest
{
    BearerToken = "your-bearer-token",
    Payload = new SafReceiversRequestPayload
    {
        LicenceKey = "your-licence-key",
        Password = "your-password",
        RequestId = Guid.NewGuid().ToString(),
        RequestTime = DateTime.UtcNow.ToString("o"),
        UserAgent = new SafUserAgent { Name = "Chrome", Version = "126.0.6478.270" }
    }
};
var response = await safGeneralApiService.GetReceiversAsync(request);
```

---

### 3. Public Key Operations (`ISafPkiApiService`)

All PKI responses have the following structure:

- `IsSuccess`: Indicates if the operation succeeded.
- `Data`: The result data (type depends on the operation).
- `Error`: Error details if the operation failed.

#### UploadMemberPublicKey

```csharp
Task<SafMemberPublicKeysResponse> UploadMemberPublicKey(SafMemberPublicKeyUploadRequest request);
```

- **Request**: `SafMemberPublicKeyUploadRequest`
  - `BearerToken` (string, required): Authentication token.
  - `Payload` (IEnumerable<SafMemberPublicKeyUploadRequestPayload>, required): List of public key payloads to upload.
    - Each `SafMemberPublicKeyUploadRequestPayload` object contains:
      - `Version` (string, required): The version of the key. Used to distinguish between different key versions (e.g., "v1").
      - `Key` (string, required): The public key value. This should be the PEM-encoded or base64-encoded public key string.
      - `KeyType` (string, required): The type of key. Typical values include:
        - `"encryption"`: Used for encryption operations.
        - `"signing"`: Used for digital signature operations.
      - `ExpireInDays` (string, required): The number of days until the key expires. This is a string representing an integer (e.g., "365" for one year).
- **Response**: `SafMemberPublicKeysResponse`
  - `IsSuccess` (bool): Operation status.
  - `Error` (SafError): Error details if the operation failed.
  - `Data` (IEnumerable<SafMemberPublicKey>): List of uploaded public keys. Each `SafMemberPublicKey` contains:
    - `KeyType` (string)
    - `KeyId` (string)
    - `MembershipId` (string)
    - `Version` (string)
    - `Key` (string)
    - `CreatedAt`, `LastUpdatedAt`, `ActivatedAt`, `ExpiryDate` (DateTime)
    - `EcoHubStatus` (string)
    - `SupportedProcesses` (IEnumerable<SafSupportedProcess>): List of supported processes for the key
      - Each `SafSupportedProcess` contains:
        - `ProcessName` (string)
        - `ProcessVersion` (string)

**Example:**

```csharp
var request = new SafMemberPublicKeyUploadRequest
{
    BearerToken = "your-bearer-token",
    Payload = new List<SafMemberPublicKeyUploadRequestPayload>
    {
        new SafMemberPublicKeyUploadRequestPayload
        {
            Version = "v1",
            Key = "your-public-key",
            KeyType = "encryption",
            ExpireInDays = "365"
        }
    }
};
var response = await safPkiApiService.UploadMemberPublicKey(request);
```

#### VerifyMemberDecryptedPublicKey

```csharp
Task<SafMemberVerifyDecryptedKeyResponse> VerifyMemberDecryptedPublicKey(SafMemberVerifyDecryptedKeyRequest request);
```

- **Request**: `SafMemberVerifyDecryptedKeyRequest`
  - `BearerToken` (string, required): Authentication token.
  - `KeyId` (string, required): Key identifier to verify.
  - `Payload` (SafMemberVerifyDecryptedKeyRequestPayload, required):
    - `VerifiedContent` (string, required): The decrypted content to verify.
- **Response**: `SafMemberVerifyDecryptedKeyResponse`
  - `IsSuccess` (bool): Operation status.
  - `Error` (SafError): Error details if the operation failed.
  - `Data` (SafMemberVerifyDecryptedKey): Verification result object.

**Example:**

```csharp
var request = new SafMemberVerifyDecryptedKeyRequest
{
    BearerToken = "your-bearer-token",
    KeyId = "your-key-id",
    Payload = new SafMemberVerifyDecryptedKeyRequestPayload
    {
        VerifiedContent = "decrypted-content"
    }
};
var response = await safPkiApiService.VerifyMemberDecryptedPublicKey(request);
```

#### ActivateMemberPublicKey

```csharp
Task<SafDynamicResponse> ActivateMemberPublicKey(string bearerToken, string keyId);
```

- **Parameters:**
  - `bearerToken` (string, required): Authentication token.
  - `keyId` (string, required): Key identifier to activate.
- **Response**: `SafDynamicResponse`
  - `IsSuccess` (bool): Operation status.
  - `Error` (SafError): Error details if the operation failed.

**Example:**

```csharp
var response = await safPkiApiService.ActivateMemberPublicKey("your-bearer-token", "your-key-id");
```

#### DeactivatePublicKey

```csharp
Task<SafValidationResponse> DeactivatePublicKey(string bearerToken, string keyId);
```

- **Parameters:**
  - `bearerToken` (string, required): Authentication token.
  - `keyId` (string, required): Key identifier to deactivate.
- **Response**: `SafValidationResponse`
  - `IsSuccess` (bool): Operation status.
  - `Error` (SafError): Error details if the operation failed.

**Example:**

```csharp
var response = await safPkiApiService.DeactivatePublicKey("your-bearer-token", "your-key-id");
```

#### DeleteInactivePublicKey

```csharp
Task<SafValidationResponse> DeleteInactivePublicKey(string bearerToken, string keyId);
```

- **Parameters:**
  - `bearerToken` (string, required): Authentication token.
  - `keyId` (string, required): Key identifier to delete.
- **Response**: `SafValidationResponse`
  - `IsSuccess` (bool): Operation status.
  - `Error` (SafError): Error details if the operation failed.

**Example:**

```csharp
var response = await safPkiApiService.DeleteInactivePublicKey("your-bearer-token", "your-key-id");
```

#### GetMemberPublicKey

```csharp
Task<SafMemberPublicKeysResponse> GetMemberPublicKey(string bearerToken, string idpNumber);
```

- **Parameters:**
  - `bearerToken` (string, required): Authentication token.
  - `idpNumber` (string, required): IDP number to query.
- **Response**: `SafMemberPublicKeysResponse`
  - `IsSuccess` (bool): Operation status.
  - `Error` (SafError): Error details if the operation failed.
  - `Data` (IEnumerable<SafMemberPublicKey>): List of public keys for the member. Each `SafMemberPublicKey` object includes the following properties:

    - `KeyType` (string): The type of key, such as `"encryption"` or `"signing"`.
    - `KeyId` (string): Unique identifier for the key.
    - `MembershipId` (string): The membership ID associated with the key.
    - `Version` (string): The version of the key (e.g., `"v1"`).
    - `Key` (string): The public key value, typically PEM or base64-encoded.
    - `CreatedAt` (DateTime): Timestamp when the key was created.
    - `LastUpdatedAt` (DateTime): Timestamp when the key was last updated.
    - `ActivatedAt` (DateTime): Timestamp when the key was activated.
    - `ExpiryDate` (DateTime): Timestamp when the key will expire.
    - `EcoHubStatus` (string): Status of the key in the EcoHub system.
    - `SupportedProcesses` (IEnumerable<SafSupportedProcess>): List of supported processes for this key, where each process has:
      - **ProcessName** (string): Name of the supported process.
      - **ProcessVersion** (string): Version of the supported process.

**Example:**

```csharp
var response = await safPkiApiService.GetMemberPublicKey("your-bearer-token", "idp-number");
```

#### GetMemberEncryptedPublicKey

```csharp
Task<SafMemberGetEncryptedKeyResponse> GetMemberEncryptedPublicKey(string bearerToken, string keyId);
```

- **Parameters:**
  - `bearerToken` (string, required): Authentication token.
  - `keyId` (string, required): Key identifier to query.
- **Response**: `SafMemberGetEncryptedKeyResponse`
  - `IsSuccess` (bool): Operation status.
  - `Error` (SafError): Error details if the operation failed.
  - `Data` (SafMemberGetEncryptedKey): The encrypted key details, with the following properties:
    - `KeyId` (string): The unique identifier of the encrypted key.
    - `KeyType` (string): The type of the key (e.g., "encryption", "signing").
    - `VerificationContent` (string): The encrypted content or payload used for verification purposes.

**Example:**

```csharp
var response = await safPkiApiService.GetMemberEncryptedPublicKey("your-bearer-token", "your-key-id");
```

#### GetAllKeys

```csharp
Task<SafMemberPublicKeysResponse> GetAllKeys(string bearerToken);
```

- **Parameters:**
  - `bearerToken` (string, required): Authentication token.
- **Response**: `SafMemberPublicKeysResponse`
  - `IsSuccess` (bool): Operation status.
  - `Error` (SafError): Error details if the operation failed.
  - `Data` (IEnumerable<SafMemberPublicKey>): List of all public keys. Each `SafMemberPublicKey` contains:
    - `KeyType` (string): The type of key, such as `"encryption"` or `"signing"`.
    - `KeyId` (string): Unique identifier for the key.
    - `MembershipId` (string): The membership ID associated with the key.
    - `Version` (string): The version of the key (e.g., `"v1"`).
    - `Key` (string): The public key value, typically PEM or base64-encoded.
    - `CreatedAt` (DateTime): Timestamp when the key was created.
    - `LastUpdatedAt` (DateTime): Timestamp when the key was last updated.
    - `ActivatedAt` (DateTime): Timestamp when the key was activated.
    - `ExpiryDate` (DateTime): Timestamp when the key will expire.
    - `EcoHubStatus` (string): Status of the key in the EcoHub system.
    - `SupportedProcesses` (IEnumerable<SafSupportedProcess>): List of supported processes for this key, where each process has:
      - `ProcessName` (string): Name of the supported process.
      - `ProcessVersion` (string): Version of the supported process.

**Example:**

```csharp
var response = await safPkiApiService.GetAllKeys("your-bearer-token");
```

#### GetKeyDetails

```csharp
Task<SafMemberPublicKeyResponse> GetKeyDetails(string bearerToken, string keyId);
```

- **Parameters:**
  - `bearerToken` (required)
  - `keyId` (required)
- **Response**: `SafMemberPublicKeyResponse`
  - `IsSuccess` (bool): Operation status.
  - `Error` (SafError): Error details if the operation failed.
  - `Data` (`SafMemberPublicKey`)
    - `KeyType` (string): The type of key, such as `"encryption"` or `"signing"`.
    - `KeyId` (string): Unique identifier for the key.
    - `MembershipId` (string): The membership ID associated with the key.
    - `Version` (string): The version of the key (e.g., `"v1"`).
    - `Key` (string): The public key value, typically PEM or base64-encoded.
    - `CreatedAt` (DateTime): Timestamp when the key was created.
    - `LastUpdatedAt` (DateTime): Timestamp when the key was last updated.
    - `ActivatedAt` (DateTime): Timestamp when the key was activated.
    - `ExpiryDate` (DateTime): Timestamp when the key will expire.
    - `EcoHubStatus` (string): Status of the key in the EcoHub system.
    - `SupportedProcesses` (IEnumerable<SafSupportedProcess>): List of supported processes for this key, where each process has:
      - `ProcessName` (string): Name of the supported process.
      - `ProcessVersion` (string): Version of the supported process.

#### GetKeysByKeyType

```csharp
Task<SafMemberPublicKeysResponse> GetKeysByKeyType(string bearerToken, string idpNumber, string keyType);
```

- **Parameters:**
  - `bearerToken` (required)
  - `idpNumber` (required)
  - `keyType` (required)
- **Response**: `SafMemberPublicKeysResponse`
  - `IsSuccess` (bool): Operation status.
  - `Error` (SafError): Error details if the operation failed.
  - `Data` (IEnumerable<SafMemberPublicKey>): List of all public keys. Each `SafMemberPublicKey` contains:
    - `KeyType` (string): The type of key, such as `"encryption"` or `"signing"`.
    - `KeyId` (string): Unique identifier for the key.
    - `MembershipId` (string): The membership ID associated with the key.
    - `Version` (string): The version of the key (e.g., `"v1"`).
    - `Key` (string): The public key value, typically PEM or base64-encoded.
    - `CreatedAt` (DateTime): Timestamp when the key was created.
    - `LastUpdatedAt` (DateTime): Timestamp when the key was last updated.
    - `ActivatedAt` (DateTime): Timestamp when the key was activated.
    - `ExpiryDate` (DateTime): Timestamp when the key will expire.
    - `EcoHubStatus` (string): Status of the key in the EcoHub system.
    - `SupportedProcesses` (IEnumerable<SafSupportedProcess>): List of supported processes for this key, where each process has:
      - `ProcessName` (string): Name of the supported process.
      - `ProcessVersion` (string): Version of the supported process.

---

### 4. REST Proxy Event Handling (`ISafRestProxyEventHandler`)

All event handler responses have the following structure:

- `IsSuccess`
- `Data`
- `Error`

### Produce Http Event

```csharp
```csharp
Task<SafSendOfferNlpiEventResponse> ProduceEventAsync(SafSendOfferNlpiEventRequest request);
```

- **Request**: `SafSendOfferNlpiEventRequest`
  - `SchemaVersionId` (string, required): Schema version ID for the event.
  - `KeySchemaVersionId` (string, required): Schema version ID for the key.
  - `BearerToken` (string, required): Authentication token.
  - `EventPayload` (object, required): The event data to be encrypted and sent. This should be a `SafOfferNlpiEvent` object with the following structure:
    - `Id` (string): Unique identifier for the event.
    - `Source` (string): Source of the event.
    - `Specversion` (string): Specification version.
    - `Type` (string): Type of the event.
    - `DataContentType` (string): Content type of the data.
    - `DataSchema` (string): Schema of the data.
    - `Subject` (string): Subject of the event.
    - `Time` (string): Timestamp of the event.
    - `LicenceKey` (string): Licence key associated with the event.
    - `UserAgent` (SafUserAgent): Information about the user agent.
      - `Name` (string): Name of the user agent.
      - `Version` (string): Version of the user agent.
    - `EventReceiver` (SafEventReceiver): Information about the event receiver.
      - `Category` (string): Receiver category.
      - `Id` (string): Receiver identifier.
    - `EventSender` (SafEventSender): Information about the event sender.
      - `Category` (string): Sender category.
      - `Id` (string): Sender identifier.
    - `ProcessId` (string): Process identifier.
    - `ProcessStatus` (string): Status of the process.
    - `SubProcessName` (string): Name of the subprocess.
    - `ProcessName` (string): Name of the process.
    - `ProcessVersion` (string): Version of the process.
    - `SubProcessStatus` (string): Status of the subprocess.
    - `Data` (SafData): The main event payload, with the following properties:
      - `Payload` (byte[]): The encrypted payload data.
      - `PublicKey` (string): Public key used for encryption.
      - `EcPrivateKey` (string): EC private key (if applicable).
      - `Links` (List<SafLink>): Related links.
        - Each `SafLink` includes:
          - `Href` (string): Link URL.
          - `Rel` (string): Relation type.
          - `Description` (string): Description of the link.
      - `PublicKeyVersion` (string): Version of the public key.
      - `PayloadSignature` (string): Signature of the payload.
      - `SignatureKeyVersion` (string): Version of the signature key.
      - `Message` (string): Additional message or information.

- **Response**: `SafSendOfferNlpiEventResponse`
  - `IsSuccess` (bool): Indicates if the operation succeeded.
  - `Error` (SafError): Error details if the operation failed.
  - `Data` (object): Response data including:
    - `KeySchemaId` (string): Schema ID used for the key.
    - `ValueSchemaId` (string): Schema ID used for the value.
    - `Offsets` (IEnumerable): Collection of partition offsets and any errors.

**Example:**

```csharp
var request = new SafSendOfferNlpiEventRequest
{
    SchemaVersionId = "event-schema-version-id",
    KeySchemaVersionId = "key-schema-version-id",
    BearerToken = "your-bearer-token",
    EventPayload = new { /* your event data */ }
};

var response = await safRestProxyEventHandler.ProduceEventAsync(request);

```

### Consume Http Event

```csharp
```csharp
Task<SafReceiveOfferNlpiEventResponse> ConsumeEventAsync(SafReceiveOfferNlpiEventRequest request);
```

- **Request**: `SafReceiveOfferNlpiEventRequest`
  - `BearerToken` (string, required): Authentication token.
  - `EcohubId` (string, required): Target Ecohub identifier.
  - `PrivateKey` (string, required): Private key for decrypting the event payloads.
  - `AutoOffsetReset` (string, optional): Offset reset strategy, e.g., `"earliest"` or `"latest"`.

- **Response**: `SafReceiveOfferNlpiEventResponse`
  - `IsSuccess` (bool): Operation status.
  - `Error` (SafError): Error details if the operation failed.
  - `Data` (IEnumerable<SafOfferNlpiEvent>): Collection of decrypted offer NLPI event objects. Each `SafOfferNlpiEvent` contains:
    - `Data` (SafData): The main event payload, with the following properties:
      - `Payload` (byte[]): The encrypted or raw payload data.
      - `PublicKey` (string): The public key used for encryption or verification.
      - `EcPrivateKey` (string): The EC private key, if applicable.
      - `Links` (List<SafLink>): Related links. Each `SafLink` includes:
        - `Href` (string): Link URL.
        - `Rel` (string): Relation type.
        - `Description` (string): Description of the link.
      - `PublicKeyVersion` (string): Version of the public key.
      - `PayloadSignature` (string): Signature of the payload.
      - `SignatureKeyVersion` (string): Version of the signature key.
      - `Message` (string): Additional message or information.

**Example:**

```csharp
var request = new SafReceiveOfferNlpiEventRequest
{
    BearerToken = "your-bearer-token",
    EcohubId = "target-ecohub-id",
    PrivateKey = "your-private-key",
    AutoOffsetReset = "earliest"
};

var response = await safRestProxyEventHandler.ConsumeEventAsync(request);

```

### 5. Kafka Event Handling (`ISafKafkaEventHandler`)

### Produce Kafka Event

```csharp
Task<SafProduceEventResponse> ProduceEventAsync(SafProduceKafkaEventRequest request);
```

- **Request**: `SafProduceKafkaEventRequest`
  - `BearerToken` (required)
  - `Payload` (required): `SafOfferNlpiEvent`
    - `Id` (string): Unique identifier for the event.
    - `Source` (string): Source of the event.
    - `Specversion` (string): Specification version.
    - `Type` (string): Type of the event.
    - `DataContentType` (string): Content type of the data.
    - `DataSchema` (string): Schema of the data.
    - `Subject` (string): Subject of the event.
    - `Time` (string): Timestamp of the event.
    - `LicenceKey` (string): Licence key associated with the event.
    - `UserAgent` (SafUserAgent): Information about the user agent.
      - `Name` (string): Name of the user agent.
      - `Version` (string): Version of the user agent.
    - `EventReceiver` (SafEventReceiver): Information about the event receiver.
      - `Category` (string): Receiver category.
      - `Id` (string): Receiver identifier.
    - `EventSender` (SafEventSender): Information about the event sender.
      - `Category` (string): Sender category.
      - `Id` (string): Sender identifier.
    - `ProcessId` (string): Process identifier.
    - `ProcessStatus` (string): Status of the process.
    - `SubProcessName` (string): Name of the subprocess.
    - `ProcessName` (string): Name of the process.
    - `ProcessVersion` (string): Version of the process.
    - `SubProcessStatus` (string): Status of the subprocess.
    - `Data` (SafData): The main event payload, with the following properties:
      - `Payload` (byte[]): The encrypted payload data.
      - `PublicKey` (string): Public key used for encryption.
      - `EcPrivateKey` (string): EC private key (if applicable).
      - `Links` (List<SafLink>): Related links.
        - Each `SafLink` includes:
          - `Href` (string): Link URL.
          - `Rel` (string): Relation type.
          - `Description` (string): Description of the link.
      - `PublicKeyVersion` (string): Version of the public key.
      - `PayloadSignature` (string): Signature of the payload.
      - `SignatureKeyVersion` (string): Version of the signature key.
      - `Message` (string): Additional message or information.
- **Response**: `SafProduceEventResponse`
  - `IsSuccess` (bool): Operation status.
  - `Error` (SafError): Error details if the operation failed.
  - `Data` (`bool`)

### Consume Event

```csharp
SafConsumeEventResponse ConsumeEvent(SafConsumeKafkaEventRequest request);
```

- **Request**: `SafConsumeKafkaEventRequest`
  - `Payload` (required): `SafOfferNlpiEvent`
- **Response**: `SafConsumeEventResponse`
  - `IsSuccess` (bool): Operation status.
  - `Error` (SafError): Error details if the operation failed.
  - `Data` (SafData): The main event payload, with the following properties:
    - `Payload` (byte[]): The encrypted or raw payload data.
    - `PublicKey` (string): The public key used for encryption or verification.
    - `EcPrivateKey` (string): The EC private key, if applicable.
    - `Links` (List<SafLink>): Related links. Each `SafLink` includes:
      - `Href` (string): Link URL.
      - `Rel` (string): Relation type.
      - `Description` (string): Description of the link.
    - `PublicKeyVersion` (string): Version of the public key.
    - `PayloadSignature` (string): Signature of the payload.
    - `SignatureKeyVersion` (string): Version of the signature key.
    - `Message` (string): Additional message or information.

---

## Response Structure

**All responses in this library follow a consistent structure:**

```csharp
public class SafBaseResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public SafError? Error { get; set; }
}
```

- **IsSuccess**: `true` if the operation succeeded, otherwise `false`.
- **Data**: The result data of type `T`. This is only populated if `IsSuccess` is `true`.
- **Error**: An object of type `SafError` containing error details if the operation failed.

**Always check `IsSuccess` before using `Data`. If `IsSuccess` is `false`, check `Error` for details.**

---

## Error Handling

All errors are returned in the `Error` property of the response, which is of type `SafError`:

```csharp
public class SafError
{
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public string ErrorMessageId { get; set; }
}
```

- `ErrorCode`: A machine-readable error code.
- `ErrorMessage`: A human-readable error message.
- `ErrorMessageId`: An optional error message identifier.

---

## Dependencies

The following dependencies are required and are included in the NuGet package:

- .NET 8.0 or later
- [Confluent.Kafka](https://www.nuget.org/packages/Confluent.Kafka)
- [Confluent.SchemaRegistry](https://www.nuget.org/packages/Confluent.SchemaRegistry)
- [Confluent.SchemaRegistry.Serdes.Json](https://www.nuget.org/packages/Confluent.SchemaRegistry.Serdes.Json)
- [Microsoft.Extensions.DependencyInjection.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions)
- [Microsoft.Extensions.Http](https://www.nuget.org/packages/Microsoft.Extensions.Http)
- [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)
- [System.Text.Json](https://www.nuget.org/packages/System.Text.Json)

---
