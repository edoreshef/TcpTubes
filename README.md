TCP Tubes
=========
TcpTubes is a super light-weight .NET class library that allows very simple and fast TCP client-server communication.
TcpTubes will:
- Expose a simple SendMessage and GetMessage API 
- Perform packet boundary restoration for incoming messages
- Performs Keepalive connection checks
- Takes care of re-connection when connection is dropped
- Notify on new or dropped connection changes using the message queue.

Concepts
--------
- No need to worry about connection state. If client is disconnected it will retry connecting to server until it manages to do so. Once connection was formed, the user will be reported on the new connection with a '#connected' message.
- All messages contain an ID (string) and a data (byte array). There are two types of messages, there are *internal* messages and *user* messages. User messages travel between the client and the server and internal messages are used for local notifications (#connected #disconnected).
- For *both* the client and server, messages are sent and received using the SendMessage and GetMessage methods.  
- GetMessage reads a message from the message queue. If queue is empty GetMessage it can either exit immediately or block until message is received (timeOut parameter).

Examples
--------
There are two examples: 
- TubesTerminal: an application that will help you test you client or server.
- SimplePingPong: an application to test the library latency (~70000 ping-pongs/sec)
