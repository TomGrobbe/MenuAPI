---
title: "Theming and banners"
---

## Theming and banners

A **theme** is a stylesheet that overrides MenuAPI's colours, radius, shadow and font. A **banner** is
an image file that replaces the picture at the top of a menu.

----

# Themes

Your stylesheet loads after MenuAPI's, so it wins on equal specificity without `!important`. Tokens
you do not set keep their default.

Themes are for recolouring only. Moving or resizing things is not supported.

## Write one

```css
/* themes/midnight.css */

#menuapi {
  --menuapi-rows-bg: rgb(12 18 38 / 82%);
  --menuapi-row-selected-bg: rgb(84 148 255 / 92%);
  --menuapi-row-selected-text: rgb(255 255 255);
}
```

The `#menuapi` selector is required, the tokens live there. Add the folder to `fxmanifest.lua`:

```lua
files {
    'index.html',
    'menuapi/**/*',

    'themes/**/*',
}
```

## Apply it

```cs
NuiTuning.RegisterTheme("midnight", "themes/midnight.css");
NuiTuning.RegisterTheme("sunset", "themes/sunset.css");

NuiTuning.SetTheme("midnight");
NuiTuning.SetTheme("sunset");   // hot swap, applies next frame
NuiTuning.SetTheme(null);       // default look
```

Paths are relative to your `index.html`. The new sheet loads before the old one is dropped, so a swap
never flashes.

| Member | Notes |
| --- | --- |
| `RegisterTheme(name, path)` | Same name again repoints it |
| `UnregisterTheme(name)` | `false` if it was not registered. Clears the applied theme if it was that one |
| `SetTheme(name)` | `null` for the default. Throws `ArgumentException` on an unregistered name |
| `Theme` | Applied theme name, or `null` |
| `Themes` | Registered names |
| `NuiTuning.Reset()` | Clears the applied theme, keeps registrations |

For one fixed theme, skip the C# and add a `<link>` to `index.html` after MenuAPI's two stylesheets.

## Tokens

Defaults are the game's own values.

### Shape, shadow, font

| Token | Default |
| --- | --- |
| `--menuapi-radius` | `0px` |
| `--menuapi-shadow` | `none` |
| `--menuapi-font` | `"GTA Chalet", "Helvetica Neue", Arial, sans-serif` |

The banner title font is not themeable, because each title font has its own measured size and offset.
Use `Menu.MenuTitleFont`.

### Subtitle

| Token | Default |
| --- | --- |
| `--menuapi-subtitle-bg` | `rgb(0 0 0 / 98%)` |
| `--menuapi-subtitle-text` | menu text colour |
| `--menuapi-freemode` | game's freemode colour (GTA Online blue) |

`--menuapi-freemode` replaces `--menuapi-subtitle-text` on a freemode subtitle.

### Rows

| Token | Default |
| --- | --- |
| `--menuapi-rows-bg` | `rgb(0 0 0 / 70.5%)` |
| `--menuapi-row-text` | menu text colour |
| `--menuapi-row-selected-bg` | `rgb(255 255 255 / 88%)` |
| `--menuapi-row-selected-text` | `rgb(0 0 0)` |
| `--menuapi-row-disabled-text` | `rgb(109 109 109)` |
| `--menuapi-row-selected-disabled-text` | `rgb(50 50 50)` |

### Banner, slider, description

| Token | Default |
| --- | --- |
| `--menuapi-header-text` | menu text colour |
| `--menuapi-slider-divider` | `rgb(255 255 255)` |
| `--menuapi-overflow-bg` | `rgb(0 0 0 / 70.5%)` |
| `--menuapi-desc-bg` | `rgb(0 0 0 / 70.5%)` |
| `--menuapi-desc-rule` | `rgb(0 0 0 / 78%)` |

### Stats panels

| Token | Default |
| --- | --- |
| `--menuapi-stats-bg` | `rgb(0 0 0 / 70.5%)` |
| `--menuapi-stat-track` | `rgb(100 100 100 / 70.5%)` |
| `--menuapi-stat-value` | `rgb(255 255 255)` |
| `--menuapi-stat-upgrade` | `rgb(93 182 229)` |
| `--menuapi-stat-upgrade-reduced` | `rgb(224 50 50)` |

### Colour picker panel

| Token | Default |
| --- | --- |
| `--menuapi-panel-bg` | game panel colour |
| `--menuapi-panel-accent` | game accent, as `r, g, b` rather than a colour |
| `--menuapi-panel-text` | menu text colour |
| `--menuapi-panel-swatch-selected` | `rgb(255 255 255)` |
| `--menuapi-panel-opacity-bar` | `rgb(136 136 136)` |

## Out of reach

Colours set from C# are inline styles and always beat the theme. That covers `MenuItem` text colour
and `MenuSliderItem.BackgroundColor` / `BarColor`. Stop setting them in C# if the theme should own
them.

----

# Banners

Both routes use the `menuapi-banners` folder next to your `fxmanifest.lua`. Do not put it inside
`menuapi`, that folder is rewritten on every build.

```lua
files {
    'menuapi-banners/**/*',
}
```

## Replace a game sprite

Name the file after the sprite: the dictionary is the folder, the texture is the file name. Every
menu drawing that sprite picks it up, no C# needed.

```
menuapi-banners/commonmenu/interaction_bgd.png    <- the default banner
menuapi-banners/my_textures/my_banner.png         <- a Menu.HeaderTexture you set
```

## Point one menu at your own image

Set `Menu.HeaderImage` to any name. The extension is optional.

```cs
Menu menu = new Menu("Outlaws", "Gang menu") { HeaderImage = "outlaw" };

menu.HeaderImage = "gangs/outlaw";   // subfolders are fine
menu.HeaderImage = null;             // back to whatever it would have shown
```

## An image in another resource

An `http`, `https` or `nui` url is used as is, so the banner can live in another resource. It still
has to be in that resource's `files`. Anything else is a name inside `menuapi-banners`.

```cs
menu.HeaderImage = "https://cfx-nui-my-theme-pack/banners/outlaw.png";
```

## Resolution order

First one that exists on disk wins:

1. `Menu.HeaderImage`
2. An image named after the game sprite
3. The game sprite

A missing file falls through, so a typo leaves the old banner instead of an empty header.

## Files

`.png`, `.jpg`, `.webp`, tried in that order. Use png for transparency.

The banner is 500x110 at 1080p and the image is stretched to fill, so use roughly **1000x220** and
keep that aspect ratio.

A menu created with a `null` title has no banner to replace.

## When something is wrong

| Symptom | Cause |
| --- | --- |
| Nothing changes at all | The folder is not in `files` |
| Still the game artwork | Wrong path, it is case sensitive |
| Your file vanished | You put it inside `menuapi` |

The default banner is resolved at page load. Others resolve the first time a menu using them opens,
then are cached for the life of the page.
