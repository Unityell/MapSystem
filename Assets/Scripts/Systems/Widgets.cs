using UnityEngine;
using Zenject;

public abstract class Widgets : MonoBehaviour
{
    [Inject] protected EventBus EventBus;

    [Header("Widget")]
    [SerializeField] 
    protected GameObject Widget;

    protected virtual void Subscribe()
    {
        EventBus.Subscribe(SignalBox);
    }

    protected virtual void Unsubscribe()
    {
        EventBus.Unsubscribe(SignalBox);
    }

    protected virtual void SignalBox(object Obj){}

    public void Enable(bool Switch)
    {
        Widget.SetActive(Switch);
    }
    
    private void OnDestroy()
    {
        Unsubscribe();
    }
}