using UnityEngine;
using MyBox;
using System;
using DG.Tweening;

public class PickUpItem : MonoBehaviour
{
    private Rigidbody _rb;
    private Outline _outline;
    private float previousYPOS;

    [SerializeField]
    private bool _seen = false;
    [SerializeField]
    private bool _followPlayer = false;

    public SpawnPosition spawnPosition;
    public LevelManagerRef levelManager;
    public PlayerController Player;
    public int Score;
    public int ID;
    public AudioClip pickedUpAudio;

    private AudioSource _audioSource;

    public Action<int> OnSeen;

    private bool AbleToMove()
    {
        return !_seen && _followPlayer;
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = pickedUpAudio;
        
        _rb = GetComponent<Rigidbody>();
        _outline = GetComponent<Outline>();
    }

    private void FixedUpdate()
    {
        if (AbleToMove())
        {
            Vector3 Target = Player.RequestPosition(ID);

            _rb.AddForce((Target - transform.position).normalized * Player.GetSpeed(), ForceMode.VelocityChange);
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, Player.GetMaxSpeed());
        }
    }

    public void StopInteraction()
    {
        _rb.velocity = Vector3.zero;
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
        Player = null;
    }

    public void HideToPlace(SpawnPosition hideout)
    {
        _rb.transform.DOMove(hideout.transform.position, 1f).OnComplete(StopInteraction);
        hideout.IsTaken = true;
    }

    public void AddPlayer(PlayerController _newPlayer, int _newID)
    {
        _audioSource.Play();
        
        ID = _newID;
        Player = _newPlayer;
        StartInteraction();
        transform.position = new Vector3(transform.position.x, _newPlayer.gameObject.transform.position.y, transform.position.z);

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
