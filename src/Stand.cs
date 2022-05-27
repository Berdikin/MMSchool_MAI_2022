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
            Debug.LogError("��������� ����������");
            float xpos = 4.52f;
            float ypos = 0.12f;
            float zpos = 2.54f;
            Vector3 wingPos = new Vector3(xpos, ypos, zpos);
            wing.transform.position = wingPos;
            float xrot = -90f;
            float yrot = 0f;
            float zrot = 180f;
            wing.transform.eulerAngles = new Vector3(xrot, yrot, zrot);
            GameObject.Find("moving_collision").GetComponent<Wing>().onStand = true;
            //wing.GetComponent<detectedCollistion>() = false;
            wing.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void Update()
    {

    }
}
