using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Ninja {

	public class Warrior {
		public int Direction;
		public GameObject GamePrefab;
		public float WaitBeforeStart;
	}

  public class WarriorSpawner : MonoBehaviour {
		public AudioClip[] ShurikenSpawnSounds;
		public float RotateSpeed = 15f;
		public float SpawnRate;
		public Vector2 MoveByMinMax;
		public float ShurikenWaitBeforeStart;
		public GameObject WarriorPrefab;
		public Transform[] WarriorXPos;

		private Vector2 screenSize;
		private MinigameManager gameManager;

		private List<Warrior> liveEntities = new List<Warrior>();
		private List<Warrior> deadEntities = new List<Warrior>();

		private float spawnTimer;

		private void Start() {
			gameManager = GetComponentInParent<MinigameManager>();

			var camera = gameManager.CurrentCamera;
			screenSize.y = camera.orthographicSize * 2;
			screenSize.x = camera.orthographicSize * camera.aspect * 2;
		}

		private void FixedUpdate() {
			if (gameManager.GameOver) return;

			if ((spawnTimer += Time.fixedDeltaTime) >= SpawnRate) {
				spawnTimer = 0;
				spawnWarrior();
			}

			var moveSpeed = Utils.DifficultyAdjuster.SpreadDifficulty(gameManager.DiffCurrent, new List<Vector2> { MoveByMinMax });

			foreach (var item in liveEntities) {
				if (item.GamePrefab == null) {
					deadEntities.Add(item);
					continue;
				}

				if (item.WaitBeforeStart <= 0) {
					item.GamePrefab.transform.position += new Vector3(item.Direction * moveSpeed[0] * Time.fixedDeltaTime, 0, 0);
					item.GamePrefab.transform.Rotate(new Vector3(0, 0, RotateSpeed * Time.fixedDeltaTime), Space.Self);
				}
				else
					item.WaitBeforeStart -= Time.fixedDeltaTime;
			}

			foreach (var item in deadEntities) {
				liveEntities.Remove(item);
				Destroy(item.GamePrefab);
			}

			deadEntities.Clear();
		}

		private void spawnWarrior() {
			var randomIndex = Random.Range(0, WarriorXPos.Length);
			var pos = WarriorXPos[randomIndex];

			var warrior = new Warrior {
				GamePrefab = Instantiate(WarriorPrefab, pos.position, Quaternion.identity, transform),
				Direction = (pos.position.x > 0) ? -1 : 1,
				WaitBeforeStart = ShurikenWaitBeforeStart,
			};

			liveEntities.Add(warrior);

			gameManager.AudioSource.PlayOneShot(ShurikenSpawnSounds[Random.Range(0, ShurikenSpawnSounds.Length - 1)]);
		}
	}
}
