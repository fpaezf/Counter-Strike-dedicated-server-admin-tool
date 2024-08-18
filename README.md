![1](https://github.com/user-attachments/assets/4891f8e9-f216-4300-a951-15bf40895bad)

# Counter-Strike dedicated server remote admin tool
<img alt="Windows" src="https://img.shields.io/badge/-Windows-0078D6?style=flat&logo=windows&logoColor=white"/> <img alt="NET" src="https://img.shields.io/badge/-Visual%20Basic-blue?style=flat&logo=.net&logoColor=white"/>

This software is a **RCON** (**R**emote **CON**sole) tool that allows you to control and manage multiple **Counter-Strike 2** or other Source engine based game servers at same time using the TCP/IP-based **Source RCON protocol**.

In the past, a lot of RCON tools were published, maybe the most popular was **HLSW** but currently the project is abandoned and no updates are released. Some clones like **Source Admin Tool** tried to take over but most of them are outdated and doesn't support Counter-Strike 2 at 100%. 

- **Source Admin Tool:** https://github.com/Drifter321/admintool

Seeing the lack of updated and functional tools to manage my own game servers i decided to code a new one by myself. In early releases of this tool i've used a RCON library i found in GitHub but it was unstable and makes the application crash.

- **RCON DotNET by Untodesu:** https://github.com/untodesu/rcon-dotnet

Finally i coded my own RCON class and rebuilt the application from scratch without the third party dlls.

- **Source RCON protocol in VB .net:** https://github.com/fpaezf/Valve-RCON-protocol-in-VB.NET

## The Source RCON protocol
The **Source RCON Protocol** is a TCP/IP-based network communication protocol which allows to remotely send console commands to any Source-based game server. The most common use of the RCON protocol is to allow server owners to manage their game servers without direct access to the machine where the game server is running on.

In order for commands to be accepted, the connection between client and server must first be authenticated using the server's **RCON password**, which can be set up using a special console variable. Normally, this variable is located inside **server.cfg** file.

 You can view more details of how the Source RCON protocol works in Valve's website:

- **Source RCON Protocol:** https://developer.valvesoftware.com/wiki/Source_RCON_Protocol

**rcon_password "123456"**

## Main features
- Visual servers list
- GeoIP-based servers country flags
- Manage workshop maps/collections
- Retrieve map group from server
- Source RCON protocol data query from servers
- Steam web API data query from servers
- Retrieve server players list via RCON commands or A2S UDP packets
- Kick/Ban/Mute players by Name, Ip or ID
- Quick server actions (add bots, change map, restart game...)
- Send console commands and show the server's response
- Autofilling console commands dropdown list
- Send/receive/log chat messages
- Edit/Save predefined broadcast messages
- Auto send messages every X minutes
- Scheduled commands (daily at specified time or every x minutes)
- Open the game and join a server
- Shutdown remote server
- Application log
- Server manager
- Metamod & Counter-StrikeSharp commands

## Why source code is not published?
I shared my RCON class in GitHub to allow users to copy and modify the code. RCON class is the core of this application, all other stuff is only a fancy GUI.
Stop asking for full source code, i will not publish it. You can use my class and create your own tool as i did.

- **Source RCON protocol in VB .net:** https://github.com/fpaezf/Valve-RCON-protocol-in-VB.NET
- **A2S_INFO implementation in VB .net:** https://github.com/fpaezf/vb.net-a2s_Info
- **A2S_PLAYER implementation in VB .net:** https://github.com/fpaezf/vb.net-a2s_Player

## Your code is a pile of sh!t
I know my code is a spagetti bowl, i'm not a professional developer, feel free to fix it and if you want, share your corrections with me to keep it updated and working.

- **If you found a bug or need any modification:** https://github.com/fpaezf/Counter-Strike-dedicated-server-admin-tool/issues
- 
## Screenshots
![1](https://github.com/user-attachments/assets/7948f26c-f243-415e-b85d-4b89c150d0bb)
![2](https://github.com/user-attachments/assets/5b8452c4-13ce-4fa9-bb98-e2e88facb7d1)
![3](https://github.com/user-attachments/assets/c3186b02-315d-4c34-84b9-4b1d393b2ded)
![4](https://github.com/user-attachments/assets/9e1085ef-2a3b-4078-849e-877c46efae5d)
![5](https://github.com/user-attachments/assets/0ff4c415-675f-4758-9fc0-592178cd1c23)
![6](https://github.com/user-attachments/assets/0cf86d95-d300-4c5b-ad2a-39742112a8e8)
![7](https://github.com/user-attachments/assets/3021d7de-8580-4cf5-98c6-158431c9daa4)
![8](https://github.com/user-attachments/assets/ce89eea6-6036-4ad9-aaf0-97b489b780f8)
![9](https://github.com/user-attachments/assets/80453c35-f6ba-4764-bd73-4a16f527bea5)

## Other CS2 admin tools
![aaaa](https://github.com/fpaezf/CS2-RCON-Tool-V2/assets/28062918/253d78d8-0b6c-4a0e-8302-f27659c58b3d)

To easly manage my own game servers i also published a tool that allows you to install, update, start and stop the servers with just one click.

- Conter-Strike 2 server manager tool: https://github.com/fpaezf/CS2-server-manager

You can publish your comments, ask for new features or send your feedback on Reddit:
- https://www.reddit.com/r/cs2/comments/172lgds/my_cs2_server_installerupdaterlauncher/
