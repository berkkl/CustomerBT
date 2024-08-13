using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Node
{
    public delegate Status Tick();
    public Tick ProcessMethod;

    public delegate Status MultiTick(int index);
    public MultiTick MultiProcessMethod;

    public int index;
    public Leaf(){}

    public Leaf(string n, Tick pm)
    {
        name = n;
        ProcessMethod = pm;
    }

    public Leaf(string n, int i, MultiTick pm)
    {
        name = n;
        index = i;
        MultiProcessMethod = pm;
    }
    public Leaf(string n, Tick pm, int s)
    {
        name = n;
        ProcessMethod = pm;
        sortOrder = s;
    }

    public override Status Process()
    {
        if(ProcessMethod != null)
            return ProcessMethod();
        if(MultiProcessMethod != null)
            return MultiProcessMethod(index);
        return Status.FAILURE;    
    }
}