using UnityEngine;

public class Rook : ChessPiece
{
    public bool IsFirstMove { get; private set; } = true;
    public Rook(string color, int x, int y, bool activated) : base(color, x, y, activated) { }

    public override bool CanMove(int targetX, int targetY, BoardManager board)
    {
        if (targetX == X || targetY == Y)
        { 
            if (board.IsPathClear(X, Y, targetX, targetY))
            {
                IsFirstMove = false;
                return board.IsCellEmpty(targetX, targetY) || board.GetPieceColor(targetX, targetY) != Color;
            }
        }
        
        return false;
    }
}
