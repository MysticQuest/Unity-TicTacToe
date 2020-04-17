using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{

    private enum DifficultyOption
    {
        Easy, Hard
    }
    [Header("Difficulty Settings")]
    [SerializeField] private DifficultyOption difficulty;
    private GameHandler gameHandler;

    private void Awake()
    {
        gameHandler = GetComponent<GameHandler>();
    }

    public int SetMove(string[] ticksArray)
    {
        if (difficulty == 0)
        {
            return PickRandomMove(ticksArray);
        }
        else
        {
            return PickNextMove(Populate2DArray(ticksArray));
        }
    }

    private int PickRandomMove(string[] ticksArray)
    {
        List<int> possibleMoves = new List<int>();
        for (int i = 0; i < ticksArray.Length; i++)
        {
            if (string.IsNullOrEmpty(ticksArray[i]))
            {
                possibleMoves.Add(i);
            }
        }
        int randomIndex = possibleMoves[Random.Range(0, possibleMoves.Count - 1)];
        return randomIndex;
    }

    private string[,] Populate2DArray(string[] ticksArray)
    {
        string[,] ticksArray2D = new string[3, 3];
        int objIndex = 0;
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                ticksArray2D[row, col] = ticksArray[objIndex];
                objIndex++;
            }
        }
        return ticksArray2D;
    }

    private int PickNextMove(string[,] ticksArray2D)
    {
        float bestScore = -Mathf.Infinity;
        int bestMove = 0;
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (string.IsNullOrEmpty(ticksArray2D[row, col]))
                {
                    ticksArray2D[row, col] = "o";
                    float score = minimax(ticksArray2D, 0, true);
                    Debug.Log(score); //score given to next "o" simulations
                    ticksArray2D[row, col] = "";
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = (row * 3) + col;
                    }
                }
            }
        }
        // Debug.Log(bestMove);
        return bestMove;
    }

    enum scores
    {
        X = 1,
        O = -1,
        draw = 0
    };

    private float minimax(string[,] ticksArray2D, int depth, bool isMaximizing)
    {

        // return 1;

        if (isMaximizing)
        {
            float bestScore = -Mathf.Infinity;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (string.IsNullOrEmpty(ticksArray2D[row, col]))
                    {
                        ticksArray2D[row, col] = "o";
                        float score = minimax(ticksArray2D, depth + 1, false);
                        ticksArray2D[row, col] = "";
                        if (score > bestScore)
                        {
                            bestScore = score;
                        }
                    }
                }
            }
            return bestScore;
        }
        else
        {
            float bestScore = Mathf.Infinity;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (string.IsNullOrEmpty(ticksArray2D[row, col]))
                    {
                        ticksArray2D[row, col] = "x";
                        float score = minimax(ticksArray2D, depth + 1, true);
                        ticksArray2D[row, col] = "";
                        if (score < bestScore)
                        {
                            bestScore = score;
                        }
                    }
                }
            }
            return bestScore;
        }
    }
}
