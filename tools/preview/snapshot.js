function sprite(dict, name, size, r = 255, g = 255, b = 255) {
    return { dict, name, size, r, g, b };
}

function row(data) {
    return { kind: "item", label: null, enabled: true, selected: false, leftIcon: null, rightIcon: null, ...data };
}

const SNAPSHOT = {
    type: "menuapi",
    visible: true,
    align: "left",

    origin: { x: 0.0125, y: 0.0185 },

    header: {
        title: "Main Menu",
        font: 1,
        titleAlign: "center",
        glare: true,
        texture: { dict: "commonmenu", name: "interaction_bgd" }
    },

    subtitle: {
        text: "Subtitle",
        counter: "5 / 32",
        freemode: true,
        colour: "rgb(64 148 214)"
    },

    rows: [
        row({
            text: "Normal Button",
            enabled: false,
            leftIcon: sprite("commonmenu", "shop_tick_icon", 38, 109, 109, 109)
        }),
        row({
            kind: "slider",
            text: "Slider",
            slider: { min: 0, max: 10, position: 5, divider: false, background: "#185d97", bar: "#35a5df", sliderLeftIcon: null }
        }),
        row({
            kind: "slider",
            text: "Slider + Bar",
            slider: { min: 0, max: 10, position: 5, divider: true, background: "#1e7a3c", bar: "#49e96f", sliderLeftIcon: null }
        }),
        row({
            kind: "slider",
            text: "Slider + Bar + Icons",
            rightIcon: sprite("mpleaderboard", "leaderboard_female_icon", 38),
            slider: {
                min: 0, max: 10, position: 5, divider: true,
                background: "#6b1414", bar: "#e03232",
                sliderLeftIcon: sprite("mpleaderboard", "leaderboard_male_icon", 38)
            }
        }),
        row({
            kind: "checkbox",
            text: "Checkbox - Style 1 (click me!)",
            selected: true,
            checkbox: { dict: "commonmenu", name: "shop_box_blankb", size: 45, shade: 255 }
        }),
        row({
            kind: "checkbox",
            text: "Checkbox - Style 2",
            checkbox: { dict: "commonmenu", name: "shop_box_tick", size: 45, shade: 255 }
        }),
        row({
            kind: "checkbox",
            text: "Checkbox (unchecked + locked)",
            enabled: false,
            leftIcon: sprite("commonmenu", "shop_lock", 38, 109, 109, 109),
            checkbox: { dict: "commonmenu", name: "shop_box_blank", size: 45, shade: 109 }
        }),
        row({ text: "Dynamic list item.", label: "~s~← -7 ~s~→" }),
        row({ text: "Hair Color", label: "~s~← Color #55 ~s~→" }),
        row({ text: "Makeup Color", label: "~s~← Color #59 ~s~→" })
    ],

    panelBackground: "rgb(0 0 0 / 73%)",
    panelAccent: "240, 240, 240",

    text: { size: 21, brightness: 225, weight: "default" },

    panel: {
        colours: true,
        opacity: 40,
        index: 12,
        title: "Opacity",
        name: "Colour 13 of 64"
    },

    overflow: true,
    description: "This checkbox can toggle the menu position! Try it out.",
    stats: null
};
