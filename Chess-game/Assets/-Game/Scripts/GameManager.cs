using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsWhiteTurn { get; private set; } = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void NextTurn()
    {
        IsWhiteTurn = !IsWhiteTurn;
    }

}
