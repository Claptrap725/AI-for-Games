using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISteering : MonoBehaviour
{
    public List<GameObject> seekTargets;
    public List<GameObject> fleeTargets;
    public List<GameObject> pursueTargets;
    public List<GameObject> evadeTargets;
    Vector3 tempSeekTarget = Vector3.zero;
    public bool wander;
    public float headingDistance;
    public float wanderTolerance = 0.2f;
    public float wanderDistance = 1;
    float wanderTimer = 0;
    public float maxSpeed;

    Rigidbody rb;
    Vector3 heading = new Vector3();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 force = new Vector3();
        if (wander)
        {
            if (wanderTimer <= 0)
            {
                tempSeekTarget = Vector3.zero;
            }
            if (tempSeekTarget == Vector3.zero)
            {
                Vector3 randDir = Random.onUnitSphere;
                randDir += heading * headingDistance;
                randDir *= wanderDistance;
                randDir = new Vector3(randDir.x, 0, randDir.z);
                randDir += transform.position;
                tempSeekTarget = randDir;
                wanderTimer = 0.5f;
            }
            else
            {
                wanderTimer -= Time.deltaTime;
                force += (tempSeekTarget - transform.position).normalized;

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
            force += (seekTargets[i].transform.position - transform.position).normalized;
        }
        for (int i = 0; i < fleeTargets.Count; i++)
        {
            force += (transform.position - fleeTargets[i].transform.position).normalized;
        }

        for (int i = 0; i < pursueTargets.Count; i++)
        {
            force += (pursueTargets[i].transform.position + pursueTargets[i].GetComponent<Rigidbody>().velocity - transform.position).normalized;
        }
        for (int i = 0; i < evadeTargets.Count; i++)
        {
            force += (transform.position - evadeTargets[i].transform.position + evadeTargets[i].GetComponent<Rigidbody>().velocity).normalized;
        }

        Collider[] walls = Physics.OverlapSphere(heading.normalized + transform.position, 1);
        for (int i = 0; i < walls.Length; i++)
        {
            if (walls[i].tag == "Wall")
            force += (transform.position - walls[i].transform.position).normalized*2; 
        }

        rb.velocity += force;
        float speed = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z);
        if (speed > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        heading = rb.velocity.normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(heading.normalized + transform.position, 1);
    }
}
