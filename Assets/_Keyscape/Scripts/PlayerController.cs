using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MyBox;

public class PlayerController : MonoBehaviour
{
    [Separator("Player Bindings")]
    [SerializeField]
    private InputAction Movement;
    [SerializeField]
    private InputAction PickUpCall;

    [Separator("Player Characteristics")]
    [SerializeField]
    private float Speed = 30f;
    [SerializeField]
    private int Score = 0;
    [SerializeField]
    private float PickUpRadius = 30f;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Movement.Enable();
    }

    private void OnDisable()
    {
        Movement.Disable();
    }

    [ButtonMethod()]
    private void CheckPickUp()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, PickUpRadius);

        foreach(Collider collider in objects)
        {
            if (collider.CompareTag("PickableItem"))
            {
                if (collider.GetComponentInParent<PickUpItem>().Player == null)
                {
                    collider.GetComponentInParent<PickUpItem>().AddPlayer(GetComponent<PlayerController>());
                    return;
                }
            }
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        Vector2 currentInput = GetPlayerMovement();

        _rb.velocity = new Vector3(currentInput.x, _rb.velocity.y, currentInput.y) * Speed;
    }

    public Vector2 GetPlayerMovement()
    {
        return Movement.ReadValue<Vector2>();
    }

    public void ChangeScore(int _addScore)
    {
        Score += _addScore;
    }
}
