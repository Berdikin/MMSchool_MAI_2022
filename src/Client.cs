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

public class Client : MonoBehaviour {
    EventBasedNetListener netListener;
    NetPacketProcessor netPacketProcessor;
    NetManager netManager;


    void Start() {
        netListener = new EventBasedNetListener();
        netPacketProcessor = new NetPacketProcessor();

        netListener.PeerConnectedEvent += (server) =>
        {
            Debug.LogError($"Connected to server: {server}");
        };

        netListener.NetworkReceiveEvent += (server, reader, deliveryMethod) =>
        {
            netPacketProcessor.ReadAllPackets(reader, server);
        };

        netPacketProcessor.SubscribeReusable<FooPacket>((packet) =>
        {
            Debug.Log("Got a foo packet!");
            Debug.Log(packet.NumberValue);
        });

        netManager = new NetManager(netListener);
        netManager.Start();
        netManager.Connect("192.168.168.171", 6431, "");
    }

    // Update is called once per frame
    void Update() {

        netManager.PollEvents();
    }
}
