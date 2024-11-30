using UnityEngine;
using Zenject;

public class AudioManagerInstaller : MonoInstaller
{   
    [Inject] Factory Factory;
    [SerializeField] private AudioData AudioData;
    
    public override void InstallBindings()
    {
        AudioManager AudioManager = new AudioManager(AudioData, Factory);
        Container.BindInstance<AudioManager>(AudioManager).AsSingle().NonLazy();
    }
}