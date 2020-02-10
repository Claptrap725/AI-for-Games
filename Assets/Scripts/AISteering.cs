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
    public bool keepHeading;
    public float wanderTolerance = 0.2f;
    public float wanderDistance = 1;
    public float speed;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    private void Update()
    {
        Vector3 force = new Vector3();
        if (wander)
        {
            if (tempSeekTarget == Vector3.zero)
            {
                Vector3 randDir = Random.onUnitSphere;
                if (keepHeading)
                    randDir += heading;
                randDir *= wanderDistance;
                randDir = new Vector3(randDir.x, 0, randDir.z);
                randDir += transform.position;
                tempSeekTarget = randDir;
            }
            else
            {
                force += ((tempSeekTarget - transform.position).normalized * speed);

                Vector3 vToTarget = tempSeekTarget - transform.position;
                float distanceToTarget = Mathf.Sqrt(vToTarget.x * vToTarget.x + vToTarget.y * vToTarget.y);
                if (distanceToTarget < wanderTolerance)
                {
                    tempSeekTarget = Vector3.zero;
                }
            }
        }
        for (int i = 0; i < seekTargets.Count; i++)
        {
            force += ((seekTargets[i].transform.position - transform.position).normalized * speed);
        }
        for (int i = 0; i < fleeTargets.Count; i++)
        {
            force += ((transform.position - fleeTargets[i].transform.position).normalized * speed);
        }
        force -= velocity;
        

        velocity += force * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        heading = velocity.normalized;
    }
}
