TCP Tubes
=========
TcpTubes is a light-weight .NET class library that provides super fast, simple and reliable [pneumatic tube communication](https://en.wikipedia.org/wiki/Pneumatic_tube) over TCP/IP. TcpTubes was designed for fast local intranet (or interprocess) communication.

What does TcpTubes do?
- Provides a single line code creation of client/server hub
- Client and server hubs provides a simple SendMessage and GetMessage methods
- Connection status monitoring is done via (application layered) keepalive checkings
- Client will automaticly and continuously try to form a connection on connection lost
- The library will reassemble received message frame (message boundary restoration)

Concepts
--------
- All messages contain an ID (string) and a payload (byte array). 
- There are two types of messages: *User* and *Internal*. *User* messages regular messages that are sent via *SendMessage*. *Internal* messages are used for local notifications (#connected #disconnected).
- GetMessage reads a message from the message queue. If queue is empty GetMessage it can either exit immediately or block until message is received (timeOut parameter).

Examples
--------
There are two examples: 
- TubesTerminal: an application that will help you test your client or server.
- EchoPerfTest: an application to test the library latency (~80,000 ping-pongs/sec)
