using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    Vector3 force = Vector3.zero;
    Rigidbody rb;

    [HideInInspector]
    public Vector3 heading = new Vector3();
    public float maxSpeed;
    public bool alive = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (alive)
        {
            transform.LookAt(heading + transform.position);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            Collider[] walls = Physics.OverlapSphere(heading.normalized * 0.4f + transform.position + Vector3.up * 0.2f, 1.8f);
            for (int i = 0; i < walls.Length; i++)
            {
                if (walls[i].tag == "Wall")
                {
                    Vector3 newForce = (transform.position - walls[i].transform.position).normalized * 10;
                    newForce.y = 0;
                    force += newForce;
                }
            }

            rb.velocity += force;
            if (rb.velocity.y < -8f)
            {
                //Debug.Log(name + "is falling!");
                Vector3 newSpeed = rb.velocity * 0.8f;
                newSpeed.y = rb.velocity.y;
                rb.velocity = newSpeed;
            }
            float speed = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z);
            if (speed > maxSpeed && force != Vector3.zero)
            {
                Vector3 newSpeed = rb.velocity.normalized * maxSpeed;
                newSpeed.y = rb.velocity.y;
                rb.velocity = newSpeed;
            }
            else
            {
                //float y = rb.velocity.y;
                rb.velocity = new Vector3(rb.velocity.x * 0.9f, rb.velocity.y, rb.velocity.z * 0.9f);
                //rb.velocity = new Vector3(rb.velocity.x, y, rb.velocity.z);
            }
        }

        heading = rb.velocity.normalized;
        force = Vector3.zero;
    }

    public void ApplyForce(Vector3 _force)
    {
        force += _force;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(heading.normalized * 0.4f + transform.position + Vector3.up * 0.2f, 1.8f);
    }
}
