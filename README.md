# SeliseBlocks.Ecohub.SAF

## Overview

`SeliseBlocks.Ecohub.SAF` is a .NET library designed to integrate Ecohub SAF functionality into your application. It provides services for authentication, event handling, and API interactions with the SAF platform. This library simplifies the process of interacting with the SAF API by offering pre-built interfaces and models.

---

## Installation

To install the `SeliseBlocks.Ecohub.SAF` library, use the following command:

```bash
dotnet add package SeliseBlocks.Ecohub.SAF
```

This will add the library to your project as a NuGet package.

---

## Getting Started

### Register Dependencies

Before using the library, you need to register its services in your application's dependency injection container. Add the following line to your `Program.cs` file:

```csharp
services.RegisterSafDriverServices("https://your-saf-api-base-url");
```

This method registers the following services:

- `ISafAuthService`: Handles authentication with the SAF API.
- `ISafApiService`: Provides methods for interacting with the SAF API.
- `ISafEventService`: Manages SAF event handling.

---

## Features and Usage

### 1. Authentication

The `ISafAuthService` interface provides methods for handling authentication with the SAF API.

#### Retrieve Bearer Token

To obtain a bearer token, use the `GetBearerToken` method:

```csharp
Task<SafBearerTokenResponse> GetBearerToken(SafBearerTokenRequest request);
```

- **Parameters**:
  - `SafBearerTokenRequest`: Contains the request URL and body for obtaining the bearer token.
    - `RequestUrl`: The endpoint for obtaining the token.
    - `Body`: Includes authentication details such as `grantType`, `clientId`, `clientSecret`, and `scope`.

- **Returns**: A `SafBearerTokenResponse` object containing the bearer token and related information.

**Example**:

```csharp
var tokenRequest = new SafBearerTokenRequest
{
    RequestUrl = "https://your-saf-api-url/token",
    Body = new SafAccessTokenRequestBody
    {
        GrantType = "client_credentials",
        ClientId = "your-client-id",
        ClientSecret = "your-client-secret",
        Scope = "your-scope"
    }
};

var tokenResponse = await authService.GetBearerToken(tokenRequest);
Console.WriteLine($"Access Token: {tokenResponse.AccessToken}");
```

---

### 2. SAF API Interactions

The `ISafApiService` interface provides methods for interacting with the SAF API.

#### Retrieve Receivers

To retrieve a list of receivers, use the `GetReceiversAsync` method:

```csharp
Task<SafReceiversResponse> GetReceiversAsync(SafReceiversRequest request);
```

- **Parameters**:
  - `SafReceiversRequest`: Contains the request details for retrieving receiver information.

- **Returns**: A `SafReceiversResponse` object containing the list of receivers.

#### Retrieve Member Public Key

To retrieve the public key of a member, use the `GetMemberPublicKey` method:

```csharp
Task<SafMemberPublicKeyResponse> GetMemberPublicKey(string bearerToken, string idpNumber);
```

- **Parameters**:
  - `bearerToken`: The authentication token.
  - `idpNumber`: The IDP number of the member.

- **Returns**: A `SafMemberPublicKeyResponse` object containing the member's public key.

**Example**:

```csharp
var publicKeyResponse = await apiService.GetMemberPublicKey("your-bearer-token", "12345");
Console.WriteLine($"Public Key: {publicKeyResponse.Key}");
```

---

### 3. SAF Event Handling

The `ISafEventService` interface provides methods for sending and receiving SAF events.

#### Send Offer NLPI Event

To send an offer NLPI event, use the `SendOfferNlpiEventAsync` method:

```csharp
Task<SafSendOfferNlpiEventResponse> SendOfferNlpiEventAsync(SafSendOfferNlpiEventRequest request);
```

- **Parameters**:
  - `SafSendOfferNlpiEventRequest`: Contains the event payload, schema version IDs, and authentication details.

- **Returns**: A `SafSendOfferNlpiEventResponse` object containing schema IDs and offsets of the sent event.

**Example**:

```csharp
var sendRequest = new SafSendOfferNlpiEventRequest
{
    SchemaVersionId = "1",
    KeySchemaVersionId = "1",
    BearerToken = "your-bearer-token",
    EventPayload = new SafOfferNlpiEvent
    {
        Id = "event-id",
        Source = "source",
        Type = "type",
        Data = new SafData
        {
            Payload = Encoding.UTF8.GetBytes("your-payload"),
            PublicKey = "your-public-key"
        }
    }
};

var sendResponse = await eventService.SendOfferNlpiEventAsync(sendRequest);
Console.WriteLine($"Schema ID: {sendResponse.ValueSchemaId}");
```

#### Receive Offer NLPI Event

To receive an offer NLPI event, use the `ReceiveOfferNlpiEventAsync` method:

```csharp
Task<SafOfferNlpiEvent> ReceiveOfferNlpiEventAsync(SafReceiveOfferNlpiEventRequest request);
```

- **Parameters**:
  - `SafReceiveOfferNlpiEventRequest`: Contains the bearer token, Ecohub ID, offset reset configuration, and private key.

- **Returns**: A `SafOfferNlpiEvent` object containing the details of the received event.

**Example**:

```csharp
var receiveRequest = new SafReceiveOfferNlpiEventRequest
{
    BearerToken = "your-bearer-token",
    EcohubId = "your-ecohub-id",
    AutoOffsetReset = "earliest",
    PrivateKey = "your-private-key"
};

var receivedEvent = await eventService.ReceiveOfferNlpiEventAsync(receiveRequest);
Console.WriteLine($"Event ID: {receivedEvent.Id}");
```

---
