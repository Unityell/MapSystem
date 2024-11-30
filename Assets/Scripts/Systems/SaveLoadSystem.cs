using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Zenject;
using System.Linq;

public class SaveLoadSystem : BaseSignal
{
    [Inject] EventBus EventBus;

    private const string SaveFileName = "PinList.json";
    private Dictionary<string, PinSetupStruct> PinList = new Dictionary<string, PinSetupStruct>();

    private void Start()
    {
        Load();
        EventBus.Subscribe(SignalBox);
    }

    void SignalBox(object Obj)
    {
        if (Obj is PinSignal PinSignal)
        {
            switch (PinSignal.Event)
            {
                case EnumPinBorderEvents.Stop:
                    Save(PinSignal.Pin);
                    break;
                case EnumPinBorderEvents.Delete:
                    Delete(PinSignal.Pin);
                    break;
                default: break;
            }
        }
    }

    void Load()
    {
        string FullPath = Path.Combine(Application.persistentDataPath, SaveFileName);

        if (File.Exists(FullPath))
        {
            string Json = File.ReadAllText(FullPath);
            Debug.Log(FullPath);
            var Serialization = JsonUtility.FromJson<Serialization<PinSetupStruct>>(Json);
            PinList.Clear();

            if (Serialization != null && Serialization.Pins != null)
            {
                foreach (var Pin in Serialization.Pins)
                {
                    PinList[Pin.ID.ToString()] = Pin;
                }

                EventBus.Invoke(new LoadSignal(new List<PinSetupStruct>(PinList.Values)));
            }
        }
    }

    void Save(Pin Pin)
    {
        PinList[Pin.PinInfo.ID.ToString()] = Pin.PinInfo;

        List<string> JsonEntries = new List<string>();

        foreach (var Item in PinList.Values)
        {
            string JsonEntry = JsonUtility.ToJson(Item);
            JsonEntries.Add(JsonEntry);
        }

        string FinalJson = "{ \"Pins\": [\n" + string.Join(",\n", JsonEntries) + "\n] }";
        File.WriteAllText(Path.Combine(Application.persistentDataPath, SaveFileName), FinalJson);

        EmitSignal(new Message(EnumSaveLoadState.Save, gameObject, $"Pin ID : {Pin.PinInfo.ID}"));
    }

    void Delete(Pin Pin)
    {
        if (PinList.Remove(Pin.PinInfo.ID.ToString()))
        {
            string FinalJson = "{ \"Pins\": [" + string.Join(",", PinList.Values.Select(Item => JsonUtility.ToJson(Item))) + "] }";
            File.WriteAllText(Path.Combine(Application.persistentDataPath, SaveFileName), FinalJson);

            EmitSignal(new Message(EnumSaveLoadState.Delete, gameObject, $"Pin ID : {Pin.PinInfo.ID}"));
        }
    }

    void OnDestroy() => EventBus.Unsubscribe(SignalBox);
}