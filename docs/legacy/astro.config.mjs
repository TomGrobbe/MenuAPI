// @ts-check
import { defineConfig } from 'astro/config';
import starlight from '@astrojs/starlight';

// Google Analytics 4 Measurement ID (loaded on every docs page)
const GA4_ID = 'G-5GXWQVLZ8Z';

// Base is env-driven so this one project can build for both URL prefixes:
//   /menuapi/legacy (canonical) and /mapi/legacy (backwards compatibility).
const BASE = process.env.DOCS_BASE || '/menuapi/legacy';

// https://astro.build/config
export default defineConfig({
  site: 'https://docs.vespura.com',
  base: BASE,
  integrations: [
    starlight({
      title: 'MenuAPI (FiveM Legacy)',
      description: 'Documentation for MenuAPI, a FiveM and RedM C# menu API.',
      favicon: '/favicon.png',
      customCss: ['./src/styles/cartoon.css'],
      expressiveCode: { themes: ['github-dark', 'github-light'] },
      logo: { src: './src/assets/logo.png', alt: 'MenuAPI', replacesTitle: false },
      // Point the header logo + title link at the MenuAPI chooser (see src/routeData.ts).
      routeMiddleware: './src/routeData.ts',
      // SocialIcons adds a "Back to all docs" pill into the header nav.
      components: { SocialIcons: './src/components/SocialIcons.astro' },
      head: [
        { tag: 'link', attrs: { rel: 'preconnect', href: 'https://fonts.googleapis.com' } },
        { tag: 'link', attrs: { rel: 'preconnect', href: 'https://fonts.gstatic.com', crossorigin: true } },
        {
          tag: 'link',
          attrs: {
            rel: 'stylesheet',
            href: 'https://fonts.googleapis.com/css2?family=Fredoka:wght@400;500;600;700&display=swap',
          },
        },
        { tag: 'script', attrs: { async: true, src: `https://www.googletagmanager.com/gtag/js?id=${GA4_ID}` } },
        {
          tag: 'script',
          content: `window.dataLayer=window.dataLayer||[];function gtag(){dataLayer.push(arguments);}gtag('js',new Date());gtag('config','${GA4_ID}');`,
        },
      ],
      social: [{ icon: 'github', label: 'GitHub', href: 'https://github.com/TomGrobbe/MenuAPI' }],
      sidebar: [
        { label: 'Basic Info', link: '/' },
        { label: 'Setup', link: '/setup/' },
        {
          label: 'API Reference',
          items: [
            { label: 'Overview', link: '/reference/' },
            { label: 'Menu', link: '/reference/menu/' },
            { label: 'MenuController', link: '/reference/menucontroller/' },
            {
              label: 'Menu Items',
              items: [
                { label: 'Overview', link: '/reference/menuitems/' },
                { label: 'MenuItem', link: '/reference/menuitems/menuitem/' },
                { label: 'MenuCheckboxItem', link: '/reference/menuitems/menucheckboxitem/' },
                { label: 'MenuListItem', link: '/reference/menuitems/menulistitem/' },
                { label: 'MenuDynamicListItem', link: '/reference/menuitems/menudynamiclistitem/' },
                { label: 'MenuSliderItem', link: '/reference/menuitems/menuslideritem/' },
              ],
            },
            { label: 'Events', link: '/reference/events/' },
          ],
        },
        { label: 'Troubleshooting & Support', link: '/support/' },
        { label: 'F.A.Q.', link: '/faq/' },
        { label: 'Changelog', link: '/changelog/' },
        {
          label: 'Links',
          items: [
            { label: 'GitHub', link: 'https://github.com/TomGrobbe/MenuAPI', attrs: { target: '_blank' } },
            { label: 'FiveM Forum', link: 'https://forum.fivem.net/t/menuapi/204992?u=vespura', attrs: { target: '_blank' } },
            { label: 'NuGet (FiveM)', link: 'https://www.nuget.org/packages/MenuAPI.FiveM/', attrs: { target: '_blank' } },
            { label: 'NuGet (RedM)', link: 'https://www.nuget.org/packages/MenuAPI.RedM/', attrs: { target: '_blank' } },
            { label: 'vespura.com', link: 'https://vespura.com/', attrs: { target: '_blank' } },
          ],
        },
      ],
    }),
  ],
});
