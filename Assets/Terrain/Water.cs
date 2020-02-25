using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public float multiplyer;
    public float speed;
    float timer = 0;
    
    void Update()
    {
        timer = Time.realtimeSinceStartup * speed;
        transform.GetChild(0).position = new Vector3(-500, 1.4f, -500) + ((Vector3.up * Mathf.Sin(timer)) * multiplyer);
    }
}
