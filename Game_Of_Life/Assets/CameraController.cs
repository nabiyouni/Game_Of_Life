using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Vector3 previousMousePosition;
    private Vector3 rotationPoint;

    void Start()
    {
        previousMousePosition = Input.mousePosition;
        rotationPoint = new Vector3(GameManager3D.rowsCount / 2, GameManager3D.colsCount / 2, GameManager3D.depsCount / 2);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - previousMousePosition;
            int sign = (int)(-this.transform.up.y / Mathf.Abs(this.transform.up.y));

            float angleWithY = Mathf.Acos(Vector3.Dot(this.transform.forward.normalized, Vector3.up));

            Debug.Log("----------------------------------------------");
            Debug.Log(angleWithY);
            Debug.Log("delta Y" + delta.y);
            if (!((angleWithY > 3.0 && delta.y < 0) || (angleWithY < 0.14 && delta.y > 0)))
            {
                this.transform.RotateAround(rotationPoint, this.transform.right, -delta.y * 0.1f);
                this.transform.RotateAround(rotationPoint, Vector3.up, delta.x * 0.1f);
            }
        }
        previousMousePosition = Input.mousePosition;
    }
}
