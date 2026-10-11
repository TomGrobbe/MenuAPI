---
title: "How menus are drawn"
---

## How menus are drawn

MenuAPI draws every menu with NUI, a web page FiveM shows on top of the game. The menu is styled to match the game's own menus. NUI is also what makes [themes and custom banners](../reference/theming/) possible.

:::note[Coming from an older MenuAPI?]
Drawing with the game's own drawing functions is gone. `MenuController.RenderMode` still compiles but does nothing. See the [Migration Guide (v4)](../migration/).
:::

----

### The page

The `index.html` from [Setup](../setup/) has three rules:

- **The background must stay transparent.** The page covers the whole screen.
- **`<div id="menuapi">` must exist** with exactly that id. The menu is built inside it.
- **Keep the script order.** `menuapi.min.js` depends on the other three, so it goes last.

You can add your own things to the page as long as these hold.

### The menuapi folder

The `menuapi` folder next to `MenuAPI.dll` holds the styles, scripts, fonts and images. It's replaced on every update, so don't edit it. The unminified sources are in the [repository](https://github.com/TomGrobbe/MenuAPI/tree/main/MenuAPI/ui).

### The manifest

```lua
ui_page 'index.html'

files {
    'MenuAPI.dll',
    'index.html',
    'menuapi/**/*',
}
```

----

## When something is wrong

| What you see | Cause |
| --- | --- |
| The screen turns black or one solid colour | The page has a background colour. It must be transparent. |
| No menu at all | `ui_page` is missing, `index.html` is not in `files`, or there is no `<div id="menuapi">` |
| The menu has no styling | `menuapi/**/*` is missing from `files`, or an `href` path is wrong |
| The fonts look wrong | Same as above, the fonts are in `menuapi/fonts` |
| No glow on the banner with `ShowHeaderGlare` on | The script order is wrong, or `glare.min.js` is missing |
