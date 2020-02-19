using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    Agent agent;
    Path path;
    int pathIterator;

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
            agent.ApplyForce(vToTarget.normalized);

            float distanceToTarget = Mathf.Sqrt(vToTarget.x * vToTarget.x + vToTarget.z * vToTarget.z);
            if (distanceToTarget < 0.3f)
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

public class Path
{
    public List<Vector3> points;

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
