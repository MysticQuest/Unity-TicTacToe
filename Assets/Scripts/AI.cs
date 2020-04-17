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

    public int SetMove(string[] ticksArray)
    {
        if (difficulty == 0)
        {
            return PickRandomMove(ticksArray);
        }
        else
        {
            string[,] ticksArray2D = Populate2DArray(ticksArray);
            return PickGoodMove(ticksArray2D);
        }
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

    private int PickGoodMove(string[,] ticksArray2D)
    {
        float bestScore = -Mathf.Infinity;
        int bestMove;
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (string.IsNullOrEmpty(ticksArray2D[row, col]))
                {
                    ticksArray2D[row, col] = "o";
                    int score = minimax(ticksArray2D);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        return bestMove = (row * 3) + col;
                    }
                }
            }
        }
        return bestMove;
    }

    private int minimax(string[,] array)
    {
        return 1;
    }
}
