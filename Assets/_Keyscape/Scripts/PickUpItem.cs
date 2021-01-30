using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class PickUpItem : MonoBehaviour
{
    private Rigidbody _rb;
    private int _id;

    [SerializeField]
    private bool _seen = false;
    [SerializeField]
    private bool _followPlayer = false;

    public PlayerController Player;
    public int Score;

    private void Update()
    {
        if (_seen)
        {
            StopInteraction();
            UnlinkPlayer();
        }
    }

    private void FixedUpdate()
    {
        if (AbleToMove())
        {
            Move();
        }
    }

    private bool AbleToMove()
    {
        return !_seen && Player != null;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Move()
    {
        Vector3 Target = Player.RequestPosition(_id);

        _rb.MovePosition(Target);
    }

    private void StopInteraction()
    {
        _rb.constraints = RigidbodyConstraints.None;
        _rb.freezeRotation = false;
        StopFollowing();
        UnseenByGuard();
    }

    private void StartInteraction()
    {
        _rb.constraints = RigidbodyConstraints.FreezePositionY;
        _rb.freezeRotation = true;
        StartFollowing();
    }

    private void ChangeScore(bool positive)
    {
        if (positive)
        {
            Player.ChangeScore(Score);
        }
        else
        {
            Player.ChangeScore(-Score);
        }
    }

    private void UnlinkPlayer()
    {
        Player.DeleteItem(this);
        Player = null;
    }

    public void AddPlayer(PlayerController _newPlayer, int _newID)
    {
        _id = _newID;
        Player = _newPlayer;
        StartInteraction();
    }

    [ButtonMethod()]
    public void SeenByGuard()
    {
        _seen = true;
    }

    [ButtonMethod()]
    public void UnseenByGuard()
    {
        _seen = false;
    }

    public void StartFollowing()
    {
        _followPlayer = true;
        ChangeScore(true);
    }

    public void StopFollowing()
    {
        _followPlayer = false;
        ChangeScore(false);
    }
}
