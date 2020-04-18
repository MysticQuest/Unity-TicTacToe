using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI : MonoBehaviour
{
#pragma warning disable 0649
    private enum DifficultyOptions
    {
        Random = 0, MinMax = 1, RandomFirstMinMax = 2, RfWMmWFlawChance = 3
    }
    [Header("Difficulty Settings")]
    [SerializeField] private DifficultyOptions difficulty;
    private GameHandler gameHandler;
#pragma warning restore 0649

    private string[,] ticksArray2D = new string[3, 3];

    private string xTick = "x";
    private string oTick = "o";
    private string eTick = "";
    private bool flawed;

    private void Awake()
    {
        gameHandler = GetComponent<GameHandler>();
    }

    public int SetMove(string[] ticksArray)
    {
        flawed = false;

        if (difficulty == 0)
        {
            return PickRandomMove(ticksArray);
        }
        else if ((int)difficulty == 1)
        {
            return PickGoodMove(Populate2DArray(ticksArray));
        }
        else if ((int)difficulty == 2)
        {
            if (gameHandler.turnCount <= 2 || gameHandler.turnCount <= 3 && !gameHandler.playerFirst)
            {
                Debug.Log("Random move");
                return PickRandomMove(ticksArray);
            }
            Debug.Log("Minmaxed move");
            return PickGoodMove(Populate2DArray(ticksArray));
        }
        else
        {
            if (gameHandler.turnCount <= 2 || gameHandler.turnCount <= 3 && !gameHandler.playerFirst)
            {
                Debug.Log("Random move");
                return PickRandomMove(ticksArray);
            }
            flawed = true;
            Debug.Log("ChanceforFlaw move");
            return PickGoodMove(Populate2DArray(ticksArray));
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

    private int PickGoodMove(string[,] ticksArray2D)
    {
        int? bestScore = int.MinValue;
        int bestMove = 0;
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (string.IsNullOrEmpty(ticksArray2D[row, col]))
                {
                    ticksArray2D[row, col] = oTick;
                    int? score = minimax(ticksArray2D, 0, false); //call simulate x
                    ticksArray2D[row, col] = eTick;
                    if (score >= bestScore)
                    {
                        bestScore = score;
                        bestMove = (row * 3) + col;
                    }
                }
            }
        }
        return bestMove;
    }


    int? minimax(string[,] ticksArray2D, int depth, bool isMaximizing)
    {
        int? result = VirtualCheckEndConditions(ticksArray2D);
        // Debug.Log("BEST MOVE SCORE: " + result);
        if (result != null)
        {
            return result;
        }

        if (isMaximizing)
        {
            int? bestScore = int.MinValue;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (string.IsNullOrEmpty(ticksArray2D[row, col]))
                    {
                        ticksArray2D[row, col] = oTick;
                        int? score = minimax(ticksArray2D, depth + 1, false); //call simulate x
                        ticksArray2D[row, col] = eTick;
                        if (score > bestScore)
                        {
                            bestScore = score;
                        }
                    }
                }
            }
            // Debug.Log("MAXIMIZING:" + bestScore);
            return bestScore;
        }
        else
        {
            int? bestScore = int.MaxValue;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (string.IsNullOrEmpty(ticksArray2D[row, col]))
                    {
                        ticksArray2D[row, col] = xTick;
                        int? score = minimax(ticksArray2D, depth + 1, true); //call simulate o
                        ticksArray2D[row, col] = eTick;
                        if (score < bestScore)
                        {
                            bestScore = score;
                        }
                    }
                }
            }
            // Debug.Log("MINIMIZING:" + bestScore);
            return bestScore;
        }
    }

    public int? VirtualCheckEndConditions(string[,] ticksArray2D)
    {
        string[] pcLoss = { xTick, xTick, xTick };
        string[] pcWin = { oTick, oTick, oTick };

        string[] h1 = { ticksArray2D[0, 0], ticksArray2D[0, 1], ticksArray2D[0, 2] };
        string[] h2 = { ticksArray2D[1, 0], ticksArray2D[1, 1], ticksArray2D[1, 2] };
        string[] h3 = { ticksArray2D[2, 0], ticksArray2D[2, 1], ticksArray2D[2, 2] };
        string[] v1 = { ticksArray2D[0, 0], ticksArray2D[1, 0], ticksArray2D[2, 0] };
        string[] v2 = { ticksArray2D[0, 1], ticksArray2D[1, 1], ticksArray2D[2, 1] };
        string[] v3 = { ticksArray2D[0, 2], ticksArray2D[1, 2], ticksArray2D[2, 2] };
        string[] d1 = { ticksArray2D[0, 0], ticksArray2D[1, 1], ticksArray2D[2, 2] };
        string[] d2 = { ticksArray2D[0, 2], ticksArray2D[1, 1], ticksArray2D[2, 0] };

        string[][] winCombos = { h1, h2, h3, v1, v2, v3, d1, d2 };

        for (int i = 0; i < winCombos.Length; i++)
        {
            //Win
            if (pcLoss.SequenceEqual(winCombos[i]))
            {
                return -1;
            }
            //Loss
            else if (pcWin.SequenceEqual(winCombos[i]))
            {
                if (flawed)
                {
                    return Random.Range(0, 2);
                }
                return 1;
            }
        }
        // Draw
        if (FindEmptySpaces() == 0)
        {
            if (flawed)
            {
                return Random.Range(0, 4);
            }
            return 0;
        }

        return null;
    }

    private int FindEmptySpaces()
    {
        int emptySpaces = 0;
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (string.IsNullOrEmpty(ticksArray2D[row, col]))
                {
                    emptySpaces++;
                }
            }
        }
        return emptySpaces;
    }
}
