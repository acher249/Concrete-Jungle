using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour //This script is attached to the world space canvas and takes an input of camera. then follows that camera.
{
    public Camera m_Camera;

    void Update()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);
    }
}