#!/bin/bash

printingServicePath=$(dirname $(readlink -f "$0"))/..
curPath=$(dirname $(readlink -f "$0"))

sudo chmod +x $curPath/dotnet-install.sh

$curPath/dotnet-install.sh --channel 7.0 --runtime aspnetcore

serviceFile=/etc/systemd/system/misexchange.service

echo -e "[Unit]\nDescription=MisExchange Service\n\n[Service]\nType=notify\nExecStart=dotnet $printingServicePath/MisExchange.dll\n\n[Install]\nWantedBy=multi-user.target" > $serviceFile

sudo systemctl daemon-reload
sudo systemctl start misexchange.service
sudo systemctl enable misexchange.service

if [ -f $serviceFile ]
then
echo "Сервис обменки установлен"
else
echo "Сервис обменки не удалось установить"
fi

