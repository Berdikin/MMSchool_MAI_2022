using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        float x = SixenseInput.Controllers[1].JoystickX * 5;
        float y = SixenseInput.Controllers[1].JoystickY * 5;

        x *= Time.deltaTime;
        y *= Time.deltatime;
        transform.Translate(x, 0, y);
    }
}
