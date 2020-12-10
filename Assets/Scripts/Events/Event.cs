using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Event : MonoBehaviour {
	public String description;
	public String battleSceneName;
    public List<Character> enemies = new List<Character>();
	public int expReward;

	private Team playerTeam;
    private SceneController sceneController;

	public delegate void OnClickDelegate(Event sender);
	public event OnClickDelegate onOpen;
	public event OnClickDelegate onClose;

    void Start() {
		playerTeam = Team.playerTeamInstance;
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
    }

	protected void OnOpen() {
		onOpen?.Invoke(this);
	}

	protected void OnClose() {
		onClose?.Invoke(this);
	}

    private void OnMouseDown() {
		OnOpen();
    }

    public virtual void OnReject() {
		OnClose();
        Destroy(gameObject);
    }

    public abstract void OnAccept();

	private void GiveExpToPlayerTeam() {
		playerTeam.AddExp(expReward);
	}

	protected void Success() {
		OnClose();
		GiveExpToPlayerTeam();
        Destroy(gameObject);
	}

	protected void MoveToBattle() {
		OnClose();
		sceneController.ChangeToBattleScene(battleSceneName, enemies, GiveExpToPlayerTeam);
        Destroy(gameObject);
	}
}
