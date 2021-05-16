using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Breakout {
	public class BoxManager: MonoBehaviour {
		public Vector2 BoxAmountColRow;
		public GameObject boxPrefab;
		public Camera Camera;
		public float MoveDownBy = 1f;

		public float MoveAfter = 1f;

		private List<GameObject> liveEntities = new List<GameObject>();
		private List<GameObject> deadEntities = new List<GameObject>();

		private Vector2 screenSize;

		private float moveTimer;

		private void Start() {
			screenSize.y = Camera.orthographicSize * 2;
			screenSize.x = Camera.orthographicSize * Camera.aspect * 2;

			var offset = new Vector2() {
				x = screenSize.x / BoxAmountColRow.x - 1,
				y = screenSize.y / BoxAmountColRow.y / 3 - 1
			};

			for (var col = 1; col <= BoxAmountColRow.x; col++) {
				for (var row = 1; row <= BoxAmountColRow.y; row++) {
					var pos = new Vector2() {
						x = (-screenSize.x / 2) + col * offset.x,
						y = (screenSize.y / 2) - row * offset.y
					};
					var newBox = Instantiate(boxPrefab, pos, Quaternion.identity, transform);
					newBox.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
					liveEntities.Add(newBox);
				}
			}
		}

		private void FixedUpdate() {
			if ((moveTimer += Time.fixedDeltaTime) > MoveAfter) {
				moveTimer = 0;
				for (var i = 0; i < liveEntities.Count; i++) {
					if (liveEntities[i] == null) {
						deadEntities.Add(liveEntities[i]);
						continue;
					}

					liveEntities[i].transform.position -= new Vector3(0, MoveDownBy, 0);
				}

				for (var i = 0; i < deadEntities.Count; i++) {
					liveEntities.Remove(deadEntities[i]);
				}

				deadEntities.Clear();
			}
		}


	}
}
