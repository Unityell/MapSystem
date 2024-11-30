using UnityEngine;
using Zenject;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class PinSpawner : MonoBehaviour
{
    [Inject] StateManager StateManager;
    [Inject] EventBus EventBus;
    [Inject] Factory Factory;
    [Inject] AudioManager AudioManager;

    [SerializeField] Pin PinPrefab;

    private Camera MainCamera;

    private bool IsWaitingForSpawn = false;
    private Vector3 InitialMousePosition;
    private float FramesWithoutMovement = 0;
    private const float WaitThreshold = 0.1f;
    private const float MovementThreshold = 0.5f;

    bool IsEnable = true;

    [Tooltip("Заглушка с иконками")]
    [SerializeField] Sprite[] Sprites;

    void Awake()
    {
        MainCamera = Camera.main;
        EventBus.Subscribe(SignalBox);
    }

    void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case PinSignal PinSignal:
                IsEnable = PinSignal.Event == EnumPinBorderEvents.Enter ? false : true;
                break;
            case LoadSignal LoadSignal:
                StartCoroutine(SpawnByLoadingInfo(LoadSignal.PinList));
                break;
            default: break;
        }
    }

    IEnumerator SpawnByLoadingInfo(List<PinSetupStruct> PinList)
    {
        foreach (var LoadedStruct in PinList)
        {
            var Pin = Factory.Create<Pin>(PinPrefab.gameObject, new Vector2(LoadedStruct.X, LoadedStruct.Y));
            Pin.Initialization(new List<object> { EventBus, StateManager, LoadedStruct, AudioManager });
            yield return null;
        }
    }

    void Spawn()
    {
        Vector3 CurrentMousePosition = Input.mousePosition;

        if (Vector3.Distance(InitialMousePosition, CurrentMousePosition) < MovementThreshold)
        {
            FramesWithoutMovement += Time.deltaTime;

            if (FramesWithoutMovement >= WaitThreshold)
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                var Point = MainCamera.ScreenToWorldPoint(InitialMousePosition);
                Point.z = 0;
                var Pin = Factory.Create<Pin>(PinPrefab.gameObject, Point);

                var RandomColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.7f, 1f);
                var PinSetupStruct = new PinSetupStruct(Pin.GetInstanceID(), "", "", Sprites[Random.Range(0, Sprites.Length)], RandomColor, Point.x, Point.y);
                Pin.Initialization(new List<object> { EventBus, StateManager, PinSetupStruct, AudioManager });
                
                IsWaitingForSpawn = false;
            }
        }
        else
        {
            IsWaitingForSpawn = false;
        }    
    }

    void Update()
    {
        if (StateManager.State != EnumApplicationState.Navigation || !IsEnable) return;

        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && !IsWaitingForSpawn)
        {
            StartWaitingForSpawn();
        }

        if (IsWaitingForSpawn)
        {
            Spawn();
        }
    }

    private void StartWaitingForSpawn()
    {
        InitialMousePosition = Input.mousePosition;
        IsWaitingForSpawn = true;
        FramesWithoutMovement = 0;
    }

    void OnDestroy() => EventBus.Unsubscribe(SignalBox);
}