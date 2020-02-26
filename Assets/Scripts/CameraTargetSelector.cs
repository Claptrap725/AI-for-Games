using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetSelector : MonoBehaviour
{
    public GameObject cameraTarget;
    Cinemachine.CinemachineVirtualCamera cinemachine;

    private void Awake()
    {
        cinemachine = GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (cinemachine.LookAt != cameraTarget.transform)
        {
            cinemachine.LookAt = cameraTarget.transform;
        }

        if (Steve.instance.inMaze)
        {
            // Change follow offset
        }
    }
}
