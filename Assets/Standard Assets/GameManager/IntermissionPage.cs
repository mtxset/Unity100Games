using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
    class IntermissionPage : MonoBehaviour
    {
        public Text TextNextGame;
        public Text CountdownText;
        public Text PlayersScoreText;
        private void OnEnable()
        {
            TextNextGame.text = $"NEXT GAME: {GameManager.Instance.GetCurrentGameName()}";
            var playersData = GameManager.Instance.GetPlayersData();

            PlayersScoreText.text = string.Empty;

            for (int i = 0; i < GameManager.Instance.GetCurrentPlayersCount(); i++)
            {
                var color = ColorUtility.ToHtmlStringRGB(playersData[i].PlayerColor);
                PlayersScoreText.text += $"<color=\"#{color}\">player {i} has {playersData[i].GameStateData.TotalScore} point(s)</color>\n";
            }

            StartCoroutine(Countdown());
        }

        private IEnumerator Countdown()
        {
            for (int i = 3; i > 0; i--)
            {
                CountdownText.text = i.ToString();
                yield return new WaitForSeconds(1);
            }

            GameManager.Instance.IntermissionDone();
        }
    }
}
