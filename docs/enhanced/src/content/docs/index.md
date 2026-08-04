---
title: "MenuAPI Documentation"
---

## MenuAPI documentation

Welcome to the **MenuAPI** documentation.

Here you'll find basic information about how to use MenuAPI in your projects.

:::tip
**Please choose a category on the left to get started.**
:::

## Most noticeable features
- Left & Right aligned menus are supported, and can be changed at any time during runtime. They'll both scale with the safezone size automatically.
- If the screen resolution is too small, the menu will automatically display fewer items at a time to make sure it never goes off-screen. (note, very long item descriptions can still cause part of the menu to go off screen in rare cases)
- Almost all menu items support left and right badges/icons.
- Menu items can have left and right text.
- Menu items can be disabled if you don't want users to interact with them.
- The menu API handles all controls for you. On a keyboard every menu control is a proper FiveM key binding, so players can rebind them from their own settings menu. The controller keybind can not be changed and will always be back/select (the Interaction Menu button, hold it for 400ms to toggle the menu).
- Multiple checkbox designs supported.
- You can prevent users from exiting the menu using the normal ESC/backspace/cancel controls. (only do this for important menus where you need progress saved or something like that, and always have a button to exit the menu! Be nice to your users!)


## Some examples
Here are some examples of what you can expect to be able to achieve with this API (if you put some time into it). Most of these screenshots are taken from vMenu after it was converted to this MenuAPI. Some are taken from the example menu provided with MenuAPI.

<details>
<summary>Makeup &amp; Hair color panels, and also opacity panels.</summary>

They actually are just list items but you can toggle these panels to appear below the description box if you want them enabled

![](https://www.vespura.com/hi/i/2018-12-15_19-35_84470_391.png)
![](https://www.vespura.com/hi/i/2018-12-15_19-38_b6ca4_392.png)
![](https://www.vespura.com/hi/i/2018-12-15_19-39_6abc3_393.png)

</details>

<details>
<summary>List items</summary>

![](https://www.vespura.com/hi/i/2018-12-15_19-40_2e308_394.png)

</details>

<details>
<summary>Slider items</summary>

You can have your own custom colors. Both background and foreground bar colors are fully customizable.

![](https://www.vespura.com/hi/i/2018-12-15_19-46_420c5_397.png)

Optionally you can also choose from 29 different sprites to appear on either side of the slider.

</details>

<details>
<summary>Checkbox items</summary>

![](https://www.vespura.com/hi/i/2018-12-15_19-54_d2c94_398.png)

</details>

<details>
<summary>Menus without banners</summary>

You can create a menu without a banner if you prefer that.

![](https://www.vespura.com/hi/i/2018-12-15_19-55_aea95_399.png)

</details>


<details>
<summary>Customizable instructional buttons (per menu)</summary>

Each menu can have it's own set of instructional buttons. By default every menu has one setup for 'select' and 'cancel/back'. But you can remove those if you don't need them.

You can also disable the instructional buttons all together if you want to handle that yourself.

Instructional buttons instantly update if the user switches between keyboard/mouse and controller.

![](https://www.vespura.com/hi/i/2018-12-15_19-56_bc2c4_400.png)

</details>


<details>
<summary>Did somebody say icons?</summary>

We've got plenty of those! (They don't even fit on one page, seriously there are 29 icons to choose from.)

You can also use icons even when you have text on the right side of an item, the text will move over for your icon automatically.

![](https://www.vespura.com/hi/i/2018-12-15_20-01_32cc2_401.png)

</details>
