using Zenject;

public class StateManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<StateManager>().AsSingle();
    }
}