using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    public String debugMessage;
    // Start is called before the first frame update
    void Start()
    {
        debugMessage = "Event triggered!";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(debugMessage);
        }
    }

    // Show event dialog window after event is triggered by mouse click
    void Show()
    {
        
    }
}
