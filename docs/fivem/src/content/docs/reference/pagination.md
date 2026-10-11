---
title: "Pagination"
---

## Pagination

Pagination splits a long menu into **pages** of a fixed size. Left and right move between pages.

```cs
Menu menu = new Menu("Online Players", "Page 1 / 42");
MenuController.AddMenu(menu);

// 48 items per page. Left and right now turn the page.
menu.SetPageSize(48);
```

The page and the visible window are separate:

1. **The page** decides *which* items the menu works with. With a page size of 48, page 3 is items 97 to 144.
2. **The window** ([MaxItemsOnScreen](../menu/#setmaxitemsonscreenint-max), up to 10) decides how many of those you see at once. Up and down scroll within the page as usual.

----

### What left and right do

In a paginated menu, left and right on a plain button mean "previous page" and "next page" instead of "go back" and "select".

|Item type|Left / right in a paginated menu|
|-|-|
|[MenuItem](../menuitems/menuitem/)|Turns the page.|
|[MenuCheckboxItem](../menuitems/menucheckboxitem/)|Turns the page. Press select to toggle it.|
|[MenuListItem](../menuitems/menulistitem/)|Unchanged, still changes the value.|
|[MenuDynamicListItem](../menuitems/menudynamiclistitem/)|Unchanged, still changes the value.|
|[MenuSliderItem](../menuitems/menuslideritem/)|Unchanged, still moves the slider.|

A disabled row (`Enabled = false`) does not block paging.

Holding left or right repeats the page turn every 250ms after a 400ms first delay, and never speeds up.

----

### Properties

|Property|Type|Default value|Description|
|-|-|-|-|
|PageSize|int|0|(Getter only) Items per page. `0` means not paginated. Use [SetPageSize()](#setpagesizeint-size) to change it.|
|Paginated|boolean|false|(Getter only) Shorthand for `PageSize > 0`.|
|PageIndex|int|0|(Getter only) The current page, counting from **0**.|
|PageCount|int|1|(Getter only) Number of pages. Always at least 1, even for an empty menu.|
|WrapPages|boolean|true|Whether paging past the last page goes to the first, and the other way round.|
|ShowPageInstructionalButtons|boolean|true|Whether the previous/next page hints are drawn. Only shown when the menu is paginated, has more than one page, and the highlighted row is not a list or slider. Those rows show the [change value hint](../menu/#instructional-buttons) instead.|
|PreviousPageButtonText|string|"Previous page"|Text of the previous page hint.|
|NextPageButtonText|string|"Next page"|Text of the next page hint.|

:::note
`Size`, `CurrentIndex`, `GetMenuItems()` and `GetCurrentMenuItem()` describe **the current page**, not the whole menu. Keep your own list if you need the total.
:::

----

### Methods

----

#### SetPageSize(int size)

Splits the menu into pages of `size` items. `0` or less turns pagination off. Resets to the first page and puts the cursor at the top.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|size|int|Items per page. `0` or less turns pagination off.|

##### Return value

_This function does not return anything_.

```cs
menu.SetPageSize(48);

// Back to a normal, unpaginated menu.
menu.SetPageSize(0);
```

----

#### GoToPage(int pageIndex)

Jumps to a page, counting from 0. Out of range values are clamped, never wrapped.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|pageIndex|int|The page to jump to, counting from 0.|

##### Return value

|Type|Description|
|-|-|
|boolean|Whether the page changed. `false` if already on that page, or if the menu is not paginated.|

```cs
// Back to the first page, for example after rebuilding the item list.
menu.GoToPage(0);
```

----

#### NextPage()

Goes to the next page. Wraps to the first page unless [WrapPages](#properties) is off.

##### Return value

|Type|Description|
|-|-|
|boolean|Whether the page changed.|

----

#### PreviousPage()

Goes to the previous page. Wraps to the last page unless [WrapPages](#properties) is off.

##### Return value

|Type|Description|
|-|-|
|boolean|Whether the page changed.|

----

### The OnPageChange event

Fires on every page change, from a key press or from `GoToPage`, `NextPage` or `PreviousPage`.

```cs
public delegate void PageChangedEvent(Menu menu, int oldPage, int newPage, bool wrapped);
```

|Parameter|Type|Description|
|-|-|-|
|menu|Menu|The menu the page changed in.|
|oldPage|int|The previous page, counting from 0.|
|newPage|int|The new page, counting from 0.|
|wrapped|boolean|Whether the move wrapped past the first or last page.|

```cs
menu.OnPageChange += (_menu, _oldPage, _newPage, _wrapped) =>
{
    // PageIndex counts from 0, players count from 1.
    _menu.MenuSubtitle = $"Page {_newPage + 1} / {_menu.PageCount}";

    if (_wrapped)
    {
        // Your own notification function, MenuAPI does not have one.
        ShowNotification("You went past the end of the list and came back around.");
    }
};
```

----

### Rebuilding the items

Adding, removing, sorting and filtering keep working and the pages recalculate. A page index that falls out of range is clamped to the last page, so call `GoToPage(0)` after replacing the whole list.

```cs
menu.ClearMenuItems();

foreach (var player in players)
{
    menu.AddMenuItem(new MenuItem(player.Name) { Label = $"#{player.ServerId}" });
}

menu.GoToPage(0);
menu.MenuSubtitle = $"Page 1 / {menu.PageCount}";
```

:::caution
[FilterMenuItems()](../menu/#filtermenuitemsfuncmenuitem-bool-predicate) runs **before** pagination. Filtering 500 items down to 12 with a page size of 48 leaves a single page, so a filter changes `PageCount`.
:::

----

### Full example

```cs
Menu menu = new Menu("Pagination", "Page 1 / 4");
MenuController.AddMenu(menu);

menu.SetPageSize(12);

// A list item and a slider keep their arrows, everything else pages.
menu.AddMenuItem(new MenuListItem("A list item", new List<string>() { "One", "Two" }, 0));
menu.AddMenuItem(new MenuSliderItem("A slider item", 0, 10, 5, true));

for (int i = 1; i <= 45; i++)
{
    menu.AddMenuItem(new MenuItem($"Item #{i}") { Label = $"#{i}" });
}

menu.OnPageChange += (_menu, _oldPage, _newPage, _wrapped) =>
{
    _menu.MenuSubtitle = _wrapped
        ? $"~y~Page {_newPage + 1} / {_menu.PageCount} (wrapped around)"
        : $"Page {_newPage + 1} / {_menu.PageCount}";
};

menu.OpenMenu();
```
