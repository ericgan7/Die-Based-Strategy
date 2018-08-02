using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour {

    public GameObject baseTile;
    public int tileWidth;
    public int tileHeight;

    public List<GameObject> mapTiles;
    private float startx;
    private float starty;

    void Awake()
    {
        mapTiles = new List<GameObject>();
        GameObject obj;
        for (int z = 0; z < tileHeight; ++z)
        {
            for (int x = 0; x < tileWidth; ++x)
            {
                obj = Instantiate(baseTile, new Vector3(2*x, 0.0f, 2*z), baseTile.transform.rotation);
                mapTiles.Add(obj);
                obj.transform.parent = transform;
            }
        }
        Transform parent = GetComponentInParent<Transform>();
        startx = parent.position.x / 2;
        starty = parent.position.z / 2;
    }

    public GameObject GetTile(Vector3 location)
    {
        Vector3 local = baseTile.transform.InverseTransformPoint(location);
        int index = Mathf.FloorToInt(local.x) + Mathf.RoundToInt(local.y) * tileWidth;
        return mapTiles[index];
    }

    public int ToTileCoordinates(Vector3 location)
    {
        Vector3 local = baseTile.transform.InverseTransformPoint(location);
        return Mathf.RoundToInt(local.x) + Mathf.RoundToInt(local.y) * tileWidth;
    }

    public List<GameObject> GetNeighbors(GameObject tile)
    {
        Vector3 pos = tile.transform.localPosition;
        List<GameObject> neighbors = new List<GameObject>();
        int index;
        int x = Mathf.RoundToInt(startx + pos.x);
        int y = Mathf.RoundToInt(starty + pos.y);
        index = x + (y * tileWidth);

        if (x > 0)
        {
            neighbors.Add(mapTiles[index-1]);
        }
        if (x < tileWidth - 1)
        {
            neighbors.Add(mapTiles[index+1]);
        }
        if (y > 0)
        {
            neighbors.Add(mapTiles[index-tileWidth]);
        }
        if (y < tileHeight - 1)
        {
            neighbors.Add(mapTiles[index+tileWidth]);
        }
        return neighbors;
    }

    public float GetDistance(GameObject start, GameObject end)
    {
        Vector3 difference = end.transform.localPosition - start.transform.localPosition;
        return Mathf.Abs(difference.x) + Mathf.Abs(difference.y) + Mathf.Abs(difference.z);
    }

}
