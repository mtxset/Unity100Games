using UnityEngine;
using UnityEngine.UI;

namespace Minigames.DroneFly
{
    [RequireComponent(typeof(Text))]
    public class ScoreText : MonoBehaviour
    {
        private MinigameManager gameManager;

        private void OnEnable()
        {
            gameManager = GetComponentInParent<MinigameManager>();
            GetComponent<Text>().text = $"Score: {gameManager.Score}";
        }
    }
}
