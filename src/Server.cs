using LiteNetLib;
using LiteNetLib.Utils;

public class PacketAboutObject
{
    public float XCoordinate { get; set; }
    public float YCoordinate { get; set; }
    public float ZCoordinate { get; set; }
    public string NameOfObject { get; set; }
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
            netProcessor.ReadAllPackets(reader, client);
        };

        netListener.PeerConnectedEvent += client =>
        {
            Console.WriteLine("We got connection: {0}", client.EndPoint); // Show peer ip
        };

        netProcessor.SubscribeReusable<PacketAboutObject>((packet) =>
        {
            Console.WriteLine("Got a packet from client!");
            Console.WriteLine("Object {0} now have a position: {1}, {2}, {3}", packet.NameOfObject, packet.XCoordinate, packet.YCoordinate, packet.ZCoordinate);
            netManager.SendToAll(netProcessor.Write(new PacketAboutObject() 
            { XCoordinate = packet.XCoordinate , YCoordinate = packet.YCoordinate , ZCoordinate = packet.ZCoordinate , NameOfObject = packet.NameOfObject }), DeliveryMethod.ReliableOrdered);
        });

        Console.WriteLine("Server Started!");
        var stop = false;
        Console.CancelKeyPress += (a, b) => stop = true;
        while (!stop)
        {
            netManager.PollEvents();
            Thread.Sleep(500);
        }
        netManager.Stop();
    }
}
