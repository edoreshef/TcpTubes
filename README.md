TCP Tubes
=========
TcpTubes is a super light-weight .NET class library that allows very simple and fast TCP client-server communication.
TcpTubes will:
- Expose a simple SendMessage and GetMessage API that support "MessageId"
- Perform packet boundary restoration for incoming messages
- Performs Keepalive connection checks
- Takes care of reconnection when connection is dropped

See examples for usage.