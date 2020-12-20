using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Visualizes battle event.
/// </summary>
[RequireComponent(typeof(Event))]
public class EventVisualizer : MonoBehaviour {
	private Event e;

    void Start() {
		e = GetComponent<Event>();
		VisualizeEvent();
    }

	/// <summary>
	/// Visualizes battle event as its generated enemies.
	/// </summary>
	private void VisualizeEvent() {
		Vector3 position = transform.position;

		foreach (Character enemy in e.Enemies) {
			Instantiate(enemy, position, Quaternion.identity, transform);
			position[0] += 1;
		}
	}
}
