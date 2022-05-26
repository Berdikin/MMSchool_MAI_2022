// Client.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;


public class PacketAboutObject
{
    public float XCoordinate { get; set; }
    public float YCoordinate { get; set; }
    public float ZCoordinate { get; set; }
    public string NameOfObject { get; set; }
}

public class Client : MonoBehaviour
{
    public GameObject wing;

    EventBasedNetListener netListener;
    NetPacketProcessor netPacketProcessor;
    NetManager netManager;


    void Start()
    {
        netListener = new EventBasedNetListener();
        netPacketProcessor = new NetPacketProcessor();

        netListener.PeerConnectedEvent += (server) =>
        {
            Debug.LogError($"Connected to server: {server}");
            netPacketProcessor.Send(server, new PacketAboutObject()
            { XCoordinate = wing.transform.position.x, YCoordinate = wing.transform.position.y, ZCoordinate = wing.transform.position.z, NameOfObject = "wing" }, DeliveryMethod.ReliableOrdered);
        };

        netListener.NetworkReceiveEvent += (server, reader, deliveryMethod) =>
        {
            netPacketProcessor.Send(server, new PacketAboutObject() 
            { XCoordinate = wing.transform.position.x, YCoordinate = wing.transform.position.y, ZCoordinate= wing.transform.position.z, NameOfObject = "Ball" }, DeliveryMethod.ReliableOrdered);
            netPacketProcessor.ReadAllPackets(reader, server);
        };

        netPacketProcessor.SubscribeReusable<PacketAboutObject>((packet) =>
        {
            Debug.LogFormat($"Object {packet.NameOfObject} now has a position: {packet.XCoordinate}, {packet.YCoordinate}, {packet.ZCoordinate}");
            Vector3 myVector = new Vector3(packet.XCoordinate, packet.YCoordinate, packet.ZCoordinate);
            wing.transform.position = myVector;
        });

        netManager = new NetManager(netListener);
        netManager.Start();
        netManager.Connect("192.168.168.171", 6431, "");
    }

    void Update()
    {
        netManager.PollEvents();
    }
}
