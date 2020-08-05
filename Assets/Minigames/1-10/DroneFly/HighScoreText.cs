using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Minigames.DroneFly
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
