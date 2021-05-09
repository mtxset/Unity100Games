#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
namespace Minigames.RepeatColors {
	[CustomEditor(typeof(GameController))]
	public class ControllerEditor : Editor
	{
			public override void OnInspectorGUI() {
					DrawDefaultInspector();

					var myScript = (GameController)target;
					if (GUILayout.Button("RED"))
							myScript.PlayerSelectsColor(0);
					else if (GUILayout.Button("GREEN"))
							myScript.PlayerSelectsColor(1);
					else if (GUILayout.Button("BLUE"))
							myScript.PlayerSelectsColor(2);
					else if (GUILayout.Button("YELLOW"))
							myScript.PlayerSelectsColor(3);
			}
	}
}
#endif
