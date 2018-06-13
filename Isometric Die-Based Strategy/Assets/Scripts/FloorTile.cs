using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour {

    public enum mat { regular, highlight };

    public int cost;

    private Renderer render;
    public Material regular;
    public Material highlight;

    void Awake()
    {
        render = GetComponent<Renderer>();
    }

    public void ChangeColor(mat type)
    {
        switch (type)
        {
            case mat.regular:
                render.material = regular;
                break;
            case mat.highlight:
                render.material = highlight;
                break;
        }
    }
}
