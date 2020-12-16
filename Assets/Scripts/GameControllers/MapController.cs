using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapController : MonoBehaviour {
	public TMP_Text countrysideEnvText;
	public TMP_Text townEnvText;
	public TMP_Text castleEnvText;
	[Space]
	public TMP_Text forestRoadText;
	public TMP_Text villageRoadText;
	public TMP_Text riverRoadText;
	[Space]
	public List<Image> forestRoadAreas;
	public List<Image> villageRoadAreas;
	public List<Image> riverRoadAreas;
	[Space]
	public TMP_Text mainRoadText;
	public TMP_Text sideRoadText;
	public TMP_Text sewerRoadText;
	[Space]
	public List<Image> mainRoadAreas;
	public List<Image> sideRoadAreas;
	public List<Image> sewerRoadAreas;
	[Space]
	public TMP_Text castleCourtyardRoadText;
	public TMP_Text wallsRoadText;
	public TMP_Text prisonRoadText;
	[Space]
	public List<Image> castleCourtyardRoadAreas;
	public List<Image> wallsRoadAreas;
	public List<Image> prisonRoadAreas;
	[Space]
	public TMP_Text royalHallText;
	[Space]
	public List<Image> royalHallArea;

	private Team playerTeam;
	private SceneController sceneController;

	private List<Dictionary<EnvironmentPath, List<Image>>> environments = new List<Dictionary<EnvironmentPath, List<Image>>>();
	private Dictionary<EnvironmentPath, List<Image>> countrysidePathToAreas;
	private Dictionary<EnvironmentPath, List<Image>> townPathToAreas;
	private Dictionary<EnvironmentPath, List<Image>> castlePathToAreas;
	private Dictionary<EnvironmentPath, List<Image>> royalHallPathToAreas;

	void Awake() {
		playerTeam = Team.playerTeamInstance;
		sceneController = GameMaster.instance.GetComponent<SceneController>();

		SetEnvironmentsNames();
		SetPathsNames();
		SetEnvironments();
	}

	void Start() {
		Show();
	}

	private void SetEnvironmentsNames() {
		countrysideEnvText.text = Environment.Countryside.ToDescription();
		townEnvText.text = Environment.Town.ToDescription();
		castleEnvText.text = Environment.Castle.ToDescription();
	}

	private void SetPathsNames() {
		forestRoadText.text = EnvironmentPath.ForestRoad.ToDescription();
		villageRoadText.text = EnvironmentPath.VillageRoad.ToDescription();
		riverRoadText.text = EnvironmentPath.RiverRoad.ToDescription();

		mainRoadText.text = EnvironmentPath.MainRoad.ToDescription();
		sideRoadText.text = EnvironmentPath.SideRoad.ToDescription();
		sewerRoadText.text = EnvironmentPath.SewerRoad.ToDescription();

		castleCourtyardRoadText.text = EnvironmentPath.CastleCourtyardRoad.ToDescription();
		wallsRoadText.text = EnvironmentPath.WallsRoad.ToDescription();
		prisonRoadText.text = EnvironmentPath.PrisonRoad.ToDescription();

		royalHallText.text = EnvironmentPath.RoyalHall.ToDescription();
	}

	private void SetEnvironments() {
		countrysidePathToAreas = new Dictionary<EnvironmentPath, List<Image>>
		{
			{EnvironmentPath.ForestRoad, forestRoadAreas},
			{EnvironmentPath.VillageRoad, villageRoadAreas},
			{EnvironmentPath.RiverRoad, riverRoadAreas}
		};
		townPathToAreas = new Dictionary<EnvironmentPath, List<Image>>()
		{
			{EnvironmentPath.MainRoad, mainRoadAreas},
			{EnvironmentPath.SideRoad, sideRoadAreas},
			{EnvironmentPath.SewerRoad, sewerRoadAreas}
		};
		castlePathToAreas = new Dictionary<EnvironmentPath, List<Image>>()
		{
			{EnvironmentPath.CastleCourtyardRoad, castleCourtyardRoadAreas},
			{EnvironmentPath.WallsRoad, wallsRoadAreas},
			{EnvironmentPath.PrisonRoad, prisonRoadAreas}
		};
		royalHallPathToAreas = new Dictionary<EnvironmentPath, List<Image>>()
		{
			{EnvironmentPath.RoyalHall, royalHallArea}
		};

		environments.Add(countrysidePathToAreas);
		environments.Add(townPathToAreas);
		environments.Add(castlePathToAreas);
		environments.Add(royalHallPathToAreas);
	}

	private void Show() {
		Image lastArea = null;
		int currentArea = playerTeam.CurrentArea;

		for (int i = 0; i < playerTeam.Paths.Count; ++i) {
			foreach (Image area in environments[i][playerTeam.Paths[i]]) {
				if (currentArea == 0)
					break;
				area.color = Color.green;
				lastArea = area;
				--currentArea;
			}

			// Colorize to grey
			foreach (KeyValuePair<EnvironmentPath, List<Image>> pair in environments[i]) {
				if (pair.Key == playerTeam.Paths[i])
					continue;
				foreach (Image area in pair.Value)
					area.color = Color.grey;
			}
		}

		if (lastArea != null)
			lastArea.color = Color.yellow;
	}

	public void BackButton() {
		sceneController.ResumeGameScene();
	}
}
