using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugNodeScore : MonoBehaviour
{
    Node node;
    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == "AStarNode")
            {
                node = colliders[i].GetComponent<Node>();
            }
        }
    }

    void Update()
    {
        text.text = node.fScore.ToString();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
