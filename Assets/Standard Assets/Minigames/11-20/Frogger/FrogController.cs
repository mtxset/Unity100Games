using UnityEngine;

namespace Minigames.Frogger
{
  public class FrogController : MonoBehaviour
  {
    public AudioSource SoundJump;
    public Camera CurrentCamera;

    private MinigameManager gameManager;
    private Rigidbody2D rigidbody2d;
    private Vector2 initialPosition;
    private Vector2 screenHalfSizeWorldUnits;

    private void Start()
    {
      float orthographicSize;
      screenHalfSizeWorldUnits = new Vector2(
          CurrentCamera.aspect * (orthographicSize = CurrentCamera.orthographicSize),
          orthographicSize);

      gameManager = GetComponentInParent<MinigameManager>();
      rigidbody2d = GetComponent<Rigidbody2D>();
      initialPosition = transform.position;

      subscribeToEvents();
    }

    private void subscribeToEvents()
    {
      gameManager.ButtonEvents.OnLeftButtonPressed += HandleLeftButtonPressed;
      gameManager.ButtonEvents.OnRightButtonPressed += HandleRightButtonPressed;
      gameManager.ButtonEvents.OnUpButtonPressed += HandleUpButtonPressed;
    }

    private void OnDisable()
    {
      unsubscribeToEvents();
    }

    private void unsubscribeToEvents()
    {
      gameManager.ButtonEvents.OnLeftButtonPressed -= HandleLeftButtonPressed;
      gameManager.ButtonEvents.OnRightButtonPressed -= HandleRightButtonPressed;
      gameManager.ButtonEvents.OnUpButtonPressed -= HandleUpButtonPressed;
    }

    private void HandleUpButtonPressed()
    {

      moveFrog(Vector2.up);
    }

    private void HandleRightButtonPressed()
    {
      moveFrog(Vector2.right);
    }

    private void HandleLeftButtonPressed()
    {
      moveFrog(Vector2.left);
    }

    private void moveFrog(Vector2 direction)
    {
      if (gameManager.GameOver)
        return;

      Vector2 newPosition;
      if (direction == Vector2.up)
      {
        newPosition = rigidbody2d.position + Vector2.up;
      }
      else
      {
        var offsetX = Mathf.Clamp(
            rigidbody2d.position.x + direction.x,
            -screenHalfSizeWorldUnits.x + transform.localScale.x,
            screenHalfSizeWorldUnits.x - transform.localScale.x);

        newPosition = new Vector2(offsetX, rigidbody2d.position.y);
      }

      SoundJump.Play();
      rigidbody2d.MovePosition(newPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("deadzone"))
      {
        gameManager.Events.EventHit();
        transform.position = initialPosition;
        Destroy(other.gameObject);
      }
      else if (other.CompareTag("scorezone"))
      {
        gameManager.Events.EventScored(10);
        transform.position = initialPosition;
      }
    }
  }
}
