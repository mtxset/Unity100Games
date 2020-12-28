using Components;
using UnityEngine;

namespace Minigames.HitClowd {
public class CloudGenerator: AddMinigameManager2 {
    public GameObject[] Clouds;

    private Camera currentCamera;
    private Vector2 minmaxY;
    private float offsetX;
    private const int maxClouds = 20;
    private int currentIndex = 0;
    private GameObject[] pool;

    private void Start() {
        pool = new GameObject[maxClouds];
        this.currentCamera = MinigameManager.CurrentCamera;

        minmaxY = new Vector2(
            -currentCamera.orthographicSize/2,
            -currentCamera.orthographicSize - 1);

        offsetX = currentCamera.orthographicSize*currentCamera.aspect;

        spawnCloud();
    }

    public void spawnCloud() {

        if (currentIndex + 1 > pool.Length) {
            currentIndex = 0;
        }

        if (pool[currentIndex] != null) {
            Destroy(pool[currentIndex]);
            pool[currentIndex] = null;
        }

        var randomIndex = Random.Range(0, Clouds.Length);

        var randomCloud = Object.Instantiate(
            Clouds[randomIndex], transform);

        var y = MinigameManager.transform.position.y;

        randomCloud.transform.position = new Vector2(
            offsetX,
            y + Random.Range(minmaxY.x, minmaxY.y));

        pool[currentIndex++] = randomCloud;    
    }
}
}