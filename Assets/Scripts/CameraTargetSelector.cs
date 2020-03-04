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
    bool setMazeHeight = false;

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

        if (Input.GetKey(KeyCode.W))
        {
            followOffsetTargetPos -= followOffsetTargetPos.normalized;
        }
        if (Input.GetKey(KeyCode.S))
        {
            followOffsetTargetPos += followOffsetTargetPos.normalized;
        }

        if (Steve.instance.inMaze && !setMazeHeight)
        {
            followOffsetTargetPos = new Vector3(0, 140, -50);
            setMazeHeight = true;
        }

        transposer.m_FollowOffset += (followOffsetTargetPos - transposer.m_FollowOffset) * 0.01f;
    }
    
}
