using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public float speed;
    public Path path;
    public bool pathFinished { get { return pathIterator >= path.points.Count; } set { if (value) pathIterator = path.points.Count; } }
    Agent agent;
    int pathIterator = 0;

    private void Awake()
    {
        agent = GetComponent<Agent>();
    }

    private void Update()
    {
        if (path != null && pathIterator < path.points.Count)
        {
            Vector3 vToTarget = path.points[pathIterator] - transform.position;
            vToTarget.y = 0;
            agent.ApplyForce(vToTarget.normalized * speed);
            

            float distanceToTarget = Mathf.Sqrt(vToTarget.x * vToTarget.x + vToTarget.z * vToTarget.z);
            if (distanceToTarget < 1f)
            {
                pathIterator++;
            }
        }
    }

    public void NewPath(Path newPath)
    {
        pathIterator = 0;
        path = newPath;
    }
}

[System.Serializable]
public class Path
{
    public List<Vector3> points;

    public Path()
    {
        points = new List<Vector3>();
    }

    public Path(List<GameObject> gameObjects)
    {
        points = new List<Vector3>();


        for (int i = 0; i < gameObjects.Count; i++)
        {
            points.Add(gameObjects[i].transform.position);
        }
    }

    public Path(List<Node> nodes)
    {
        points = new List<Vector3>();

        for (int i = 0; i < nodes.Count; i++)
        {
            points.Add(nodes[i].transform.position);
        }
    }
}
