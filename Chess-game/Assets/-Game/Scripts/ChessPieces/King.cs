using UnityEngine;

public class King : ChessPiece
{
    public King(string color, int x, int y) : base(color, x, y) { }

    public override bool CanMove(int targetX, int targetY)
    {
        return false;
    }
}
