using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoCard : BaseSignal
{
    [SerializeField] TMP_InputField Name;
    [SerializeField] TMP_InputField HINT;
    [SerializeField] Image Icon;

    Image NameImage, HINTImage;

    PinSetupStruct PinSetupStruct;

    void Start()
    {
        NameImage = Name.GetComponent<Image>();
        HINTImage = HINT.GetComponent<Image>();
    }

    public void SetUp(PinSetupStruct PinSetupStruct)
    {
        this.PinSetupStruct = PinSetupStruct;

        Name.text = PinSetupStruct.Name;
        HINT.text = PinSetupStruct.HINT;
        Icon.sprite = PinSetupStruct.Sprite;
    }

    public PinSetupStruct GetInfo()
    {
        PinSetupStruct.Name = Name.text;
        PinSetupStruct.HINT = HINT.text;
        return PinSetupStruct;
    }

    public void Save()
    {
        ChangeColor(Color.white);
        SetEnable(false);
        EmitSignal(EnumInfoCardSignals.Save);
        EmitSignal(EnumInfoCardSignals.Close);
    }

    public void Change()
    {
        ChangeColor(new Color(1, 1, 0, 1));
        SetEnable(true);
        EmitSignal(EnumInfoCardSignals.Change);
    }

    void SetEnable(bool Switch)
    {
        Name.enabled = Switch;
        HINT.enabled = Switch;        
    }

    void ChangeColor(Color Color)
    {
        NameImage.color = Color;
        HINTImage.color = Color;
    }

    public void Delete()
    {
        EmitSignal(EnumInfoCardSignals.Delete);
    }

    public void Close()
    {
        ChangeColor(Color.white);
        SetEnable(false);
        EmitSignal(EnumInfoCardSignals.Close);
    }
}