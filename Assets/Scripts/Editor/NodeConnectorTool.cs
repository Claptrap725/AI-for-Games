using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Node))]
public class NodeConnectorTool : Editor
{
    static bool running = false;
    static Node lastT;

    void OnSceneGUI()
    {
        Event cur = Event.current;

        if (running)
        {
            Handles.BeginGUI();
            GUI.Box(new Rect(20, 20, 100, 50), "Node Connecting");
            Handles.EndGUI();
        }

        if (cur.isKey && cur.keyCode == KeyCode.RightControl)
        {
            if (cur.type == EventType.KeyDown)
            {
                running = !running;
            }
        }
        else if (cur.type == EventType.MouseDown && cur.isMouse && cur.button == 0 && running)
        {
            Node t = target as Node;
            if (t == null)
                return;
            
            Debug.Assert(lastT != t);


            if (lastT == null)
            {
                lastT = t;
            }
            else if (t == lastT)
            {
                return;
            }
            else
            {
                //Debug.Log("let's form a connection?");
                for (int i = 0; i < t.neighbors.Count; i++)
                {
                    if (t.neighbors[i].node == lastT)
                    {
                        lastT = null;
                        return;
                    }
                }

                for (int i = 0; i < lastT.neighbors.Count; i++)
                {
                    if (lastT.neighbors[i].node == t)
                    {
                        lastT = null;
                        return;
                    }
                }

                LinkNodes(t, lastT);
                lastT = null;
                running = false;
            }
        }
    }

    public void LinkNodes(Node nodeA, Node nodeB)
    {
        nodeA.neighbors.Add(new Neighbor(nodeB));
        nodeB.neighbors.Add(new Neighbor(nodeA));
        nodeA.OnValidate();
        nodeB.OnValidate();
    }
}