# MenuAPI

**FiveM & RedM C# Menu API**

[![Discord](https://discordapp.com/api/guilds/285424882534187008/widget.png)](https://vespura.com/discord) [![CodeFactor](https://www.codefactor.io/repository/github/tomgrobbe/menuapi/badge)](https://www.codefactor.io/repository/github/tomgrobbe/menuapi) [![Build status](https://ci.appveyor.com/api/projects/status/8nqoeyg0e9rn10ih/branch/master?svg=true)](https://ci.appveyor.com/project/TomGrobbe/menuapi/branch/master) [![Patreon](https://img.shields.io/badge/donate-Patreon-orange.svg)](https://www.patreon.com/vespura)

Designed specifically as a replacement of **NativeUI**, MenuAPI features performance improvements, **RedM** _and_ **FiveM** support, improved stability, better features, less bugs, full safezone alignment support for both left and right algined menus (FiveM only) and less (in my opinion) unnecessary features.

This has been coded from the ground up. Using decompiled scripts from GTA V & RedM to figure out what some undocumented natives were used for.

## Installation

_Note, this is only for resource developers, don't install this on your server manually if you're not making a resource with it._

You have 2 options:

1. Download the latest release zip and use the correct version (FiveM/RedM) for your resource. Simply include the DLL as a reference in your C# project and add `using MenuAPI;` to each file where you need to use MenuAPI.
2. Use the NuGet package, which can be found [here](https://www.nuget.org/packages/MenuAPI.FiveM/) for FiveM, and [here](https://www.nuget.org/packages/MenuAPI.RedM/) for RedM.

After doing either of the above and you're ready to build and publish your resource, add `files {'MenuAPI.dll'}` to your `fxmanifest.lua` file, and make sure that you include the `MenuAPI.dll` file in the folder of your resource.

## Documentation

Limited documentation is available [here](https://docs.vespura.com/mapi).

Feel like contributing to the documentation? The repository for the documentation site can be found [here](https://github.com/TomGrobbe/MenuAPI-Docs), thanks!

## Copyright / License

Copyright © Tom Grobbe 2018-2021.

MenuAPI is a free resource, using a custom license.
Conditions are listed below.

### You are allowed to

- Include the pre-built files in your projects, for both commercial and non-commercial use
- Modify this code, feel free to create PR's :)

### You are NOT allowed to

- Sell this code or a modified version of it.
- If you release a paid resource that uses MenuAPI, you are not allowed to include MenuAPI in the resource. You will need to provide a free way for anyone to download the MenuAPI version of your resource.
- Re-release this code without using the Fork feature.

### You must

- Provide appropritate credits when including this in your project.
- State any changes you made if you want to re-release this code.
- Keep this license when editing the source code or using this code in your own projects.

### In short

It's very simple, don't steal my stuff, don't try to take credit for code that isn't yours and don't try to make money using my work. That's all I ask.

If you'd like to do something that's not allowed per this license, contact me and we might be able to figure something out.

Nothing is guaranteed to work, I do not take responsibility for any bugs or damages caused by this code. Use at your own risk.
