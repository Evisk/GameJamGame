
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Helper : MonoBehaviour {
#if UNITY_EDITOR
    [MenuItem("Helper/Init SOs")]
    public static void InitSOs() {
        var values = Enum.GetValues(typeof(StatisticName));

        foreach (var value in values) {
            var name = Enum.GetName(typeof(StatisticName), value);

            StatSO asset = ScriptableObject.CreateInstance<StatSO>();
            asset.stat = (StatisticName)value;

            asset.effectiveness = new List<StatSO.Effectiveness>() {
                new StatSO.Effectiveness() {
                    team = TeamName.Aliens,
                    effectiveness = 0,
				},
                new StatSO.Effectiveness() {
                    team = TeamName.FlockOfBirds,
                    effectiveness = 0,
                },
                new StatSO.Effectiveness() {
                    team = TeamName.GameJam,
                    effectiveness = 0,
                },
                new StatSO.Effectiveness() {
                    team = TeamName.PacMan,
                    effectiveness = 0,
                },
                new StatSO.Effectiveness() {
                    team = TeamName.Pirates,
                    effectiveness = 0,
                },
            };
            
            AssetDatabase.CreateAsset(asset, $"Assets/Stats/{name}.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
        }
    }
    #endif

    public static string GetStatName(StatisticName stat) {
        switch (stat) {
            case StatisticName.ALCowRustling:
                return "Cow Rustling";
            case StatisticName.ALCropCircleDrawing:
                return "Crop Circle Drawing";
            case StatisticName.ALGlutimusMaximusExploration:
                return "Glutimus Maximus Exploration";
            case StatisticName.FBLeadership:
                return "Leadership";
            case StatisticName.FBNaturalMiniBombs:
                return "Natural Mini-Bombs";
            case StatisticName.FBNavigation:
                return "Navigation";
            case StatisticName.GJGadgetWidgets:
                return "Gadgets / Widgets";
            case StatisticName.GJHackerTools:
                return "Hacker Tools";
            case StatisticName.GJInspiration:
                return "Inspiration";
            case StatisticName.PMAppetite:
                return "Appetite";
            case StatisticName.PMMazeArcadePower:
                return "Maze Arcade Power";
            case StatisticName.PMStealth:
                return "Stealth";
            case StatisticName.PPillagingLooting:
                return "Pillaging / Looting";
            case StatisticName.PPirateCodex:
                return "Pirate Codex";
            case StatisticName.PWeaponry:
                return "Weaponry";
            default:
                return stat.ToString();
		}
	}
}