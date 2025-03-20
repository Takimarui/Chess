using System.Linq.Expressions;
using UnityEngine;

public class King : ChessPiece
{
    public bool IsFirstMove { get; private set; } = true;
    public King(string color, int x, int y, bool activated) : base(color, x, y, activated) { }

    public override bool CanMove(int targetX, int targetY, BoardManager board)
    {
        if (IsFirstMove)
        {
            //castling
        }

        if ((targetX == X + 1 || targetX == X - 1 || targetX == X) &&
            (targetY == Y + 1 || targetY == Y - 1 || targetY == Y))
        {
            if (board.IsCellEmpty(targetX, targetY) || board.GetPieceColor(targetX, targetY) != Color)
            {
                IsFirstMove = false;
                return true;
            }
        }

        return false;
    }
}
