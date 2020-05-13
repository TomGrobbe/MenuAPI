if not defined WEBHOOK_URL goto end
curl -s -o nul -F "file=@MenuAPI-%VERSION_NAME%-%GAME%.zip" %WEBHOOK_URL%

if %APPVEYOR_REPO_TAG%==true goto end
curl -H "Content-Type:application/json" -X POST -d "{\"embeds\":[{\"title\":\"%APPVEYOR_PROJECT_NAME% (%VERSION_NAME%-%GAME%)\",\"description\":\"Build passed!\",\"color\":4502298,\"author\":{\"name\":\"Committed by %APPVEYOR_ACCOUNT_NAME%\",\"url\":\"https://github.com/%APPVEYOR_ACCOUNT_NAME%/\"},\"fields\":[{\"name\":\"AppVeyor Build\",\"value\":\"[Here](%APPVEYOR_URL%/project/%APPVEYOR_ACCOUNT_NAME%/%APPVEYOR_PROJECT_SLUG%/builds/%APPVEYOR_BUILD_ID%)\"},{\"name\":\"GitHub Commit (%APPVEYOR_REPO_COMMIT%)\",\"value\":\"[%APPVEYOR_REPO_COMMIT%](https://github.com/%APPVEYOR_ACCOUNT_NAME%/%APPVEYOR_PROJECT_NAME%/commit/%APPVEYOR_REPO_COMMIT%) - %APPVEYOR_REPO_COMMIT_MESSAGE%%APPVEYOR_REPO_COMMIT_MESSAGE_EXTENDED%\"},{\"name\":\"GitHub Branch\",\"value\":\"[%APPVEYOR_REPO_BRANCH%](https://github.com/%APPVEYOR_ACCOUNT_NAME%/%APPVEYOR_PROJECT_NAME%/tree/%APPVEYOR_REPO_BRANCH%)\"}]}]}" %WEBHOOK_URL%

:end