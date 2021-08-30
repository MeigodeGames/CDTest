using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateObject : MonoBehaviour
{
    public float m_RotationSpeed = 20.0f;
    public float Direction = 0.0f;

    public void Rotate(GameObject gameObject)
    {
        gameObject.transform.Rotate(Vector3.up, m_RotationSpeed * Mathf.Deg2Rad * Direction);
    }

    private void OnMouseDrag()
    {
        /*
        float rotation = Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Deg2Rad;

        transform.Rotate(Vector3.right, rotation);
        */
    }
}
