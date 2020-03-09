using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Node : MonoBehaviour
{
    public List<Neighbor> neighbors = new List<Neighbor>();
    public Node previous;
    public float gScore;
    public float hScore;
    public float fScore { get { return gScore + hScore; } }

    private void Awake()
    {
        gScore = 0;
    }

    private void Start()
    {
        AStar.instance.AddNodeToList(this);
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i].difficulty == 0)
                neighbors[i].difficulty = 1;

            neighbors[i].cost = Vector3.Distance(transform.position, neighbors[i].node.transform.position) * neighbors[i].difficulty;
        }
    }

    public void OnValidate()
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i].difficulty == 0)
                neighbors[i].difficulty = 1;

            neighbors[i].cost = Vector3.Distance(transform.position, neighbors[i].node.transform.position) * neighbors[i].difficulty;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < neighbors.Count; i++)
        {
            Gizmos.DrawLine(transform.position, neighbors[i].node.transform.position);
        }
    }

    private void OnMouseDown()
    {
        transform.parent.GetComponent<AStar>().NodeClicked(this);
    }
    
}

[System.Serializable]
public class Neighbor
{
    public Node node;
    public float difficulty = 1;
    public float cost;

    public Neighbor()
    {

    }

    public Neighbor(Node _node)
    {
        node = _node;
    }
}
