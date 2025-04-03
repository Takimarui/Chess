using System.Linq.Expressions;
using UnityEngine;

public class King : ChessPiece
{
    public bool IsFirstMove { get; private set; } = true;
    public King(string color, int x, int y, bool activated) : base(color, x, y, activated) { }

    public override bool CanMove(int targetX, int targetY, BoardManager board)
    {
        if (IsFirstMove && targetY == Y && (targetX == X + 2 || targetX == X - 2))
        {
            int rookX = (targetX > X) ? X + 3 : X - 4;

            ChessPiece rook = board.GetPieceAt(rookX, Y);

            if (rook is Rook && Color ==  rook.Color && ((Rook)rook).IsFirstMove)
            {
                if (board.IsPathClear(X, Y, rookX, targetY))
                {
                    IsFirstMove = false;
                    return true;
                }
            }
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