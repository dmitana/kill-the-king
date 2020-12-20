using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

/// <summary>
/// Game environments.
/// </summary>
public enum Environment {
	[Description("Countryside")]
	Countryside,
	[Description("Town")]
	Town,
	[Description("Castle")]
	Castle
}

/// <summary>
/// Game paths in environments.
/// </summary>
public enum EnvironmentPath {
	[Description(("Forest Road"))]
	ForestRoad,
	[Description(("Village Road"))]
	VillageRoad,
	[Description(("Village Road"))]
	RiverRoad,
	[Description(("Main Road"))]
	MainRoad,
	[Description(("Side Road"))]
	SideRoad,
	[Description(("Sewer Road"))]
	SewerRoad,
	[Description(("Castle Courtyard Road"))]
	CastleCourtyardRoad,
	[Description(("Walls Road"))]
	WallsRoad,
	[Description(("Prison Road"))]
	PrisonRoad,
	[Description(("Royal Hall"))]
	RoyalHall
}

/// <summary>
/// Defines extensions applyable at enums.
/// </summary>
public static class EnumExtensions {
	/// <summary>
	/// Returns Description field of a enum value.
	/// </summary>
	/// <param name="val">Enum value to be used</param>
	public static string ToDescription(this Enum val) {
		DescriptionAttribute[] attributes = (DescriptionAttribute[])val
		   .GetType()
		   .GetField(val.ToString())
		   .GetCustomAttributes(typeof(DescriptionAttribute), false);
		return attributes.Length > 0 ? attributes[0].Description : string.Empty;
	}
}

/// <summary>
/// Represents constants in a game.
/// </summary>
public class Constants {
	public static Dictionary<String, EnvironmentPath> SceneToEnvironmentPath = new Dictionary<String, EnvironmentPath>
	{
		{"ForestRoad", EnvironmentPath.ForestRoad},
		{"MainRoad", EnvironmentPath.MainRoad},
		{"CastleCourtyardRoad", EnvironmentPath.CastleCourtyardRoad},
		{"RoyalHall", EnvironmentPath.RoyalHall}
	};
}
