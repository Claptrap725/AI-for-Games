using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This goes on a child object of a Boid with a trigger collider and handles the boids neighbors list
/// </summary>
public class BoidTrigger : MonoBehaviour
{
    /// <summary>
    /// parent boid
    /// </summary>
    Boid boid;

    private void Awake()
    {
        boid = transform.parent.GetComponent<Boid>();
    }



    // Add or remove Boids from the neighbors list
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
