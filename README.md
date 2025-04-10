
# SeliseBlocks.Ecohub.SAF

## Overview

`SeliseBlocks.Ecohub.SAF` is a driver designed to integrate Ecohub SAF integration into your application.

## Installation

To install `SeliseBlocks.Ecohub.SAF`, add the NuGet package to your project:

```sh
dotnet add package SeliseBlocks.Ecohub.SAF
```

## Usage

### Register Dependencies

Before using `SeliseBlocks.Ecohub.SAF`, ensure that all required dependencies are registered in your application's dependency injection container. Add the following line in your `Program.cs`:

use below methods
to retrieve the bearer token use below method of the interface `ISafAuthService`

```csharp
GetBearerToken(SafBearerTokenRequest request);
```

to retrieve a list of all receivers use below method of the interface `ISafDriverService`

```csharp
GetReceiversAsync(SafReceiversRequest request);
```

