using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum Status {SUCCESS, RUNNING, FAILURE}
    public Status status;
    public List<Node> children = new List<Node>();
    public int currentChild = 0;
    public string name;
    public int sortOrder = 0;
    
    public Node () {}
    
    public Node(string n)
    {
        this.name = n;
    }

    public Node(string n, int s)
    {
        this.name = n;
        this.sortOrder = s;
    }

    public virtual Status Process()
    {
        return children[currentChild].Process();
    }
    public void AddChildren(Node n)
    {
        children.Add(n);
    }

    public int GetChildrenCount()
    {
        return children.Count;
    }

    public Node GetChild(int index)
    {
        if (index >= 0 && index < children.Count)
        {
            return children[index];
        }
        return null;
    }

}