import { defineRouteMiddleware } from '@astrojs/starlight/route-data';

export const onRequest = defineRouteMiddleware((context) => {
  // Point the header logo + title link at the MenuAPI chooser (one level up from
  // this doc set), so visitors can switch between Legacy, Enhanced and RedM.
  // Base-agnostic so it works under both /menuapi/ and /mapi/.
  const chooser = import.meta.env.BASE_URL.replace(/\/(enhanced)\/?$/, '/');
  context.locals.starlightRoute.siteTitleHref = chooser;
});
