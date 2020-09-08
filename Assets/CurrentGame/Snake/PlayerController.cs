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
        private float squareSide;
        private float initialMoveAfter;

        private void Start()
        {
            initialMoveAfter = MoveAfter;
            snake = new List<SnakePart>();
            deathBlocks = new List<GameObject>();

            var head = new SnakePart
            {
                Part = Instantiate(SnakePartPrefab, transform),
                Head = true
            };
            lastPart = head;
            snake.Add(head);

            squareSide = SnakePartPrefab.transform.localScale.x;
            head.Part.GetComponent<BoxCollider>().enabled = true;

            subscribe();
        }

        private void HandleHeadCollision(Collision obj)
        {
            MinigameManager.Events.EventHit();
            reset();
        }

        private void reset()
        {
            lastPart = snake[0];
            MoveAfter = initialMoveAfter; 
            removeAllParts();
            removeAllDeathObjects();
        }
        
        private void removeAllParts()
        {
            for (int i = 1; i < snake.Count; i++)
            {
                Destroy(snake[i].Part);
            }

            snake.RemoveRange(1, snake.Count - 1);
        }
        private void removeAllDeathObjects()
        {
            foreach (var item in deathBlocks)
            {
                Destroy(item);
            }

            deathBlocks.Clear();
        }

        private void subscribe()
        {
            MinigameManager.ButtonEvents.OnRightButtonPressed += HandleRight;
            MinigameManager.ButtonEvents.OnLeftButtonPressed += HandleLeft;
            MinigameManager.ButtonEvents.OnUpButtonPressed += HandleUp;
            MinigameManager.ButtonEvents.OnDownButtonPressed += HandleDown;
            MinigameManager.ButtonEvents.OnActionButtonPressed += HandleAction;

            snake[0].Part.GetComponent<CollisionEvents>().OnCollisionEnterEvent 
                += HandleHeadCollision; 
        }

        private void unsubscribe()
        {
            MinigameManager.ButtonEvents.OnRightButtonPressed -= HandleRight;
            MinigameManager.ButtonEvents.OnLeftButtonPressed -= HandleLeft;
            MinigameManager.ButtonEvents.OnUpButtonPressed -= HandleUp;
            MinigameManager.ButtonEvents.OnDownButtonPressed -= HandleDown;
            MinigameManager.ButtonEvents.OnActionButtonPressed -= HandleAction;

            snake[0].Part.GetComponent<CollisionEvents>().OnCollisionEnterEvent 
                -= HandleHeadCollision; 
        }

        private void HandleAction() => growSnake();
        private void HandleRight() => tryApplyDirection(Vector3.right);
        private void HandleLeft() => tryApplyDirection(Vector3.left);
        private void HandleUp() => tryApplyDirection(Vector3.up);
        private void HandleDown() => tryApplyDirection(Vector3.down);

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

        private void tryApplyDirection(Vector3 direction)
        {
            if (Direction * -1 != direction)
                Direction = direction;
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

            lastPart = newBody;
            snake.Add(newBody);

            spawnDeathBlock();
            MinigameManager.Events.EventScored();
        }

        private void moveSnakeHeadForward()
        {
            if (Direction == Vector3.zero) return;

            snake[0].Part.transform.forward = Direction;

            snake[0].LastPosition = snake[0].Part.transform.position;

            snake[0].Part.transform.position += 
                snake[0].Part.transform.forward * snake[0].Part.transform.localScale.x;
        }

        private void moveSnakeBody()
        {
            foreach (var item in snake)
            {
                if (item.Head) continue;

                if (item.ActiveAfter == 0)
                {
                    item.Part.GetComponent<BoxCollider>().enabled = true;
                    item.LastPosition = item.Part.transform.position;
                    item.Part.transform.position = item.FollowPart.LastPosition;
                }
                else
                {
                    if (item.FollowPart.ActiveAfter == 0)
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

            Rect rect;
            do
            {
                randomPos.x = Random.Range(-maxX, maxX);
                randomPos.y = Random.Range(-maxY, maxY);

                rect = new Rect(
                    randomPos.x - squareSide/2,
                    randomPos.y - squareSide/2,
                    squareSide,
                    squareSide);

            } while (checkIfRectOverlaps(rect));

            var newBlock = Instantiate(
                DeathBlockPrefab, 
                randomPos, 
                Quaternion.identity,
                transform);

            newBlock.GetComponent<BoxCollider>().enabled = true;

            deathBlocks.Add(newBlock);
        }

        private bool checkIfRectOverlaps(Rect rect)
        { 
            foreach (var item in snake)
            {
                var tempRect = new Rect(
                    new Vector2(
                        item.Part.transform.position.x - squareSide/2,
                        item.Part.transform.position.y - squareSide/2), 
                    new Vector2(squareSide, squareSide));

                if (tempRect.Overlaps(rect))
                    return true;
            }
            return false;
        }
    }
}
