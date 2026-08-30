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

    const bannerFiles = new Map();

    function bannerFile(key) {
        if (bannerFiles.has(key)) {
            return bannerFiles.get(key);
        }

        bannerFiles.set(key, null);

        const candidates = BANNER_HAS_EXTENSION.test(key)
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
        h: null,
        n: null,
        s: null
    };

    function markup(value) {
        const fragment = document.createDocumentFragment();

        if (!value) {
            return fragment;
        }

        let colour = null;

        for (const part of String(value).split("~")) {
            if (part === "") {
                continue;
            }

            if (part.length <= 24 && !part.includes(" ") && Object.hasOwn(TOKENS, part.toLowerCase())) {
                colour = TOKENS[part.toLowerCase()];

                continue;
            }

            if (part.startsWith("HUD_COLOUR_")) {
                colour = null;

                continue;
            }

            const node = document.createElement("span");

            node.textContent = part;

            if (colour) {
                node.style.color = colour;
            }

            fragment.append(node);
        }

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
            node.append(text(data.arrows ? `↓ ${data.text ?? ""} ↓` : data.text ?? ""));

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

    function render(data) {
        applyTheme(data.theme);

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
