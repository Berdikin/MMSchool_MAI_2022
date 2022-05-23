using LiteNetLib;
using LiteNetLib.Utils;

public class FooPacket{
    public int NumberValue { get; set; }
    public string StringValue { get; set; }
}

public static class Programm
{
    public static void Main(string[] args)
    {
        NetPacketProcessor netProcessor = new NetPacketProcessor();

        EventBasedNetListener netListener = new EventBasedNetListener();
        NetManager netManager = new NetManager(netListener);
        netManager.Start(6431 /* port */);

        netListener.ConnectionRequestEvent += request =>
        {
           request.Accept();
        };

        netListener.PeerConnectedEvent += client =>
        {

            Console.WriteLine("We got connection: {0}", client.EndPoint); // Show peer ip
            netProcessor.Send(client, new FooPacket(){NumberValue = 1, StringValue = "TEST"}, DeliveryMethod.ReliableOrdered);
        };

        Console.WriteLine("Server Started!");
        var stop = false;
        Console.CancelKeyPress += (a, b) => stop = true;
        while (!stop)
        {
            netManager.PollEvents();
            Thread.Sleep(15);
        }
        netManager.Stop();
    }
}
