using UnityEngine;

public class Rook : ChessPiece
{
    public Rook(string color, int x, int y, bool activated) : base(color, x, y, activated) { }

    public override bool CanMove(int targetX, int targetY, BoardManager board)
    {
        if (targetX == X || targetY == Y)
        { 
            if (board.IsPathClear(X, Y, targetX, targetY))
            {
                return board.IsCellEmpty(targetX, targetY) || board.GetPieceColor(targetX, targetY) != Color;
            }
        }
        
        return false;
    }
}
