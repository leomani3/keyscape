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

    public List<PickUpItem> ItemsQueue;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        PickUpCall.performed += _ => CheckPickUp();
    }

    private void OnEnable()
    {
        PickUpCall.Enable();
        Movement.Enable();
    }

    private void OnDisable()
    {
        PickUpCall.Disable();
        Movement.Disable();
    }

    private void Start()
    {
        ItemsQueue = new List<PickUpItem>();
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
                    PickUpItem tempItem = collider.GetComponentInParent<PickUpItem>();

                    tempItem.AddPlayer(GetComponent<PlayerController>());
                    ItemsQueue.Add(tempItem);
                    return;
                }
            }
        }
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

    public void DeleteItem(PickUpItem item)
    {
        ItemsQueue.Remove(item);
    }
}
