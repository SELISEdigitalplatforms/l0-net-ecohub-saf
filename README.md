# SeliseBlocks.Ecohub.SAF

[![NuGet Version](https://img.shields.io/nuget/v/SeliseBlocks.Ecohub.SAF?style=flat-square)](https://www.nuget.org/packages/SeliseBlocks.Ecohub.SAF/1.0.0-beta.4)

## Overview

`SeliseBlocks.Ecohub.SAF` is a .NET library for integrating with the SAF (Secure Access Facility) platform. It provides comprehensive services for:

- Authentication and token management
- Public key operations
- Event handling via REST or Kafka

## Table of Contents

1. [Installation](#installation)
2. [Getting Started](#getting-started)
3. [Features and Usage](#features-and-usage)
   - [Authentication (ISafAuthService)](#1-authentication-isafauthservice)
   - [SAF API Interactions (ISafApiService)](#2-saf-api-interactions-isafapiservice)
   - [SAF Event Handling (ISafRestProxyEventHandler, ISafKafkaEventHandler)](#3-saf-event-handling)
4. [Response Structure](#response-structure)
5. [Error Handling](#error-handling)
6. [Dependencies](#dependencies)

---

## Installation

To install the `SeliseBlocks.Ecohub.SAF` library, use the following command:

```bash
dotnet add package SeliseBlocks.Ecohub.SAF
```

---

## Getting Started

### Register Dependencies

Before using the library, register its services in your application's dependency injection container:

```csharp
services.AddSafDriverServices("https://your-saf-api-base-url");
```

This registers:

- `ISafAuthService`
- `ISafApiService`
- `ISafRestProxyEventHandler`
- `ISafKafkaEventHandler`

---

## Features and Usage

### 1. Authentication (`ISafAuthService`)

All authentication responses have the following structure:

- `IsSuccess`: Indicates if the operation succeeded.
- `Data`: The result data (type depends on the operation).
- `Error`: Error details if the operation failed.

#### Tech User Enrolment

```csharp
Task<SafTechUserEnrolmentResponse> EnrolTechUserAsync(SafTechUserEnrolmentRequest request);
```

- **Request**: `SafTechUserEnrolmentRequest`  
  - `Iak` (required)
  - `IdpUserId` (required)
  - `LicenceKey` (required)
  - `Password` (required)
  - `RequestId`
  - `RequestTime`
  - `UserAgent`
- **Response**: `SafTechUserEnrolmentResponse`
  - `IsSuccess`
  - `Data` (`SafTechUserEnrolment`): Contains `TechUserCert`, `OAuth2` credentials
  - `Error` (`SafError`): Error details if any

**Example:**

```csharp
var request = new SafTechUserEnrolmentRequest
{
    Iak = "your-iak",
    IdpUserId = "your-user-id",
    LicenceKey = "your-licence-key",
    Password = "your-password",
    RequestId = Guid.NewGuid().ToString(),
    RequestTime = DateTime.UtcNow.ToString("o"),
    UserAgent = new SafUserAgent { Name = "Chrome", Version = "126.0.6478.270" }
};
var response = await authService.EnrolTechUserAsync(request);
if (response.IsSuccess)
{
    // response.Data is SafTechUserEnrolment
    Console.WriteLine($"Tech User Cert: {response.Data.TechUserCert}");
    Console.WriteLine($"Client ID: {response.Data.OAuth2.ClientId}");
}
else
{
    // response.Error is SafError
    Console.WriteLine($"Error: {response.Error?.ErrorMessage}");
}
```

#### Get OpenID Configuration

```csharp
Task<SafOpenIdConfigurationResponse> GetOpenIdConfigurationAsync(Uri openIdUrl);
```

- **Response**: `SafOpenIdConfigurationResponse`
  - `IsSuccess`
  - `Data` (`SafOpenIdConfiguration`): OpenID configuration
  - `Error` (`SafError`)

#### Retrieve Bearer Token

```csharp
Task<SafBearerTokenResponse> GetBearerToken(SafBearerTokenRequest request);
```

- **Response**: `SafBearerTokenResponse`
  - `IsSuccess`
  - `Data` (`SafBearerToken`): access token, token type, expiration
  - `Error` (`SafError`)

---

### 2. SAF API Interactions (`ISafApiService`)

All API responses have the following structure:

- `IsSuccess`
- `Data`
- `Error`

#### Retrieve Receivers

```csharp
Task<SafReceiversResponse> GetReceiversAsync(SafReceiversRequest request);
```

- **Response**: `SafReceiversResponse`
  - `IsSuccess`
  - `Data` (`IEnumerable<SafReceiver>`)
  - `Error` (`SafError`)

#### Retrieve Member Public Key

```csharp
Task<SafMemberPublicKeyResponse> GetMemberPublicKey(string bearerToken, string idpNumber);
```

- **Response**: `SafMemberPublicKeyResponse`
  - `IsSuccess`
  - `Data` (`SafMemberPublicKey`)
  - `Error` (`SafError`)

#### Upload Member Public Key

```csharp
Task<SafMemberPublicKeyResponse> UploadMemberPublicKey(SafMemberPublicKeyUploadRequest request);
```

- **Response**: `SafMemberPublicKeyResponse`
  - `IsSuccess`
  - `Data` (`SafMemberPublicKey`)
  - `Error` (`SafError`)

#### Retrieve Member's Encrypted Public Key

```csharp
Task<SafMemberGetEncryptedKeyResponse> GetMemberEncryptedPublicKey(string bearerToken, string keyId);
```

- **Response**: `SafMemberGetEncryptedKeyResponse`
  - `IsSuccess`
  - `Data` (`SafMemberGetEncryptedKey`)
  - `Error` (`SafError`)

#### Verify Member Decrypted Public Key

```csharp
Task<SafMemberVerifyDecryptedKeyResponse> VerifyMemberDecryptedPublicKey(SafMemberVerifyDecryptedKeyRequest request);
```

- **Response**: `SafMemberVerifyDecryptedKeyResponse`
  - `IsSuccess`
  - `Data` (`SafMemberVerifyDecryptedKey`)
  - `Error` (`SafError`)

#### Activate Member Public Key

```csharp
Task<SafDynamicResponse> ActivateMemberPublicKey(string bearerToken, string keyId);
```

- **Response**: `SafDynamicResponse`
  - `IsSuccess`
  - `Data` (`dynamic`)
  - `Error` (`SafError`)

---

### 3. SAF Event Handling

All event handler responses have the following structure:

- `IsSuccess`
- `Data`
- `Error`

#### REST Proxy Event Handling (`ISafRestProxyEventHandler`)

- **Send Offer NLPI Event**

  ```csharp
  Task<SafSendOfferNlpiEventResponse> SendOfferNlpiEventAsync(SafSendOfferNlpiEventRequest request);
  ```

  - **Response**: `SafSendOfferNlpiEventResponse`
    - `IsSuccess`
    - `Data` (`SafSendOfferNlpiEvent`)
    - `Error` (`SafError`)

- **Receive Offer NLPI Event**

  ```csharp
  Task<SafReceiveOfferNlpiEventResponse> ReceiveOfferNlpiEventAsync(SafReceiveOfferNlpiEventRequest request);
  ```

  - **Response**: `SafReceiveOfferNlpiEventResponse`
    - `IsSuccess`
    - `Data` (`IEnumerable<SafOfferNlpiEvent>`)
    - `Error` (`SafError`)

#### Kafka Event Handling (`ISafKafkaEventHandler`)

- **Produce Event**

  ```csharp
  Task<SafProduceEventResponse> ProduceEventAsync(SafProduceKafkaEventRequest request);
  ```

  - **Response**: `SafProduceEventResponse`
    - `IsSuccess`
    - `Data` (`bool`)
    - `Error` (`SafError`)

- **Consume Event**

  ```csharp
  SafConsumeEventResponse ConsumeEvent(SafConsumeKafkaEventRequest request);
  ```
  
  - **Response**: `SafConsumeEventResponse`
    - `IsSuccess`
    - `Data` (`SafOfferNlpiEvent`)
    - `Error` (`SafError`)

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
