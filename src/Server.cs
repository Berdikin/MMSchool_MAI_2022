using LiteNetLib;
using LiteNetLib.Utils;

public class FooPacket
{
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
        netManager.Start(6431);

        netListener.ConnectionRequestEvent += request =>
        {
            request.Accept();
        };

        netListener.NetworkReceiveEvent += (client, reader, deliveryMethod) => {
            netPacketProcessor.ReadAllPackets(reader, client);
        };

        netListener.PeerConnectedEvent += client =>
        {
            Console.WriteLine("We got connection: {0}", client.EndPoint); // Show peer ip
            netProcessor.Send(client, new FooPacket() { NumberValue = 1, StringValue = "From server" }, DeliveryMethod.ReliableOrdered);
        };

        netProcessor.SubscribeReusable<FooPacket>((packet) =>
        {
            Console.WriteLine("Got a packet from client!");
            Console.WriteLine(packet.StringValue);
            if (packet.NumberValue == 2)
            {
                netManager.SendToAll(netProcessor.Write(new FooPacket() { NumberValue = packet.NumberValue, StringValue = "From client to client" }), DeliveryMethod.ReliableOrdered);
            }
        });

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
