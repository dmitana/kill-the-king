using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls message window UI used for displaying event result messages.
/// </summary>
public class MessageWindowUI : MonoBehaviour {
	public Image messageWindowUI;
	public TMP_Text messageText;
	public int hideAfterSec = 5;

	private List<Event> events;

	/// <summary>
	/// Finds all events and assigns callbacks to events they emit.
	/// </summary>
	void OnEnable() {
		events = new List<Event>();
		GameObject[] eventsGO = GameObject.FindGameObjectsWithTag("Event");

		foreach (GameObject go in eventsGO) {
			Event e = go.GetComponent<Event>();
			e.onFinish += Show;
			events.Add(e);
		}
	}

	/// <summary>
	/// Activates message window, displays event message and start hide coroutine.
	/// </summary>
	/// <param name="e">Event to be showed.</param>
	private void Show(Event e) {
		messageWindowUI.gameObject.SetActive(true);
		messageText.text = e.Message;
		StartCoroutine(Hide());
	}

	/// <summary>
	/// Deactivates message window after defined time.
	/// </summary>
	 private IEnumerator Hide() {
         yield return new WaitForSeconds(hideAfterSec);
         messageWindowUI.gameObject.SetActive(false);
     }
}
