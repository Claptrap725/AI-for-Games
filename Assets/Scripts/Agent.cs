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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Collider[] walls = Physics.OverlapSphere(heading.normalized + transform.position, 1);
        for (int i = 0; i < walls.Length; i++)
        {
            if (walls[i].tag == "Wall" && transform.position.y < walls[i].transform.position.y)
                force += (transform.position - walls[i].transform.position).normalized * 2;
        }



        rb.velocity += force;
        float speed = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z);
        if (speed > maxSpeed && force != Vector3.zero)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        else
        {
            float y = rb.velocity.y;
            rb.velocity = rb.velocity * 0.9f;
            rb.velocity = new Vector3(rb.velocity.x, y, rb.velocity.z);
        }
        heading = rb.velocity.normalized;
        force = Vector3.zero;
    }

    public void ApplyForce(Vector3 _force)
    {
        force += _force;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(heading.normalized + transform.position, 1);
    }
}
