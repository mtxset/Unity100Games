using Components;
using UnityEngine;

namespace Minigames.Ninja {
  public class HitCollider : AddMinigameManager2 {
		public AudioClip ShurikenHitSound;
		private void OnCollisionEnter2D(Collision2D other) {
			if (other.collider.gameObject.tag == "hit") {
				MinigameManager.Events.EventScored();
				MinigameManager.AudioSource.PlayOneShot(ShurikenHitSound);
				Destroy(other.gameObject);
			}
		}
	}
}
