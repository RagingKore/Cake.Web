#Download and install Chocolatey
iex ((new-object net.webclient).DownloadString("https://chocolatey.org/install.ps1"))

## Install WebPI
cInst webpicommandline -h

#Install Url Rewrite and ARR
$webPiProducts = @('UrlRewrite2', 'ARRv3_0') 
WebPICMD /Install /Products:"$($webPiProducts -join ',')" /AcceptEULA

#Start Services
Start-Service W3SVC
Start-Service WAS