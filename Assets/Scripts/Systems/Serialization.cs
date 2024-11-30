using System.Collections.Generic;
    
[System.Serializable]
public class Serialization<T>
{
    public List<T> Pins;

    public Serialization(Dictionary<string, T> dictionary)
    {
        Pins = new List<T>(dictionary.Values);
    }

    public Dictionary<string, T> ToDictionary()
    {
        Dictionary<string, T> dictionary = new Dictionary<string, T>();
        foreach (var Item in Pins)
        {
            var Pin = (PinSetupStruct)(object)Item;
            dictionary[Pin.ID.ToString()] = Item;
        }
        return dictionary;
    }
}