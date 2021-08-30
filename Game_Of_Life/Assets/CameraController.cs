using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Vector3 previousMousePosition;
    private Vector3 rotationPoint;
    private Vector3 deltaRotation;
    private const float motionRate = 1.05f;

    void Start()
    {
        previousMousePosition = Input.mousePosition;
        rotationPoint = new Vector3(GameManager3D.rowsCount / 2, GameManager3D.colsCount / 2, GameManager3D.depsCount / 2);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 deltaRotationThisFrame = Input.mousePosition - previousMousePosition;
            deltaRotation = deltaRotationThisFrame;
        }
        Vector3 deltaRotationToApply = deltaRotation / motionRate;
        float angleWithY = Mathf.Acos(Vector3.Dot(this.transform.forward.normalized, Vector3.up));
        if (!((angleWithY > 3.0 && deltaRotationToApply.y < 0) || (angleWithY < 0.14 && deltaRotationToApply.y > 0)))
        {
            this.transform.RotateAround(rotationPoint, this.transform.right, -deltaRotationToApply.y * 0.1f);
            this.transform.RotateAround(rotationPoint, Vector3.up, deltaRotationToApply.x * 0.1f);
        }

        deltaRotation = deltaRotation / motionRate;
        previousMousePosition = Input.mousePosition;
    }
}
