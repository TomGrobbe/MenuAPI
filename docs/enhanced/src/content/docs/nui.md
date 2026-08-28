---
title: "NUI render mode"
---

## NUI render mode

MenuAPI draws menus either with the game's own natives (`MenuRenderMode.Native`, the default) or as a
NUI web page (`MenuRenderMode.Nui`). Both look the same out of the box, the page was measured against
the real menu. NUI mode is what makes [theming and custom banners](../reference/theming/) possible.

It needs a page to draw into, so there is a little setup.

## 1. index.html

Next to your `fxmanifest.lua`:

```html
<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8">
    <title>My Resource</title>
    <link rel="stylesheet" href="menuapi/colour-list.min.css">
    <link rel="stylesheet" href="menuapi/menuapi.min.css">
</head>

<body style="margin: 0; background: transparent; overflow: hidden;">

    <div id="menuapi"></div>

    <script src="menuapi/sprite-cache.min.js"></script>
    <script src="menuapi/glare.min.js"></script>
    <script src="menuapi/colour-list.min.js"></script>
    <script src="menuapi/menuapi.min.js"></script>
</body>

</html>
```

Three things are load bearing:

- **The body must stay transparent.** The page covers the screen over the game, so a background colour blacks the game out.
- **`<div id="menuapi">` must exist**, with that id. It is what the menu is built into.
- **Script order matters.** These are classic scripts sharing a global scope, and `menuapi.min.js` uses what the other three define.

The `menuapi` folder is produced by the build, from the NuGet package or a project reference. Do not
edit anything in it.

## 2. fxmanifest.lua

```lua
ui_page 'index.html'

files {
    'index.html',
    'menuapi/**/*',
}
```

FiveM serves nothing the manifest does not name.

## 3. Switch to it

```cs
MenuController.RenderMode = MenuRenderMode.Nui;
```

Switchable at runtime, an open menu follows.

## File names

The shipped css and js are minified and carry `.min` in their names. Only the minified files are
packed. Readable sources are in the
[repository](https://github.com/TomGrobbe/MenuAPI/tree/fivem-enhanced/MenuAPI/ui).

Coming from a page written before this change, add `.min` to all six paths. See the
[Migration guide](../migration/).

## When something is wrong

| Symptom | Cause |
| --- | --- |
| Screen is black or a solid colour | Body is not transparent |
| No menu at all | `ui_page` missing, `index.html` not in `files`, or no `<div id="menuapi">` |
| Menu renders unstyled | `menuapi/**/*` missing from `files`, or a wrong `href` |
| Wrong fonts | Same, the fonts are under `menuapi/fonts` |
| No banner glow | Script order wrong, or `glare.min.js` missing |
