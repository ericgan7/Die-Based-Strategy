using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	public virtual int Attack()
    {
        return 0;
    }
    public virtual int Defence()
    {
        return 0;
    }
    public virtual int Speed()
    {
        return 0;
    }
    public virtual int Endurance()
    {
        return 0;
    }
    public virtual int Armor()
    {
        return 0;
    }
    public virtual int Health()
    {
        return 0;
    }
    public virtual int[] Use()
    {
        int[] effects = { 0, 0, 0, 0, 0, 0 };
        return effects;
    }
    public virtual StatusEffects Status()
    {
        return GetComponent<StatusEffects>();
        
    }
    public virtual Card Card()
    {
        return GetComponent<Card>();
    }
}
