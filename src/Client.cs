// Client.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class FooPacket
{
    public int NumberValue { get; set; }
    public string StringValue { get; set; }
}

public class Client : MonoBehaviour
{
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
        };

        //çàïóñêàåòñÿ êîãäà ïðèõîäèò ñîîáùåíèå (îò êîãî ïðèøëî, äëÿ ÷òåíèÿ, êàê îòïðàâëåííî)
        netListener.NetworkReceiveEvent += (server, reader, deliveryMethod) =>
        {
            netPacketProcessor.Send(server, new FooPacket() { NumberValue = 2, StringValue = "From client" }, DeliveryMethod.ReliableOrdered);
            netPacketProcessor.ReadAllPackets(reader, server);
        };

        netPacketProcessor.SubscribeReusable<FooPacket>((packet) =>
        {
            Debug.Log("Got a packet!");
            Debug.Log(packet.StringValue);
        });

        netManager = new NetManager(netListener);
        netManager.Start();
        netManager.Connect("192.168.168.171", 6431, "");
    }

    // Update is called once per frame
    void Update()
    {
        netManager.PollEvents();
    }
}
