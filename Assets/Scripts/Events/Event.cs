using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

/// <summary>
/// Abstract class represents Event.
/// </summary>
public abstract class Event : MonoBehaviour {
	public String title;
	public String description;
	public int eventExpReward = 25;
	public int expRewardPerEnemy = 10;
	public float successRate = 0.5f;
	[Space]
	public String expMessage = "You earned {0} experience points.";
	public String eventMessage;
	[Space]
	public String battleSceneName;
	public int minEnemiesCount = 1;
	public int maxEnemiesCount = 1;
    public List<Character> possibleEnemies = new List<Character>();

	public List<Character> Enemies { get; private set; } = new List<Character>();
	public String Message { get; private set; }

	private Team playerTeam;
    private SceneController sceneController;

	public delegate void OnOpenDelegate(Event sender);
	public delegate void OnCloseDelegate();
	public event OnOpenDelegate onOpen;
	public event OnCloseDelegate onClose;
	public event OnOpenDelegate onFinish;

	protected Random rnd = new Random();

	void Awake() {
		playerTeam = Team.playerTeamInstance;
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
		Initialize();
	}

	protected void OnOpen() {
		onOpen?.Invoke(this);
	}

	protected void OnClose() {
		onClose?.Invoke();
	}

	protected void OnFinish() {
		onFinish?.Invoke(this);
	}

    private void OnMouseDown() {
		OnOpen();
    }

	/// <summary>
	/// Defines interface for derived Events to be able to modify initialization in Awake method.
	/// </summary>
	protected virtual void Initialize() {}

	/// <summary>
	/// Defines interface for derived Events to be able to modify event message.
	/// </summary>
	protected virtual String ModifyEventMessage() {
		return eventMessage;
	}

	/// <summary>
	/// Defines interface for derived Events to be able to modify behavior after player
	/// clicks on the reject button in a pop up window.
	/// </summary>
    public virtual void OnReject() {
		OnClose();
        Destroy(gameObject);
    }

	/// <summary>
	/// Defines compulsory interface which each derived Event has to implement.
	/// This method is executed when player clicks on the accept button in a pop up window.
	/// </summary>
    public abstract void OnAccept();

	/// <summary>
	/// Adds experience points to player team.
	/// </summary>
	/// <param name="exp">Number of experience points to be added.</param>
	private void GiveExpToPlayerTeam(int exp) {
		playerTeam.AddExp(exp);
		Message += String.Format(expMessage, exp);
		OnFinish();
	}

	/// <summary>
	/// Adds experience points to player team from the event.
	/// </summary>
	private void GiveEventExpReward() {
		GiveExpToPlayerTeam(eventExpReward);
	}

	/// <summary>
	/// Adds experience points to player team from the battle.
	/// </summary>
	private void GiveBattleExpReward() {
		GiveExpToPlayerTeam(expRewardPerEnemy * Enemies.Count);
	}

	/// <summary>
	/// Generates random number of possible enemies to the battle.
	/// </summary>
	protected void GenerateEnemies() {
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

	/// <summary>
	/// Defines behavior when the event ends successfully without battle.
	/// </summary>
	protected void Success() {
		OnClose();
		Message += ModifyEventMessage() + " ";
		GiveExpToPlayerTeam(eventExpReward);
        Destroy(gameObject);
	}

	/// <summary>
	/// Defines behavior when the event ends in a battle.
	///
	/// Changes scene to battle scene and assigns callback which will be
	/// performed at return from the battle. This callback adds exp to the player.
	///
	/// If it is final event (battle with King), then callback is scene change to
	/// end game scene.
	/// </summary>
	/// <param name="isFinalBattle">Number of experience points to be added.</param>
	protected void MoveToBattle(bool isFinalBattle = false) {
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
