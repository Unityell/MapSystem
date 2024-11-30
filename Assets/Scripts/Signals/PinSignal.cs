public class PinSignal
{
    public readonly Pin Pin;
    public readonly EnumPinBorderEvents Event;

    public PinSignal(Pin Pin, EnumPinBorderEvents Event)
    {
        this.Pin = Pin;
        this.Event = Event;
    }
}