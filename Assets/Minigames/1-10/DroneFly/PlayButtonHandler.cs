using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Minigames.DroneFly
{
    public class PlayButtonHandler : MonoBehaviour
    {
        private MinigameManager gameManager;
        private void Start()
        {
            this.gameManager = GetComponentInParent<MinigameManager>();
            if (this.gameManager == null)
            {
                throw new Exception("Game Manager was not injected");
            }
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            this.gameManager.StartGame();
        }
    }
}
