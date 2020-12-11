using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Event))]
public class EventVisualizer : MonoBehaviour {
	private Event e;

    void Start() {
		e = GetComponent<Event>();
		VisualizeEvent();
    }

	private void VisualizeEvent() {
		Vector3 position = transform.position;

		foreach (Character enemy in e.Enemies) {
			Instantiate(enemy, position, Quaternion.identity, transform);
			position[0] += 1;
		}
	}
}
