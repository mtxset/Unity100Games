using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Minigames.DroneFly
{
    [RequireComponent(typeof(Text))]
    public class CountDownText : MonoBehaviour
    {
        public Text CountdownText;
        private MinigameManager gameManager;

        private void Start()
        {
            this.gameManager = GetComponentInParent<MinigameManager>();
        }

        private void OnEnable()
        {
            this.CountdownText = GetComponent<Text>();
            this.CountdownText.text = "3";
            StartCoroutine(Countdown());
        }

        private IEnumerator Countdown()
        {
            for (int i = 3; i > 0; i--)
            {
                this.CountdownText.text = i.ToString();
                yield return new WaitForSeconds(1);
            }

            this.gameManager.DroneEvents.EventCountdownFinished();
        }
    }
}
