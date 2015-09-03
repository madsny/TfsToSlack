# TfsToSlack
Polling build results from TFS and pushing them to slack via webhook

1. Create an incoming webHook in the slack team of your choice
2. Modify appSettings.config
3. Build
4. TfsPoller.exe install
5. TfsPoller.exe start


##Additional application settings:
* slack.username (default "TFS Build Bot")
* slack.defaultchannel (default #general) - where all non-failed-build notifications are posted
* slack.failedchannel (default #general)
* slack.failedicon (default :name_badge:)
* slack.defaulticon (default :white_check_mark:)
