using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentController : MonoBehaviour
{
    Agent agent;
    AISteering steering;
    NavMeshPath path;
    bool moving;
    int currentCorner = 0;

    private void Awake()
    {
        agent = GetComponent<Agent>();
        steering = GetComponent<AISteering>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                GoToPoint(hit.point);
            }
        }

        if (moving)
        {
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                Vector3 newForce = new Vector3();
                newForce += (path.corners[currentCorner] - transform.position).normalized;

                Vector3 vToTarget = path.corners[currentCorner] - transform.position;
                float distanceToTarget = Mathf.Sqrt(vToTarget.x * vToTarget.x + vToTarget.z * vToTarget.z);
                if (distanceToTarget < steering.wanderTolerance)
                {
                    currentCorner++;
                    if (currentCorner == path.corners.Length)
                    {
                        moving = false;
                    }
                }
                agent.ApplyForce(newForce);
            }
        }
    }

    public void GoToPoint(Vector3 point)
    {
        NavMeshPath newPath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position - Vector3.down, point, 0, newPath);
        path = newPath;
        moving = true;
        currentCorner = 0;
    }
}
