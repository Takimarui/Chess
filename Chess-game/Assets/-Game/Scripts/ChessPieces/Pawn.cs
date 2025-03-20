using UnityEngine;

public class Pawn : ChessPiece
{
    public bool IsFirstMove { get; private set; } = true;

    public Pawn(string color, int x, int y, bool activated) : base(color, x, y, activated) { }

    public override bool CanMove(int targetX, int targetY, BoardManager board)
    {
        int direction = Color == "White" ? -1 : 1;

        if (IsFirstMove)
        {
            if (targetX == X && targetY == Y + 2 * direction)
            {
                if (board.IsCellEmpty(targetX, targetY) && board.IsCellEmpty(X, Y + direction))
                {
                    IsFirstMove = false;
                    return true;
                }
            }
        }
        
        if (targetX == X && targetY == Y + direction)
        {
            if (board.IsCellEmpty(targetX, targetY))
            {
                IsFirstMove = false;
                return true;
            }
            
        }

        if ((targetX == X + 1 || targetX == X - 1) && targetY == Y + direction)
        {
            return !board.IsCellEmpty(targetX, targetY) && board.GetPieceColor(targetX, targetY) != Color;
        }

        return false;
    }
}
