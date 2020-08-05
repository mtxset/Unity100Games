using UnityEngine;
using UnityEngine.UI;

namespace Assets.Minigames.DroneFly
{
    [RequireComponent(typeof(Text))]
    public class ScoreText : MonoBehaviour
    {
        private MinigameManager gameManager;

        private void OnEnable()
        {
            this.gameManager = GetComponentInParent<MinigameManager>();
            GetComponent<Text>().text = $"Score: {this.gameManager.Score}";
        }
    }
}
