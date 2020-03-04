using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    Rigidbody rb;
    Agent agent;

    /// <summary>
    /// force multiplier for seperation of boids
    /// </summary>
    public float seperateWeight = 0;
    /// <summary>
    /// force multiplier for cohesion of boids
    /// </summary>
    public float cohesionWeight = 0;
    /// <summary>
    /// force multiplier for alignment of boids
    /// </summary>
    public float alignmentWeight = 0;

    /// <summary>
    /// amount seperation has randomly fluxated its weight
    /// </summary>
    float seperateWeightFlex = 0;
    /// <summary>
    /// amount cohesion has randomly fluxated its weight
    /// </summary>
    float cohesionWeightFlex = 0;
    /// <summary>
    /// amount alignment has randomly fluxated its weight
    /// </summary>
    float alignmentWeightFlex = 0;

    /// <summary>
    /// max amount of difference the forces will change by
    /// </summary>
    public float fluxuateWeights = 0;
    /// <summary>
    /// timer used to call FlexWeights() will count down from between 0.5 - 2 seconds
    /// </summary>
    float fluxTimer = 0;

    /// <summary>
    /// list of other nearby boid
    /// </summary>
    [HideInInspector]
    public List<Boid> neighbors = new List<Boid>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<Agent>();
    }

    // Update is called once per frame
    void Update()
    {
        // fluxTimer control to call FlewWeights() every 0.5 to 2 seconds
        if (fluxTimer <= 0)
        {
            FlexWeights();
            fluxTimer = Random.Range(0.5f, 2);
        }
        else
        {
            fluxTimer -= Time.deltaTime;
        }

        if (seperateWeight > 0)
        {
            // foreach neighbor add to the Vector3 force to get an angle that moves away from all other boids nearby
            Vector3 force = Vector3.zero;
            for (int i = 0; i < neighbors.Count; i++)
            {
                force += transform.position - neighbors[i].transform.position;
            }

            // reduce it so other forces even out
            if (neighbors.Count != 0)
                force /= neighbors.Count;

            // multiply in our weight and apply force
            Vector3 newForce = (force - rb.velocity) * seperateWeight;
            agent.ApplyForce(newForce);
        }

        if (cohesionWeight > 0)
        {
            // foreach neighbor add to the Vector3 force to get an angle that moves away from all other boids nearby
            Vector3 force = Vector3.zero;
            for (int i = 0; i < neighbors.Count; i++)
            {
                force += transform.position - neighbors[i].transform.position;
            }

            // reduce it so other forces even out
            if (neighbors.Count != 0)
                force /= neighbors.Count;

            // multiply in our weight and apply force
            Vector3 newForce = (force - rb.velocity) * -cohesionWeight;
            agent.ApplyForce(newForce);
        }

        if (alignmentWeight > 0)
        {
            // foreach neighbor add to the Vector3 force to get an angle that moves away from all other boids nearby
            Vector3 force = Vector3.zero;
            for (int i = 0; i < neighbors.Count; i++)
            {
                force += neighbors[i].rb.velocity;
            }

            // reduce it so other forces even out
            if (neighbors.Count != 0)
                force /= neighbors.Count;

            // multiply in our weight and apply force
            Vector3 newForce = (force - rb.velocity) * alignmentWeight;
            agent.ApplyForce(newForce);
        }
    }

    public void FlexWeights()
    {
        // set flex variables to random numbers
        seperateWeightFlex = Random.Range(-fluxuateWeights, fluxuateWeights);
        cohesionWeightFlex = Random.Range(-fluxuateWeights, fluxuateWeights);
        alignmentWeightFlex = Random.Range(-fluxuateWeights, fluxuateWeights);

        // add random numbers to weights
        seperateWeight += seperateWeightFlex;
        cohesionWeight += cohesionWeightFlex;
        alignmentWeight += alignmentWeightFlex;
    }
}
