---
title: "Setup"
---

## Setup

_Note, this is only for resource developers, don't install this on your server manually if you're not making a resource with it._

:::caution[Pre-release]
MenuAPI for FiveM Enhanced is a work in progress. Releases are published as a **pre-release** (`MenuAPI.FiveM.Enhanced`, currently `0.0.1-alpha`), so enable "include prerelease" in your NuGet client, and expect breaking changes while it stabilises.
:::

You have 2 options:

1. Download the latest release zip and include the DLL as a reference in your C# project, then add `using MenuAPI;` to each file where you need to use MenuAPI.
2. Use the NuGet package, which can be found [here](https://www.nuget.org/packages/MenuAPI.FiveM.Enhanced/).

After doing either of the above and you're ready to build and publish your resource, add `files {'MenuAPI.dll'}` to your `fxmanifest.lua` or `__resource.lua` file, and make sure that you include the `MenuAPI.dll` file in the folder of your resource.

---

<details>
<summary>Old setup instructions</summary>

_These are the old instructions, they're still relevant but slightly outdated._

## 1. Adding the dependency

Just like any other API. Add it as a reference in your C# client project. Check the [FiveM docs](https://docs.fivem.net/) for info on how to setup a C# resource.

## 2. Using the API

Checkout the example menu provided in the download. It'll show you exactly what all the features are. The rest is up to you.

## 3. Where can I find a reference of all functions, classes and types?

A full [API Reference](../reference/) is available on these docs. You can also use the source code as a guide, since there are helpful comments throughout, and the example menu for some basic info. It's designed similar to NativeUI's structure, so if you're familiar with NativeUI (C# version) then this should be an easy switch.

</details>
