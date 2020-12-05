using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharactersDetailController : MonoBehaviour {
	public GameObject charactersDetailUI;
	public CharacterDetailUI characterDetailUI;
	public GameObject characterDetailPosition;
	public Button nextButton;
	public Button backButton;

	private Team playerTeam;
	private SceneController sceneController;

	void Awake() {
		playerTeam = Team.playerTeamInstance;
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
	}

    void OnEnable() {
		SceneManager.activeSceneChanged += EnableNextOrBackButton;
    }

    void OnDisable() {
        SceneManager.activeSceneChanged -= EnableNextOrBackButton;
    }

	void Start() {
		ShowCharactersDetail();
	}

	void Update() {
		CanContinue();
	}

	private void ShowCharactersDetail() {
		Vector3 offset = new Vector3(characterDetailUI.GetComponent<RectTransform>().rect.width + 10, 0, 0);

		for (int i = 0; i < playerTeam.Characters.Count; ++i) {
			CharacterDetailUI characterDetailUIInstantiated = Instantiate(
				characterDetailUI, characterDetailPosition.transform.position,
				Quaternion.identity, charactersDetailUI.transform
			);
			characterDetailUIInstantiated.transform.localPosition += i * offset;
			characterDetailUIInstantiated.Show(playerTeam.Characters[i]);
		}
	}

	private void CanContinue() {
		int totalSkillPoints = 0;
		foreach (Character character in playerTeam.Characters) {
			totalSkillPoints += character.SkillPoints;
		}
		nextButton.interactable = totalSkillPoints > 0 ? false : true;
	}

    private void EnableNextOrBackButton(Scene sceneCurrent, Scene sceneNext) {
		if (sceneController == null)
			return;

		if (sceneController.IsGameStarted) {
			nextButton.gameObject.SetActive(false);
			backButton.gameObject.SetActive(true);
		}
		else {
			nextButton.gameObject.SetActive(true);
			backButton.gameObject.SetActive(false);
		}
    }

	public void NextButton() {
		sceneController.ChangeScene("PathSelection", true);
	}

	public void BackButton() {
		sceneController.ResumeGameScene();
	}
}
