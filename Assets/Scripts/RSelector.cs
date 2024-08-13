using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Random Selector Node, selects a random child to process
public class RSelector : Node
{
    bool isShuffled = false;

    public RSelector(string n)
    {
        this.name = n;
    }

    public override Status Process()
    {
        if (!isShuffled)
        {
            children.Shuffle();
            isShuffled = true;
        }
        Status childStatus = children[currentChild].Process();
        if (childStatus == Status.RUNNING) // middle of the performing task, keep going
            return Status.RUNNING;
        if (childStatus == Status.SUCCESS) // task is done, return success
        {
            currentChild = 0;
            isShuffled = false;
            return Status.SUCCESS;
        }

        currentChild++;

        if (currentChild >= children.Count) // all tasks failed, return failure
        {
            currentChild = 0;
            isShuffled = false;
            return Status.FAILURE;
        }
        return Status.RUNNING;
    }
}