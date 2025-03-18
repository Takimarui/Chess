using UnityEngine;

public class King : ChessPiece
{
    public King(string color, int x, int y, bool activated) : base(color, x, y, activated) { }

    public override bool CanMove(int targetX, int targetY, BoardManager board)
    {
        return false;
    }
}
