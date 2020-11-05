using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Event : MonoBehaviour {
	public GameObject popUpWindow;
	public TMP_Text popUpText;
	public Button acceptButton;
	public Button rejectButton;
	public String description;
    public String debugMessage;

    void Awake() {
		rejectButton.onClick.AddListener(OnRejectButtonClick);
		acceptButton.onClick.AddListener(OnAcceptButtonClick);
    }

	private void OnMouseDown() {
		popUpWindow.SetActive(true);
		popUpText.text = description;
	}

	private void OnRejectButtonClick() {
		popUpWindow.SetActive(false);
		Destroy(gameObject);
	}

	// TODO: Add implementation
	private void OnAcceptButtonClick() {
		Debug.Log("Click on Accept button");
		popUpWindow.SetActive(false);
		Destroy(gameObject);
		SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
	}
}
