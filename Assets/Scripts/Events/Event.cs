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
        SceneManager.LoadScene("Battle", LoadSceneMode.Single);
        Scene scene = SceneManager.GetActiveScene();
        
        Debug.Log("Before instantiate");
        FillEnemiesToBattle();
        Debug.Log("After instantiate");
    }

    private void FillEnemiesToBattle() {
        var playerTeam = GameObject.FindGameObjectsWithTag("PlayerTeam")[0];
        // var enemyTeam = GameObject.FindGameObjectsWithTag("EnemyTeam")[0];
        var position = playerTeam.transform.position;
        Debug.Log(position);
        position[0] += 5;
        Instantiate(enemies[0], position, Quaternion.identity);
        Debug.Log(position);
    }
}