using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {
	public GameObject[] instructions;
	public Event exampleEvent;
	public Battle exampleBattleEvent;
	public CharactersUI charactersUI;
	public MapUI mapUI;

	private int instructionIndex = 0;

	private List<Event> events;
	private bool isExampleEventOpen = false;
	private bool isExampleEventFinished = false;
	private bool isExampleBattleEventOpen = false;
	private bool isExampleBattleEventFinished = false;

	private bool isCharactersUIOpen = false;
	private bool isMapUIOpen = false;


	void OnEnable() {
		exampleEvent.onOpen += OpenExampleEvent;
		exampleEvent.onClose += CloseExampleEvent;
		exampleEvent.onFinish += FinishExampleEvent;

		exampleBattleEvent.onOpen += OpenExampleBattleEvent;
		exampleBattleEvent.onFinish += FinishExampleBattleEvent;

		charactersUI.onClick += OpenCharactersUI;
		mapUI.onClick += OpenMapUI;
	}

	void OnDisable() {
		exampleEvent.onOpen -= OpenExampleEvent;
		exampleEvent.onClose -= CloseExampleEvent;
		exampleEvent.onFinish -= FinishExampleEvent;

		exampleBattleEvent.onOpen -= OpenExampleBattleEvent;
		exampleBattleEvent.onFinish -= FinishExampleBattleEvent;

		charactersUI.onClick -= OpenCharactersUI;
		mapUI.onClick -= OpenMapUI;
	}

	void Start() {
		// Do not allow player to click on event before tutorial will allow it
		events = new List<Event>();
		GameObject[] eventsGO = GameObject.FindGameObjectsWithTag("Event");

		foreach (GameObject go in eventsGO) {
			Event e = go.GetComponent<Event>();
			e.IsClickable = false;
			events.Add(e);
		}
	}

    void Update() {

		for (int i = 0; i < instructions.Length; i++) {
			if (i == instructionIndex)
				instructions[i].SetActive(true);
			else
				instructions[i].SetActive(false);
		}

		// Move
		if (instructionIndex == 0) {
			if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
					Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) {
				++instructionIndex;
			}
		}
		// Event 1
		else if (instructionIndex == 1) {
			exampleEvent.IsClickable = true;
			if (isExampleEventOpen)
				++instructionIndex;
		}
		// Event 2
		else if (instructionIndex == 2) {
			if (isExampleEventFinished)
				++instructionIndex;
		}
		// Battle event 1
		else if (instructionIndex == 3) {
			exampleBattleEvent.IsClickable = true;
			if (isExampleBattleEventOpen)
				++instructionIndex;
		}
		// Battle event 2
		else if (instructionIndex == 4) {
			if (isExampleBattleEventFinished)
				++instructionIndex;
		}
		// Characters' detail
		else if (instructionIndex == 5) {
			// TODO: press C is not working
			if (Input.GetButtonDown("Characters"))
				isCharactersUIOpen = true;
			if (isCharactersUIOpen)
				++instructionIndex;
		}
		// Map
		else if (instructionIndex == 6) {
			if (isMapUIOpen)
				++instructionIndex;
		}
		else {
			// Make all events clickable
			foreach (Event e in events)
				e.IsClickable = true;

			instructions[instructionIndex - 1].SetActive(false);
			Destroy(gameObject);
		}
    }

	private void OpenExampleEvent(Event e) {
		isExampleEventOpen = true;
	}

	private void CloseExampleEvent() {
		isExampleEventFinished = true;
	}

	private void FinishExampleEvent(Event e) {
		isExampleEventFinished = true;
	}

	private void OpenExampleBattleEvent(Event e) {
		isExampleBattleEventOpen = true;
	}

	private void FinishExampleBattleEvent(Event e) {
		isExampleBattleEventFinished = true;
	}

	private void OpenCharactersUI() {
		isCharactersUIOpen = true;
	}

	private void OpenMapUI() {
		isMapUIOpen = true;
	}
}
