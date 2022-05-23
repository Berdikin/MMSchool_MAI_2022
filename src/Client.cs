// Client.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;

public class Client : MonoBehaviour {
    NetManager netManager;
    // Start is called before the first frame update
    void Start() {
        EventBasedNetListener netListener = new EventBasedNetListener();
        netManager = new NetManager(netListener);
        netManager.Start();
        netListener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
        {
            Debug.Log("Received");
            //GameObject MyGameObject = new GameObject(dataReader.GetString(100));
            Debug.LogFormat("We got:{0}", dataReader.GetString(100));
            dataReader.Recycle();
        };

        netManager.Connect("192.168.168.171", 6431, "");
    }

    // Update is called once per frame
    void Update() {
        netManager.PollEvents();
    }
}
