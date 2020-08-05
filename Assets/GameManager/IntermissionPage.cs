using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameManager
{
    class IntermissionPage : MonoBehaviour
    {
        public Text TextNextGame = null;
        public Text CountdownText = null;
        public Text PlayersScoreText = null;
        private void OnEnable()
        {
            this.TextNextGame.text = $"NEXT GAME: {GameManager.Instance.GetCurrentGameName()}";
            var playersData = GameManager.Instance.GetPlayersData();

            this.PlayersScoreText.text = string.Empty;

            for (int i = 0; i < GameManager.Instance.GetCurrentPlayersCount(); i++)
            {
                var color = ColorUtility.ToHtmlStringRGB(playersData[i].PlayerColor);
                this.PlayersScoreText.text += $"<color=\"#{color}\">player {i} has {playersData[i].GameStateData.TotalScore} point(s)</color>\n";
            }

            StartCoroutine(Countdown());
        }

        private IEnumerator Countdown()
        {
            for (int i = 3; i > 0; i--)
            {
                this.CountdownText.text = i.ToString();
                yield return new WaitForSeconds(1);
            }

            Assets.GameManager.GameManager.Instance.IntermissionDone();
        }
    }
}
