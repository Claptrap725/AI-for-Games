using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    

    public float seperateWeight = 1;
    Vector3 seperationForce = Vector3.zero;
    [HideInInspector]
    public List<Boid> neighbors = new List<Boid>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        for (int i = 0; i < neighbors.Count; i++)
        {
            seperationForce += transform.position - neighbors[i].transform.position;
        }

        if (neighbors.Count != 0)
            seperationForce /= neighbors.Count;

        Vector3 forceToApply = (seperationForce -) * seperateWeight;
        */
    }
}
