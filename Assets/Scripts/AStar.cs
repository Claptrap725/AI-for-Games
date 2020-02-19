﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AStar : MonoBehaviour
{
    public PathFollower NPC;
    public static List<Node> allNodes = new List<Node>();

    Node targetNode;
    Node startNode;

    List<Node> openList = new List<Node>();
    List<Node> closedList = new List<Node>();
    
    public void NodeClicked(Node caller)
    {
        targetNode = caller;
        for (int i = 0; i < allNodes.Count; i++)
        {
            allNodes[i].gScore = 0;
            allNodes[i].hScore = 0;
            if (startNode == null || Vector3.Distance(startNode.transform.position, NPC.transform.position) > Vector3.Distance(allNodes[i].transform.position, NPC.transform.position))
            {
                startNode = allNodes[i];
            }
        }
        NPC.NewPath(GetPath());
    }

    public Path GetPath()
    {
        List<Node> path = new List<Node>();
        openList.Clear();
        closedList.Clear();

        openList.Add(startNode);
        Node current = startNode;
        int loops = 0;

        for (;;)
        {
            loops++;
            if (loops > 20000)
            {
                Debug.Log("Faild to find Path!!");
                return new Path(path);
            }
            current = openList[0];
            for (int i = 0; i < openList.Count; i++)
            {
                if (current.fScore > openList[i].fScore)
                    current = openList[i];
            }

            if (current == targetNode)
            {
                break;
            }

            openList.Remove(current);
            closedList.Add(current);
            for (int i = 0; i < current.neighbors.Count; i++)
            {
                if (!closedList.Contains(current.neighbors[i].node))
                {
                    openList.Add(current.neighbors[i].node);
                    current.neighbors[i].node.gScore = current.gScore + current.neighbors[i].cost;
                    current.neighbors[i].node.hScore = Vector3.Distance(current.neighbors[i].node.transform.position, targetNode.transform.position);
                    current.neighbors[i].node.previous = current;
                }
                else if (current.neighbors[i].node.fScore > current.neighbors[i].node.hScore + current.gScore + current.neighbors[i].cost)//compare fScores
                {
                    closedList.Remove(current.neighbors[i].node);
                    openList.Add(current.neighbors[i].node);
                    current.neighbors[i].node.gScore = current.gScore + current.neighbors[i].cost;
                    current.neighbors[i].node.hScore = Vector3.Distance(current.neighbors[i].node.transform.position, current.transform.position);
                    current.neighbors[i].node.previous = current;
                }
            }
        }

        
        while (current != startNode)
        {
            path.Add(current);
            current = current.previous;
        }

        path.Reverse();
        return new Path(path);
    }
}