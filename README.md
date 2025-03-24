
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
to Retrieve all SAF receivers which are available to the caller

```csharp
RetrieveTechUserCredentials();
```

to Retrieve a list of all insurers in SAF along with the standards they support

```csharp
RetrieveSafReceivers();
```

to Retrieve techUser credentials for SAF

```csharp
RetrieveSafInsurers();
```
