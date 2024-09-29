using System;
using System.Collections.Generic;
using UnityEngine;

public class GOAPAction
{
    public Dictionary<string, bool> preconditions { get; private set; }

    public Func<GoapState, bool> Preconditions = delegate { return true; };

    public Dictionary<string, bool> effects { get; private set; }

    public Func<GoapState, GoapState> Effects;

    public float Cost { get; private set; }

    public string Name { get; private set; }

    public GOAPAction(string name)
    {
        this.Name = name;
        Cost = 1f;
        effects = new Dictionary<string, bool>();
    }

    public GOAPAction SetCost(float cost)
    {
        if (cost < 1f)
        {
            Debug.Log(string.Format("Warning: Using cost < 1f for '{0}' could yield sub-optimal results", Name));
        }

        this.Cost = cost;
        return this;
    }

    public GOAPAction Pre(string s, bool value)
    {
        preconditions[s] = value;
        return this;
    }

    public GOAPAction Pre(Func<GoapState, bool> p)
    {
        Preconditions = p;
        return this;
    }

    public GOAPAction Effect(string s, bool value)
    {
        effects[s] = value;
        return this;
    }

    public GOAPAction Effect(Func<GoapState, GoapState> e)
    {
        Effects = e;
        return this;
    }
}
