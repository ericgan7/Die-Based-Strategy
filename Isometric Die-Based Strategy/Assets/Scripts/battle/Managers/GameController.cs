using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject currentCharacter;
    private CharacterMovement character;

    public Game game;

    public int tileWidth;

    public List<GameObject> characterLocations;
    public List<GameObject> allies;
    public List<GameObject> enemies;
    public int currentTurn;
    public int allyTurn;
    public int enemyTurn;

    private bool canMove;
    private int speed;
    public List<GameObject> validTiles;
    public Stack<GameObject> path;
    public List<GameObject> currentPath;
    public GameObject lastTile;
    private int results = 0;

    public int BattleDone()
    {
        if (allies.Count == 0)
        {
            return 1;
        }
        else if (enemies.Count == 0)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }

    public void Update()
    {
        if (results == 0)
        {
            results = BattleDone();
            if (results != 0)
            {
                Debug.Log("BATTLE DONE");
                //FindObjectOfType<GameState>() modify recent victory, stats, etc.
                FindObjectOfType<GameSceneManager>().loadScene("Traversal");
            }
        }
    }
    public void SetPath(GameObject destination)
    {
        if (canMove)
        {
            if (destination == lastTile)
            {
                canMove = false;
                for (int i = 0; i < currentPath.Count; ++i)
                {
                    character.AddDestination(currentPath[i].transform.position);
                }
                character.state = CharacterMovement.charState.move;
            }
            else if (isValidTile(destination))
            {
                List<GameObject> pathToDestination = Astar(lastTile, destination);
                if (pathToDestination.Count > 0)
                {
                    lastTile = destination;
                    for (int i = 0; i < pathToDestination.Count; ++i)
                    {
                        path.Push(pathToDestination[i]);
                        --speed;
                    }
                    for (int i = 0; i < pathToDestination.Count; ++i)
                    {
                        //path.count would always stop after 2 pops?
                        currentPath.Add(path.Pop());
                    }
                    GetMovementTiles(destination, speed);
                }
            }
        }
    }

    public void ClearPath()
    {
        speed = character.speed;
        currentPath.Clear();
        GetMovementTiles(currentCharacter, speed);
        lastTile = game.map.mapTiles[game.map.ToTileCoordinates(character.transform.position)];
    }

    List<GameObject> Astar(GameObject start, GameObject end)
    {
        PriorityQueue frontier = new PriorityQueue(true);
        HashSet<GameObject> visited = new HashSet<GameObject>();
        Dictionary<GameObject, path> paths = new Dictionary<GameObject, path>();
        pair current;
        List<GameObject> neighbors;
        frontier.add(0f, start);
        visited.Add(start);
        paths.Add(start, new path(start, 0f));

        int countIterations = 0;

        while (!frontier.isEmpty() && countIterations < 20)
        {
            ++countIterations;
            current = frontier.pop();
            visited.Add(current.value);
            neighbors = game.map.GetNeighbors(current.value);
            //finished
            if (current.value == end)
            {
                List<GameObject> result = new List<GameObject>();
                result.Add(end);
                GameObject currentPath = paths[end].previous;
                while (currentPath != start)
                {
                    result.Add(currentPath);
                    currentPath = paths[currentPath].previous;
                }
                return result;
            }
            for(int i = 0; i < neighbors.Count; ++i)
            {
                float newCost = paths[current.value].cost + neighbors[i].GetComponent<FloorTile>().cost;
                if (characterLocations[game.map.ToTileCoordinates(neighbors[i].transform.position)] != null)
                {
                    if (game.map.GetTile(neighbors[i].transform.position) != end)
                    {
                        newCost += 100.0f;
                    }
                }
                if (!visited.Contains(neighbors[i]) || newCost < paths[neighbors[i]].cost)
                {
                    paths[neighbors[i]] = new path(current.value, newCost);
                    frontier.add(game.map.GetDistance(neighbors[i], end) + newCost, neighbors[i]);
                }
            }
        }
        return new List<GameObject>();
    }

    public void StartBattle(GameObject opponent, bool isAttacking)
    {
        CharacterMovement opp = opponent.GetComponent<CharacterMovement>();
        if (opp.endurance == 0)
        {
            opp.damage(-character.attack);
        }
        else {
            opp.useStamina(-1);
            game.battleUI.SetBattle(character, opp, isAttacking);
            game.playerController.state = PlayerController.playerState.battle;
        }
    }

    public void EndBattle()
    {
        game.playerController.state = PlayerController.playerState.movement;
        character.reset();
        NextTurn();
    }

    public void OnClickDieDown(GameObject die)
    {
        if (die == null)
        {
            game.battleUI.setThrowing(false);
        }
        else if (die.CompareTag ("Die") && !die.GetComponent<dieMovement>().red)
        {
            Debug.Log("blue");
            game.battleUI.setThrowing(true);
        }
    }

    public void OnClickDieUp(Vector3 position)
    {
        if (game.battleUI.isThrowing)
        {
            StartCoroutine(game.battleUI.RollDie(position));
            StartCoroutine(game.battleUI.Fight());
        }
        
    }

    void Start()
    {
        GridMovement map = GameObject.FindObjectOfType<GridMovement>();
        int max = Mathf.Max(allies.Count, enemies.Count);
        characterLocations = new List<GameObject>(map.tileWidth * map.tileHeight);
        for (int i = 0; i < map.tileWidth * map.tileHeight; ++i)
        {
            characterLocations.Add(null);
        }
        for (int i = 0; i < max; ++i)
        {
            if (i < allies.Count)
            {
                characterLocations[map.ToTileCoordinates(allies[i].transform.position)] = allies[i];
            }
            if (i < enemies.Count)
            {
                characterLocations[map.ToTileCoordinates(enemies[i].transform.position)] = enemies[i];
            }
        }
        path = new Stack<GameObject>();
        currentTurn = 0;
        currentCharacter = allies[0];
        character = currentCharacter.GetComponent<CharacterMovement>();
        canMove = true;
        speed = character.speed;
        GetMovementTiles(currentCharacter, speed);
        lastTile = game.map.mapTiles[game.map.ToTileCoordinates(character.transform.position)];
    }

    public void SetCharacterLocation(GameObject c, Vector3 previous)
    {
        characterLocations[game.map.ToTileCoordinates(previous)] = null;
        characterLocations[game.map.ToTileCoordinates(c.transform.position)] = c;
    }

    public void NextTurn()
    {
        currentTurn = (currentTurn + 1) % 2;
        if (currentTurn == 0)
        {
            allyTurn = (allyTurn + 1) % allies.Count;
            currentCharacter = allies[allyTurn];
        }
        else
        {
            enemyTurn = (enemyTurn + 1) % enemies.Count;
            currentCharacter = enemies[enemyTurn];
        }
        character = currentCharacter.GetComponent<CharacterMovement>();
        currentPath.Clear();
        lastTile = game.map.mapTiles[game.map.ToTileCoordinates(character.transform.position)];
        GetMovementTiles(currentCharacter, character.speed);
        speed = character.speed;
        canMove = true;
    }

    void GetMovementTiles(GameObject pos, int speed)
    {
        for(int i = 0; i < validTiles.Count; ++i)
        {
            validTiles[i].GetComponent<FloorTile>().ChangeColor(FloorTile.mat.regular);
        }
        validTiles = new List<GameObject>();
        int index;
        GameObject tile;
        int location = game.map.ToTileCoordinates(pos.transform.position);
        int xStart = location % game.map.tileWidth;
        int yStart = location / game.map.tileWidth;
        for (int x = -speed; x <= speed; ++x)
        {
            for (int y = -speed; y <= speed; ++y)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) > speed)
                {
                    continue;
                }
                if (xStart + x >= 0 && xStart + x < game.map.tileWidth && 
                    yStart + y >= 0 && yStart + y < game.map.tileHeight)
                {
                    index = location + x + game.map.tileWidth * y;
                    tile = game.map.mapTiles[index];
                    if (tile)
                    {
                        validTiles.Add(tile);
                        tile.GetComponent<FloorTile>().ChangeColor(FloorTile.mat.highlight);
                    }
                }
            }
        }
    }

    bool isValidTile(GameObject tile)
    {
        if (validTiles.Contains(tile))
        {
            return true;
        }
        return false;
    }

    public void removeChar(GameObject unit, bool isAlly)
    {
        Debug.Log("dead");
        if (isAlly)
        {
            allies.Remove(unit);
        }
        else
        {
            Debug.Log("REMOVE");
            enemies.Remove(unit);
        }
        characterLocations[game.map.ToTileCoordinates(unit.transform.position)] = null;
    }
}
