using System;
using Components.UnityComponents.v2;
using UnityEngine.UI;

namespace Minigames.Ninja {
  public class MinigameManager : MinigameManager2 {
		public Text DifficultyText ;
		public event Action OnPlayerLightAction;

		public void PlayerLightAction() {
			OnPlayerLightAction?.Invoke();
		}

		private void LateUpdate() {
			DifficultyText.text = $"DIFFICULTY: {Math.Round(this.DiffCurrent * 100, 2)}";
		}

	}
}
