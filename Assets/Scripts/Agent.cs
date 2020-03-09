using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    /// <summary>
    /// vector that gets add to our velocity
    /// </summary>
    Vector3 force = Vector3.zero;

    Rigidbody rb;
    Steve steve;

    /// <summary>
    /// direction of current velocity
    /// </summary>
    [HideInInspector]
    public Vector3 heading = new Vector3();
    /// <summary>
    /// maxSpeed of movement in x and z direction. Does not prevent falling faster than maxSpeed
    /// </summary>
    public float maxSpeed;
    /// <summary>
    /// Is the agent alive
    /// </summary>
    public bool alive = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        steve = GetComponent<Steve>();
    }

    void Update()
    {
        if (alive)
        {
            // face our heading
            transform.LookAt(heading + transform.position);
            // don't tip over though
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            // if this agent is steve, he may skip wall checks if he is collecting berries
            if (steve == null || !steve.collectingBerries)
            {
                // check for colliders in front of us
                Collider[] walls = Physics.OverlapSphere(heading.normalized * 0.4f + transform.position + Vector3.up * 0.2f, 1.8f);
                for (int i = 0; i < walls.Length; i++)
                {
                    // if the collider is a wall then don't run into it
                    // also will try to doge swordsmen so they don't run into each other but if they have killed steve then they can crowd him
                    if (walls[i].tag == "Wall" || (walls[i].tag == "Swordman" && Steve.instance.health > 0))
                    {
                        // applyforce away from wall
                        Vector3 newForce = (transform.position - walls[i].transform.position).normalized * 10;
                        newForce.y = 0;
                        force += newForce;
                    }
                }
            }

            // change our rigidbody velocity by force
            rb.velocity += force;
            // if we are falling then drag on x and z movemnt (prevent walking on air)
            if (rb.velocity.y < -15f)
            {
                Vector3 newSpeed = rb.velocity * 0.8f;
                newSpeed.y = rb.velocity.y;
                rb.velocity = newSpeed;
            }

            // if moving too fast then clamp
            float speed = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z);
            if (speed > maxSpeed && force != Vector3.zero)
            {
                Vector3 newSpeed = rb.velocity.normalized * maxSpeed;
                newSpeed.y = rb.velocity.y;
                rb.velocity = newSpeed;
            }
            else
            {
                // normal drag
                rb.velocity = new Vector3(rb.velocity.x * 0.9f, rb.velocity.y, rb.velocity.z * 0.9f);
            }
        }

        // set heading
        heading = rb.velocity.normalized;
        // reset force
        force = Vector3.zero;
    }

    /// <summary>
    /// Increases force on agent and is called by all scripts that control movement.
    /// Force is likely to take affect next game loop
    /// </summary>
    /// <param name="_force"></param>
    public void ApplyForce(Vector3 _force)
    {
        force += _force;
    }

    private void OnDrawGizmosSelected()
    {
        // draws wall checker
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(heading.normalized * 0.4f + transform.position + Vector3.up * 0.2f, 1.8f);
    }
}
