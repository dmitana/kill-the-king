using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour {
	public TMP_Text pathText;
	public List<Image> areas;

	private SceneController sceneController;
	private Team playerTeam;

	void Awake() {
		sceneController = GameMaster.instance.GetComponent<SceneController>();
		playerTeam = Team.playerTeamInstance;
	}

	void Update() {
		Show();
	}

	private void Show() {
		EnvironmentPath lastPath = playerTeam.Paths.LastOrDefault();

		// Set text
		pathText.text = playerTeam.Paths.LastOrDefault().ToDescription();

		// Royal Hall is different
		if (lastPath == EnvironmentPath.RoyalHall) {
			pathText.transform.parent.GetComponent<Image>().color = Color.yellow;
			areas[0].transform.parent.gameObject.SetActive(false);
			return;
		}

		// Set areas
		int lastArea = (playerTeam.CurrentArea - 1) % 3;
		for (int i = 0; i < lastArea; ++i)
			areas[i].color = Color.green;
		if (lastArea < areas.Count && lastArea >= 0)
			areas[lastArea].color = Color.yellow;
	}

	public void OnClick() {
		sceneController.ChangeFromGameScene("Map");
	}
}
