using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class Stand : MonoBehaviour
{
    public GameObject wing;

    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == wing)
        {
            Debug.LogError("Ïðîèçîøëî ñîåäèíåíèå");
            float xpos = 1.41f;
            float ypos = 0.12f;
            float zpos = 0f;
            Vector3 wingPos = new Vector3(xpos, ypos, zpos);
            wing.transform.position = wingPos;
            float xrot = -90f;
            float yrot = 0f;
            float zrot = 0f;
            wing.transform.eulerAngles = new Vector3(xrot, yrot, zrot);
            wing.GetComponent<isKinematic>() = true;
            wing.GetComponent<detectCollisions>() = false;
        }
    }

    void Update()
    {
       
    }
}
