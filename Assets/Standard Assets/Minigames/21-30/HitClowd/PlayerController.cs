using Components.UnityComponents;
using Components.UnityComponents.v2;
using UnityEngine;

namespace Minigames.HitClowd {
public class PlayerController : BasicControls {
    public GameObject EffectOnCloudHit;
    public float MovementSpeed;
    public float VerticalForce;
    public float HorizontalForce;

    private MinigameManager2 minigameManager;
    private Rigidbody2D rigidbody2d;
    private Vector2 startingPosition;

    public void Start() {
        startingPosition = transform.position;
        minigameManager = GetComponentInParent<MinigameManager2>();
        minigameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalStateChange;
        rigidbody2d = GetComponent<Rigidbody2D>();
    }  
    
    private void OnDisable() {
        minigameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalStateChange;
    }

    private void move_player() {
        var direction = (float)HorizontalState * Time.fixedDeltaTime * MovementSpeed;
        var new_position = Vector2.right * direction;

        rigidbody2d.velocity += new_position;
    }

    private void FixedUpdate() {
        if (minigameManager.GameOver) {
            rigidbody2d.simulated = false;
            return;   
        }

        move_player();

        if (transform.position.y + minigameManager.transform.position.y 
            < -minigameManager.CurrentCamera.orthographicSize) {
                minigameManager.Events.EventHit();
                rigidbody2d.velocity = Vector2.zero;
                transform.position = startingPosition;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("deadzone"))
            minigameManager.Events.EventHit();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.CompareTag("scorezone")) 
            return;
        
        Destroy(Instantiate(EffectOnCloudHit, transform.position, Quaternion.identity, other.transform), 1f);

        minigameManager.Events.EventScored();
        this.rigidbody2d.velocity = Vector2.zero;
        this.rigidbody2d.AddForce(new Vector2(HorizontalForce, VerticalForce));
    }
} 
}