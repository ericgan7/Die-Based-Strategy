using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue {
    private List<pair> queue;
    private bool isMin;
    public PriorityQueue(bool lessThan)
    {
        queue = new List<pair>();
        isMin = lessThan;
    }

    public void add(float k, GameObject v)
    {
        queue.Add(new pair(k, v));
        sort();
    }

    public pair pop()
    {
        pair end = queue[queue.Count - 1];
        queue.RemoveAt(queue.Count - 1);
        return end;
    }

    // bubble sort the new value, assuming rest is already sorted;
    void sort()
    {
        int index = queue.Count - 1;
        pair temp;
        while (index > 0 && needsReorder(index))
        {
            temp = queue[index];
            queue[index] = queue[index - 1];
            queue[index - 1] = temp;
            --index;
        }
    }
    
    bool needsReorder(int index)
    {
        if (isMin)
        {
            return queue[index] > queue[index - 1];
        }
        else
        {
            return queue[index] < queue[index - 1];
        }
    }

    public bool isEmpty()
    {
        if (queue.Count == 0)
        {
            return true;
        }
        return false;
    }
}

public class pair
{
    public float key;
    public GameObject value;
    public pair(float k, GameObject v)
    {
        key = k;
        value = v;
    }
    static public bool operator <(pair p1, pair p2)
    {
        if (p1.key < p2.key)
        {
            return true;
        }
        return false;
    }
    public static bool operator >(pair p1, pair p2)
    {
        if (p1.key > p2.key)
        {
            return true;
        }
        return false;
    }
}

public class path
{
    public GameObject previous;
    public float cost;
    public path( GameObject p, float c)
    {
        previous = p;
        cost = c;
    }
}
