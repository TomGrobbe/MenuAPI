import { defineRouteMiddleware } from '@astrojs/starlight/route-data';

export const onRequest = defineRouteMiddleware((context) => {
  // Point the header logo + title link at the MenuAPI chooser (one level up from
  // this doc set), so visitors can switch between Legacy, Enhanced and RedM.
  // Base-agnostic so it works under both /menuapi/ and /mapi/.
  const chooser = import.meta.env.BASE_URL.replace(/\/(legacy|redm)\/?$/, '/');
  context.locals.starlightRoute.siteTitleHref = chooser;
  context.locals.starlightRoute.entry.data.banner = {
    content: `These are the docs for MenuAPI 3.x and older, and they are no longer updated. Using MenuAPI 4.0 or newer? Read the <a href="${chooser}fivem/">new FiveM docs</a> instead.`,
  };
});
