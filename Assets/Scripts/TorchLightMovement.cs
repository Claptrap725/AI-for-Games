using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLightMovement : MonoBehaviour
{
    public float maxTravelRadius = 0.5f;
    public float speed = 0.1f;
    Vector3 startPos;
    Steve steve;
    bool setPhysics = false;
    bool pos = false;

    private void Start()
    {
        startPos = transform.localPosition;
        steve = GetComponentInParent<Steve>();
    }

    private void Update()
    {
        if (steve == null || steve.health > 0)
        {
            if (steve != null)
            {
                if (!pos && steve.collectingBerries)
                {
                    pos = true;
                    startPos += Vector3.up;
                }
                else if (pos && !steve.collectingBerries)
                {
                    pos = false;
                    startPos += Vector3.down;
                }
            }


            transform.localPosition += new Vector3(Random.Range(-1, 1) * speed, Random.Range(-1, 1) * speed, Random.Range(-1, 1) * speed);

            if (Vector3.Distance(transform.localPosition, startPos) > maxTravelRadius)
            {
                transform.localPosition = ((transform.localPosition - startPos).normalized * maxTravelRadius) + startPos;
            }
        }
        else if (!setPhysics)
        {
            setPhysics = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<SphereCollider>().enabled = true;
            transform.parent = null;
        }
    }
}
