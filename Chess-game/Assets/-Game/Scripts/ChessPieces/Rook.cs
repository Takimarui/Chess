using UnityEngine;

public class Rook : ChessPiece
{
    public Rook(string color, int x, int y) : base(color, x, y) { }

    public override bool CanMove(int targetX, int targetY)
    {
        return false;
    }
}
