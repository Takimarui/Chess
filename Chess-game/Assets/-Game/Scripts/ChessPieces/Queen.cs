using UnityEngine;

public class Queen : ChessPiece
{
    public Queen(string color, int x, int y, bool activated) : base(color, x, y, activated) { }

    public override bool CanMove(int targetX, int targetY, BoardManager board)
    {
        return false;
    }
}
