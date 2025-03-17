using UnityEngine;

public class Bishop : ChessPiece
{
    public Bishop(string color, int x, int y) : base(color, x, y) { }

    public override bool CanMove(int targetX, int targetY)
    {
        return false;
    }
}
