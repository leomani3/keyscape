using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    
    [SerializeField] CinemachineMixingCamera mixingCamera;
    [SerializeField] float changeDuration;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCamera(int index) => SetCamera(index, changeDuration);

    public void SetCamera(int index, float time)
    {
        if (!isActiveAndEnabled)
            return;
        for (int i = 0; i < mixingCamera.ChildCameras.Length; i++)
        {
            bool active;
            active = i == index;
            DOTween.To(() => mixingCamera.GetWeight(i), v => mixingCamera.SetWeight(i, v), active ? 1.0f : 0.0f, time).SetEase(Ease.InOutSine);
        }
    }

    public float ChangeDuration
    {
        set => changeDuration = value;
    }
}