using UnityEngine;

public class Queen : ChessPiece
{
    public Queen(string color, int x, int y, bool activated) : base(color, x, y, activated) { }

    public override bool CanMove(int targetX, int targetY, BoardManager board)
    {
        if (targetX == X || targetY == Y || Mathf.Abs(X - targetX) == Mathf.Abs(Y - targetY))
        {
            if (board.IsPathClear(X, Y, targetX, targetY))
            {
                return board.IsCellEmpty(targetX, targetY) || board.GetPieceColor(targetX, targetY) != Color;
            }
        }
        return false;
    }
}
