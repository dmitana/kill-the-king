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
    public List<Character> enemies = new List<Character>();

    private SceneController _sceneController;

    void Awake() {
        rejectButton.onClick.AddListener(OnRejectButtonClick);
        acceptButton.onClick.AddListener(OnAcceptButtonClick);
        
        UnityEngine.Object obj = FindObjectOfType(typeof(SceneController));
        _sceneController = (SceneController) obj;
    }

    private void OnMouseDown() {
        popUpWindow.SetActive(true);
        popUpText.text = description;
    }

    private void OnRejectButtonClick() {
        popUpWindow.SetActive(false);
        Destroy(gameObject);
    }
    
    private void OnAcceptButtonClick() {
        popUpWindow.SetActive(false);
        _sceneController.ChangeScene("Battle");
        Destroy(gameObject);
    }
}