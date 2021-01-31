using UnityEngine;
using MyBox;
using System;
using DG.Tweening;

public class PickUpItem : MonoBehaviour
{
    private Rigidbody _rb;
    private Outline _outline;

    [SerializeField]
    private bool _seen = false;
    [SerializeField]
    private bool _followPlayer = false;
    [SerializeField]
    private float _speedMultiplier = 2f;

    public SpawnPosition spawnPosition;
    public LevelManagerRef levelManager;
    public PlayerController Player;
    public int Score;
    public int ID;

    public Action<int> OnSeen;

    private bool AbleToMove()
    {
        return !_seen && _followPlayer;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _outline = GetComponent<Outline>();
    }

    private void Move()
    {
        if (AbleToMove())
        {
            Vector3 Target = Player.RequestPosition(ID);

            _rb.AddForce((Target - transform.position).normalized * Player.GetSpeed() * _speedMultiplier, ForceMode.VelocityChange);
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, Player.GetMaxSpeed());
        }
    }

    private void StopInteraction()
    {
        _rb.constraints = RigidbodyConstraints.None;
        _rb.freezeRotation = false;
        StopFollowing();
        UnlinkPlayer();
        UnseenByGuard();
        _outline.enabled = true;
    }

    private void StartInteraction()
    {
        _rb.constraints = RigidbodyConstraints.FreezePositionY;
        _rb.freezeRotation = true;
        StartFollowing();
        _outline.enabled = false;
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
        Player.OnMove -= Move;
        Player = null;
    }

    public void HideToPlace(SpawnPosition hideout)
    {
        _rb.transform.DOMove(hideout.transform.position, 1f).OnComplete(StopInteraction);
        hideout.IsTaken = true;
    }

    public void AddPlayer(PlayerController _newPlayer, int _newID)
    {
        ID = _newID;
        Player = _newPlayer;
        StartInteraction();
        Player.OnMove += Move;

        levelManager.levelManager.SetPositionIsTaken(spawnPosition, false);
        spawnPosition = null;
    }

    [ButtonMethod()]
    public void SeenByGuard()
    {
        if (AbleToMove())
        {
            _seen = true;
            spawnPosition = levelManager.levelManager.GetNearestAvailablePosition(this);
            HideToPlace(spawnPosition);
            OnSeen?.Invoke(ID);
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
