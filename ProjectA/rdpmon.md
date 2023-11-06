# How to:

* Query User command: You can use the query user command to display information about user sessions on a Remote Desktop Session Host server. This command returns the following information: Name of the user, Name of the session on the Remote Desktop Session Host server, Session ID, State of the session (active or disconnected), Idle time (the number of minutes since the last keystroke or mouse movement at the session), and Date and time the user logged on. You can use this command to find out if a specific user is logged on to a specific Remote Desktop Session Host server 1.

example:
```
C:\Users\Administrator>query user
 USERNAME              SESSIONNAME        ID  STATE   IDLE TIME  LOGON TIME
>administrator         rdp-tcp#0           2  Active          .  11/3/2023 3:31 PM
```

* qwinsta command: You can run the qwinsta command on the server to get a list of all sessions (active and disconnected by an RDP timeout) on an RDS server or in a desktop Windows 10 (11) edition (even if you allowed multiple RDP connections to it). This tool returns a list of all sessions with their session ID, username, session name, and session state 2.

example:
```
C:\Users\Administrator>qwinsta
 SESSIONNAME       USERNAME                 ID  STATE   TYPE        DEVICE
 services                                    0  Disc
 console                                     1  Conn
>rdp-tcp#0         Administrator             2  Active
 rdp-tcp                                 65536  Listen
```

 Please note that you must have admin authority or special access permission to use these commands.
