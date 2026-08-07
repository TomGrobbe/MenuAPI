-- Manifest data
fx_version 'cerulean'
games { 'gta5' }

-- Resource stuff
name 'MenuAPI TestMenu'
description 'Example resource that exercises every MenuAPI menu item type. Not shipped, for testing only.'
author 'Tom Grobbe'
url 'https://github.com/tomgrobbe/menuapi/'

-- The build copies the whole resolved assembly graph next to the entry script
-- (CopyLocalLockFileAssemblies), and FiveM only loads what is listed here.
files {
    'CitizenFX.Base.dll',
    'CitizenFX.FiveM.Shared.dll',
    'CitizenFX.FiveM.Client.dll',

    -- Dependencies of the CitizenFX client package rather than anything MenuAPI uses directly.
    'MessagePack.dll',
    'MessagePack.Annotations.dll',
    'Microsoft.NET.StringTools.dll',

    'MenuAPI.dll',
}

-- Client assembly
client_script 'TestMenu.net.dll'
