using UnityEngine;

public class Rook : ChessPiece
{
    public Rook(string color, int x, int y, bool activated) : base(color, x, y, activated) { }

    public override bool CanMove(int targetX, int targetY, BoardManager board)
    {
        return false;
    }
}
