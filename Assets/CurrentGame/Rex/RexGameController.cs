using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Rex {
    public class RexGameController : MonoBehaviour {
        public Transform SpawnPoint;
        public GameObject CactusPrefab;
        public Camera CurrentCamera;

        public float SpawnAfter = 1f;
        public float InitialSpeed = 5f;

        private List<GameObject> liveObjects = new List<GameObject>();
        private List<GameObject> deadObjects = new List<GameObject>();
        private float currentSpeed = 0.0f;
        private float xOffscreen = 0;

        private float spawnTimer = 0f;

        private void Start() {
            xOffscreen = 
                -CurrentCamera.orthographicSize * CurrentCamera.aspect - (CactusPrefab.transform.localScale.x * 2);

            currentSpeed = InitialSpeed;
            spawnTimer = SpawnAfter;
        }

        private void FixedUpdate() {

            if ((spawnTimer += Time.fixedDeltaTime) > SpawnAfter) {
                spawnTimer = 0;
                liveObjects.Add(Instantiate(CactusPrefab, SpawnPoint.position, Quaternion.identity));
            }

            foreach (var item in liveObjects) {
                item.transform.position -= new Vector3(currentSpeed * Time.fixedDeltaTime, 0, 0);

                if (item.transform.position.x < xOffscreen)
                    deadObjects.Add(item);
            }

            foreach (var item in deadObjects) {
                Destroy(item);
                liveObjects.Remove(item);
            }

            deadObjects.Clear();
        }


    } 
}