using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// rotates the camera on mouse drag
public class CameraController : MonoBehaviour
{

    private Vector3 previousMousePosition;
    private Vector3 rotationPoint;
    private Vector3 deltaRotation;
    private const float motionRate = 0.05f;
    private const float MaxCameraPitch = Mathf.Deg2Rad * 120;
    private const float MinCameraPitch = Mathf.Deg2Rad * 60;
    private const float mouseSensitivity = 0.17f;

    void Start()
    {
        previousMousePosition = Input.mousePosition;
        rotationPoint = new Vector3(GameManager3D.rowsCount / 2, GameManager3D.colsCount / 2, GameManager3D.depthsCount / 2);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // if mouse is dragging add to the delta rotation
            Vector3 deltaRotationThisFrame = Input.mousePosition - previousMousePosition;
            deltaRotation += deltaRotationThisFrame;
        }
        // apply part of the current rotation to the camera to create momentum
        Vector3 deltaRotationToApply = deltaRotation * motionRate;
        this.transform.RotateAround(rotationPoint, this.transform.right, -deltaRotationToApply.y * mouseSensitivity);
        this.transform.RotateAround(rotationPoint, Vector3.up, deltaRotationToApply.x * mouseSensitivity);

        // reduce the delta rotation by a rate each frame so the rotation can continue after left click up and create momentum
        deltaRotation = deltaRotation * (1 - motionRate);
        previousMousePosition = Input.mousePosition;
    }
}
