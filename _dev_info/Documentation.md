# Documentation NTIPOS-backend

This is the documentation for the backend side of NTIPOS. 

## How to setup server

The instructions are written to for Debian 12

1. Install dotnet 9.0 following [instructions from Microsoft](https://learn.microsoft.com/dotnet/core/install/)
2. Install git using `apt install git`.
3. Clone the repository `git clone https://github.com/NTIG-Uppsala/NTIPOS-backend`
4. Run `cp ~/NTIPOS-backend/server_setup/StockAPI.service.template /etc/systemd/system/StockAPI.service`
5. Edit the file according to the instructions in the file to use the correct user, ip address and port.
6. Change directory to the project directory. `cd ~/NTIPOS-backend/project_files/StockAPI`. 
7. Build the project. `dotnet publish -c Release -o /srv/StockAPI/`.  
It will succeed with one warning. 
8. Update the services and start the API service.  
```
systemctl daemon-reload
systemctl start StockAPI.service
```

## How to update the API on the server

1. Change directory to the project directory. `cd ~/NTIPOS-backend/project_files/StockAPI`.
2. Pull the new version (or use git to find the version you want) `git pull`
3. Rebuild the project and restart the service.  
```
dotnet publish -c Release -o /srv/StockAPI/
systemctl restart StockAPI.service
```
It will succeed with one warning. 

If the application has unexpected behavior try to enter the following:

```
rm -r /srv/StockAPI
dotnet publish -c Release -o /srv/StockAPI/
systemctl restart StockAPI.service
```
---
[Go back to README](../README.md)
