using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData")]
public class AudioData : ScriptableObject
{
    [SerializeField] protected List<AudioSetting> _AudioCollection;
    public List<AudioSetting> AudioCollection => _AudioCollection;
}

[System.Serializable]
public class AudioSetting
{
    public string Name;
    public AudioClip[] Clips;
    public float Volume;
}