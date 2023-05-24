using NetCoreServer;

namespace Cube.Socket.Server;

public class CommandHandler
{
    public async Task RequestWelcome(TcpSession session, string message)
    {
        await Task.Delay(10 * 1000);
        session.SendAsync("Message received complete...");        
    }
}
