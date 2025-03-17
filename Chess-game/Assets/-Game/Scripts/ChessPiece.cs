using UnityEngine;

public abstract class ChessPiece
{
    public string Color { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public ChessPiece(string color, int x, int y)
    {
        Color = color;
        X = x;
        Y = y;
    }

    public abstract bool CanMove(int targetX, int targetY);
}
