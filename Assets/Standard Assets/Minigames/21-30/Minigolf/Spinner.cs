using UnityEngine;

namespace Minigames.Minigolf {
public class Spinner : MonoBehaviour {
    public float RotationSpeed;
    public float StartDelay;

    private float startTimer;
    private bool started;
    
    private void FixedUpdate() {

        if ((startTimer += Time.fixedDeltaTime) > StartDelay) {
            started = true;
        }

        if (started) spin();
    }

    private void spin() {
        transform.RotateAround(
            transform.position, Vector3.forward, RotationSpeed * Time.fixedDeltaTime);
    }
}
}