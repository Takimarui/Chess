using UnityEngine;

public class Pawn : ChessPiece
{
    public Pawn(string color, int x, int y) : base(color, x, y) { }

    public override bool CanMove(int targetX, int targetY)
    {
        return false;
    }
}
