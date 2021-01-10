# MenuAPI

FiveM C# Menu API.

[![CodeFactor](https://www.codefactor.io/repository/github/tomgrobbe/menuapi/badge)](https://www.codefactor.io/repository/github/tomgrobbe/menuapi) [![Build status](https://ci.appveyor.com/api/projects/status/8nqoeyg0e9rn10ih/branch/master?svg=true)](https://ci.appveyor.com/project/TomGrobbe/menuapi/branch/master) [![Discord](https://discordapp.com/api/guilds/285424882534187008/widget.png)](https://vespura.com/discord) [![Patreon](https://img.shields.io/badge/donate-Patreon-orange.svg)](https://www.patreon.com/vespura)

Designed specifically as a replacement of NativeUI for vMenu with improved performance (somewhat), more features, less bugs, and easier to use functions (somewhat).

Full safezone scaling supported, both left and right aligned menus supported.

This has been coded from the ground up. Using GTA V Decompiled scripts to figure out what some undocumented natives were used for.

## Installation

_Note, this is only for resource developers, don't install this on your server manually if you're not making a resource with it._

You have 2 options:

1. Download the latest release zip and use the correct version (FiveM/RedM) for your resource. Simply include the DLL as a reference in your C# project and add `using MenuAPI;` to each file where you need to use MenuAPI.
2. Use the NuGet package, which can be found [here](https://www.nuget.org/packages/MenuAPI.FiveM/) for FiveM, and [here](https://www.nuget.org/packages/MenuAPI.RedM/) for RedM.

After doing either of the above and you're ready to build and publish your resource, add `files {'MenuAPI.dll'}` to your `fxmanifest.lua` file, and make sure that you include the `MenuAPI.dll` file in the folder of your resource.

## Documentation

Minimal documentation is available [here](https://docs.vespura.com/mapi).

Documentation contents will be improved/expanded in the (near) future.

## Copyright

Copyright © Tom Grobbe 2018-2021.

## License (custom license)

You can use this API in your own resources, you can edit this API if you want to add features. Feel free to PR them.

You can host re-releases of this API, but ONLY as a FORK created via GitHub. If it's not a forked repo, then the release will be taken down by DMCA request.

It's very simple, don't steal my stuff and try to take credit. That's all I ask.

When creating a resource, you are **not** required to mention/link this API, as long as you do not claim it to be your own work.
If you feel like it you can link it just because you're nice, but there's no need to do this. I'd appreciate it if you just put the link to this repo somewhere in your credits/readme file.:)
