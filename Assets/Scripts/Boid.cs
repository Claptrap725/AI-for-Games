using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    Rigidbody rb;
    Agent agent;
    public float seperateWeight = 0;
    public float cohesionWeight = 0;
    public float alignmentWeight = 0;
    float seperateWeightFlex = 0;
    float cohesionWeightFlex = 0;
    float alignmentWeightFlex = 0;
    public float fluxuateWeights = 0;
    float fluxTimer = 0;
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
            Vector3 force = Vector3.zero;
            for (int i = 0; i < neighbors.Count; i++)
            {
                force += transform.position - neighbors[i].transform.position;
            }

            if (neighbors.Count != 0)
                force /= neighbors.Count;

            Vector3 newForce = (force - rb.velocity) * (seperateWeight + seperateWeightFlex);
            agent.ApplyForce(newForce);
        }

        if (cohesionWeight > 0)
        {
            Vector3 force = Vector3.zero;
            for (int i = 0; i < neighbors.Count; i++)
            {
                force += transform.position - neighbors[i].transform.position;
            }

            if (neighbors.Count != 0)
                force /= neighbors.Count;

            Vector3 newForce = (force - rb.velocity) * ((cohesionWeight + cohesionWeightFlex) * -1);
            agent.ApplyForce(newForce);
        }

        if (alignmentWeight > 0)
        {
            Vector3 force = Vector3.zero;
            for (int i = 0; i < neighbors.Count; i++)
            {
                force += neighbors[i].rb.velocity;
            }

            if (neighbors.Count != 0)
                force /= neighbors.Count;

            Vector3 newForce = (force - rb.velocity) * (alignmentWeight + alignmentWeightFlex);
            agent.ApplyForce(newForce);
        }
    }

    public void FlexWeights()
    {
        seperateWeightFlex = Random.Range(-fluxuateWeights, fluxuateWeights);
        cohesionWeightFlex = Random.Range(-fluxuateWeights, fluxuateWeights);
        alignmentWeightFlex = Random.Range(-fluxuateWeights, fluxuateWeights);

        seperateWeight += seperateWeightFlex;
        cohesionWeight += cohesionWeightFlex;
        alignmentWeight += alignmentWeightFlex;
    }
}
