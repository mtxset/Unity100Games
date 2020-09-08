using System;
using UnityEngine;

public class CollisionEvents : MonoBehaviour {

    public event Action<Collision> OnCollisionEnterEvent;
    public event Action<Collision2D> OnCollisionEnter2DEvent;
    private void OnCollisionEnter(Collision other) 
        => OnCollisionEnterEvent?.Invoke(other);
    private void OnCollisionEnter2D(Collision2D other) 
        => OnCollisionEnter2DEvent?.Invoke(other);
}