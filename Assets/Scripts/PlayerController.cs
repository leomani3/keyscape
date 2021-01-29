using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float Speed = 30f;

    [SerializeField]
    private InputAction ActionInput;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        ActionInput.Enable();
    }

    private void OnDisable()
    {
        ActionInput.Disable();
    }

    private void Update()
    {
        Vector2 currentInput = ActionInput.ReadValue<Vector2>();

        rb.velocity = new Vector3(currentInput.x, rb.velocity.y, currentInput.y) * Speed;
        Debug.Log(ActionInput.ReadValue<Vector2>());
    }
}
