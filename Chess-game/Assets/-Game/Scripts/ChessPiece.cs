using UnityEngine;

public abstract class ChessPiece
{
    public string Color { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool Activated { get; set; }

    public GameObject pieceObject;

    public ChessPiece(string color, int x, int y, bool activated)
    {
        Color = color;
        X = x;
        Y = y;
        Activated = activated;
    }

    public abstract bool CanMove(int targetX, int targetY, BoardManager board);
}
