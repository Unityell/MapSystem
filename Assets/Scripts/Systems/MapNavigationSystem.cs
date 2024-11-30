using UnityEngine;
using Zenject;

public class MapNavigationSystem : BaseSignal
{
    [Inject] StateManager StateManager;
    [Inject] EventBus EventBus;

    [Header("Map")]
    [SerializeField] SpriteRenderer MapSpriteRenderer;

    [Header("Zoom settings")]
    [Tooltip("Скорость изменения Orthographic размера")]
    [SerializeField] float ZoomSpeed;
    [Tooltip("Минимальное значение Orthographic размера")]
    [SerializeField] float MinZoom;
    [Tooltip("Максимальное значение Orthographic размера")]
    [SerializeField] float MaxZoom;
    [Tooltip("Сила сглаженной интерполяции при переходе от Orthographic размера Min к Max и обратно")]
    [SerializeField] float ZoomSmoothTime;

    private Vector3 LastMousePos;
    private Vector3 TargetPosition;
    private float TargetZoom;
    private float ZoomVelocity;

    private Camera MainCamera;

    void Start()
    {
        EventBus.Subscribe(SignalBox);

        MainCamera = Camera.main;

        TargetPosition = MainCamera.transform.position;
        TargetZoom = MainCamera.orthographicSize;
    }

    void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case PinSignal PinSignal :
                if(PinSignal.Event == EnumPinBorderEvents.Down)
                {
                    TargetZoom = 1;
                    TargetPosition = PinSignal.Pin.transform.position;                    
                }
                break;
            default: break;
        }
    }

    void Update()
    {
        if(StateManager.State == EnumApplicationState.Navigation)
        {
            if (Input.GetMouseButtonDown(0))
            {
                LastMousePos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 delta = LastMousePos - Input.mousePosition;
                TargetPosition += delta * (MainCamera.orthographicSize / 5f) * Time.deltaTime;

                LastMousePos = Input.mousePosition;
            }              
        }

        MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, new Vector3(TargetPosition.x, TargetPosition.y, -10), Time.deltaTime * 5f);
        MainCamera.orthographicSize = Mathf.SmoothDamp(MainCamera.orthographicSize, TargetZoom, ref ZoomVelocity, ZoomSmoothTime);  

        float ZoomDelta = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        TargetZoom = Mathf.Clamp(TargetZoom - ZoomDelta, MinZoom, MaxZoom);

        ClampCameraPosition();
    }

    private void ClampCameraPosition()
    {
        if (MapSpriteRenderer == null) return;

        float SpriteMinX = MapSpriteRenderer.bounds.min.x;
        float SpriteMaxX = MapSpriteRenderer.bounds.max.x;
        float SpriteMinY = MapSpriteRenderer.bounds.min.y;
        float SpriteMaxY = MapSpriteRenderer.bounds.max.y;

        float CameraHeight = MainCamera.orthographicSize * 2f;
        float CameraWidth = CameraHeight * MainCamera.aspect;

        TargetPosition.x = Mathf.Clamp(TargetPosition.x, SpriteMinX + CameraWidth / 2, SpriteMaxX - CameraWidth / 2);
        TargetPosition.y = Mathf.Clamp(TargetPosition.y, SpriteMinY + CameraHeight / 2, SpriteMaxY - CameraHeight / 2);
    }
}