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
    public bool onStand { get; set; }
}

public class Client : MonoBehaviour
{
    public GameObject wing;
    public GameObject stand;

    EventBasedNetListener netListener;
    NetPacketProcessor netPacketProcessor;
    NetManager netManager;


    void Start()
    {
        netListener = new EventBasedNetListener();
        netPacketProcessor = new NetPacketProcessor();

        netListener.PeerConnectedEvent += (server) =>
        {
            netPacketProcessor.Send(server, new PacketAboutObject()
            {
                XCoordinate = wing.transform.position.x,
                YCoordinate = wing.transform.position.y,
                ZCoordinate = wing.transform.position.z,
                onStand = GameObject.Find("moving_collision").GetComponent<Wing>().onStand
            }, DeliveryMethod.ReliableOrdered) ;
        };

        netListener.NetworkReceiveEvent += (server, reader, deliveryMethod) =>
        {
            netPacketProcessor.Send(server, new PacketAboutObject()
            {
                XCoordinate = wing.transform.position.x,
                YCoordinate = wing.transform.position.y,
                ZCoordinate = wing.transform.position.z,
                onStand = GameObject.Find("moving_collision").GetComponent<Wing>().onStand
            }, DeliveryMethod.ReliableOrdered);
            netPacketProcessor.ReadAllPackets(reader, server);
        };

        netPacketProcessor.SubscribeReusable<PacketAboutObject>((packet) =>
        {
            if (packet.onStand)
            {
                Vector3 vectorPosition_s = new Vector3(stand.transform.position.x, stand.transform.position.y, stand.transform.position.z);
                wing.transform.position = vectorPosition_s;
                GameObject.Find("moving_collision").GetComponent<Wing>().onStand = packet.onStand;
            }
            else
            {
                Vector3 vectorPosition = new Vector3(packet.XCoordinate, packet.YCoordinate, packet.ZCoordinate);
                wing.transform.position = vectorPosition;
            }
            
        });

        netManager = new NetManager(netListener);
        netManager.Start();
        netManager.Connect("192.168.168.171", 6431, "");
    }

    void Update()
    {
        netManager.PollEvents();

        if (GameObject.Find("moving_collision").GetComponent<Wing>().onStand)
        {
            Vector3 vectorPosition = new Vector3(stand.transform.position.x, stand.transform.position.y, stand.transform.position.z);
            wing.transform.position = vectorPosition;
        }
    }
}
