using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpWindowUI : MonoBehaviour {
	public Image popUpWindow;
	public TMP_Text popUpText;
	public Button acceptButton;
	public Button rejectButton;

	private List<Event> events;

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

	void OnDisable() {
		foreach (Event e in events) {
			e.onOpen -= Show;
			e.onClose -= Hide;
		}
	}

	private void Show(Event e) {
		popUpWindow.gameObject.SetActive(true);
		popUpText.text = e.description;
		rejectButton.onClick.AddListener(e.OnReject);
		acceptButton.onClick.AddListener(e.OnAccept);
	}

	private void Hide(Event e) {
		popUpWindow.gameObject.SetActive(false);
	}
}
