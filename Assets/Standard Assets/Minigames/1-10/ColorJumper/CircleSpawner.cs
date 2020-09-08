using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Utils;

namespace Minigames.ColorJumper
{
    public class DifficultySetup
    {
        public float RotationSpeed;
        public float Scale;
        public float SpaceInBetween;
    }

    public class CircleEntity
    {
        public GameObject CircleGameObject;
        public float RotationSpeed;
    }
    class CircleSpawner : MonoBehaviour
    {
        public GameObject CirclePrefab;
        public GameObject ColorSwitcherPrefab;
        public Text SpeedText;
        public Camera CurrentCamera;

        public float AfterCircleOffset = 2f;
        public float PrespawnYOffset = 10;
        public Vector2 SpawnSpaceInBetweenMinMax;
        [SerializeField] public Vector2 SpawnSizeMinMax;
        public Vector2 RotationSpeedMinMax;
        public float IncreaseRateAfter = 2f;
        public float IncreaseRateBy = 0.1f;

        private MinigameManager gameManager;

        private float difficultyTimer;
        private float screenHeight;
        private float currentDifficulty;

        private List<CircleEntity> liveEntities;
        private List<CircleEntity> deadEntities;
        private List<Color> colorList;

        private float lastSpawnY;

        private void Start()
        {
            screenHeight = CurrentCamera.orthographicSize;

            gameManager = GetComponentInParent<MinigameManager>();
            colorList = new List<Color>(gameManager.ColorList);
            liveEntities = new List<CircleEntity>();
            deadEntities = new List<CircleEntity>();

            SpeedText.text = $"DIFFICULTY: {currentDifficulty * 100}";

            lastSpawnY = gameManager.transform.position.y + SpawnSpaceInBetweenMinMax.y;
        }

        private void Update()
        {

            if (gameManager.GameOver)
            {
                return;
            }

            if (currentDifficulty < 1.0f)
            {
                difficultyTimer += Time.deltaTime;
            }

            checkDifficulty();

            checkIfSpawnNew();

            checkWhetherOutOffScreen();
        }

        private void checkIfSpawnNew()
        {
            if (lastSpawnY < CurrentCamera.transform.position.y + PrespawnYOffset)
            {
                liveEntities.Add(spawnCirclePair());
            }
        }

        private void checkWhetherOutOffScreen()
        {
            foreach (var item in liveEntities)
            {
                item.CircleGameObject.transform.Rotate(0, 0, item.RotationSpeed * Time.deltaTime);

                if (CurrentCamera.transform.position.y - screenHeight * 1.5f >
                    item.CircleGameObject.transform.position.y)
                {
                    deadEntities.Add(item);
                }
            }

            foreach (var item in deadEntities)
            {
                liveEntities.Remove(item);
                Destroy(item.CircleGameObject);
            }

            deadEntities.Clear();
        }

        private void checkDifficulty()
        {
            if (!(difficultyTimer >= IncreaseRateAfter))
            {
                return;
            }
            
            currentDifficulty += IncreaseRateBy;
            SpeedText.text = $"DIFFICULTY: {currentDifficulty * 100}";
            difficultyTimer = 0;
        }
        private CircleEntity spawnCirclePair()
        {
            var newEntity = new CircleEntity
            {
                CircleGameObject = Instantiate(CirclePrefab, gameObject.transform)
            };
            newEntity.CircleGameObject.transform.position = new Vector2(0, lastSpawnY);

            var difficulty = difficultySetup();

            newEntity.RotationSpeed = difficulty.RotationSpeed;
            newEntity.CircleGameObject.transform.localScale =
                Vector3.one * difficulty.Scale;

            var colors = new List<Color>(colorList);
            colors.ShuffleList();
            // coloring
            var spriteRenderers = newEntity
                                    .CircleGameObject
                                    .GetComponentsInChildren<SpriteRenderer>();

            Assert.IsTrue(colors.Count >= spriteRenderers.Length, 
                "Color count should equal or larger than 4");
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].color = colors[i];
            }

            lastSpawnY += newEntity.CircleGameObject.transform.localScale.y +
                               AfterCircleOffset;

            var newColorEntity = Instantiate(ColorSwitcherPrefab, gameObject.transform);
            newColorEntity.transform.position = new Vector2(0, lastSpawnY);

            lastSpawnY += newColorEntity.transform.localScale.y +
                               difficulty.SpaceInBetween;

            return newEntity;
        }

        private DifficultySetup difficultySetup()
        {

            var vectorList = new List<Vector2>
            {
                RotationSpeedMinMax,
                // reverse scale because bigger is easier
                new Vector2(SpawnSizeMinMax.y, SpawnSizeMinMax.x),
                // reverse space because bigger space is easier
                new Vector2(SpawnSpaceInBetweenMinMax.y, SpawnSpaceInBetweenMinMax.x)
            };

            var unparsed = DifficultyAdjuster.SpreadDifficulty(currentDifficulty, vectorList);

            var newDifficultySetup = new DifficultySetup
            {
                RotationSpeed = unparsed[0],
                Scale = unparsed[1],
                SpaceInBetween = unparsed[2]
            };


            return newDifficultySetup;
        }
    }
}
