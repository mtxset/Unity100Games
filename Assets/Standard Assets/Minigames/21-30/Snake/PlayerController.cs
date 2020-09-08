using Components;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Snake
{
    internal class SnakePart
    {
        public bool Head;
        public int ActiveAfter;
        public GameObject Part;
        public SnakePart FollowPart;
        public Vector3 LastPosition;
        public Rect Rectangle;
    }

    class PlayerController : AddMinigameManager2
    {
        public GameObject DeathBlockPrefab;
        public GameObject SnakePartPrefab;
        public Vector3 Direction;
        public float MoveAfter;
        public float IncreaseSpeedBy;

        private float moveTimer;
        private List<SnakePart> snake;
        private SnakePart lastPart;

        private List<GameObject> deathBlocks;

        private void Start()
        {
            subscribe();

            snake = new List<SnakePart>();

            var head = new SnakePart
            {
                Part = Instantiate(SnakePartPrefab),
                Head = true
            };

            lastPart = head;
            snake.Add(head);
        }

        private void subscribe()
        {
            MinigameManager.ButtonEvents.OnRightButtonPressed += HandleRight;
            MinigameManager.ButtonEvents.OnLeftButtonPressed += HandleLeft;
            MinigameManager.ButtonEvents.OnUpButtonPressed += HandleUp;
            MinigameManager.ButtonEvents.OnDownButtonPressed += HandleDown;
            MinigameManager.ButtonEvents.OnActionButtonPressed += HandleAction;
        }

        private void unsubscribe()
        {
            MinigameManager.ButtonEvents.OnRightButtonPressed -= HandleRight;
            MinigameManager.ButtonEvents.OnLeftButtonPressed -= HandleLeft;
            MinigameManager.ButtonEvents.OnUpButtonPressed -= HandleUp;
            MinigameManager.ButtonEvents.OnDownButtonPressed -= HandleDown;
            MinigameManager.ButtonEvents.OnActionButtonPressed -= HandleAction;
        }

        private void HandleAction() => growSnake();
        private void HandleRight() => Direction = Vector3.right;
        private void HandleLeft() => Direction = Vector3.left;
        private void HandleUp() => Direction = Vector3.up;
        private void HandleDown() => Direction = Vector3.down;

        private void OnDisable()
        {
            unsubscribe();
        }

        private void FixedUpdate()
        {
            if ((moveTimer += Time.fixedDeltaTime) > MoveAfter)
            {
                moveSnakeHeadForward();
                moveSnakeBody();
                moveTimer = 0;
            }
        }

        private void growSnake()
        {
            MoveAfter -= IncreaseSpeedBy;
            var squareSide = SnakePartPrefab.transform.localScale.x;
            var newBody = new SnakePart
            {
                Part = Instantiate(SnakePartPrefab, 
                    lastPart.Part.transform.position,
                    Quaternion.identity,
                    transform),
                FollowPart = lastPart,
                ActiveAfter = 1,
            };

            newBody.Rectangle = new Rect(
                new Vector2(
                    newBody.Part.transform.position.x - squareSide/2,
                    newBody.Part.transform.position.y - squareSide/2), 
                new Vector2(squareSide, squareSide));
            

            lastPart = newBody;
            spawnDeathBlock();

            snake.Add(newBody);
        }

        private void moveSnakeHeadForward()
        {
            if (Direction == Vector3.zero) return;

            snake[0].Part.transform.forward = Direction;

            snake[0].LastPosition = snake[0].Part.transform.position;

            snake[0].Part.transform.position += snake[0].Part.transform.forward * snake[0].Part.transform.localScale.x;
        }

        private void moveSnakeBody()
        {
            foreach (var item in snake)
            {
                if (item.Head) continue;

                if (item.ActiveAfter == 0)
                {
                    item.LastPosition = item.Part.transform.position;
                    item.Part.transform.position = item.FollowPart.LastPosition;
                }
                else
                {
                    item.ActiveAfter--;
                }
            }
        }

        private void spawnDeathBlock()
        {
            Vector2 randomPos;
            var maxX = MinigameManager.CurrentCamera.orthographicSize 
                * MinigameManager.CurrentCamera.aspect;
            var maxY = MinigameManager.CurrentCamera.orthographicSize;

            do
            {
                var x = Random.Range(-maxX, maxX);
                var y = Random.Range(-maxY, maxY);
                randomPos.x = x;
                randomPos.y = y;
            } while (checkIfPointInsideRect(randomPos));

            var newBlock = Instantiate(
                DeathBlockPrefab, 
                randomPos, 
                Quaternion.identity,
                transform);

            deathBlocks.Add(newBlock);
        }

        private bool checkIfPointInsideRect(Vector2 point)
        {
            foreach (var item in snake)
            {
                if (item.Rectangle.Contains(point))
                    return true;
            }

            return false;
        }
    }
}
