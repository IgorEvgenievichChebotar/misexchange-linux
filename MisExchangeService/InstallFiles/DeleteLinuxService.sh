#!/bin/bash
sudo systemctl stop misexchange.service
sudo systemctl disable misexchange.service
rm /etc/systemd/system/misexchange.service
sudo systemctl reset-failed
