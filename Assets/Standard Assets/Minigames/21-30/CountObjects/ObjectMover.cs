using System.Collections.Generic;
using Components;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.CountObjects { 
public class ObjectMover : AddMinigameManager2 {

    public Camera CurrentCamera;
    public GameObject[] ObjectsToSpawn;
    public Text Information;
    public Text Difficulty;
    public Vector2 FlightSpeedMinMax;
    public Vector2 SpawnAmountMinMax;

    public float SpawnInBetween = 2.0f;
    public float CurrentDifficulty = 0.1f;

    public float IncreaseBy = 0.05f;

    private List<GameObject> liveObjects;
    private float gameManagerYOffset;
    private float flightOverOffsetX;
    
    private bool flight;
    private bool counting;

    private int currentCount;
    private int playersCount;

    private float currentFlightSpeed;
    private int currentSpawnCount;

    private float difficultyTimer;

    private void Start() {
        Difficulty.text = $"DIFFICULTY: {CurrentDifficulty * 100}";
        liveObjects = new List<GameObject>();

        flightOverOffsetX = 
            CurrentCamera.orthographicSize * CurrentCamera.aspect + SpawnInBetween;

        MinigameManager.ButtonEvents.OnActionButtonPressed += HandleAction;

        gameManagerYOffset = MinigameManager.transform.position.y;

        startState();
    }

    private void OnDisable() {
        MinigameManager.ButtonEvents.OnActionButtonPressed -= HandleAction;
    }

    private void HandleAction() {
        if (counting) playersCount++;
    }

    private void startState() {
        Information.text = "";
        counting = false;
        playersCount = 0;
        updateDifficulty();
        spawnObjects(currentSpawnCount);
        currentCount = liveObjects.Count;
        prepareState();
    }

    private void updateDifficulty() {

        Difficulty.text = $"DIFFICULTY: {CurrentDifficulty * 100}";
        var vectors = new List<Vector2> {
            FlightSpeedMinMax,
            SpawnAmountMinMax
        };

        var spread = DifficultyAdjuster.SpreadDifficulty(CurrentDifficulty, vectors);

        currentFlightSpeed = spread[0];
        currentSpawnCount = (int)spread[1];
    }

    // 1. Spawn Random amount of one type of objects outside the screen on left side
    public void spawnObjects(int amount) {

        var initialPositionX = 
            -CurrentCamera.orthographicSize * CurrentCamera.aspect - SpawnInBetween;
        var offset = 0.0f;
 
        for (var i = 0; i < amount; i++) {
            var newObject = Instantiate(
                ObjectsToSpawn[Random.Range(0, ObjectsToSpawn.Length)], 
                transform);
            
            newObject.transform.position += new Vector3(
                initialPositionX - newObject.transform.localScale.x - offset,
                // 1.5. Put them in some formation
                gameManagerYOffset + Random.Range(
                    -CurrentCamera.orthographicSize + 1.0f, 
                    CurrentCamera.orthographicSize - 1.0f), 
                0);
            
            offset += newObject.transform.localScale.x + SpawnInBetween;

            liveObjects.Add(newObject);
        }
    }

    // 2. It says prepare! 1 second says Count for 0.5 ms
    private void prepareState() {
        Information.text = "Prepare to COUNT!";
        StartCoroutine(
            Components.Delay.StartDelay(1.5f, startTheFlight, null));  
    }

    private void startTheFlight() { 
        Information.text = "";
        flight = true;
    }
    
    // 3. Objects fly through in MinMax speed depending on difficulty
    private void FixedUpdate() {
        if (flight) {

            foreach (var item in liveObjects) {
                item.transform.position += new Vector3(
                    currentFlightSpeed * Time.fixedDeltaTime, 0, 0);
            }

            if (liveObjects[liveObjects.Count-1].transform.position.x 
                > flightOverOffsetX) {
                flight = false;

                foreach (var item in liveObjects) {
                    Destroy(item);
                }

                liveObjects.Clear();
                countingState();
            }
        }
    }
    
    // 4. It gives 3 seconds to count, you press that many times with visual indication
    private void countingState() {
        counting = true;
        Information.text = "Press ACTIVE for each object!";

        StartCoroutine(
            Components.Delay.StartDelay(3f, checkState, null));  
    }

    // 5. If you guess correctly difficulty goes up, if no you lose life, repeat cycle
    private void checkState() {
        CurrentDifficulty += IncreaseBy;

        counting = false;
        Information.text = $"YOUR COUNT: {playersCount}, ACTUAL: {currentCount}";
        if (currentCount == playersCount) {
            MinigameManager.Events.EventScored(playersCount);
        } else {
            MinigameManager.Events.EventHit();
        }

        StartCoroutine(
            Components.Delay.StartDelay(2f, startState, null));  
    }

}
}