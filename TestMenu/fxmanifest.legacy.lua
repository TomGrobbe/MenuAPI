-- Manifest data
fx_version 'cerulean'
games { 'gta5' }

ui_page 'index.html'

-- Resource stuff
name 'MenuAPI TestMenu'
description 'Example resource that exercises every MenuAPI menu item type. Not shipped, for testing only.'
author 'Tom Grobbe'
url 'https://github.com/tomgrobbe/menuapi/'

-- The Legacy build of the TestMenu. The build copies this over as fxmanifest.lua. The CitizenFX
-- assemblies are part of the game here, so only MenuAPI itself has to be listed.
files {
    'MenuAPI.dll',

    'index.html',
    'menuapi/**/*',

    'themes/**/*',
    'menuapi-banners/**/*',
}

-- Client assembly
client_script 'TestMenu.net.dll'
