using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoHeader : BaseSignal
{
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI ButtonText;
    [SerializeField] Image Icon;

    public void SetUp(PinSetupStruct PinSetupStruct)
    {
        if(string.IsNullOrEmpty(PinSetupStruct.Name))
        {
            ButtonText.text = "Изменить";
            Name.text = "Пусто";
        }
        else
        {
            ButtonText.text = "Читать дальше";
            Name.text = PinSetupStruct.Name;
        }
        
        Icon.sprite = PinSetupStruct.Sprite;
    }

    public void Close()
    {
        EmitSignal(EnumInfoCardSignals.Close);
    }
}