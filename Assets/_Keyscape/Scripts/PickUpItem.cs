using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class PickUpItem : MonoBehaviour
{
    [SerializeField]
    private bool _seen = false;
    [SerializeField]
    private bool _followPlayer = false;

    public int Score;
    public PlayerController Player;

    private void Update()
    {
        if (_seen)
        {
            StopFollowing();
            UnlinkPlayer();
            UnseenByGuard();
        }
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
        Player.DeleteItem(GetComponent<PickUpItem>());
        Player = null;
    }

    public void AddPlayer(PlayerController _newPlayer)
    {
        Player = _newPlayer;
        StartFollowing();
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
