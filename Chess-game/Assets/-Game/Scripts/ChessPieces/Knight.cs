using UnityEngine;

public class Knight : ChessPiece
{
    public Knight(string color, int x, int y) : base(color, x, y) { }

    public override bool CanMove(int targetX, int targetY)
    {
        return false;
    }
}
