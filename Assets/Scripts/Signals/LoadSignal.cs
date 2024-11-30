using System.Collections.Generic;

public class LoadSignal
{
    public readonly List<PinSetupStruct> PinList;

    public LoadSignal(List<PinSetupStruct> PinList)
    {
        this.PinList = new List<PinSetupStruct>(PinList);
    }
}