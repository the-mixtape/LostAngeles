<div align="center">
  <img src="FiveMConfig/LostAngelesIcon_Full.jpg" width="200px"/>  
</div>

<h1>LostAngeles</h1>

First, in the **FiveMConfig** folder, copy the server.cfg.sample file to server.cfg. And set `sv_licenseKey` and `steam_webApiKey`.

To build it, run `make build`. To run it, run the following commands to make a symbolic link in your server data directory:

```dos
cd /d [PATH TO THIS RESOURCE]
mklink /d <...>\server-data\resources\[lostangeles]\LostAngeles <...>\LostAngeles\Dist
mklink <...>\server-data\server.cfg <...>\LostAngeles\FiveMConfig\server.cfg
mklink <...>\server-data\LostAngelesIcon.png <...>\LostAngeles\FiveMConfig\LostAngelesIcon.png
```

Afterwards, you can use `ensure LostAngeles` in your server.cfg or server console to start the resource.