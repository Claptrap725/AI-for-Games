using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISteering : MonoBehaviour
{
    [HideInInspector]
    public Agent agent;
    [HideInInspector]
    public Rigidbody rb;

    /// <summary>
    /// Agent will move towards all objects in seekTargets
    /// </summary>
    public List<GameObject> seekTargets;
    /// <summary>
    /// Agent will away towards all objects in fleeTargets
    /// </summary>
    public List<GameObject> fleeTargets;
    /// <summary>
    /// Agent will chase all objects in pursueTargets, taking into account the direction they are moving
    /// </summary>
    public List<GameObject> pursueTargets;
    /// <summary>
    /// Agent will try to escape all objects in evadeTargets, taking into account the direction they are moving
    /// </summary>
    public List<GameObject> evadeTargets;


    Vector3 tempSeekTarget = Vector3.zero;
    public bool wander;
    public float headingDistance;
    public float wanderTolerance = 0.2f;
    public float wanderDistance = 1;
    float wanderTimer = 0;
    public float forceStrength = 1;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<Agent>();
    }

    private void Update()
    {
        Vector3 newForce = new Vector3();
        if (wander)
        {
            if (wanderTimer <= 0)
            {
                tempSeekTarget = Vector3.zero;
            }
            if (tempSeekTarget == Vector3.zero)
            {
                Vector3 randDir = Random.onUnitSphere;
                randDir += agent.heading * headingDistance;
                randDir *= wanderDistance;
                randDir = new Vector3(randDir.x, 0, randDir.z);
                randDir += transform.position;
                tempSeekTarget = randDir;
                wanderTimer = 0.5f;
            }
            else
            {
                wanderTimer -= Time.deltaTime;
                newForce += (tempSeekTarget - transform.position).normalized;

                Vector3 vToTarget = tempSeekTarget - transform.position;
                float distanceToTarget = Mathf.Sqrt(vToTarget.x * vToTarget.x + vToTarget.z * vToTarget.z);
                if (distanceToTarget < wanderTolerance)
                {
                    tempSeekTarget = Vector3.zero;
                }
            }
        }

        for (int i = 0; i < seekTargets.Count; i++)
        {
            newForce += (seekTargets[i].transform.position - transform.position).normalized;
        }
        for (int i = 0; i < fleeTargets.Count; i++)
        {
            newForce += (transform.position - fleeTargets[i].transform.position).normalized;
        }

        for (int i = 0; i < pursueTargets.Count; i++)
        {
            newForce += (pursueTargets[i].transform.position + pursueTargets[i].GetComponent<Rigidbody>().velocity - transform.position).normalized;
        }
        for (int i = 0; i < evadeTargets.Count; i++)
        {
            newForce += (transform.position - evadeTargets[i].transform.position + evadeTargets[i].GetComponent<Rigidbody>().velocity).normalized;
        }
        newForce.y = 0;
        agent.ApplyForce(newForce * forceStrength);
    }

    
}
