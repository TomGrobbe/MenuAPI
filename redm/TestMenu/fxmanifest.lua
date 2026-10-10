fx_version 'cerulean'
game 'rdr3'
rdr3_warning 'I acknowledge that this is a prerelease build of RedM, and I am aware my resources *will* become incompatible once RedM ships.'

name 'MenuAPI TestMenu'
description 'Example resource that exercises every MenuAPI menu item type. Not shipped, for testing only.'
author 'Tom Grobbe'
url 'https://github.com/tomgrobbe/menuapi/'

files {
    'MenuAPI.dll',
}

client_script 'TestMenu.net.dll'
