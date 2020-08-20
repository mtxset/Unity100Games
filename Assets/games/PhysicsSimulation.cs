using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSimulation : MonoBehaviour {
	void Start ()
    {
        Physics2D.Simulate(Time.unscaledDeltaTime);
	}
}
