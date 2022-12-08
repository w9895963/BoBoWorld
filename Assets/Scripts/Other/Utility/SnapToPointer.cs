using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SnapToPointer : MonoBehaviour
{
    public InputAction input = new InputAction("pointer", InputActionType.Value, "<Pointer>/position");
    public bool UseScreenPosition = true;
    public Vector2 preventEdgeMax = Vector2.zero;
    public Vector2 preventEdgeMin = Vector2.zero;
    void Start()
    {
        input.performed += PointerAction;
        input.Enable();

        Snap(Mouse.current.position.ReadValue());


    }

    private void PointerAction(InputAction.CallbackContext d)
    {
        Snap(d.ReadValue<Vector2>());
    }

    private void Snap(Vector2 position)
    {
        Vector2 resMax = new Vector2(Screen.width - preventEdgeMax.x, Screen.height - preventEdgeMax.y);
        Vector2 resMin = preventEdgeMin;
        position = position.Clamp(resMin, resMax);

        if (UseScreenPosition == false)
        {
            position = position.ScreenToWold();
        }

        gameObject.SetPosition(position);
    }


    private void OnDisable()
    {
        input.performed -= PointerAction;
        input.Disable();
    }


}
