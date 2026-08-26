"use strict";

(() => {
    const CONCURRENCY = 8;
    const TRIES = 3;
    const RETRY_MS = 150;

    const HELLO_MS = 500;
    const HELLO_TRIES = 40;

    const cache = new Map();

    let mode = null;
    let hello = null;

    function live(dict, name) {
        return `https://nui-img/${dict}/${name}`;
    }

    function key(sprite) {
        return `${sprite.dict}/${sprite.name}`;
    }

    window.MENUAPI_SPRITE_URL = (dict, name) => cache.get(`${dict}/${name}`) ?? live(dict, name);

    function resource() {
        if (typeof window.GetParentResourceName === "function") {
            return window.GetParentResourceName();
        }

        return window.location.hostname.replace(/^cfx-nui-/, "");
    }

    function post(payload) {
        return fetch(`https://${resource()}/menuapiSprites`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        }).catch(() => { });
    }

    function wait(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    async function viaFetch(url) {
        const response = await fetch(url, { cache: "no-store" });

        if (!response.ok) {
            throw new Error(`http ${response.status}`);
        }

        const blob = await response.blob();

        if (blob.size === 0) {
            throw new Error("empty");
        }

        return blob;
    }

    function decode(url, cors) {
        return new Promise((resolve, reject) => {
            const probe = new Image();

            if (cors) {
                probe.crossOrigin = "anonymous";
            }

            probe.onload = () => (probe.naturalWidth > 0 ? resolve(probe) : reject(new Error("empty")));
            probe.onerror = () => reject(new Error("failed"));
            probe.src = url;
        });
    }

    async function viaCanvas(url) {
        const source = await decode(url, true);
        const canvas = document.createElement("canvas");

        canvas.width = source.naturalWidth;
        canvas.height = source.naturalHeight;
        canvas.getContext("2d").drawImage(source, 0, 0);

        return new Promise((resolve, reject) => {
            try {
                canvas.toBlob(blob => (blob ? resolve(blob) : reject(new Error("blocked"))), "image/png");
            } catch (error) {
                reject(error);
            }
        });
    }

    async function pickMode(sprites) {
        const samples = sprites.slice(0, 3);

        for (const sprite of samples) {
            try {
                await viaFetch(live(sprite.dict, sprite.name));

                return "blob";
            } catch { /* try the next way in */ }
        }

        for (const sprite of samples) {
            try {
                await viaCanvas(live(sprite.dict, sprite.name));

                return "canvas";
            } catch { /* fall through to leaving them where they are */ }
        }

        return "live";
    }

    async function store(sprite) {
        const url = live(sprite.dict, sprite.name);

        for (let attempt = 1; attempt <= TRIES; attempt++) {
            try {
                if (mode === "live") {
                    await decode(url, false);

                    return true;
                }

                const blob = mode === "blob" ? await viaFetch(url) : await viaCanvas(url);

                cache.set(key(sprite), URL.createObjectURL(blob));

                return true;
            } catch {
                if (attempt < TRIES) {
                    await wait(attempt * RETRY_MS);
                }
            }
        }

        return false;
    }

    async function run(sprites) {
        const missing = [];
        let next = 0;

        async function worker() {
            while (next < sprites.length) {
                const sprite = sprites[next++];

                if (!await store(sprite)) {
                    missing.push(key(sprite));
                }
            }
        }

        const workers = Math.max(1, Math.min(CONCURRENCY, sprites.length));

        await Promise.all(Array.from({ length: workers }, worker));

        return missing;
    }

    async function preload(message) {
        const sprites = Array.isArray(message.sprites) ? message.sprites : [];

        if (sprites.length === 0) {
            return;
        }

        if (!mode) {
            mode = await pickMode(sprites);
        }

        const missing = await run(sprites);

        if (missing.length > 0) {
            console.warn(`[menuapi] ${missing.length} sprites missing after round ${message.round}: ${missing.join(", ")}`);
        }

        post({
            stage: "done",
            round: message.round,
            mode,
            cached: cache.size,
            missing: missing.join(",")
        });
    }

    function announce() {
        let tries = 0;

        hello = setInterval(() => {
            if (++tries > HELLO_TRIES) {
                clearInterval(hello);
                hello = null;

                return;
            }

            post({ stage: "ready" });
        }, HELLO_MS);

        post({ stage: "ready" });
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

        if (!data || data.type !== "menuapi:preload") {
            return;
        }

        if (hello) {
            clearInterval(hello);
            hello = null;
        }

        preload(data);
    });

    announce();
})();
