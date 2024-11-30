using UnityEngine;

[System.Serializable]
public struct PinSetupStruct
{
    public int ID;
    public string Name;
    public string HINT;
    public Sprite Sprite;
    public Color Color;
    public float X;
    public float Y;

    public PinSetupStruct(int ID, string Name, string HINT, Sprite Sprite, Color Color, float X, float Y)
    {
        this.ID = ID;
        this.Name = Name;
        this.HINT= HINT;
        this.Sprite = Sprite;
        this.Color = Color;
        this.X = X;
        this.Y = Y;
    }
}