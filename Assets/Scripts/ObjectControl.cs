using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ColorObject))]
[RequireComponent(typeof(MoveObject))]
[RequireComponent(typeof(RotateObject))]
[RequireComponent(typeof(ModelManager))]
public class ObjectControl : MonoBehaviour
{
    public GameObject m_ActiveObject;

    private ColorObject m_ColorBehaviour;
    private MoveObject m_MovementBehaviour;
    private RotateObject m_RotationBehaviour;
    private ModelManager m_ModelManager;

    private void Awake()
    {
        m_ColorBehaviour = GetComponent<ColorObject>();
        m_MovementBehaviour = GetComponent<MoveObject>();
        m_RotationBehaviour = GetComponent<RotateObject>();
        m_ModelManager = GetComponent<ModelManager>();
    }

    private void FixedUpdate()
    {
        if (!m_ActiveObject) return;

        m_RotationBehaviour.Rotate(m_ActiveObject);
        m_MovementBehaviour.Move(m_ActiveObject);
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        m_RotationBehaviour.Direction = context.ReadValue<float>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();

        m_MovementBehaviour.Movement.x = input.x;
        m_MovementBehaviour.Movement.z = input.y;
    }

    public void OnCopy(InputAction.CallbackContext context)
    {
        if (!m_ActiveObject) return;
        if (!context.action.triggered) return;

        Vector3 newPosition = m_ActiveObject.transform.position;
        newPosition.x += 2;
        newPosition.z += 2;

        GameObject newModel = Instantiate(m_ActiveObject, newPosition, m_ActiveObject.transform.rotation);
        m_ModelManager.m_ObjectList.Add(newModel);
    }

    public void OnScale(InputAction.CallbackContext context)
    {
        if (!m_ActiveObject) return;
        if (!context.action.triggered) return;

        float input = context.ReadValue<float>();
        Vector3 newScale = m_ActiveObject.transform.localScale + Vector3.one * input * 0.2f;

        if (newScale.magnitude < Vector3.one.magnitude * 0.7) return;

        m_ActiveObject.transform.localScale = newScale;
    }

    public void OnColorChange(InputAction.CallbackContext context)
    {
        if (!m_ActiveObject) return;
        if (!context.action.triggered) return;

        m_ColorBehaviour.ChangeColor(m_ActiveObject, (int)context.ReadValue<float>());
    }
}
