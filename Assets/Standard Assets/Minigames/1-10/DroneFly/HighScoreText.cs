using UnityEngine;
using UnityEngine.UI;

namespace Minigames.DroneFly
{
    [RequireComponent(typeof(Text))]
    public class HighScoreText : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponent<Text>().text = $"Highscore: {PlayerPrefs.GetInt("Highscore")}";
        }
    }
}
