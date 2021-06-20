using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Ninja {
  public class Spawner : MonoBehaviour {
		public AudioClip[] SpawnSounds;
		public AudioClip[] MeteorDeathSounds;

		public GameObject MeteorPrefab;
		public GameObject MeteorTrailPrefab;
		public float SpawnMeteorRate;
		public Vector2 MoveByMinMax;
		public float SpawnEffectsRate;

		public float DimScreenRate;
		public SpriteRenderer DimmerBox;

		private Vector2 screenSize;

		private List<GameObject> liveEntities = new List<GameObject>();
		private List<GameObject> deadEntities = new List<GameObject>();
		private float currentEffectsTimer;
		private float meteorSpawnTimer;
		private float dimmerTimer;

		private MinigameManager gameManager;

		private void Start() {
			gameManager = GetComponentInParent<MinigameManager>();

			var camera = gameManager.CurrentCamera;
			screenSize.y = camera.orthographicSize * 2;
			screenSize.x = camera.orthographicSize * camera.aspect * 2;

			gameManager.OnPlayerLightAction += HandlePlayerLightAction;

			spawnMeteor();
		}

		private void HandlePlayerLightAction() {
			dimmerTimer = 0;
		}

		private void FixedUpdate() {

			if (gameManager.GameOver) return;

			var spawnEffect = false;
			if ((currentEffectsTimer += Time.fixedDeltaTime) >= SpawnEffectsRate) {
				currentEffectsTimer = 0;
				spawnEffect = true;
			}

			if ((meteorSpawnTimer += Time.fixedDeltaTime) >= SpawnMeteorRate) {
				meteorSpawnTimer = 0;
				spawnMeteor();
			}

			// DimScreenRate - 100
			// dimmerTimer - x
			dimmerTimer += Time.fixedDeltaTime;
			DimmerBox.color = new Color(DimmerBox.color.r, DimmerBox.color.g, DimmerBox.color.b, dimmerTimer/DimScreenRate);

			var moveSpeed = Utils.DifficultyAdjuster.SpreadDifficulty(gameManager.DiffCurrent, new List<Vector2> { MoveByMinMax });

			foreach (var item in liveEntities) {

				if (item.gameObject == null) {
					deadEntities.Add(item);
					continue;
				}

				if (spawnEffect) {
					var effects = Instantiate(MeteorTrailPrefab, item.transform.position, Quaternion.identity);
					Destroy(effects, 2);
				}

				item.transform.position -= new Vector3(0, moveSpeed[0] * Time.fixedDeltaTime, 0);

				if (item.transform.position.y <= -screenSize.y / 2 + 3f) {
					deadEntities.Add(item);
				}
			}

			foreach (var item in deadEntities) {
				gameManager.Events.EventScored();
				liveEntities.Remove(item);
				gameManager.AudioSource.PlayOneShot(MeteorDeathSounds[Random.Range(0, MeteorDeathSounds.Length - 1)], 0.5f);
				Destroy(item);
			}

			deadEntities.Clear();
		}

		private void spawnMeteor() {
			var pos = new Vector2() {
				x = Random.Range(-screenSize.x / 2, screenSize.x / 2),
				y = screenSize.y / 2
			};
			var meteor = Instantiate(MeteorPrefab, pos, Quaternion.identity, transform);
			liveEntities.Add(meteor);
			gameManager.AudioSource.PlayOneShot(SpawnSounds[Random.Range(0, SpawnSounds.Length - 1)]);
		}

	}
}
