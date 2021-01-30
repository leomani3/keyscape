using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField]
    private bool _seen = false;
    [SerializeField]
    private bool _followPlayer = false;

    public int Score;
    public PlayerController Player;

    void Update()
    {
        if (_seen)
        {
            StopFollowing();
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

    public void AddPlayer(PlayerController _newPlayer)
    {
        Player = _newPlayer;
        StartFollowing();
    }

    public void SeenByGuard()
    {
        _seen = true;
    }

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
