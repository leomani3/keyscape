using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MyBox;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;

    [Separator("Player Bindings")]
    [SerializeField]
    private InputAction Movement;
    [SerializeField]
    private InputAction PickUpCall;
    [SerializeField]
    private List<Vector3> PreviousPositions;

    [Separator("Player Characteristics")]
    [SerializeField]
    private float Speed = 30f;
    [SerializeField]
    private int Score = 0;
    [SerializeField]
    private float PickUpRadius = 30f;

    [Separator("Queue Characteristics")]
    [SerializeField]
    private float StoreRange;
    [SerializeField]
    private int Spacing;

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
        PreviousPositions = new List<Vector3>();
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

                    tempItem.AddPlayer(this, ItemsQueue.Count);
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

    private void FixedUpdate()
    {
        if (PreviousPositions.Count != 0 && PreviousPositions[0] != transform.position)
        {
            PreviousPositions.Add(transform.position);
        }

        if (PreviousPositions.Count > StoreRange)
        {
            PreviousPositions.RemoveAt(PreviousPositions.Count);
        }
    }

    public Vector3 RequestPosition(int id)
    {
        return PreviousPositions[id * Spacing];
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
