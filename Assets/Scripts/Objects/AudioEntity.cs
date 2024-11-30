using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioEntity : MonoBehaviour
{
    public AudioSource AudioSource;

    void Awake() => AudioSource = GetComponent<AudioSource>();

    public void Play()
    {
        AudioSource.Play();
        Invoke(nameof(Deactivate), AudioSource.clip.length);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}