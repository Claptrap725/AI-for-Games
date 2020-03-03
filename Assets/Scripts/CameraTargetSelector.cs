using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetSelector : MonoBehaviour
{
    public GameObject cameraTarget;
    CinemachineVirtualCamera cinemachine;
    CinemachineOrbitalTransposer transposer;
    public Vector3 followOffsetTargetPos;

    private void Awake()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();
        transposer = cinemachine.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void Start()
    {
        followOffsetTargetPos = transposer.m_FollowOffset;
    }

    void Update()
    {
        if (cinemachine.LookAt != cameraTarget.transform)
        {
            cinemachine.LookAt = cameraTarget.transform;
        }

        if (Steve.instance.inMaze)
        {
            followOffsetTargetPos = new Vector3(0, 300, -20);
        }

        transposer.m_FollowOffset += (followOffsetTargetPos - transposer.m_FollowOffset) * 0.01f;
    }
    
}
