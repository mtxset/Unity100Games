using System;
using System.Collections.Generic;
using Components;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace Minigames.Rex {
    public class RexGameController : AddMinigameManager2 {
        public Transform SpawnPoint;
        public GameObject[] CactusPrefabs;
        public Camera CurrentCamera;
        public Text TextDifficulty;
        public Transform Rex;

        public Vector2 MinMaxSpeed;
        public int MaxCactusesOnScreen = 2;
        public float SpawnAfter = 1f;
        public float IncreaseDifficultyBy = 0.02f;
        public float CurrentDifficulty = 0.01f;

        private List<GameObject> liveObjects = new List<GameObject>();
        private List<GameObject> deadObjects = new List<GameObject>();
        private float currentSpeed = 0.0f;
        private float xOffscreen = 0;

        private GameObject lastCactus;        
        private float spawnTimer = 5f;
        private bool cleanUp = false;

        private void Start() {
            xOffscreen = 
                -CurrentCamera.orthographicSize * CurrentCamera.aspect - (CactusPrefabs[0].transform.localScale.x * 2);

            currentSpeed = MinMaxSpeed.x;
            spawnTimer = SpawnAfter;
            MinigameManager.Events.OnHit += HandleHit;
        }

        private void HandleHit() => cleanUp = true;

        private void FixedUpdate() {

            if (MinigameManager.GameOver) return;

            if (lastCactus == null)
                spawnCactus();

            spawnTimer = 0;
            if (liveObjects.Count <= MaxCactusesOnScreen && lastCactus.transform.position.x < Rex.transform.position.x) {
                MinigameManager.Events.EventScored();
                spawnCactus();

                if (CurrentDifficulty < 1.0f) {
                    CurrentDifficulty += IncreaseDifficultyBy;
                    var difficulty = DifficultyAdjuster.SpreadDifficulty(CurrentDifficulty, new List<Vector2>() { MinMaxSpeed });
                    currentSpeed = difficulty[0];
                    TextDifficulty.text = $"DIFFICULTY: {System.Math.Round(CurrentDifficulty, 2) * 100}";
                }
            }

            foreach (var item in liveObjects) {
                item.transform.position -= new Vector3(currentSpeed * Time.fixedDeltaTime, 0, 0);

                if (item.transform.position.x < xOffscreen)
                    deadObjects.Add(item);
            }

            if (cleanUp) {
                deadObjects.AddRange(liveObjects);
                cleanUp = false;
            }

            foreach (var item in deadObjects) {
                Destroy(item);
                liveObjects.Remove(item);
            }

            deadObjects.Clear();
        }

        private void spawnCactus() {
            var randomIndex = Random.Range(0, CactusPrefabs.Length);
            var newCactus = Instantiate(CactusPrefabs[randomIndex], SpawnPoint.position, Quaternion.identity);
            liveObjects.Add(newCactus);
            lastCactus = newCactus;
        }
    } 
}