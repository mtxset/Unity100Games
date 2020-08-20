using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.MissTheTarget
{
    internal class ZePlanet
    {
        public GameObject Planet;
        public Queue<Vector2> CurrentPoints;
        public Vector2 TargetPosition;
        public float TraverseSpeed;
    }
    public class RandomRotation : MonoBehaviour
    {
        public Text SpeedText;
        public GameObject Sun;
        public GameObject[] Planets;
        
        public float IncreaseAfter = 1f;
        public float IncreaseBy = 0.01f;
        public float CurrentDifficulty;
        public Vector2 AngleMinMax;
        public Vector2 RotationSpeedMinMax;

        private List<ZePlanet> planetsData;
        private float timer;
        private MinigameManager gameManager;
        
        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.gameManager.Events.OnHit += HandleHit;
            
            this.planetsData = new List<ZePlanet>();
            
            foreach (var item in Planets)
            {
                planetsData.Add( new ZePlanet
                {
                    Planet = item,
                });
            }

            foreach (var item in planetsData)
            {
                generateNewPoints(item);
            }

            this.SpeedText.text = $"SPEED: {this.CurrentDifficulty * 100}";
        }
        
        private void OnDisable()
        {
            this.gameManager.Events.OnHit -= HandleHit;
        }
        
        private void HandleHit()
        {
            this.CurrentDifficulty = 0;
        }

        private void generateNewPoints(ZePlanet item)
        {

            var data = Oscillator.Osccilate(
                item.Planet.transform.position,
                this.Sun.transform.position,
                1,
                new Vector2(AngleMinMax.y, AngleMinMax.x), 
                RotationSpeedMinMax,
                this.CurrentDifficulty);
        
            item.CurrentPoints = new Queue<Vector2>(data.Points);
            item.TraverseSpeed = data.TraverseSpeed;
            item.TargetPosition = item.CurrentPoints.Dequeue();
        }

        private void checkDifficulty()
        {
            if ((this.timer += Time.deltaTime) >= this.IncreaseAfter)
            {
                this.CurrentDifficulty += this.IncreaseBy;
                this.SpeedText.text = $"SPEED: {this.CurrentDifficulty * 100}";
                this.timer = 0;
            }
        }

        private void Update()
        {
            if (this.gameManager.GameOver)
            {
                return;
            }
            
            this.checkDifficulty();
            
            foreach (var item in planetsData)
            {
                item.Planet.transform.position = Vector2.Lerp(
                    item.Planet.transform.position,
                    item.TargetPosition,
                    item.TraverseSpeed * Time.deltaTime);

                if (Vector2.Distance(item.Planet.transform.position, item.TargetPosition) <= 0.3f)
                {
                    if (item.CurrentPoints.Count != 0)
                    {
                        item.TargetPosition = item.CurrentPoints.Dequeue();
                    }
                    else
                    {
                        this.generateNewPoints(item);
                    }
                }
            }
        }
    }
}