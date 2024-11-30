using UnityEngine;

public class ApplicationSettings : MonoBehaviour
{
    [SerializeField, Range(30, 60)] int TargetFrameRate;

    void Start()
    {
        Application.targetFrameRate = TargetFrameRate;
    }
}