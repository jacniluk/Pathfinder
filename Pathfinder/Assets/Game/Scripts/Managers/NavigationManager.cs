using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager Instance;

    private List<Node> nodes;
    private int mapLength;
    private float gridStartX;
    private float gridStartZ;

    private float findingPathTime;

    public float FindingPathTime => findingPathTime;

    private void Awake()
    {
        Instance = this;
    }

    public void BuildNavigationSystem()
    {
        int mapWidth = (int)MapManager.Instance.Ground.transform.localScale.x * 10;
        mapLength = (int)MapManager.Instance.Ground.transform.localScale.z * 10;
        gridStartX = -(mapWidth / 2.0f) + 0.5f;
        gridStartZ = -(mapLength / 2.0f) + 0.5f;

        nodes = new List<Node>();
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapLength; j++)
            {
                Vector3 position = new Vector3(gridStartX + i, 0.0f, gridStartZ + j);
                bool traversable = Physics.CheckBox(
                    position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, ReferencesManager.Instance.ObstacleLayerMask) == false;
                Node node = new Node(position, traversable);
                nodes.Add(node);
            }
        }

        Node createdNode;
        for (int i = 0; i < nodes.Count; i++)
        {
            createdNode = nodes[i];
            createdNode.neighbourNodes = new List<Node>();
            SetNeighbor(createdNode, 0.0f, -1.0f);
            SetNeighbor(createdNode, -1.0f, 0.0f);
            SetNeighbor(createdNode, 1.0f, 0.0f);
            SetNeighbor(createdNode, 0.0f, 1.0f);
        }
    }

    private void SetNeighbor(Node node, float offsetX, float offsetZ)
    {
        Node neighborNode = FindNodeWithPoint(node.position + new Vector3(offsetX, 0.0f, offsetZ));
        if (neighborNode != null && neighborNode.traversable)
        {
            node.neighbourNodes.Add(neighborNode);
        }
    }

    public Node FindNodeWithPoint(Vector3 point)
    {
        float x = point.x + 0.5f;
        float z = point.z + 0.5f;
        x = Mathf.Round(x) - 0.5f;
        z = Mathf.Round(z) - 0.5f;
        if (x < gridStartX || x > -gridStartX || z < gridStartZ || z > -gridStartZ)
        {
            return null;
        }

        int widthFactor = (int)(x - gridStartX);
        widthFactor *= mapLength;
        int lengthFactor = (int)(z - gridStartZ);
        int index = widthFactor + lengthFactor;

        return nodes[index];
    }

    public List<Vector3> FindPath(Node start, Node target)
    {
        float startTime = Time.realtimeSinceStartup;
        List<Vector3> path = BFS(start, target);
        findingPathTime = Time.realtimeSinceStartup - startTime;

        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Clear();
        }

        return path;
    }

    private List<Vector3> BFS(Node start, Node target)
    {
        List<Node> queue = new List<Node>() { start };
        start.visited = true;
        Node currentNode;
        Node neighbour;
        while (queue.Count > 0)
        {
            currentNode = queue[0];
            if (currentNode == target)
            {
                break;
            }
            queue.RemoveAt(0);
            for (int i = 0; i < currentNode.neighbourNodes.Count; i++)
            {
                neighbour = currentNode.neighbourNodes[i];
                if (neighbour.visited == false)
                {
                    neighbour.visited = true;
                    neighbour.previousNode = currentNode;
                    queue.Add(neighbour);
                }
            }
        }

        return ReconstructPath(target);
    }

    private List<Vector3> ReconstructPath(Node currentNode)
    {
        if (currentNode.previousNode != null)
        {
            List<Vector3> path = ReconstructPath(currentNode.previousNode);
            path.Add(currentNode.position);

            return path;
        }
        else
        {
            return new List<Vector3>();
        }
    }

    // Algorithms for testing

    //private List<Vector3> AStar(Node start, Node target)
    //{
    //    List<Node> openSet = new List<Node>() { start };
    //    start.g = 0.0f;
    //    start.h = Vector3.Distance(start.position, target.position);
    //    Node currentNode;
    //    Node neighbour;
    //    while (openSet.Count > 0)
    //    {
    //        currentNode = openSet[0];
    //        if (currentNode == target)
    //        {
    //            break;
    //        }
    //        openSet.RemoveAt(0);
    //        for (int i = 0; i < currentNode.neighbourNodes.Count; i++)
    //        {
    //            neighbour = currentNode.neighbourNodes[i];
    //            float tentativeG = currentNode.g + 1.0f;
    //            if (tentativeG < neighbour.g)
    //            {
    //                neighbour.previousNode = currentNode;
    //                neighbour.g = tentativeG;
    //                neighbour.h = Vector3.Distance(neighbour.position, target.position);
    //                neighbour.f = neighbour.g + neighbour.h;

    //                bool added = false;
    //                for (int j = 0; j < openSet.Count; j++)
    //                {
    //                    if (neighbour.f <= openSet[j].f)
    //                    {
    //                        openSet.Insert(j, neighbour);
    //                        added = true;

    //                        break;
    //                    }
    //                }
    //                if (added == false)
    //                {
    //                    openSet.Add(neighbour);
    //                }
    //            }
    //        }
    //    }

    //    return ReconstructPath(target);
    //}

    //public List<Vector3> Dijkstra(Node start, Node target)
    //{
    //    List<Node> openSet = new List<Node>() { start };
    //    start.f = 0;
    //    Node currentNode;
    //    Node neighbour;
    //    while (openSet.Count > 0)
    //    {
    //        currentNode = openSet[0];
    //        if (currentNode == target)
    //        {
    //            break;
    //        }
    //        openSet.Remove(currentNode);
    //        for (int i = 0; i < currentNode.neighbourNodes.Count; i++)
    //        {
    //            neighbour = currentNode.neighbourNodes[i];
    //            float tentativeF = currentNode.f + 1.0f;
    //            if (tentativeF < neighbour.f)
    //            {
    //                neighbour.previousNode = currentNode;
    //                neighbour.f = tentativeF;

    //                bool added = false;
    //                for (int j = 0; j < openSet.Count; j++)
    //                {
    //                    if (neighbour.f <= openSet[j].f)
    //                    {
    //                        openSet.Insert(j, neighbour);
    //                        added = true;

    //                        break;
    //                    }
    //                }
    //                if (added == false)
    //                {
    //                    openSet.Add(neighbour);
    //                }
    //            }
    //        }
    //    }

    //    return ReconstructPath(target);
    //}

    // Code for testing in FindPath()

    //float startTime = Time.realtimeSinceStartup;
    //List<Vector3> path = BFS(start, target);
    //findingPathTime = Time.realtimeSinceStartup - startTime;
    //Debug.Log("BFS - " + path.Count + " steps in " + findingPathTime* 1000.0f + "ms");

    //for (int i = 0; i<nodes.Count; i++)
    //{
    //    nodes[i].Clear();
    //}

    //startTime = Time.realtimeSinceStartup;
    //List<Vector3> path2 = AStar(start, target);
    //findingPathTime = Time.realtimeSinceStartup - startTime;
    //Debug.Log("A* - " + path2.Count + " steps in " + findingPathTime * 1000.0f + "ms");

    //for (int i = 0; i < nodes.Count; i++)
    //{
    //    nodes[i].Clear();
    //}

    //startTime = Time.realtimeSinceStartup;
    //List<Vector3> path3 = Dijkstra(start, target);
    //findingPathTime = Time.realtimeSinceStartup - startTime;
    //Debug.Log("Dijkstra - " + path3.Count + " steps in " + findingPathTime * 1000.0f + "ms");

    //for (int i = 0; i < nodes.Count; i++)
    //{
    //    nodes[i].Clear();
    //}
}
