using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindowUI : MonoBehaviour {
	public Image messageWindowUI;
	public TMP_Text messageText;
	public int hideAfterSec = 5;

	private List<Event> events;

	void OnEnable() {
		events = new List<Event>();
		GameObject[] eventsGO = GameObject.FindGameObjectsWithTag("Event");

		foreach (GameObject go in eventsGO) {
			Event e = go.GetComponent<Event>();
			e.onFinish += Show;
			events.Add(e);
		}
	}

	private void Show(Event e) {
		messageWindowUI.gameObject.SetActive(true);
		messageText.text = e.Message;
		StartCoroutine(Hide());
	}

	 IEnumerator Hide() {
         yield return new WaitForSeconds(hideAfterSec);
         messageWindowUI.gameObject.SetActive(false);
     }
}
