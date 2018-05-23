using UnityEngine;
using System.Collections;

public class CameraFacingBillboardLocked : MonoBehaviour //This script is attached to the world space canvas and takes an input of camera. then follows that camera.
{
    void Update()
    {
        var v = Camera.main.transform.forward;
        v.y = 0;
        transform.rotation = Quaternion.LookRotation(v, Vector3.up);
    }
}