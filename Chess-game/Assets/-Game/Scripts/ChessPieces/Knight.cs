using UnityEngine;

public class Knight : ChessPiece
{
    public Knight(string color, int x, int y, bool activated) : base(color, x, y, activated) { }

    public override bool CanMove(int targetX, int targetY, BoardManager board)
    {
        return false;
    }
}
