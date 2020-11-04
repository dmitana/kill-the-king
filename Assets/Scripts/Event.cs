using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
	public String description;
    public String debugMessage;

    // Start is called before the first frame update
    void Start()
    {
        debugMessage = "Event triggered!";
    }

    // Update is called once per frame
    void Update()
    {

    }

	void OnMouseDown()
	{
		Debug.Log(debugMessage);
		Debug.Log(description);
	}

    // Show event dialog window after event is triggered by mouse click
    void Show()
    {

    }
}
