using UnityEngine;
using Components.UnityComponents;

namespace Minigames.PingPong
{
  public class PaddleController : BasicControls
  {
    public float PaddleMovementSpeed;

    private MinigameManager gameManager;
    private Rigidbody2D rigidBody;

    private void Start()
    {
      rigidBody = GetComponent<Rigidbody2D>();
      gameManager = GetComponentInParent<MinigameManager>();


      gameManager.ButtonEvents.OnVerticalPressed += HandleVerticalStateChange;
    }

    private void Update() {
      if (gameManager.GameOver) return;

      var direction = (float)VerticalState;
      rigidBody.velocity = new Vector2(
        rigidBody.velocity.x,
        PaddleMovementSpeed * direction);
    }

    private void OnDisable()
    {
      gameManager.ButtonEvents.OnVerticalPressed -= HandleVerticalStateChange;
    }
  }
}
