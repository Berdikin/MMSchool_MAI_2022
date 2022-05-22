// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using LiteNetLib;
// using System.Net;

// public class Serverscript : MonoBehaviour
// {
// 	NetManager netManager;
// 	EventBasedNetListener netListener;
//     // Start is called before the first frame update
//     void Start()
//     {
//         Debug.LogError("АФФТАР ВЫПЕЙ ЙАДУ");
//         netListener = new EventBasedNetListener();
        
//         netListener.ConnectionRequestEvent += (request) => {
//         request.Accept();
//     };
    
//     netListener.PeerConnectedEvent += (client) => {
//     	Debug.LogError($"Client conncted: {client}");
//     };
    
//     netManager = new NetManager(netListener);
//     }

//     // Update is called once per frame
//     void Update()
//     {
//     netManager.PollEvents();
//     }
// }
