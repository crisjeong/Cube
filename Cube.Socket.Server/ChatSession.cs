using Microsoft.Extensions.Logging;
using NetCoreServer;
using Serilog;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Cube.Socket.Server;


class ChatSession : TcpSession
{    
    //비동기 deleate 선언
    public delegate Task AsyncMethodDelegate<in T1, in T2>(T1 arg1, T2 arg2);

    CommandHandler commandHandler = new CommandHandler();
    Dictionary<string, AsyncMethodDelegate<TcpSession, string>> HandlerMap = new Dictionary<string, AsyncMethodDelegate<TcpSession, string>>();
    

    public ChatSession(TcpServer server) : base(server) 
    {
        RegistHandler();
    }

    void RegistHandler()
    {
        HandlerMap.Add("welcome", commandHandler.RequestWelcome);
    }

    protected override void OnConnected()
    {
        //Console.WriteLine($"Chat TCP session with Id {Id} connected!. Total Sessions= {Server.ConnectedSessions}");
        Log.Logger.Information($"Chat TCP session with Id {Id} connected!. Total Sessions= {Server.ConnectedSessions}");

        // Send invite message
        string message = "Hello from TCP chat! Please send a message or '!' to disconnect the client!";
        SendAsync(message);
    }

    protected override void OnDisconnected()
    {
        //Console.WriteLine($"Chat TCP session with Id {Id} disconnected!. Total Sessions= {Server.ConnectedSessions}");
        Log.Logger.Information($"Chat TCP session with Id {Id} disconnected!. Total Sessions= {Server.ConnectedSessions}");
    }
    
    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
        IPEndPoint? iPEndPoint = (IPEndPoint?) this.Socket.RemoteEndPoint;

        Log.Logger.Information($"Incoming {DateTime.Now} :: Address= {iPEndPoint?.Address.ToString()}, {iPEndPoint?.Port} :: {message}");

        if (HandlerMap.ContainsKey(message))
        {
            HandlerMap[message](this, message);
        }
        else
        {
            // Multicast message to all connected sessions
            //Server.Multicast(message);
            SendAsync(message);
        }

        Log.Logger.Information($"outgoing {DateTime.Now} :: Address= {iPEndPoint?.Address.ToString()}, {iPEndPoint?.Port} :: {message}");
        
        // If the buffer starts with '!' the disconnect the current session
        if (message == "!")
            Disconnect();
    }

    protected override void OnError(SocketError error)
    {
        Log.Logger.Error($"Chat TCP session caught an error with code {error}");
    }
}
