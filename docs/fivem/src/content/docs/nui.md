---
title: "How menus are drawn"
---

## How menus are drawn

MenuAPI draws every menu with **NUI**. NUI is a small web page that FiveM shows on top of the game. MenuAPI builds the menu inside that page and keeps it up to date while the player uses it. It looks just like the game's own menus, because the page was measured against them.

Drawing with NUI is also what makes [themes and custom banners](../reference/theming/) possible.

:::note[Coming from an older MenuAPI?]
Older versions could also draw menus with the game's own drawing functions, and that was even the default. That way of drawing is gone, NUI is the only one now. `MenuController.RenderMode` still exists so old code keeps compiling, but it does nothing anymore. See the [Migration guide](../migration/).
:::

The [Setup](../setup/) page shows the `index.html` page and the `fxmanifest.lua` lines you need. This page explains why they look the way they do.

----

### The page

Three things in `index.html` really matter:

- **The page must stay see through.** The page covers the whole screen, on top of the game. That is why the body has `background: transparent`. Any background colour would hide the game behind it.
- **`<div id="menuapi">` must exist**, with exactly that id. MenuAPI builds the menu inside that element.
- **The order of the scripts matters.** They share their variables with each other, and `menuapi.min.js` uses what the other three set up, so it has to come last.

You can add your own things to the page, like your own HUD. Just keep these three rules intact.

### The menuapi folder

When you build your resource, a folder called `menuapi` appears next to `MenuAPI.dll`. It holds the styles, scripts, fonts and images the page uses. It comes from the NuGet package, or from the zip if you downloaded that. Do not edit anything in it, because it is replaced every time you update MenuAPI.

The css and js files are minified, which means all spaces and comments are stripped out to make them smaller, and their names end in `.min`. The readable versions are in the [repository](https://github.com/TomGrobbe/MenuAPI/tree/main/MenuAPI/ui).

### The manifest

```lua
ui_page 'index.html'

files {
    'MenuAPI.dll',
    'index.html',
    'menuapi/**/*',
}
```

FiveM only sends files to players when the manifest names them. `menuapi/**/*` means "everything inside the menuapi folder, including its subfolders".

----

## When something is wrong

| What you see | What is causing it |
| --- | --- |
| The screen turns black or one solid colour | The page has a background colour. It must be transparent. |
| No menu at all | `ui_page` is missing, `index.html` is not in `files`, or there is no `<div id="menuapi">` |
| The menu shows up, but without any styling | `menuapi/**/*` is missing from `files`, or one of the `href` paths is wrong |
| The fonts look wrong | Same as above, the fonts live in `menuapi/fonts` |
| No glow on the banner | The script order is wrong, or `glare.min.js` is missing |
