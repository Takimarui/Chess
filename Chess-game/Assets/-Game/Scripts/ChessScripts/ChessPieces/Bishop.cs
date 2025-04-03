using UnityEngine;

public class Bishop : ChessPiece
{
    public Bishop(string color, int x, int y, bool activated) : base(color, x, y, activated) { }

    public override bool CanMove(int targetX, int targetY, BoardManager board)
    {
        if (Mathf.Abs(X - targetX) == Mathf.Abs(Y - targetY))
        {
            if (board.IsPathClear(X, Y, targetX, targetY))
            {
                return board.IsCellEmpty(targetX, targetY) || board.GetPieceColor(targetX, targetY) != Color;
            }
        }

        return false;
    }
}
