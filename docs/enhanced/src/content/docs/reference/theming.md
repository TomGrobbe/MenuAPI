---
title: "Theming and banners"
---

## Theming and banners

Two ways to make the menu look like yours. A **theme** is a stylesheet you ship that overrides
MenuAPI's colours, radius, shadow and font. A **banner** is an image file that replaces the picture
across the top of a menu.

:::caution
Both are NUI render mode only. In native mode the game draws the menu and there is nothing to attach
to. See [NUI render mode](../../nui/).
:::

----

# Themes

Your stylesheet is loaded after MenuAPI's, so it wins on equal specificity with no `!important`.
Anything you do not set keeps the default, which means a theme touching one token stays valid across
updates.

Themes are for recolouring and reskinning. Layout is measured against the real game menu, so moving
or resizing things is not supported and not protected against.

## Write one

```css
/* themes/midnight.css */

#menuapi {
  --menuapi-rows-bg: rgb(12 18 38 / 82%);
  --menuapi-row-selected-bg: rgb(84 148 255 / 92%);
  --menuapi-row-selected-text: rgb(255 255 255);
}
```

The `#menuapi` selector is required, that is where the tokens live.

Add the folder to `fxmanifest.lua`, or nothing will load:

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

Paths are relative to your `index.html`. The new sheet is loaded before the old one is dropped, so a
swap never flashes.

| Member | Notes |
| --- | --- |
| `RegisterTheme(name, path)` | Same name again repoints it |
| `UnregisterTheme(name)` | `false` if it was not registered. Clears the applied theme if it was that one |
| `SetTheme(name)` | `null` for the default. Throws `ArgumentException` on an unregistered name |
| `Theme` | Applied theme name, or `null` |
| `Themes` | Registered names |
| `NuiTuning.Reset()` | Clears the applied theme, keeps registrations |

For a single fixed theme, skip the C# and add a `<link>` to `index.html` after MenuAPI's two
stylesheets.

## Tokens

Defaults are the game's own values.

### Shape, shadow, font

| Token | Default |
| --- | --- |
| `--menuapi-radius` | `0px` |
| `--menuapi-shadow` | `none` |
| `--menuapi-font` | `"GTA Chalet", "Helvetica Neue", Arial, sans-serif` |

The banner title font is not themeable. Each title font has a measured size and vertical nudge, so
swapping only the family misaligns it. Use `Menu.TitleFont`.

### Subtitle

| Token | Default |
| --- | --- |
| `--menuapi-subtitle-bg` | `rgb(0 0 0 / 98%)` |
| `--menuapi-subtitle-text` | menu text colour |
| `--menuapi-freemode` | game's freemode colour |

`--menuapi-freemode` replaces `--menuapi-subtitle-text` on a freemode subtitle. Left alone it is the
GTA Online blue.

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

Per item colours set from C# are written as inline styles and always beat the theme, deliberately.
That covers `MenuItem` text colour and `MenuSliderItem.BackgroundColor` / `BarColor`. Stop setting
them in C# if you want the theme to own them.

----

# Banners

Both routes use the same folder, `menuapi-banners`, next to your `fxmanifest.lua`.

```lua
files {
    'menuapi-banners/**/*',
}
```

Not inside `menuapi`, that folder is rewritten on every build.

## Replace a game sprite

Name the file after the sprite. Dictionary is the folder, texture is the file name. Every menu
drawing that sprite picks it up, no C#.

```
menuapi-banners/commonmenu/interaction_bgd.png    <- the default banner
menuapi-banners/my_textures/my_banner.png         <- a Menu.HeaderTexture you set
```

## Point one menu at your own image

Name it what you like, set `Menu.HeaderImage`.

```cs
Menu menu = new Menu("Outlaws", "Gang menu") { HeaderImage = "outlaw" };

menu.HeaderImage = "gangs/outlaw";   // subfolders are fine
menu.HeaderImage = null;             // back to whatever it would have shown
```

Extension optional, `"outlaw.png"` works too.

## An image in another resource

A full url is taken as it is, so a banner does not have to live in the resource drawing the menu:

```cs
menu.HeaderImage = "https://cfx-nui-my-theme-pack/banners/outlaw.png";
```

That is how a resource can hand a menu a banner of its own without anything being copied around. The
file still has to be in that resource's `files`, and only `http`, `https` and `nui` urls count as
full ones, everything else is a name inside `menuapi-banners` as above.

## Resolution order

First one that exists on disk wins:

1. `Menu.HeaderImage`
2. An image named after the game sprite
3. The game sprite

A missing file falls through, so a typo leaves the old banner rather than an empty header.

## Files

`.png`, `.jpg`, `.webp`, tried in that order. Use png for transparency.

The banner is 500x110 at 1080p and the image is stretched to fill, so roughly **1000x220** keeps it
sharp on bigger screens. Match the aspect, a square image will look squashed.

A menu created with a `null` title has no banner to replace.

## When something is wrong

| Symptom | Cause |
| --- | --- |
| Nothing changes at all | Native render mode, or the folder is not in `files` |
| Still the game artwork | Wrong path, it is case sensitive |
| Your file vanished | You put it inside `menuapi` |

The default banner is resolved at page load so it never flickers. Others resolve the first time a
menu wanting them opens, then are remembered for the life of the page.
