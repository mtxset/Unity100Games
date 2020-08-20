using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.BubbleStruggle
{
    public class Ball : MonoBehaviour
    {
        public Vector2 StartForce;

        private MinigameManager gameManager;
        private Rigidbody2D rigidbody2d;

        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.rigidbody2d = this.GetComponent<Rigidbody2D>();
            this.rigidbody2d.AddForce(this.StartForce, ForceMode2D.Impulse);
        }
        
        public void Split()
        {
            var scale = this.transform.localScale / 1.5f;
            var ballLeft = Instantiate(
                this.gameObject, 
                this.rigidbody2d.position + Vector2.left / 4f,
                Quaternion.identity,
                this.gameManager.transform);
            
            var ballRight = Instantiate(
                this.gameObject, 
                this.rigidbody2d.position + Vector2.right / 4f,
                Quaternion.identity,
                this.gameManager.transform);

            ballLeft.transform.localScale = scale;
            ballRight.transform.localScale = scale;

            var randomColor =  Random.ColorHSV(
                0f, 1f, 1f, 1f, 0.5f, 1f);
            ballLeft.GetComponent<SpriteRenderer>().color = randomColor;
            ballRight.GetComponent<SpriteRenderer>().color = randomColor;

            ballLeft.GetComponent<Ball>().StartForce = new Vector2(-2, 5);
            ballRight.GetComponent<Ball>().StartForce = new Vector2(2, 5);
            
            this.gameManager.Events.EventScored();
            
            this.gameManager.Balls.Add(ballLeft);
            this.gameManager.Balls.Add(ballRight);
                
            Destroy(this.gameObject);
        }
    }
}