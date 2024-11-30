using UnityEngine;
using Zenject;

public class PinInfoWidget : Widgets
{
    [Inject] StateManager StateManager;

    [Header("Widget settings")]
    [SerializeField] InfoHeader InfoHeader;
    [SerializeField] InfoCard InfoCard;
    Camera MainCamera;

    Pin Target;

    void Start()
    {
        Subscribe();
        MainCamera = Camera.main;

        InfoCard.Subscribe(SignalBox);
        InfoHeader.Subscribe(SignalBox);
    }

    protected override void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case PinSignal PinSignal :
                if(PinSignal.Event == EnumPinBorderEvents.Down)
                {

                    InfoCard.gameObject.SetActive(false);
                    InfoHeader.gameObject.SetActive(false);
                    Enable(false);

                    Enable(true);
                    Target = PinSignal.Pin;

                    InfoCard.SetUp(Target.PinInfo); 
                    InfoHeader.SetUp(Target.PinInfo);

                    InfoHeader.gameObject.SetActive(true);
                    StateManager.State = EnumApplicationState.Info;
                }
                break;
            case EnumInfoCardSignals.Save :
                Target.SetUp(InfoCard.GetInfo());
                break;
            case EnumInfoCardSignals.Close :
                Invoke(nameof(Clear), 0.1f);;
                break;
            case EnumInfoCardSignals.Delete :
                Target.Delete();
                Clear();
                break;
            case EnumInfoCardSignals.Change :
                break;
            default: break;
        }
    }

    void Clear()
    {
        StateManager.State = EnumApplicationState.Navigation;
        InfoCard.gameObject.SetActive(false);
        InfoHeader.gameObject.SetActive(false);
        Enable(false);
        Target = null;        
    }

    void Update()
    {
        if(InfoCard.gameObject.activeSelf && Target) InfoCard.transform.position = MainCamera.WorldToScreenPoint(Target.transform.position);
        if(InfoHeader.gameObject.activeSelf && Target) InfoHeader.transform.position = MainCamera.WorldToScreenPoint(Target.transform.position);
    }

    protected override void Unsubscribe()
    {
        base.Unsubscribe();
        InfoCard.Unsubscribe(SignalBox);
        InfoHeader.Unsubscribe(SignalBox);
    }
}