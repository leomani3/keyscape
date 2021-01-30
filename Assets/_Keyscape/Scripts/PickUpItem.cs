using UnityEngine;
using MyBox;
using System;
using DG.Tweening;

public class PickUpItem : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField]
    private bool _seen = false;
    [SerializeField]
    private bool _followPlayer = false;

    public PlayerController Player;
    public int Score;
    public int ID;

    public Action<int> OnSeen;

    private void FixedUpdate()
    {
        if (AbleToMove())
        {
            Move();
        }
    }

    private bool AbleToMove()
    {
        return !_seen && _followPlayer;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Move()
    {
        Vector3 Target = Player.RequestPosition(ID);

        _rb.AddForce((Target - transform.position).normalized * Player.GetSpeed(), ForceMode.VelocityChange);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, Player.GetMaxSpeed());
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

    public void HideToPlace(Vector3 hideout)
    {
        _rb.transform.DOMove(hideout, 5f);
    }

    public void AddPlayer(PlayerController _newPlayer, int _newID)
    {
        ID = _newID;
        Player = _newPlayer;
        StartInteraction();
    }

    [ButtonMethod()]
    public void SeenByGuard()
    {
        if (!_seen && _followPlayer)
        {
            _seen = true;
            OnSeen?.Invoke(ID);
            StopInteraction();
            UnlinkPlayer();
        }
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
