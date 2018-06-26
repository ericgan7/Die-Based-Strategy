using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement: MonoBehaviour {

    public enum charState { idle, move, attack, dead };
    public enum charAttacks { normal, pierce, flurry };
    public enum charDefenses { normal, deflect};
    public charState state;
    //stats
    public int speed;
    public int attack;
    public int defense;
    public int health;
    public bool ally;
    public charAttacks[] attackMoves;
    public charDefenses[] defenseMoves;

    public List<Vector3> destination;
    private int place;
    public Vector3 previousPos;
    public bool isBattling;

    public Game game;

    void Awake()
    {
        destination = new List<Vector3>();
        previousPos = transform.position;
        state = charState.idle;
        place = 0;
    }

    public void reset()
    {
        destination.Clear();
        previousPos = transform.position;
        state = charState.idle;
        place = 0;
    }

    void FixedUpdate()
    {
        if (state == charState.move)
        {
            if (destination.Count - place == 1)
            {
                if (game.gameController.characterLocations[game.map.ToTileCoordinates(destination[place])])
                {
                    Debug.Log("Fight");
                    state = charState.attack;
                    place = 0;
                    game.gameController.StartBattle(game.gameController.characterLocations[game.map.ToTileCoordinates(destination[destination.Count - 1])].gameObject, ally);
                }
                else
                {
                    game.gameController.EndBattle();
                }
            }
            else if (place < destination.Count)
            {
                if (Vector3.Distance(destination[place], transform.position) < 0.01f)
                {
                    ++place;
                    game.gameController.SetCharacterLocation(gameObject, previousPos);
                    previousPos = transform.position;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, destination[place], Time.deltaTime * 3);
                }
            }
        }
        else if (state == charState.attack)
        {
            
        }
        else if (state == charState.dead)
        {
            game.gameController.removeChar(gameObject, ally);
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        }
    }

    public void AddDestination(Vector3 d)
    {
        destination.Add(new Vector3(d.x, transform.position.y, d.z));
    }

    public void damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            state = charState.dead;
        }
    }
}
