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
            this.screenHeight = this.CurrentCamera.orthographicSize;

            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.colorList = new List<Color>(this.gameManager.ColorList);
            this.liveEntities = new List<CircleEntity>();
            this.deadEntities = new List<CircleEntity>();

            this.SpeedText.text = $"DIFFICULTY: {this.currentDifficulty * 100}";

            this.lastSpawnY = this.gameManager.transform.position.y + this.SpawnSpaceInBetweenMinMax.y;
        }

        private void Update()
        {

            if (this.gameManager.GameOver)
            {
                return;
            }

            if (this.currentDifficulty < 1.0f)
            {
                this.difficultyTimer += Time.deltaTime;
            }

            checkDifficulty();

            checkIfSpawnNew();

            checkWhetherOutOffScreen();
        }

        private void checkIfSpawnNew()
        {
            if (this.lastSpawnY < this.CurrentCamera.transform.position.y + this.PrespawnYOffset)
            {
                this.liveEntities.Add(this.spawnCirclePair());
            }
        }

        private void checkWhetherOutOffScreen()
        {
            foreach (var item in liveEntities)
            {
                item.CircleGameObject.transform.Rotate(0, 0, item.RotationSpeed * Time.deltaTime);

                if (this.CurrentCamera.transform.position.y - this.screenHeight * 1.5f >
                    item.CircleGameObject.transform.position.y)
                {
                    this.deadEntities.Add(item);
                }
            }

            foreach (var item in deadEntities)
            {
                this.liveEntities.Remove(item);
                Destroy(item.CircleGameObject);
            }

            this.deadEntities.Clear();
        }

        private void checkDifficulty()
        {
            if (!(this.difficultyTimer >= this.IncreaseRateAfter))
            {
                return;
            }
            
            this.currentDifficulty += this.IncreaseRateBy;
            this.SpeedText.text = $"DIFFICULTY: {this.currentDifficulty * 100}";
            this.difficultyTimer = 0;
        }
        private CircleEntity spawnCirclePair()
        {
            var newEntity = new CircleEntity
            {
                CircleGameObject = Instantiate(this.CirclePrefab, this.gameObject.transform)
            };
            newEntity.CircleGameObject.transform.position = new Vector2(0, this.lastSpawnY);

            var difficulty = difficultySetup();

            newEntity.RotationSpeed = difficulty.RotationSpeed;
            newEntity.CircleGameObject.transform.localScale =
                Vector3.one * difficulty.Scale;

            var colors = new List<Color>(this.colorList);
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

            this.lastSpawnY += newEntity.CircleGameObject.transform.localScale.y +
                               AfterCircleOffset;

            var newColorEntity = Instantiate(this.ColorSwitcherPrefab, this.gameObject.transform);
            newColorEntity.transform.position = new Vector2(0, lastSpawnY);

            this.lastSpawnY += newColorEntity.transform.localScale.y +
                               difficulty.SpaceInBetween;

            return newEntity;
        }

        private DifficultySetup difficultySetup()
        {

            var vectorList = new List<Vector2>
            {
                this.RotationSpeedMinMax,
                // reverse scale because bigger is easier
                new Vector2(this.SpawnSizeMinMax.y, this.SpawnSizeMinMax.x),
                // reverse space because bigger space is easier
                new Vector2(this.SpawnSpaceInBetweenMinMax.y, this.SpawnSpaceInBetweenMinMax.x)
            };

            var unparsed = DifficultyAdjuster.SpreadDifficulty(this.currentDifficulty, vectorList);

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
