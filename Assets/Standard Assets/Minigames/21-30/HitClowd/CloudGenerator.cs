using System.Collections.Generic;
using Components;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.HitClowd {
public class CloudGenerator: AddMinigameManager2 {
    public GameObject InitialCloud;
    public GameObject[] Clouds;
    public float CurrentDifficulty = 0.1f;
    public float IncreaseBy = 0.05f;
    public float IncreaseAfter = 2f;
    public Text TextSpeed;

    public Vector2 CloudMoveSpeedMinMax;
    public float SpawnInBetween = 2f;

    private Camera currentCamera;
    private Vector2 minmaxY;
    private float offsetX;
    private const int maxClouds = 20;
    private int currentIndex = 0;
    private GameObject[] cloudPool;
    private float currentCloudSpeed = 0;

    private float spawnTimer = 0;
    private float difficultyTimer = 0;

    private void Start() {
        TextSpeed.text = $"DIFFICULTY: {System.Math.Round(CurrentDifficulty, 2) * 100}";
        currentCloudSpeed = CloudMoveSpeedMinMax.x;
        spawnTimer = SpawnInBetween;

        cloudPool = new GameObject[maxClouds];
        currentCamera = MinigameManager.CurrentCamera;

        minmaxY = new Vector2(
            -currentCamera.orthographicSize/2,
            -currentCamera.orthographicSize - 1);

        offsetX = currentCamera.orthographicSize*currentCamera.aspect;
        Destroy(InitialCloud, 3);
    }

    public void spawnCloud() {

        if (currentIndex + 1 > cloudPool.Length)
            currentIndex = 0;

        if (cloudPool[currentIndex] != null) {
            Destroy(cloudPool[currentIndex]);
            cloudPool[currentIndex] = null;
        }

        var randomIndex = Random.Range(0, Clouds.Length);

        var randomCloud = Object.Instantiate(
            Clouds[randomIndex], transform);

        var y = MinigameManager.transform.position.y;

        randomCloud.transform.position = new Vector2(
            offsetX + randomCloud.transform.localScale.x,
            y + Random.Range(minmaxY.x, minmaxY.y) + randomCloud.transform.localScale.y);

        cloudPool[currentIndex++] = randomCloud;    
    }

    private void FixedUpdate() {

        if ((spawnTimer += Time.fixedDeltaTime) > SpawnInBetween) {
            spawnTimer = 0;
            spawnCloud();
        }

        if ((difficultyTimer += Time.fixedDeltaTime) > IncreaseAfter) {
            difficultyTimer = 0;
            var vectors = new List<Vector2> { CloudMoveSpeedMinMax };
            CurrentDifficulty += IncreaseBy;
            currentCloudSpeed = DifficultyAdjuster.SpreadDifficulty(CurrentDifficulty, vectors)[0]; 
            TextSpeed.text = $"DIFFICULTY: {System.Math.Round(CurrentDifficulty, 2) * 100}";
        }

        foreach (var item in cloudPool) {
            if (item == null) continue;
            item.transform.position -= new Vector3(currentCloudSpeed * Time.fixedDeltaTime, 0, 0);
        }
    }
}
}