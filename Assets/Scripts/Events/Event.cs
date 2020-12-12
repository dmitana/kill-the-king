using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public abstract class Event : MonoBehaviour {
	public String description;
	public int eventExpReward = 25;
	public int expRewardPerEnemy = 10;
	[Space]
	public String battleSceneName;
	public int minEnemiesCount = 1;
	public int maxEnemiesCount = 1;
    public List<Character> possibleEnemies = new List<Character>();

	public List<Character> Enemies { get; private set; } = new List<Character>();

	private Team playerTeam;
    private SceneController sceneController;

	public delegate void OnClickDelegate(Event sender);
	public event OnClickDelegate onOpen;
	public event OnClickDelegate onClose;

	void Awake() {
		playerTeam = Team.playerTeamInstance;
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
		generateEnemies();
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

	private void GiveExpToPlayerTeam(int exp) {
		playerTeam.AddExp(exp);
	}

	private void GiveEventExpReward() {
		GiveExpToPlayerTeam(eventExpReward);
	}

	private void GiveBattleExpReward() {
		GiveExpToPlayerTeam(expRewardPerEnemy * Enemies.Count);
	}

	private void generateEnemies() {
		Random rnd = new Random();
		int enemiesCount = rnd.Next(minEnemiesCount, maxEnemiesCount + 1);

		while (enemiesCount > 0) {
			foreach (Character enemy in possibleEnemies) {
				if (rnd.NextDouble() < 0.5f) {
					Enemies.Add(enemy);
					if (--enemiesCount <= 0)
						break;
				}
			}
		}
	}

	protected void Success() {
		OnClose();
		GiveExpToPlayerTeam(eventExpReward);
        Destroy(gameObject);
	}

	protected void MoveToBattle(bool isFinalBattle) {
		OnClose();
		if (isFinalBattle)
			sceneController.ChangeToBattleScene(
				battleSceneName, Enemies, () => sceneController.EndGameWin()
			);
		else
			sceneController.ChangeToBattleScene(battleSceneName, Enemies, GiveBattleExpReward);
        Destroy(gameObject);
	}
}
