using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Event : MonoBehaviour {
    public GameObject popUpWindow;
    public TMP_Text popUpText;
    public Button acceptButton;
    public Button rejectButton;
    public String description;
	[Space]
    public List<Character> enemies = new List<Character>();
	public int expReward;

	private Team playerTeam;
    private SceneController sceneController;

    void Awake() {
        rejectButton.onClick.AddListener(OnRejectButtonClick);
        acceptButton.onClick.AddListener(OnAcceptButtonClick);

		playerTeam = Team.playerTeamInstance;
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
    }

    private void OnMouseDown() {
        popUpWindow.SetActive(true);
        popUpText.text = description;
    }

    private void OnRejectButtonClick() {
        popUpWindow.SetActive(false);
        Destroy(gameObject);
    }

    protected abstract void OnAcceptButtonClick();

	private void GiveExpToPlayerTeam() {
		playerTeam.AddExp(expReward);
	}

	protected void Success() {
        popUpWindow.SetActive(false);
		GiveExpToPlayerTeam();
        Destroy(gameObject);
	}

	protected void MoveToBattle(String battleSceneName) {
        popUpWindow.SetActive(false);
		sceneController.ChangeToBattleScene(battleSceneName, enemies, GiveExpToPlayerTeam);
        Destroy(gameObject);
	}
}
