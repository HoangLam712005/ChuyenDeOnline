using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    public CinemachineVirtualCamera virtualcamera;
    public void AssignCamera(Transform playertransform)
    {
        virtualcamera.Follow = playertransform;
        virtualcamera.LookAt = playertransform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
