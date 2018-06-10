using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dieMovement : MonoBehaviour {
    public enum dieType { regular, pierce, flurry };
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    private Rigidbody dieRB;
    public bool isRolling;
    public bool isDone;

    private Vector3 gravity;
    public float gravityStrength;
    public float bounceForce;

    private Vector3 previousPos;
    private Vector3 upPosition;

    public Vector3[] directions;
    public int[] values;
    public int dieValue;
    public dieType type;

    public Game game;
    public bool red;

    public bool isDisplaying;
    private Vector3 destination;
    private Vector3 rotation;
    private Vector3 currentRotation;
    public float speed;

    void Start()
    {
        dieRB = GetComponent<Rigidbody>();
        gravity = Camera.main.transform.forward;
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
        isRolling = false;
        transform.gameObject.SetActive(false);
        dieValue = 0;
        values = new int[]{ 3, 4, 5, 2, 1, 6 };
        type = dieType.regular;
        isDone = true;
    }
	
    public void rollDie(Vector3 position, float force)
    {
        isRolling = true;
        isDone = false;
        dieRB.AddForce(position*force);
        float rx = Random.Range(5, 10);
        float ry = Random.Range(5, 10);
        float rz = Random.Range(5, 10);
        dieRB.AddTorque(new Vector3(rx, ry, rz));
    }

    public void reset()
    {
        transform.position = defaultPosition;
        transform.rotation = defaultRotation;
        dieRB.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        dieRB.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
        isRolling = false;
        dieValue = 0;
        isDisplaying = false;
        GetComponent<MeshCollider>().enabled = true;
    }
	
	void FixedUpdate () {
        if (isRolling)
        {
            previousPos = transform.position;
            dieRB.AddForce(gravity * gravityStrength);
        }
        if (isDisplaying)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
            currentRotation = Vector3.Lerp(currentRotation, rotation, Time.deltaTime * speed);
            transform.eulerAngles = currentRotation;
            if (Vector3.Distance(transform.position, destination) < 0.01f && Vector3.Distance(currentRotation, rotation) < 0.01f)
            {
                isDisplaying = false;
                Debug.Log("done");
            }
        }
	}
    void OnCollisionEnter(Collision other)
    {
        float willBounce = Random.Range(0.0f, 6.0f);
        if (!isDone && other.gameObject.CompareTag("Floor"))
        {
            upPosition = -other.transform.forward;
            if (willBounce > 5.0f)
            {
                Vector3 bounce = -other.transform.forward;
                dieRB.AddForce(bounce * bounceForce);
            }
            else
            {
                isDone = true; 
                StartCoroutine(SetValue());
            }
        }
    }

    IEnumerator SetValue()
    {
        yield return new WaitForSeconds(1.5f);
        bool gettingValue = true;
        directions = new Vector3[] { transform.up, -transform.up, transform.right, -transform.right, transform.forward, -transform.forward };
        while (gettingValue)
        {
            if (Mathf.Approximately(previousPos.x, transform.position.x) &&
                Mathf.Approximately(previousPos.y, transform.position.y) &&
                Mathf.Approximately(previousPos.z, transform.position.z))

            {
                gettingValue = false;
                float minimum = Vector3.Angle(directions[0], upPosition);
                dieValue = 3;
                for (int i = 1; i < directions.Length; ++i)
                {
                    float direction = Vector3.Angle(directions[i], upPosition);
                    if (direction < minimum)
                    {
                        minimum = direction;
                        dieValue = values[i];
                    }
                }
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }

        }
        if (red)
        {
            Debug.Log("red" + dieValue);
            destination = game.rightUI.addDie(gameObject, dieValue, type);
        }
        else
        {
            Debug.Log("blue" + dieValue);
            destination = game.leftUI.addDie(gameObject, dieValue, type);
        }
        fixRotation();
        isRolling = false;
        GetComponent<MeshCollider>().enabled = false;
    }

    void fixRotation()
    {
        Debug.Log("fixRotation");
        switch (dieValue)
        {
            case 1:
                rotation = new Vector3(210f, 45f, 0f);
                break;
            case 2:
                rotation = new Vector3(120f, 45f, 90f);
                break;
            case 3:
                rotation = new Vector3(0f, -45f, 60f);
                break;
            case 4:
                rotation = new Vector3(120f, 45f, 0f);
                break;
            case 5:
                rotation = new Vector3(-60f, 45f, 90f);
                break;
            case 6:
            default:
                rotation = new Vector3(30f, 45f, 0f);
                break;
        }
        currentRotation = transform.eulerAngles;
        isDisplaying = true;
    }
}
