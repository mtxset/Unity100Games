using Components;
using UnityEngine;

namespace Minigames.Minigolf {
public class PowerController : AddMinigameManager2 {
    public GameObject Ball;
    public GameObject PowerBar;
    public LevelGenerator LevelGenerator;

    public float IncreaseBy = 0.1f;
    public float Force = 0.1f;
    public float Friction = 0.01f;
    public float StopThreshold = 0.01f;
    public float RotateBy = 1.0f;
    public Vector2 ForceMinMax;
    public Vector2 BallVelocity;

    private Rigidbody2D ballRigidBody2d;

    private void Start() {
        ballRigidBody2d = Ball.GetComponent<Rigidbody2D>();

        MinigameManager.ButtonEvents.OnActionButtonPressed += HandleAction;
    }

    private void OnDisable() {
        MinigameManager.ButtonEvents.OnActionButtonPressed -= HandleAction;
    }

    private void HandleAction() => Shoot();

    public void RotateBar(int leftRight) {
        PowerBar.transform.RotateAround(
            Ball.transform.position, 
            Vector3.forward * leftRight, RotateBy);
    }

    public void IncreasePower() {
        PowerBar.transform.localScale -= new Vector3(0, IncreaseBy, 0);
        changePower(1);
    }


    public void Shoot() {
        ballRigidBody2d.AddForce(PowerBar.transform.up * Force, ForceMode2D.Impulse);
    }

    private void Update() {
        BallVelocity = ballRigidBody2d.velocity;
        drawIfStopped();

        RotateBar((int)MinigameManager.Controls.HorizontalState);

        changePower((int)MinigameManager.Controls.VerticalState);
    }

    private void changePower(int direction) {
        if (direction == 0) return;
        var oldForce = Force;
        Force = Mathf.Clamp(Force + (IncreaseBy * direction), ForceMinMax.x, ForceMinMax.y);

        if (oldForce != Force)
            PowerBar.transform.localScale -= new Vector3(0, IncreaseBy/5 * direction, 0);
    }

    private void drawIfStopped() {
        var state = Mathf.Abs(ballRigidBody2d.velocity.x) < StopThreshold 
            && Mathf.Abs(ballRigidBody2d.velocity.y) < StopThreshold;

        PowerBar.SetActive(state);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("scorezone")) {
            stopBallMotion();
            MinigameManager.Events.EventScored(10);
            LevelGenerator.NextMap();
        } else if (other.CompareTag("deadzone")) {
            MinigameManager.Events.EventHit();
            stopBallMotion();
            Ball.transform.position = LevelGenerator.CurrentMapStartingPoint;
        }
    }

    private void stopBallMotion() {
        ballRigidBody2d.velocity = Vector2.zero;
        ballRigidBody2d.angularVelocity = 0;
    } 
}
}