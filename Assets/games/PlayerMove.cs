using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour {
    private Rigidbody2D self;
    public float moveSpeed;
    public float switchTime;
    private bool gameOver = false;
    private int seconds = 0;
    public Text clock;
    public GameObject ball;

	void Start ()
    {
        self = GetComponent<Rigidbody2D>();
        InvokeRepeating("SwitchDirections", switchTime, switchTime * 2);
        StartCoroutine(SpawnBallLoop());
        StartCoroutine(Count());
	}

    IEnumerator SpawnBallLoop ()
    {
        while (gameOver == false)
        {
            yield return new WaitForSecondsRealtime(2f);
            SpawnBall();
        }
    }

    void SpawnBall ()
    {
        Instantiate(ball, new Vector3(Random.Range(-2.18f, 2.18f), 4.6f, 0f), Quaternion.identity);
    }

    void UpdateClock ()
    {
        seconds += 1;
        clock.text = "Time: " + seconds;
    }
	
    void SwitchDirections ()
    {
        moveSpeed *= -1;
    }

	void FixedUpdate ()
    {
        self.velocity = new Vector2(moveSpeed, 0f);
        if (Input.GetMouseButton(0))
        {
            Time.timeScale = 0.5f;
        }
        else
        {
            Time.timeScale = 1f;
        }
	}

    IEnumerator Count ()
    {
        while (gameOver == false)
        {
            yield return new WaitForSecondsRealtime(1);
            UpdateClock();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ball")
        {
            GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
