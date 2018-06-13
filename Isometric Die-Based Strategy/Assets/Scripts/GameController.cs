using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject currentCharacter;
    private CharacterMovement character;

    public Game game;

    public int tileWidth;

    public List<GameObject> characterLocations;
    public GameObject[] allies;
    public GameObject[] enemies;
    public int currentTurn;
    public int allyTurn;
    public int enemyTurn;

    public void SetCharacterPath(GameObject destination)
    {
        List<GameObject> pathToDestination = Astar(game.map.GetTile(currentCharacter.transform.position), destination);
        if (pathToDestination.Count > 0)
        {
            for (int i = 0; i < pathToDestination.Count; ++i)
            {
                character.AddDestination(pathToDestination[i].transform.position);
            }
            character.state = CharacterMovement.charState.move;
        }
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
        game.battleUI.SetBattle(character, opponent.GetComponent<CharacterMovement>(), isAttacking);
        game.playerController.state = PlayerController.playerState.battle;
    }

    public void EndBattle()
    {
        game.playerController.state = PlayerController.playerState.movement;
        character.state = CharacterMovement.charState.idle;
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
        int max = Mathf.Max(allies.Length, enemies.Length);
        characterLocations = new List<GameObject>(map.tileWidth * map.tileHeight);
        for (int i = 0; i < map.tileWidth * map.tileHeight; ++i)
        {
            characterLocations.Add(null);
        }
        for (int i = 0; i < max; ++i)
        {
            if (i < allies.Length)
            {
                characterLocations[map.ToTileCoordinates(allies[i].transform.position)] = allies[i];
            }
            if (i < enemies.Length)
            {
                characterLocations[map.ToTileCoordinates(enemies[i].transform.position)] = enemies[i];
            }
        }
        currentTurn = 0;
        currentCharacter = allies[0];
        character = currentCharacter.GetComponent<CharacterMovement>();
        character.SetValidTiles(GetMovementTiles());
    }

    public void SetCharacterLocation(GameObject c, Vector3 previous)
    {
        characterLocations[game.map.ToTileCoordinates(previous)] = null;
        characterLocations[game.map.ToTileCoordinates(c.transform.position)] = c;
    }

    public void NextTurn()
    {
        currentTurn = (currentTurn + 1) % 2;
        character.SetValidTiles(null);
        if (currentTurn == 0)
        {
            allyTurn = (allyTurn + 1) % allies.Length;
            currentCharacter = allies[allyTurn];
        }
        else
        {
            enemyTurn = (enemyTurn + 1) % enemies.Length;
            currentCharacter = enemies[enemyTurn];
        }
        character = currentCharacter.GetComponent<CharacterMovement>();
        character.SetValidTiles(GetMovementTiles());
    }

    List<GameObject> GetMovementTiles()
    {
        Debug.Log(character.speed);
        List<GameObject> validTiles = new List<GameObject>();
        int location = game.map.ToTileCoordinates(currentCharacter.transform.position);
        int index;
        GameObject tile;
        for (int x = -character.speed; x <= character.speed; ++x)
        {
            for (int y = -character.speed; y <= character.speed; ++y)
            {
                if (location%game.map.tileWidth + x >= 0 && location%game.map.tileWidth + x < game.map.tileWidth &&
                    location/game.map.tileHeight + y >= 0 && location/game.map.tileHeight + y < game.map.tileHeight)
                {
                    index = location + x  + tileWidth * y;
                    Debug.Log(index);
                    tile = game.map.mapTiles[index];
                    if (tile && !validTiles.Contains(tile))
                    {
                        validTiles.Add(tile);
                        tile.GetComponent<FloorTile>().ChangeColor(FloorTile.mat.highlight);
                    }
                }
            }
        }
        return validTiles;
    }
}
