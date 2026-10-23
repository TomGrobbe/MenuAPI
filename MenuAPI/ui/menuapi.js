"use strict";

(() => {
    const root = document.getElementById("menuapi");

    if (!root) {
        return;
    }

    root.hidden = true;

    function spriteUrl(dict, name) {
        return typeof window.MENUAPI_SPRITE_URL === "function"
            ? window.MENUAPI_SPRITE_URL(dict, name)
            : `https://nui-img/${dict}/${name}`;
    }

    // Probed through an Image first: a CSS background that failed is never retried, and the url
    // does not change when the dictionary finally streams in.
    const sprites = new Map();
    const SPRITE_TRIES = 5;
    const SPRITE_RETRY_MS = 200;

    function spriteReady(url) {
        // Preloaded sprites are already bytes in this page, there is nothing left to wait for.
        if (url.startsWith("blob:") || url.startsWith("data:")) {
            return true;
        }

        const state = sprites.get(url);

        if (state) {
            return state.ok;
        }

        const next = { ok: false, tries: 0 };

        sprites.set(url, next);
        loadSprite(url, next);

        return false;
    }

    function loadSprite(url, state) {
        const probe = new Image();

        probe.onload = () => {
            state.ok = true;
            showSprite(url);
        };

        probe.onerror = () => {
            state.tries += 1;

            if (state.tries >= SPRITE_TRIES) {
                console.warn(`[menuapi] sprite never arrived: ${url}`);

                return;
            }

            setTimeout(() => loadSprite(url, state), state.tries * SPRITE_RETRY_MS);
        };

        probe.src = url;
    }

    const BANNER_FOLDER = "menuapi-banners";
    const BANNER_EXTENSIONS = ["png", "jpg", "webp"];
    const BANNER_HAS_EXTENSION = /\.(png|jpe?g|webp)$/i;
    const BANNER_IS_ABSOLUTE = /^(?:https?:|nui:)\/\//i;

    const bannerFiles = new Map();

    function bannerFile(key) {
        if (bannerFiles.has(key)) {
            return bannerFiles.get(key);
        }

        bannerFiles.set(key, null);

        // A full url points at a file another resource serves, so it is never looked for in this one.
        const candidates = BANNER_IS_ABSOLUTE.test(key)
            ? [key]
            : BANNER_HAS_EXTENSION.test(key)
                ? [`${BANNER_FOLDER}/${key}`]
                : BANNER_EXTENSIONS.map(extension => `${BANNER_FOLDER}/${key}.${extension}`);

        let index = 0;

        const attempt = () => {
            if (index >= candidates.length) {
                return;
            }

            const url = candidates[index++];
            const probe = new Image();

            probe.onload = () => {
                bannerFiles.set(key, url);
                applyBanner();
            };

            probe.onerror = attempt;
            probe.src = url;
        };

        attempt();

        return null;
    }

    function showSprite(url) {
        for (const node of root.querySelectorAll(".menuapi-icon")) {
            if (node.dataset.sprite === url) {
                node.style.setProperty("--sprite", `url("${url}")`);
            }
        }

        if (bannerUrl === url) {
            headerBg.src = url;
            headerBg.hidden = false;
        }
    }

    function rescale() {
        root.style.setProperty("--menuapi-scale", window.innerHeight / 1080);
        root.style.setProperty("--menuapi-aspect", window.innerWidth / window.innerHeight);
    }

    rescale();
    window.addEventListener("resize", rescale);

    let glare = null;
    let glareRunning = false;

    function setGlare(wanted) {
        if (wanted && !glare && typeof GtaMenuGlare !== "undefined") {
            glare = new GtaMenuGlare(headerGlare, {});
            glare.load().then(() => {
                if (glareRunning) {
                    glare.start();
                }
            }).catch(() => {
                glare = null;
            });
        }

        if (!glare) {
            headerGlare.hidden = !wanted;

            return;
        }

        headerGlare.hidden = !wanted;

        if (wanted === glareRunning) {
            return;
        }

        glareRunning = wanted;

        if (wanted) {
            glare.open();
            glare.start();
        } else {
            glare.stop();
        }
    }

    const header = element("div", "menuapi-header");
    const headerBg = element("img", "menuapi-header__bg");
    const headerGlare = element("canvas", "menuapi-header__glare");
    const headerTitle = element("div", "menuapi-header__title");
    const subtitle = element("div", "menuapi-subtitle");
    const subtitleText = element("span", "menuapi-subtitle__text");
    const subtitleCounter = element("span", "menuapi-subtitle__counter");
    const rows = element("div", "menuapi-rows");
    const overflow = element("div", "menuapi-overflow");
    const description = element("div", "menuapi-desc");
    const stats = element("div", "menuapi-stats");
    const panel = element("div", "menuapi-panel");

    headerBg.alt = "";
    header.append(headerBg, headerGlare, headerTitle);

    let bannerUrl = null;
    let banner = null;
    subtitle.append(subtitleText, subtitleCounter);
    overflow.append(text("↑"), text("↓"));
    root.append(header, subtitle, rows, overflow, description, panel, stats);

    function element(tag, className) {
        const node = document.createElement(tag);

        if (className) {
            node.className = className;
        }

        return node;
    }

    function text(value) {
        const node = document.createElement("span");

        node.textContent = value;

        return node;
    }

    const TOKENS = {
        r: "#e03232",
        g: "#42a05b",
        b: "#3f9dd4",
        y: "#f0c419",
        o: "#e8910e",
        p: "#9b59b6",
        w: "#ffffff",
        d: "#2f5c73",
        f: "#5db6e5",
        q: "#cb3694",
        c: "#8c8c8c",
        t: "#8c8c8c",
        m: "#646464",
        l: "#000000",
        v: null,
        u: null
    };

    const TOKEN_PATTERN = /~([^~\s]{1,24})~/g;

    function markup(value) {
        const fragment = document.createDocumentFragment();

        if (!value) {
            return fragment;
        }

        const source = String(value);

        let colour = null;
        let bold = false;
        let italic = false;
        let index = 0;

        function append(content) {
            if (content === "") {
                return;
            }

            const node = document.createElement("span");

            node.textContent = content;

            if (colour) {
                node.style.color = colour;
            }

            if (bold) {
                node.style.fontWeight = "bold";
            }

            if (italic) {
                node.style.fontStyle = "italic";
            }

            fragment.append(node);
        }

        TOKEN_PATTERN.lastIndex = 0;

        for (let match = TOKEN_PATTERN.exec(source); match !== null; match = TOKEN_PATTERN.exec(source)) {
            append(source.slice(index, match.index));

            index = TOKEN_PATTERN.lastIndex;

            const token = match[1].toLowerCase();

            if (token === "n") {
                fragment.append(document.createElement("br"));
            } else if (token === "h" || token === "bold") {
                bold = !bold;
            } else if (token === "italic") {
                italic = !italic;
            } else if (token === "s") {
                colour = null;
                bold = false;
                italic = false;
            } else if (token.startsWith("hud_colour_")) {
                colour = null;
            } else if (Object.hasOwn(TOKENS, token)) {
                colour = TOKENS[token];
            }
        }

        append(source.slice(index));

        return fragment;
    }

    function icon(spec, className) {
        const node = element("span", className ? "menuapi-icon " + className : "menuapi-icon");

        const url = spriteUrl(spec.dict, spec.name);

        node.style.setProperty("--menuapi-icon-size", `${spec.size}px`);
        node.style.setProperty("--tint", `rgb(${spec.r} ${spec.g} ${spec.b})`);
        node.dataset.sprite = url;

        if (spriteReady(url)) {
            node.style.setProperty("--sprite", `url("${url}")`);
        }

        return node;
    }

    function renderHeader(data) {
        if (!data) {
            header.hidden = true;
            setGlare(false);

            return;
        }

        header.hidden = false;

        banner = data.texture ? { image: data.image, texture: data.texture } : null;

        applyBanner();

        headerTitle.dataset.align = data.titleAlign;
        headerTitle.dataset.font = data.font;
        headerTitle.replaceChildren(markup(data.title));

        setGlare(!!data.glare);
    }

    function applyBanner() {
        if (!banner) {
            bannerUrl = null;
            headerBg.hidden = true;

            return;
        }

        const texture = banner.texture;

        const file = (banner.image ? bannerFile(banner.image) : null)
            ?? bannerFile(`${texture.dict}/${texture.name}`);

        if (file) {
            bannerUrl = file;
            headerBg.src = file;
            headerBg.hidden = false;

            return;
        }

        bannerUrl = texture.ready === false ? null : spriteUrl(texture.dict, texture.name);
        headerBg.hidden = !bannerUrl || !spriteReady(bannerUrl);

        if (!headerBg.hidden) {
            headerBg.src = bannerUrl;
        }
    }

    function renderSubtitle(data) {
        subtitle.classList.toggle("menuapi-subtitle--freemode", !!data.freemode);

        if (data.colour) {
            root.style.setProperty("--menuapi-hud-freemode", data.colour);
        }
        subtitleText.replaceChildren(markup(data.text));
        subtitleCounter.replaceChildren(markup(data.counter));
    }

    function renderRow(data) {
        const node = element("div", "menuapi-row");

        node.classList.toggle("menuapi-row--selected", data.selected);
        node.classList.toggle("menuapi-row--disabled", !data.enabled);

        node.classList.toggle("menuapi-row--icon-left", !!data.leftIcon);
        node.classList.toggle("menuapi-row--icon-right", !!data.rightIcon || !!data.checkbox);

        node.classList.toggle("menuapi-row--slider-icons", !!(data.slider?.sliderLeftIcon && data.rightIcon));

        if (data.kind === "separator") {
            node.classList.add("menuapi-row--separator");
            node.append(markup(data.arrows ? `↓ ${data.text ?? ""} ↓` : data.text ?? ""));

            return node;
        }

        if (data.leftIcon) {
            node.append(icon(data.leftIcon, "menuapi-icon--left"));
        }

        const label = element("span", "menuapi-row__text");

        label.append(markup(data.text));
        node.append(label);

        if (data.label) {
            const value = element("span", "menuapi-row__label");

            value.append(markup(data.label));
            node.append(value);
        }

        if (data.slider) {
            if (data.slider.sliderLeftIcon) {
                node.append(icon(data.slider.sliderLeftIcon, "menuapi-icon--slider"));
            }

            node.append(renderSlider(data.slider));
        }

        if (data.checkbox) {
            node.append(icon({
                dict: data.checkbox.dict,
                name: data.checkbox.name,
                size: data.checkbox.size,
                r: data.checkbox.shade,
                g: data.checkbox.shade,
                b: data.checkbox.shade
            }, "menuapi-icon--checkbox"));
        }

        if (data.rightIcon) {
            node.append(icon(data.rightIcon, "menuapi-icon--right"));
        }

        return node;
    }

    function renderSlider(data) {
        const node = element("span", "menuapi-slider");
        const bar = element("span", "menuapi-slider__bar");
        const span = Math.max(1, data.max - data.min);

        node.style.backgroundColor = data.background;
        bar.style.backgroundColor = data.bar;

        bar.style.left = `${((data.position - data.min) / span) * 50}%`;

        node.append(bar);

        if (data.divider) {
            node.append(element("span", "menuapi-slider__divider"));
        }

        return node;
    }

    let colours = null;

    let palette = null;

    let paletteIndex = 0;

    function renderPanel(data) {
        if (!data) {
            panel.hidden = true;

            return;
        }

        if (!colours && typeof GtaColourList !== "undefined") {
            colours = new GtaColourList(panel, { arrows: false });
            applyPalette();
        }

        if (!colours) {
            panel.hidden = true;

            return;
        }

        panel.hidden = false;

        colours.SET_TITLE(data.title, data.name ?? "", data.opacity ?? -1);
        colours.SHOW_OPACITY(data.opacity !== null && data.opacity !== undefined, true);

        panel.classList.toggle("menuapi-panel--no-colours", !data.colours);

        if (data.colours) {
            paletteIndex = data.index;
            colours.SET_HIGHLIGHT(paletteIndex);
        }
    }

    function applyPalette() {
        if (!colours || !palette) {
            return;
        }

        colours.SET_DATA_SLOT_EMPTY();

        palette.forEach((rgb, i) => colours.SET_DATA_SLOT(i, rgb[0], rgb[1], rgb[2]));

        colours.DISPLAY_VIEW();

        colours.SET_HIGHLIGHT(paletteIndex);
    }

    function applyText(data) {
        if (!data) {
            return;
        }

        root.style.setProperty("--menuapi-text-size", data.size);
        root.style.setProperty("--menuapi-text-rgb", `${data.brightness} ${data.brightness} ${data.brightness}`);
        root.dataset.textWeight = data.weight;
    }

    function renderStats(data) {
        if (!data) {
            stats.hidden = true;

            return;
        }

        stats.hidden = false;
        stats.replaceChildren(...data.map(entry => {
            const node = element("div", "menuapi-stat");
            const label = element("span", "menuapi-stat__label");
            const track = element("div", "menuapi-stat__track");
            const upgrade = element("div", "menuapi-stat__upgrade");
            const value = element("div", "menuapi-stat__value");

            const reduced = entry.upgrade < entry.value;

            upgrade.classList.toggle("menuapi-stat__upgrade--reduced", reduced);
            upgrade.style.width = `${Math.max(entry.upgrade, entry.value) * 100}%`;
            value.style.width = `${Math.min(entry.upgrade, entry.value) * 100}%`;

            label.textContent = entry.label;

            track.append(upgrade, value);
            node.append(label, track);

            return node;
        }));
    }

    function render(data) {
        if (!data.visible) {
            root.hidden = true;
            setGlare(false);

            for (const [url, state] of sprites) {
                if (!state.ok) {
                    sprites.delete(url);
                }
            }

            return;
        }

        root.hidden = false;
        root.dataset.align = data.align;

        root.style.setProperty("--menuapi-origin-x", data.origin.x);
        root.style.setProperty("--menuapi-origin-y", data.origin.y);

        renderHeader(data.header);
        renderSubtitle(data.subtitle);

        rows.replaceChildren(...data.rows.map(renderRow));

        overflow.hidden = !data.overflow;

        root.classList.toggle("menuapi--overflow", !!data.overflow);

        description.hidden = !data.description;

        root.classList.toggle("menuapi--no-desc", !data.description);
        description.replaceChildren(markup(data.description));

        renderPanel(data.panel);
        renderStats(data.stats);

        root.style.setProperty("--menuapi-hud-panel-bg", data.panelBackground);
        root.style.setProperty("--menuapi-hud-panel-accent", data.panelAccent);

        applyText(data.text);
    }

    bannerFile("commonmenu/interaction_bgd");

    window.addEventListener("message", event => {
        let data = event.data;

        if (typeof data === "string") {
            try {
                data = JSON.parse(data);
            } catch {
                return;
            }
        }

        if (!data || typeof data !== "object") {
            return;
        }

        if (data.type === "menuapi") {
            render(data);
        } else if (data.type === "menuapi:glare" && glare) {
            glare.setHeading(data.heading || 0, false);
        } else if (data.type === "menuapi:palette") {
            palette = data.colours;
            applyPalette();
        }
    });
})();

