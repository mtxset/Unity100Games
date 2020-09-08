using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.DroneFly
{
    [RequireComponent(typeof(Text))]
    public class CountDownText : MonoBehaviour
    {
        public Text CountdownText;
        private MinigameManager gameManager;

        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();
        }

        private void OnEnable()
        {
            CountdownText = GetComponent<Text>();
            CountdownText.text = "3";
            StartCoroutine(Countdown());
        }

        private IEnumerator Countdown()
        {
            for (int i = 2; i > 0; i--)
            {
                CountdownText.text = i.ToString();
                yield return new WaitForSeconds(1);
            }

            gameManager.DroneEvents.EventCountdownFinished();
        }
    }
}
