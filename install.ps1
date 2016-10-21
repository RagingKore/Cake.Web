## Install WebPI
cInst webpicommandline -y

#Install Url Rewrite and ARR
$webPiProducts = @('UrlRewrite2', 'ARRv3_0')
WebPICMD /Install /Products:"$($webPiProducts -join ',')" /AcceptEULA

#Start Services
Start-Service W3SVC
Start-Service WAS
