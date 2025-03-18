using UnityEngine;

public class Pawn : ChessPiece
{
    public bool IsFirstMove { get; private set; } = true;

    public Pawn(string color, int x, int y, bool activated) : base(color, x, y, activated) { }

    public override bool CanMove(int targetX, int targetY, BoardManager board)
    {
        int direction = Color == "White" ? 1 : -1;

        if (IsFirstMove)
        {
            if (targetX == X + 2 * direction && targetY == Y)
            {
                if (board.IsCellEmpty(targetX, targetY) && board.IsCellEmpty(X + direction, Y))
                {
                    IsFirstMove = false;
                    return true;
                }
            }
        }

        if (targetX == X + direction && targetY == Y)
        {
            return board.IsCellEmpty(targetX, targetY);
        }

        if (targetX == X + direction && (targetY == Y + 1 || targetY == Y - 1))
        {
            return !board.IsCellEmpty(targetX, targetY) && board.GetPieceColor(targetX, targetY) != Color;
        }

        return true;
    }
}
