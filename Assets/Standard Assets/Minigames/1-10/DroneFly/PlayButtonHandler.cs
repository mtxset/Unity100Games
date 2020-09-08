using System;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.DroneFly
{
    public class PlayButtonHandler : MonoBehaviour
    {
        private MinigameManager gameManager;
        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();
            if (gameManager == null)
            {
                throw new Exception("Game Manager was not injected");
            }
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            gameManager.StartGame();
        }
    }
}
