---
title: "Setup"
---

## Setup

Add the [MenuAPI.RedM](https://www.nuget.org/packages/MenuAPI.RedM/) NuGet package:

```sh
dotnet add package MenuAPI.RedM
```

Then add `MenuAPI.dll` to the `files` list in your `fxmanifest.lua`:

```lua
files {
    'MenuAPI.dll',
}
```

The [API Reference](../reference/) has everything else.

Building for FiveM? See the [FiveM docs](../../fivem/).
