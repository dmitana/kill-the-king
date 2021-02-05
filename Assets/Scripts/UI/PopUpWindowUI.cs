using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls pop up window UI used for displaying events.
/// </summary>
public class PopUpWindowUI : MonoBehaviour {
	public Image popUpWindow;
	public TMP_Text popUpTitle;
	public TMP_Text popUpText;
	public Button acceptButton;
	public Button rejectButton;

	private List<Event> events;

	/// <summary>
	/// Finds all events and assigns callbacks to events they emit.
	/// </summary>
	void OnEnable() {
		events = new List<Event>();
		GameObject[] eventsGO = GameObject.FindGameObjectsWithTag("Event");

		foreach (GameObject go in eventsGO) {
			Event e = go.GetComponent<Event>();
			e.onOpen += Show;
			e.onClose += Hide;
			events.Add(e);
		}
	}

	/// <summary>
	/// Removes callbacks from events.
	/// </summary>
	void OnDisable() {
		foreach (Event e in events) {
			e.onOpen -= Show;
			e.onClose -= Hide;
		}
	}

	/// <summary>
	/// Activates pop up window, shows event and assigns listeners to reject and accept buttons.
	/// </summary>
	/// <param name="e">Event to be showed.</param>
	private void Show(Event e) {
		popUpWindow.gameObject.SetActive(true);
		popUpTitle.text = e.title;
		popUpText.text = e.description;
		rejectButton.onClick.AddListener(e.OnReject);
		acceptButton.onClick.AddListener(e.OnAccept);
	}

	/// <summary>
	/// Deactivates pop up window.
	/// </summary>
	private void Hide() {
		// Listeners from buttons have to be removed, because in
		// Show method will be added new listeners
		rejectButton.onClick.RemoveAllListeners();
		acceptButton.onClick.RemoveAllListeners();

		popUpWindow.gameObject.SetActive(false);
	}
}
