using System.Collections.Generic;
using Components;
using UnityEngine;

namespace Minigames.Ninja
{
  public class Spawner : AddMinigameManager2 {
		public GameObject MeteorPrefab;
		public GameObject MeteorTrailPrefab;
		public float SpawnMeteorRate;
		public float MeteorMoveBy;
		public float SpawnEffectsRate;

		private Vector2 screenSize;

		private List<GameObject> liveEntities = new List<GameObject>();
		private List<GameObject> deadEntities = new List<GameObject>();
		private float currentEffectsTimer;
		private float meteorSpawnTimer;

		private void Start() {
			var camera = MinigameManager.CurrentCamera;
			screenSize.y = camera.orthographicSize * 2;
			screenSize.x = camera.orthographicSize * camera.aspect * 2;

			spawnMeteor();
		}

		private void FixedUpdate() {
			var spawnEffect = false;
			if ((currentEffectsTimer += Time.fixedDeltaTime) >= SpawnEffectsRate) {
				currentEffectsTimer = 0;
				spawnEffect = true;
			}

			if ((meteorSpawnTimer += Time.fixedDeltaTime) >= SpawnMeteorRate) {
				meteorSpawnTimer = 0;
				spawnMeteor();
			}

			foreach (var item in liveEntities) {
				if (spawnEffect) {
					var effects = Instantiate(MeteorTrailPrefab, item.transform.position, Quaternion.identity);
					Destroy(effects, 2);
				}

				item.transform.position -= new Vector3(0, MeteorMoveBy * Time.fixedDeltaTime, 0);

				if (item.transform.position.y <= -screenSize.y / 2 + 3f) {
					deadEntities.Add(item);
				}
			}

			foreach (var item in deadEntities) {
				liveEntities.Remove(item);
				Destroy(item);
			}

			deadEntities.Clear();
		}

		private void spawnMeteor() {
			var pos = new Vector2() {
				x = Random.Range(-screenSize.x / 2, screenSize.x / 2),
				y = screenSize.y / 2
			};
			var meteor = Instantiate(MeteorPrefab, pos, Quaternion.identity);
			liveEntities.Add(meteor);
		}

	}
}

// TODO: enemies spawn from both sides randomly and you have to kill them, sometimes screen darkens and you need to use down/up to light it up
