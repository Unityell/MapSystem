using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Pin : MonoBehaviour
{
    EventBus EventBus;
    StateManager StateManager;
    AudioManager AudioManager;

    [SerializeField] PinBorder PinBorder;
    private Vector3 OriginalScale;
    Camera MainCamera;
    public PinSetupStruct PinInfo {get; private set;}

    private void OnEnable()
    {
        MainCamera = Camera.main;

        DoTweenScaleAnimation();

        PinBorder.Subscribe(SignalBox);
    }

    public void Initialization(List<object> Systems)
    {
        EventBus = Systems.Find(x => x is EventBus) as EventBus;
        StateManager = Systems.Find(x => x is StateManager) as StateManager;
        AudioManager = Systems.Find(x => x is AudioManager) as AudioManager;
        SetUp(Systems.OfType<PinSetupStruct>().FirstOrDefault());
    }

    public void SetUp(PinSetupStruct PinSetupStruct)
    {
        PinInfo = new PinSetupStruct();

        PinSetupStruct.X = transform.position.x;
        PinSetupStruct.Y = transform.position.y;

        PinInfo = PinSetupStruct;
        PinBorder.Sprite.color = PinSetupStruct.Color;

        EventBus.Invoke(new PinSignal(this, EnumPinBorderEvents.Stop));
    }

    void DoTweenScaleAnimation()
    {
        OriginalScale = PinBorder.transform.localScale;
        PinBorder.transform.localScale = Vector3.zero; 
        
        PinBorder.transform.DOScale(OriginalScale, 0.2f).SetEase(Ease.OutBounce);        
    }

    void SignalBox(object Obj)
    {
        if(Obj is EnumPinBorderEvents Enum) 
        {
            EventBus.Invoke(new PinSignal(this, Enum));

            switch (Enum)
            {
                case EnumPinBorderEvents.Move :
                    StateManager.State = EnumApplicationState.PinMoving;
                    break;
                case EnumPinBorderEvents.Stop :
                    SetUp(PinInfo);
                    StateManager.State = EnumApplicationState.Navigation;
                    break;
                case EnumPinBorderEvents.Down :
                    AudioManager.PlaySound("Click");
                    break;
                default: break;
            }
        }
    }

    void Update()
    {
        transform.localScale = Vector3.one * MainCamera.orthographicSize / 2;
    }

    public void Delete()
    {
        gameObject.SetActive(false);
        EventBus.Invoke(new PinSignal(this, EnumPinBorderEvents.Delete));
        PinBorder.Unsubscribe(SignalBox);   
        AudioManager.PlaySound("Delete");
    } 
}