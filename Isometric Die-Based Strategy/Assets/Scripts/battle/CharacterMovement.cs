using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
    public int maxHealth;

    public int endurance;
    public int maxEndurance;

    public bool ally;
    public charAttacks[] attackMoves;
    public charDefenses[] defenseMoves;

    public List<Vector3> destination;
    private int place;
    public Vector3 previousPos;
    public bool isBattling;
    public StatusBar statusBar;

    public Game game;

    void Awake()
    {
        destination = new List<Vector3>();
        previousPos = transform.position;
        state = charState.idle;
        place = 0;
        statusBar.updateHealth(health, maxHealth);
        statusBar.updateEnd(endurance, maxEndurance);
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
            if (destination.Count - place == 0) {
                Debug.Log("STOP");
                game.gameController.EndBattle();
            }
            else if (place < destination.Count)
            {
                if (game.gameController.characterLocations[game.map.ToTileCoordinates(destination[place])])
                {
                    Debug.Log("Fight");
                    state = charState.attack;
                    game.gameController.StartBattle(game.gameController.characterLocations[game.map.ToTileCoordinates(destination[place])].gameObject, ally);
                    place = 0;
                }
                else if (Vector3.Distance(destination[place], transform.position) < 0.01f)
                {
                    ++place;
                    Debug.Log("NEXT");
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
            state = charState.idle;
        }
    }

    public void AddDestination(Vector3 d)
    {
        destination.Add(new Vector3(d.x, transform.position.y, d.z));
    }

    public void damage(int amount)
    {
        health += amount;
        statusBar.updateHealth(health, maxHealth);
        if (health <= 0)
        {
            state = charState.dead;
        }
    }

    public void useStamina(int amount)
    {
        endurance += amount;
        statusBar.updateEnd(endurance, maxEndurance);
    }
}
