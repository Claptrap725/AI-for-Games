using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidTrigger : MonoBehaviour
{
    Boid boid;

    private void Awake()
    {
        boid = transform.parent.GetComponent<Boid>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC")
            boid.neighbors.Add(other.GetComponent<Boid>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "NPC")
            boid.neighbors.Remove(other.GetComponent<Boid>());
    }
}
