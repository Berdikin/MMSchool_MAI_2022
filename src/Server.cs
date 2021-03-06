using LiteNetLib;
using LiteNetLib.Utils;

public class PacketAboutObject
{
    public float XCoordinate { get; set; }
    public float YCoordinate { get; set; }
    public float ZCoordinate { get; set; }
    public float FirstPlayerXCoordinate { get; set; }
    public float FirstPlayerYCoordinate { get; set; }
    public float FirstPlayerZCoordinate { get; set; }
    public float SecondPlayerXCoordinate { get; set; }
    public float SecondPlayerYCoordinate { get; set; }
    public float SecondPlayerZCoordinate { get; set; }
    public bool onStand { get; set; }
}

public static class Programm
{
    public static void Main(string[] args)
    {
        NetPacketProcessor netProcessor = new NetPacketProcessor();
        NetPeer first = null;

        EventBasedNetListener netListener = new EventBasedNetListener();
        NetManager netManager = new NetManager(netListener);
        netManager.Start(6431);

        netListener.ConnectionRequestEvent += request =>
        {
            request.Accept();
        };

        netListener.NetworkReceiveEvent += (client, reader, deliveryMethod) => {
            first = client;
            netProcessor.ReadAllPackets(reader, client);
        };

        netListener.PeerConnectedEvent += client =>
        {
            Console.WriteLine("We got connection: {0}", client.EndPoint);
        };

        netProcessor.SubscribeReusable<PacketAboutObject>((packet) =>
        {
            Console.WriteLine("Got a packet from client!");
            foreach (NetPeer c in netManager.ConnectedPeerList)
            {
                if (c != first)
                {
                    netProcessor.Send(c, new PacketAboutObject()
                    {
                        XCoordinate = packet.XCoordinate,
                        YCoordinate = packet.YCoordinate,
                        ZCoordinate = packet.ZCoordinate,
                        FirstPlayerXCoordinate = packet.FirstPlayerXCoordinate,
                        FirstPlayerYCoordinate = packet.FirstPlayerYCoordinate,
                        FirstPlayerZCoordinate = packet.FirstPlayerZCoordinate,
                        SecondPlayerXCoordinate = packet.SecondPlayerXCoordinate,
                        SecondPlayerYCoordinate = packet.SecondPlayerYCoordinate,
                        SecondPlayerZCoordinate = packet.SecondPlayerZCoordinate,
                        onStand = packet.onStand
                    }, DeliveryMethod.ReliableOrdered);
                }
            }
        });

        Console.WriteLine("Server Started!");
        var stop = false;
        Console.CancelKeyPress += (a, b) => stop = true;
        while (!stop)
        {
            netManager.PollEvents();
            Thread.Sleep(10);
        }
        netManager.Stop();
    }
}
