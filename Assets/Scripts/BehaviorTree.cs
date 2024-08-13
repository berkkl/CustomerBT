using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : Node
{
    public BehaviorTree()
    {
        name = "Tree";
    }

    public BehaviorTree(string n)
    {
        this.name = n;
    }

    public override Status Process()
    {
        if (children.Count == 0) // If there are no children, keep going until the child is created
            // This is a safeguard to prevent the tree from getting stuck
            return Status.SUCCESS;
        return children[currentChild].Process();
    }

    struct NodeLevel
    {
        public int level;
        public Node node;
    }
}