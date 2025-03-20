using UnityEngine;

public class Knight : ChessPiece
{
    public Knight(string color, int x, int y, bool activated) : base(color, x, y, activated) { }

    public override bool CanMove(int targetX, int targetY, BoardManager board)
    {
        int dx = Mathf.Abs(targetX - X);
        int dy = Mathf.Abs(targetY - Y);

        if ((dx == 2 && dy == 1) || (dx == 1 && dy == 2))
        {
            return board.IsCellEmpty(targetX, targetY) || board.GetPieceColor(targetX, targetY) != Color;
        }

        return false;
    }
}
