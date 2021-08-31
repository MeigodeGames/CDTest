using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveObject : MonoBehaviour
{
    public float m_MovementSpeed = 2.0f;
    public Vector3 Movement = Vector3.zero;

    public void Move(GameObject gameObject)
    {
        Vector3 displacement = Movement.normalized * m_MovementSpeed * Time.fixedDeltaTime;

        gameObject.transform.position += displacement;
    }
}
