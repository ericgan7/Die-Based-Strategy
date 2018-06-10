using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement: MonoBehaviour {

    public enum charState { idle, move, attack, dead };
    public enum charAttacks { normal, pierce, flurry };
    public enum charDefenses { normal, deflect};
    public charState state;
    //stats
    public float speed;
    public int attack;
    public int defense;
    public int health;
    public charAttacks[] attackMoves;
    public charDefenses[] defenseMoves;

    public List<Vector3> destination;
    public Vector3 previousPos;
    public bool isBattling;
    private bool isTurn;

    public Game game;

    void Awake()
    {
        destination = new List<Vector3>();
        isTurn = false;
        previousPos = transform.position;
        state = charState.idle;
    }

    void FixedUpdate()
    {
        if (state == charState.move)
        {
            if (destination.Count > 0)
            {
                if (Vector3.Distance(destination[destination.Count - 1], transform.position) < 0.01f)
                {
                    destination.RemoveAt(destination.Count - 1);
                    game.gameController.SetCharacterLocation(gameObject, previousPos);
                    previousPos = transform.position;
                    if (game.gameController.characterLocations[game.map.ToTileCoordinates(destination[destination.Count - 1])])
                    {
                        state = charState.attack;
                        game.gameController.StartBattle(game.gameController.characterLocations[game.map.ToTileCoordinates(destination[destination.Count - 1])].gameObject, true);

                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, destination[destination.Count - 1], Time.deltaTime * speed);
                }
            }
        }
        else if (state == charState.attack)
        {
            
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (isTurn)
        {
            if (gameObject.tag == "Ally")
            {
                if (col.gameObject.CompareTag("Enemy"))
                {
                    game.gameController.StartBattle(col.gameObject, true);
                }
            }
            else if (col.gameObject.CompareTag("Ally"))
            {
                game.gameController.StartBattle(col.gameObject, false);
            }
        }
    }

    public void AddDestination(Vector3 d)
    {
        destination.Add(new Vector3(d.x, transform.position.y, d.z));
    }

    public void SetTurn(bool t)
    {
        isTurn = t;
    }
}