// The theme belongs to the whole page, the menu and the instructional buttons share it. It is only
// changed by something on screen, so hiding one of them never takes the theme away from the other.
(() => {
    let themeUrl = null;
    let themeLink = null;

    function applyTheme(url) {
        url = url || null;

        if (url === themeUrl) {
            return;
        }

        themeUrl = url;

        const previous = themeLink;

        if (!url) {
            themeLink = null;
            previous?.remove();

            return;
        }

        const link = document.createElement("link");

        link.rel = "stylesheet";
        link.id = "menuapi-theme";
        link.href = url;

        const drop = () => {
            if (previous !== themeLink) {
                previous?.remove();
            }
        };

        link.addEventListener("load", drop, { once: true });
        link.addEventListener("error", drop, { once: true });

        themeLink = link;

        document.head.append(link);
    }

    window.addEventListener("message", event => {
        let data = event.data;

        if (typeof data === "string") {
            try {
                data = JSON.parse(data);
            } catch {
                return;
            }
        }

        if ((data?.type === "menuapi" || data?.type === "menuapi:buttons") && data.visible) {
            applyTheme(data.theme);
        }
    });
})();

// Instructional buttons. Kept apart from the menu above so they still work on a page without #menuapi.
(() => {
    const INLINE = {
        up: ["0 0 16 16", "M8 14.5V1.5M4 5.5l4-4 4 4"],
        down: ["0 0 16 16", "M8 1.5v13M4 10.5l4 4 4-4"],
        left: ["0 0 24 16", "M23 8H1.5M5.5 4l-4 4 4 4"],
        right: ["0 0 24 16", "M1 8h21.5M18.5 4l4 4-4 4"],
        backspace: ["0 0 28 16", "M27 8H1.5M5.5 4l-4 4 4 4"],
        enter: ["0 0 20 16", "M17 2.5V10H2.5M6 6.5 2.5 10 6 13.5"],
        shift: ["0 0 16 16", "M8 1.5 14.5 8h-3.5v6.5H5V8H1.5z"],
        hollowdown: ["0 0 16 16", "M8 14.5 14.5 8h-3.5V1.5H5V8H1.5z"],
        home: ["0 0 16 16", "M14 14 3 3M3 9V3h6M1.5 1.5h6"],
        pageup: ["0 0 16 16", "M8 14.5V1.5M4 5.5l4-4 4 4M4.5 9h7M4.5 12h7"],
        pagedown: ["0 0 16 16", "M8 1.5v13M4 10.5l4 4 4-4M4.5 7h7M4.5 4h7"],
        space: ["0 0 16 16", "M2.5 8.5v5h11v-5"],
        isoenter: ["0 0 16 16", "M2 5h11v6.5"],
    };

    const key = (text, size) => ({ text, size });
    const numpad = text => ({ text, numpad: true });

    const KEYS = {
        130: numpad("-"), 131: numpad("+"), 132: numpad("."), 133: numpad("/"), 134: numpad("*"),
        135: numpad("{enter}"), 146: numpad("="), 147: numpad(","), 148: numpad("÷"), 149: numpad("x"),
        150: key("Intro"), 151: key("{isoenter}"),
        194: key("{up}", "tall"), 195: key("{down}", "tall"), 196: key("{left}", "wide"), 197: key("{right}", "wide"),
        198: key("Del"), 199: key("Esc"), 200: key("Ins"), 201: key("End"),
        202: key("Suppr"), 203: key("Échap"), 204: key("Fin"),
        205: key("Entf"), 206: key("Einfg"), 207: key("Ende"),
        208: key("Canc"), 209: key("Fine"),
        210: key("Supr"), 211: key("Insertar"), 212: key("Fin"),
        213: key("Supr"), 214: key("Insertar"), 215: key("Fin"),
        216: key("¨"), 217: key("`"),
        996: key("", "wide"), 997: key(""), 999: key(""),
        1000: key("L {shift}"), 1001: key("R {shift}"), 1002: key("Tab"),
        1003: key("{enter}", "wide"), 1004: key("{backspace}", "wide"),
        1005: key("Print\nScreen"), 1006: key("Scroll\nLock"), 1007: key("Pause"), 1008: key("Home"),
        1009: key("Page\nUp"), 1010: key("Page\nDown"), 1011: key("Num\nLock"), 1012: key("Caps"),
        1013: key("L Ctrl"), 1014: key("R Ctrl"), 1015: key("L Alt"), 1016: key("R Alt"),
        1017: key("Menu"), 1018: key("L Win"), 1019: key("R Win"),
        1020: key("Impr\nécran"), 1021: key("Arrêt\ndéfil"), 1022: key("{home}"), 1023: key("{pageup}"),
        1024: key("{pagedown}"), 1025: key("Verr\nNumm"), 1026: key("Verr\nMaj"), 1027: key("Ctrl G"), 1028: key("Ctrl D"),
        1029: key("Druck"), 1030: key("Rollen\n{hollowdown}"), 1031: key("Pos 1"), 1032: key("Bild {up}"),
        1033: key("Bild {down}"), 1034: key("Num\n{hollowdown}"), 1035: key("{hollowdown}", "wide"),
        1036: key("Strg L"), 1037: key("Strg R"),
        1038: key("Maiusc\nsx"), 1039: key("Maiusc\ndx"), 1040: key("Invio"), 1041: key("Stampa"),
        1042: key("Bloc\nScorr"), 1043: key("Pausa"), 1044: key("{home}"), 1045: key("Pag {up}"), 1046: key("Pag {down}"),
        1047: key("Bloc\nNum"), 1048: key("Bloc\nMaiusc"), 1049: key("Ctrl sx"), 1050: key("Ctrl dx"), 1051: key("Alt gr"),
        1052: key("Impr\nPant"), 1053: key("Bloq\nDespl"), 1054: key("Pausa"), 1055: key("Inicio"), 1056: key("Re Pág"),
        1057: key("Av Pág"), 1058: key("Bloq\nNum"), 1059: key("Bloq\nMayús"), 1060: key("Ctrl I"), 1061: key("Ctrl D"),
        1062: key("Menú"),
        1063: key("Impr\nPant"), 1064: key("Bloq\nDespl"), 1065: key("Pausa"), 1066: key("Inicio"), 1067: key("Re Pág"),
        1068: key("Av Pág"), 1069: key("Bloq\nNum"), 1070: key("Mayús"), 1071: key("Opciones"),
        1072: key("Maj G"), 1073: key("Maj D"), 1074: key("Alt", "wide"), 1075: key("Alt D"),
        1076: key("I {shift}"), 1077: key("D {shift}"),
        2000: key("{space}"),
    };

    for (let i = 0; i < 10; i++) {
        KEYS[136 + i] = numpad(String(i));
    }

    for (let i = 0; i < 24; i++) {
        KEYS[170 + i] = key(`F${i + 1}`);
    }

    const TRIANGLES = {
        u: "M12 1.6l2.4 3.2H9.6z", d: "M12 22.4l2.4-3.2H9.6z", l: "M1.6 12l3.2 2.4V9.6z", r: "M22.4 12l-3.2 2.4V9.6z",
    };

    const marks = (directions, paths = TRIANGLES) => [...directions].map(direction => `<path class="w" d="${paths[direction]}"/>`).join("");

    const DPAD_MARKS = {
        u: "M9.6 1h4.8v2.6H9.6z", d: "M9.6 20.4h4.8V23H9.6z", l: "M1 9.6h2.6v4.8H1z", r: "M20.4 9.6H23v4.8h-2.6z",
    };

    const BIG_ARROWS = {
        u: "M12 5.5l7 11.5H5z", d: "M12 18.5 19 7H5z", l: "M5.5 12 17 5v14z", r: "M18.5 12 7 5v14z",
    };

    const PAIR_ARROWS = {
        u: "M12 2.5l4.2 6.5H7.8z", d: "M12 21.5l4.2-6.5H7.8z", l: "M2.5 12 9 7.8v8.4z", r: "M21.5 12 15 7.8v8.4z",
    };

    const CROSS_ARROWS = {
        u: "M12 2.5l3 4.5H9z", d: "M12 21.5l3-4.5H9z", l: "M2.5 12 7 9v6z", r: "M21.5 12 17 9v6z",
    };

    const dpad = directions => `<path class="p" d="M8.2 1h7.6v7.2H23v7.6h-7.2V23H8.2v-7.2H1V8.2h7.2z"/>${marks(directions, DPAD_MARKS)}`;
    const letter = (text, y = 16.8, size = 13) => `<text class="w" x="12" y="${y}" font-size="${size}">${text}</text>`;
    const stick = (side, directions) => `<circle class="p" cx="12" cy="12" r="11"/>${directions ? letter(side, 15.9, 11) : letter(side)}${marks(directions)}`;
    const rotate = side => `<path class="ws" d="M19.8 7.5A9 9 0 1 1 15.8 3.9"/><path class="w" d="M18.2 5l-3.9 0.6 1.9-4z"/>${letter(side, 16.4, 11)}`;
    const face = (name, circle) => `${circle ? `<circle class="p" cx="12" cy="12" r="11"/>` : ""}<text class="face face--${name.toLowerCase()}" x="12" y="18.2" font-size="17">${name}</text>`;
    const start = `<circle class="p" cx="12" cy="12" r="11"/><path class="ws" d="M7 8.5h10M7 12h10M7 15.5h10"/>`;
    const back = `<circle class="p" cx="12" cy="12" r="11"/><rect class="ws" x="6.5" y="7.5" width="7" height="5.5" rx="1"/><rect class="ws" x="10.5" y="11" width="7" height="5.5" rx="1"/>`;

    const MOUSE_BODY = "M12 1.5c-4.8 0-8 3.1-8 7.4v6.2c0 4.3 3.2 7.4 8 7.4s8-3.1 8-7.4V8.9c0-4.3-3.2-7.4-8-7.4z";
    const MOUSE_LEFT = `<path class="mh" d="M11.6 2C7.6 2.3 4.5 4.8 4.5 8.9v.6h7.1z"/>`;
    const MOUSE_RIGHT = `<path class="mh" d="M12.4 2c4 .3 7.1 2.8 7.1 6.9v.6h-7.1z"/>`;

    function mouse({ part = "", badge = "", side = "", arrow = "" } = {}) {
        let markup = `<path class="mb" d="${MOUSE_BODY}"/>${part === "left" ? MOUSE_LEFT : ""}${part === "right" ? MOUSE_RIGHT : ""}`
            + `<path class="ml" d="M4 10h16M12 1.5V10"/>`
            + `<rect class="${part === "wheel" ? "mw mw--on" : "mw"}" x="10.6" y="3.6" width="2.8" height="4.8" rx="1.4"/>`;

        if (badge) {
            markup += `<rect class="mb" x="13" y="12.5" width="10" height="10.5" rx="1.6"/><text class="mt" x="18" y="21.2" font-size="9.5">${badge}</text>`;
        }

        let left = 0;
        let right = 24;

        if (side.includes("l")) {
            markup += `<path class="w" d="M-1.5 12l3.4-2.6v5.2z"/>`;
            left = -2.5;
        }

        if (side.includes("r")) {
            markup += `<path class="w" d="M25.5 12l-3.4-2.6v5.2z"/>`;
            right = 26.5;
        }

        if (arrow) {
            const heads = (arrow.includes("u") ? "M20.6 8.2 23 5.2l2.4 3" : "") + (arrow.includes("d") ? "M20.6 15.8 23 18.8l2.4-3" : "");

            markup += `<path class="ws" d="M23 4.5v15${heads}"/>`;
            right = 26.5;
        }

        return { markup, viewBox: `${left} 0 ${right - left} 24` };
    }

    const GLYPHS = {
        0: marks("u", BIG_ARROWS), 1: marks("d", BIG_ARROWS), 2: marks("l", BIG_ARROWS), 3: marks("r", BIG_ARROWS),
        4: dpad("u"), 5: dpad("d"), 6: dpad("l"), 7: dpad("r"), 8: dpad(""), 9: dpad("udlr"), 10: dpad("ud"), 11: dpad("lr"),
        12: stick("L", "u"), 13: stick("L", "d"), 14: stick("L", "l"), 15: stick("L", "r"), 16: stick("L", ""),
        17: stick("L", ""), 18: stick("L", "ud"), 19: stick("L", "lr"), 20: rotate("L"),
        21: stick("R", "u"), 22: stick("R", "d"), 23: stick("R", "l"), 24: stick("R", "r"), 25: stick("R", ""),
        26: stick("R", ""), 27: stick("R", "ud"), 28: stick("R", "lr"), 29: rotate("R"),
        30: face("A"), 31: face("B"), 32: face("X"), 33: face("Y", true),
        38: start, 39: back,
        45: marks("ud", PAIR_ARROWS), 46: marks("lr", PAIR_ARROWS), 47: marks("udlr", CROSS_ARROWS),
    };

    const MICE = {
        100: mouse({ part: "left" }), 101: mouse({ part: "right" }), 102: mouse({ part: "wheel" }),
        103: mouse({ badge: "4" }), 104: mouse({ badge: "5" }), 105: mouse({ badge: "6" }), 106: mouse({ badge: "7" }), 107: mouse({ badge: "8" }),
        108: mouse({ side: "l" }), 109: mouse({ side: "r" }), 110: mouse({ arrow: "u" }), 111: mouse({ arrow: "d" }),
        112: mouse({ side: "lr" }), 113: mouse({ arrow: "ud" }), 114: mouse(),
        115: mouse({ part: "wheel", arrow: "u" }), 116: mouse({ part: "wheel", arrow: "d" }), 117: mouse({ part: "wheel", arrow: "ud" }),
    };

    const SHOULDERS = { 34: "LB", 35: "LT", 36: "RB", 37: "RT" };

    const SPINNERS = { 44: "crescent", 48: "ring", 49: "ring ring--alt", 50: "ring" };

    // Going by which way the tail of each spinner points, the rest turn clockwise.
    const COUNTER_CLOCKWISE = new Set([44, 50]);

    const UNBOUND = 995;
    const PLUS = 998;
    const MISSING = new Set([40, 41, 42, 43]);

    const TOKEN = /^([tTb])_(.+)$/;
    const TEXT_TOKEN = /~[^~\s]{1,24}~/g;

    // Two icon strings added together without a % in between, which older examples did.
    const GLUED = /[tTb]_.*?(?=[tTb]_|$)/g;
    const GLUED_START = /^[tTb]_/;

    let bar = null;

    function getBar() {
        if (bar?.isConnected) {
            return bar;
        }

        bar = document.getElementById("menuapi-buttons");

        if (!bar) {
            bar = document.createElement("div");
            bar.id = "menuapi-buttons";
            document.body.append(bar);
        }

        bar.hidden = true;
        rescale();

        return bar;
    }

    function rescale() {
        bar?.style.setProperty("--menuapi-buttons-scale", window.innerHeight / 1080);
    }

    window.addEventListener("resize", rescale);

    function element(tag, className, content) {
        const node = document.createElement(tag);

        node.className = className;

        if (content !== undefined) {
            node.textContent = content;
        }

        return node;
    }

    function svg(viewBox, markup) {
        return `<svg viewBox="${viewBox}" aria-hidden="true">${markup}</svg>`;
    }

    function glyph(markup, viewBox = "0 0 24 24") {
        const node = element("span", "menuapi-key menuapi-key--glyph");

        node.innerHTML = svg(viewBox, markup);

        return node;
    }

    function line(text) {
        const node = element("span", "menuapi-key__line");

        for (const part of text.split(/(\{\w+\})/)) {
            const name = /^\{(\w+)\}$/.exec(part)?.[1];

            if (name && INLINE[name]) {
                const icon = element("span", `menuapi-key__icon menuapi-key__icon--${name}`);

                icon.innerHTML = svg(INLINE[name][0], `<path d="${INLINE[name][1]}"/>`);
                node.append(icon);
            } else if (part !== "") {
                node.append(part);
            }
        }

        return node;
    }

    function cap(text, size, numpad) {
        const lines = text.split("\n");
        const node = element("span", "menuapi-key menuapi-key--cap");

        if (size) {
            node.classList.add(`menuapi-key--${size}`);
        }

        if (numpad) {
            node.classList.add("menuapi-key--numpad");
        }

        if (lines.length > 1) {
            node.classList.add("menuapi-key--two-lines");
        } else if (text.length > 6 && !text.includes("{")) {
            node.classList.add("menuapi-key--small");
        }

        node.append(...lines.map(line));

        return node;
    }

    const reported = new Set();

    function unknown(token) {
        if (!reported.has(token)) {
            reported.add(token);
            console.warn(`[menuapi] no instructional button icon for "${token}"`);
        }

        return null;
    }

    function icon(token) {
        const match = TOKEN.exec(token);

        if (!match) {
            return unknown(token);
        }

        if (match[1] !== "b") {
            return cap(match[2], match[1] === "T" ? "wide" : null);
        }

        const id = Number(match[2]);

        if (id === PLUS) {
            return element("span", "menuapi-key menuapi-key--plus", "+");
        }

        if (id === UNBOUND) {
            return element("span", "menuapi-key menuapi-key--cap menuapi-key--unbound", "???");
        }

        if (MISSING.has(id)) {
            return element("span", "menuapi-key menuapi-key--missing");
        }

        if (SHOULDERS[id]) {
            return element("span", "menuapi-key menuapi-key--shoulder", SHOULDERS[id]);
        }

        if (SPINNERS[id]) {
            const node = element("span", "menuapi-key menuapi-key--spinner");
            const spinner = element("span", `menuapi-spinner ${SPINNERS[id].split(" ").map(name => `menuapi-spinner--${name}`).join(" ")}`);

            spinner.classList.toggle("menuapi-spinner--ccw", COUNTER_CLOCKWISE.has(id));
            node.append(spinner);

            return node;
        }

        if (GLYPHS[id]) {
            return glyph(GLYPHS[id]);
        }

        if (MICE[id]) {
            return glyph(MICE[id].markup, MICE[id].viewBox);
        }

        if (KEYS[id]) {
            return cap(KEYS[id].text, KEYS[id].size, KEYS[id].numpad);
        }

        return unknown(token);
    }

    // The game's button strings list their icons right to left, so the last one is drawn first.
    function icons(buttons) {
        const node = element("span", "menuapi-buttons__keys");
        const tokens = String(buttons ?? "")
            .split("%")
            .flatMap(part => GLUED_START.test(part) ? part.match(GLUED) : part === "" ? [] : [part])
            .reverse();

        for (const token of tokens) {
            const child = icon(token);

            if (child) {
                node.append(child);
            }
        }

        return node;
    }

    function render(data) {
        const target = getBar();

        if (!data.visible) {
            target.hidden = true;
            target.replaceChildren();

            return;
        }

        target.style.setProperty("--menuapi-buttons-inset", data.inset ?? 0.05);

        target.replaceChildren(...(data.buttons ?? []).map(button => {
            const item = element("div", "menuapi-buttons__item");
            const text = String(button.text ?? "").replace(TEXT_TOKEN, "");

            if (text !== "") {
                item.append(element("span", "menuapi-buttons__text", text));
            }

            item.append(icons(button.buttons));

            return item;
        }));

        target.hidden = target.childElementCount === 0;
    }

    window.addEventListener("message", event => {
        let data = event.data;

        if (typeof data === "string") {
            try {
                data = JSON.parse(data);
            } catch {
                return;
            }
        }

        if (data?.type === "menuapi:buttons") {
            render(data);
        }
    });
})();
