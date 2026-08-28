---
title: "Pagination"
---

## Pagination

A normal menu shows a sliding window of up to ten items and you scroll through the rest with up and down. That works fine for twenty items. It stops working somewhere around a hundred, and it is hopeless at a thousand, because getting to the bottom means holding the down key for a very long time.

Pagination splits the menu into **pages** instead. You pick how many items belong on one page, and left and right move between those pages.

```cs
Menu menu = new Menu("Online Players", "Page 1 / 42");
MenuController.AddMenu(menu);

// 48 items per page. Left and right now turn the page.
menu.SetPageSize(48);
```

Turning pagination on does not change how many items fit on screen. Those are two separate things stacked on top of each other:

1. **The page** decides *which* items the menu is currently working with. With a page size of 48, page 3 is items 97 to 144.
2. **The window** ([MaxItemsOnScreen](../menu/#setmaxitemsonscreenint-max), up to 10) decides how many of those 48 you can see at once. Up and down still scroll through the page exactly like they always did.

So a page of 48 items with the default window of 10 gives you five screens worth of scrolling before you reach the end of the page, and then left or right takes you to the next 48.

----

### What left and right do

This is the one behaviour change to be aware of. In a menu that is **not** paginated, left and right on a plain button mean "go back" and "select". In a paginated menu they mean "previous page" and "next page" instead.

|Item type|Left / right in a paginated menu|
|-|-|
|[MenuItem](../menuitems/menuitem/)|Turns the page.|
|[MenuCheckboxItem](../menuitems/menucheckboxitem/)|Turns the page. Press select to toggle it.|
|[MenuListItem](../menuitems/menulistitem/)|Unchanged, still changes the value.|
|[MenuDynamicListItem](../menuitems/menudynamiclistitem/)|Unchanged, still changes the value.|
|[MenuSliderItem](../menuitems/menuslideritem/)|Unchanged, still moves the slider.|

Items that hold a value keep their arrows, because there is nothing else those arrows could mean on them. Everything else pages.

A locked row (`Enabled = false`) does not block paging. Turning the page is something the menu does, not something the highlighted row does, so it works wherever the cursor happens to be sitting.

Holding left or right repeats more slowly than it does on a slider, and it never speeds up. Without that, holding the key for a second would fly past hundreds of pages.

----

### Properties

|Property|Type|Default value|Description|
|-|-|-|-|
|PageSize|int|0|(Getter only) How many items fit on one page. `0` means the menu is not paginated. Use [SetPageSize()](#setpagesizeint-size) to change it.|
|Paginated|boolean|false|(Getter only) Whether this menu is split into pages. Shorthand for `PageSize > 0`.|
|PageIndex|int|0|(Getter only) The page currently being shown, counting from **0**. So the first page is `0`, and you add 1 to it when you show it to a player.|
|PageCount|int|1|(Getter only) How many pages there are. Always at least 1, so an empty menu still reads as "page 1 of 1".|
|WrapPages|boolean|true|Whether moving left on the first page jumps to the last page, and moving right on the last page jumps back to the first.|
|ShowPageInstructionalButtons|boolean|true|Whether the previous/next page hints are drawn at the bottom of the screen. Only ever drawn when the menu is paginated, there is more than one page, and the highlighted row is not a list or a slider. On those rows left and right change the value instead of turning the page, so the [change value hint](../menu/#instructional-buttons) is shown there.|
|PreviousPageButtonText|string|"Previous page"|The text next to the previous page hint.|
|NextPageButtonText|string|"Next page"|The text next to the next page hint.|

:::note
`Size`, `CurrentIndex`, `GetMenuItems()` and `GetCurrentMenuItem()` all describe **the current page**, not the whole menu. If you need the total, that is `PageCount * PageSize` at most, or just keep your own list of whatever you put in the menu.
:::

----

### Methods

----

#### SetPageSize(int size)

Splits the menu into pages of `size` items. Pass `0` or less to turn pagination off again.

Resets the menu to the first page and puts the cursor back at the top.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|size|int|How many items belong on one page. `0` or less turns pagination off.|

##### Return value

_This function does not return anything_.

```cs
menu.SetPageSize(48);

// Back to a normal, unpaginated menu.
menu.SetPageSize(0);
```

----

#### GoToPage(int pageIndex)

Jumps straight to a page, counting from 0. Values outside the range are clamped, so this never wraps around.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|pageIndex|int|The page to jump to, counting from 0.|

##### Return value

|Type|Description|
|-|-|
|boolean|Whether the page actually changed. `false` if you were already on that page, or if the menu is not paginated.|

```cs
// Back to the first page, for example after rebuilding the item list.
menu.GoToPage(0);
```

----

#### NextPage()

Goes to the next page. On the last page it wraps around to the first one, unless you turned [WrapPages](#properties) off.

##### Return value

|Type|Description|
|-|-|
|boolean|Whether the page actually changed.|

----

#### PreviousPage()

Goes to the previous page. On the first page it wraps around to the last one, unless you turned [WrapPages](#properties) off.

##### Return value

|Type|Description|
|-|-|
|boolean|Whether the page actually changed.|

----

### The OnPageChange event

Fires whenever the page changes, however it changed: a key press, or your own call to `GoToPage`, `NextPage` or `PreviousPage`.

```cs
public delegate void PageChangedEvent(Menu menu, int oldPage, int newPage, bool wrapped);
```

|Parameter|Type|Description|
|-|-|-|
|menu|Menu|The menu the page changed in.|
|oldPage|int|The page it was on, counting from 0.|
|newPage|int|The page it is on now, counting from 0.|
|wrapped|boolean|Whether the move rolled past the first or last page and came out the other end.|

The `wrapped` flag is there so you can tell the player what happened. Jumping from page 42 straight back to page 1 looks like a bug if nothing says otherwise.

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

Adding, removing, sorting and filtering all keep working, and the pages recalculate themselves. If you swap out the whole list, send the menu back to the first page afterwards, otherwise the player is left on page 30 of a list that now has three pages (which is clamped, but still not where you want them).

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
[FilterMenuItems()](../menu/#filtermenuitemsfuncmenuitem-bool-predicate) runs **before** pagination, not after. Filtering 500 items down to 12 with a page size of 48 leaves you with a single page, not twelve pages with holes in them. That is almost always what you want, but it does mean a filter changes how many pages there are.
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
