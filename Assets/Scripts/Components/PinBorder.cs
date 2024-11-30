using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.EventSystems;

public class PinBorder : BaseSignal
{
    public SpriteRenderer Sprite;
    [SerializeField] private GameObject Border;
    [SerializeField] private GameObject MoveCheck;
    private bool CanShake = true;
    private Camera MainCamera;

    private float MouseDownTime;
    private const float HoldDuration = 0.5f;
    private bool IsDragging;

    void Start() 
    {
        MainCamera = Camera.main;
        MoveCheck.transform.localScale = Vector3.zero;
    }

    private void OnMouseEnter()
    {
        if(IsDragging)
        {
            Border.SetActive(false);
            return;
        }

        EmitSignal(EnumPinBorderEvents.Enter);
        Border.SetActive(true);
    }

    private void OnMouseExit()
    {
        if(IsDragging)
        {
            Border.SetActive(false);
            return;
        }

        EmitSignal(EnumPinBorderEvents.Exit);
        Border.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            MouseDownTime = HoldDuration;
            MoveCheck.SetActive(true); 
        }
    }

    private void OnMouseUp()
    {
        if (MouseDownTime > 0)
        {
            if(EventSystem.current.IsPointerOverGameObject()) return;

            if (CanShake) StartCoroutine(ShakeCooldownCoroutine());
            EmitSignal(EnumPinBorderEvents.Down);
        }
        else
        {
            EmitSignal(EnumPinBorderEvents.Stop);
        }

        MoveCheck.transform.localScale = Vector3.zero; 
        MoveCheck.SetActive(false);

        MouseDownTime = 0;
        IsDragging = false;
    }

    private void Update()
    {
        if (MouseDownTime > 0)
        {
            MouseDownTime -= Time.deltaTime;

            float scale = Mathf.Lerp(0, 0.4f, (HoldDuration - MouseDownTime) / HoldDuration);
            MoveCheck.transform.localScale = new Vector3(scale, scale, scale);
        }

        if (Input.GetMouseButton(0) && MouseDownTime < 0)
        {
            Vector3 MousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            MousePosition.z = 0;
            transform.parent.position = MousePosition;

            if (!IsDragging)
            {
                EmitSignal(EnumPinBorderEvents.Move);
                IsDragging = true;
            }
        }
    }

    private IEnumerator ShakeCooldownCoroutine()
    {
        CanShake = false;
        Vector3 originalScale = transform.localScale;

        transform.DOScale(originalScale * 1.1f, 0.1f).SetEase(Ease.OutBounce)
            .OnComplete(() => transform.DOScale(originalScale, 0.1f).SetEase(Ease.OutBounce));

        yield return new WaitForSeconds(0.2f);
        CanShake = true;
    }
}