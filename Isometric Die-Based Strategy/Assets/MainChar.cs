using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainChar : MonoBehaviour {
    public Vector3 velocity;
    public Vector3 acceleration;
    public float maxSpeed;

    private float angle;
    private float stop;

    public enum movement
    {
        go, stop
    }
    private movement state;

    void Start()
    {
        angle = 0f;
        state = movement.stop;
        acceleration = new Vector3(0f, 0f, 0f);
        velocity = new Vector3(0f, 0f, 0f);
        Debug.Log(maxSpeed);
    }

    void FixedUpdate()
    {
        //have animations?
        /*
        float x = transform.position.x + maxSpeed * Time.deltaTime * Mathf.Cos(Mathf.Deg2Rad * angle);
        float y = transform.position.y + maxSpeed * Time.deltaTime * Mathf.Sin(Mathf.Deg2Rad * angle);
        if (speed == 0)
        {
            Debug.Log(x);
        }
        transform.position = new Vector3(x, y);
        */
        if (state == movement.go)
        {
            acceleration = new Vector3(maxSpeed * Mathf.Cos(Mathf.Deg2Rad * angle), maxSpeed * Mathf.Sin(Mathf.Deg2Rad * angle));
            velocity += acceleration;
            if (velocity.x + velocity.y > maxSpeed)
            {
                velocity = new Vector3(velocity.normalized.x * maxSpeed, velocity.normalized.y * maxSpeed);
            }
        }
        else if (state == movement.stop)
        {
            if (velocity.x + velocity.y <= 0.1)
            {
                velocity = new Vector3(0f, 0f);
            }
            velocity = new Vector3(velocity.x / 2, velocity.y / 2);
        }
        transform.position += velocity * Time.deltaTime;
    }

    public void SetAcceleration(float speed, movement s)
    {
        Debug.Log("CHANGE SPEED");
        maxSpeed = speed;
        state = s;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            float angle = Vector2.Angle(other.transform.up, Vector2.up);
            transform.eulerAngles = new Vector3(0f, 0f, angle);
        }
    }
}
