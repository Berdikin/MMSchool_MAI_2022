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

    void OnCollisionEnter(Collider other)
    {
        if (other.tag == "Wing")
        {
            Debug.LogError("Произошло соединение");
            float xpos = 1.41;
            float ypos = 0.12;
            float zpos = 0;
            Vector3 wingPos = new Vector3(xpos, ypos, zpos);
            wing.transform.position = wingPos;
            float xrot = -90;
            float yrot = 0;
            float zrot = 0;
            wing.transform.eulerAngles = new Vector3(xrot, yrot, zrot);
        }
    }

    void Update()
    {
       
    }
}

