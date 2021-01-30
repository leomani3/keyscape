using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MyBox;
using System;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;

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
    [SerializeField]
    private float MaxSpeed = 100f;

    [Separator("Queue Characteristics")]
    [SerializeField]
    private float StoreRange;
    [SerializeField]
    private int Spacing;
    [SerializeField]
    private float DistanceBetween;
    [SerializeField]
    private List<Vector3> PreviousPositions;

    public List<PickUpItem> ItemsQueue;
    public Action OnMove;

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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, PickUpRadius);

        foreach (Vector3 position in PreviousPositions)
        {
            Gizmos.DrawSphere(position, 0.05f);
        }
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

                    HandleNewItem(tempItem);
                    Debug.Log($"Adding new item: {ItemsQueue.Count - 1}");
                    tempItem.AddPlayer(this, ItemsQueue.Count - 1);
                    return;
                }
            }
        }
    }

    private void UpdateIDs(int seenID)
    {
        for(int i = seenID + 1; i < ItemsQueue.Count; i++)
        {
            ItemsQueue[i].ID -= 1; 
        }
    }

    private void HandleNewItem(PickUpItem tempItem)
    {
        if (StoreRange < (ItemsQueue.Count + 1) * Spacing)
        {
            for (int i = 0; i < (ItemsQueue.Count + 1) * Spacing - StoreRange; i++)
            {
                PreviousPositions.Add(transform.position);
            }

            StoreRange += (ItemsQueue.Count + 1) * Spacing - StoreRange;
        }

        tempItem.OnSeen += UpdateIDs;
        ItemsQueue.Add(tempItem);
    }

    private void FixedUpdate()
    {
        Vector2 currentInput = GetPlayerMovement();

        _rb.AddForce(new Vector3(currentInput.x * Speed, _rb.velocity.y, currentInput.y * Speed), ForceMode.VelocityChange);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, MaxSpeed);

        if (PreviousPositions.Count != 0 && Vector3.Distance(transform.position, PreviousPositions[PreviousPositions.Count - 1]) > DistanceBetween)
        {
            AddNewPosition(transform.position);
        }

        if (PreviousPositions.Count > StoreRange)
        {
            PreviousPositions.RemoveAt(0);
        }
    }

    private void AddNewPosition(Vector3 _position)
    {
        PreviousPositions.Add(_position);
        OnMove?.Invoke();
    }

    public Vector3 RequestPosition(int id)
    {
        return PreviousPositions[PreviousPositions.Count - 1 - id * Spacing];
    }

    public float GetSpeed()
    {
        return Speed;
    }

    public float GetMaxSpeed()
    {
        return MaxSpeed;
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
        StoreRange -= Spacing;
        item.OnSeen -= UpdateIDs;

        PreviousPositions.RemoveRange(0, Spacing);
        ItemsQueue.Remove(item);
    }
}
