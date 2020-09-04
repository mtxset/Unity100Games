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
            gameManager = GetComponentInParent<MinigameManager>();
            rigidbody2d = GetComponent<Rigidbody2D>();
            rigidbody2d.AddForce(StartForce, ForceMode2D.Impulse);
        }
        
        public void Split()
        {
            var scale = transform.localScale / 1.5f;
            var ballLeft = Instantiate(
                gameObject, 
                rigidbody2d.position + Vector2.left / 4f,
                Quaternion.identity,
                gameManager.transform);
            
            var ballRight = Instantiate(
                gameObject, 
                rigidbody2d.position + Vector2.right / 4f,
                Quaternion.identity,
                gameManager.transform);

            ballLeft.transform.localScale = scale;
            ballRight.transform.localScale = scale;

            var randomColor =  Random.ColorHSV(
                0f, 1f, 1f, 1f, 0.5f, 1f);
            ballLeft.GetComponent<SpriteRenderer>().color = randomColor;
            ballRight.GetComponent<SpriteRenderer>().color = randomColor;

            ballLeft.GetComponent<Ball>().StartForce = new Vector2(-2, 5);
            ballRight.GetComponent<Ball>().StartForce = new Vector2(2, 5);
            
            gameManager.Events.EventScored();
            
            gameManager.Balls.Add(ballLeft);
            gameManager.Balls.Add(ballRight);
                
            Destroy(gameObject);
        }
    }
}