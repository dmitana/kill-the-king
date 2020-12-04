using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactersDetailController : MonoBehaviour {
	public GameObject charactersDetailUI;
	public CharacterDetailUI characterDetailUI;
	public GameObject characterDetailPosition;
	public Button nextButton;

	private Team playerTeam;
	private SceneController sceneController;

	void Awake() {
		playerTeam = Team.playerTeamInstance;
		sceneController = GameMaster.instance.gameObject.GetComponent<SceneController>();
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
		Debug.Log(totalSkillPoints);
		nextButton.interactable = totalSkillPoints > 0 ? false : true;
	}

	public void NextButton() {
		sceneController.ChangeScene("PathSelection", true);
	}

}
