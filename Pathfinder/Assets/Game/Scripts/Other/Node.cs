using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 position;
    public bool traversable;

    public List<Node> neighbourNodes;
    public Node previousNode;
    public bool visited;

    //public float f; // A* & Dijkstra
    //public float g; // A*
    //public float h; // A*

    public Node(Vector3 _position, bool _traversable)
    {
        position = _position;
        traversable = _traversable;

        Clear();
    }

    public void Clear()
    {
        previousNode = null;
        visited = false;

        //f = float.MaxValue; // A* & Dijkstra
        //g = float.MaxValue; // A*
        //h = 0.0f; // A*
    }
}
