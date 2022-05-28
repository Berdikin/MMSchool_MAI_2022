using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class Animation : MonoBehaviour
{
    public GameObject wing;

    void Start()
    {
        
    }

    void wingAnimation()
    {
        GameObject movingPart = map[X, Y].gameObject.transform.Find("movingParts_SOLIDS").gameObject;
        movingPart.transform.Translate(Vector3.forward * Time.deltaTime);
    }

    void Update()
    {
    }
}

