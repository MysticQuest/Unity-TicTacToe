// using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    private AI aiHandler;
    [Header("Sprite Settings")]
    [SerializeField] private Sprite[] xoSprites = new Sprite[] { null, null };
    [SerializeField] private float restartDelay = 2f;
    [SerializeField] private float aiMinDelay = 0.5f;
    [SerializeField] private float aiMaxDelay = 1.5f;

#pragma warning disable 0649
    [SerializeField] private Button[] buttonArray;
#pragma warning restore 0649

    private string[] ticksArray = new string[9];
    private string xTick = "x";
    private string oTick = "o";
    private int emptyScaces = 9;

    private Text msgText;
    private Text scoreXText;
    private Text scoreOText;
    private Image[] crossings;

    private int xScore;
    private int oScore;

    private bool playersTurn;
    [HideInInspector]
    public bool playerFirst;
    private bool gameEnded = false;

    private int crossIndex;
    [HideInInspector]
    public int turnCount;

    private void Awake()
    {
        aiHandler = GetComponent<AI>();

        msgText = GameObject.Find("MsgText").GetComponent<Text>();
        scoreXText = GameObject.Find("XText").GetComponent<Text>();
        scoreOText = GameObject.Find("OText").GetComponent<Text>();
        crossings = GameObject.Find("CrossingLine").GetComponentsInChildren<Image>();
    }

    private void Start()
    {
        DeactivateAllSpaces();
        whoPlaysFirst();
    }

    private void whoPlaysFirst()
    {
        playersTurn = (Random.Range(0, 2) == 0);
        playerFirst = playersTurn;
        if (playersTurn)
        {
            PlayersTurn();
        }
        else
        {
            StartCoroutine(aiTurn());
        }
    }

    private void PlayersTurn()
    {
        msgText.text = "Your turn!";
        ActivateFreeSpaces();
    }

    private IEnumerator aiTurn()
    {
        DeactivateAllSpaces();
        msgText.text = "Computer's turn..";
        yield return new WaitForSeconds(Random.Range(aiMinDelay, aiMaxDelay));
        Tick(aiHandler.SetMove(ticksArray));
    }

    //button function
    public void Tick(int i)
    {
        //pick sprite and register tick
        turnCount++;
        buttonArray[i].image.sprite = xoSprites[System.Convert.ToInt32(playersTurn)];
        buttonArray[i].interactable = false;
        if (playersTurn) { ticksArray[i] = xTick; } else { ticksArray[i] = oTick; };
        //

        ApplyEndConditions(CheckEndConditions(), crossIndex);

        SwitchPlayers();
    }

    private void SwitchPlayers()
    {
        if (playersTurn && !gameEnded)
        {
            playersTurn = false;
            StartCoroutine(aiTurn());
        }
        else if (!playersTurn && !gameEnded)
        {
            playersTurn = true;
            PlayersTurn();
        }
    }

    public int? CheckEndConditions()
    {
        emptyScaces--;

        string[] playerWin = { xTick, xTick, xTick };
        string[] playerLoss = { oTick, oTick, oTick };

        string[] h1 = { ticksArray[0], ticksArray[1], ticksArray[2] };
        string[] h2 = { ticksArray[3], ticksArray[4], ticksArray[5] };
        string[] h3 = { ticksArray[6], ticksArray[7], ticksArray[8] };
        string[] v1 = { ticksArray[0], ticksArray[3], ticksArray[6] };
        string[] v2 = { ticksArray[1], ticksArray[4], ticksArray[7] };
        string[] v3 = { ticksArray[2], ticksArray[5], ticksArray[8] };
        string[] d1 = { ticksArray[0], ticksArray[4], ticksArray[8] };
        string[] d2 = { ticksArray[2], ticksArray[4], ticksArray[6] };

        string[][] winCombos = { h1, h2, h3, v1, v2, v3, d1, d2 };

        for (int i = 0; i < winCombos.Length; i++)
        {
            if (playerWin.SequenceEqual(winCombos[i]) && !gameEnded)
            {
                crossIndex = i;
                return 1;
            }
            else if (playerLoss.SequenceEqual(winCombos[i]) && !gameEnded)
            {
                crossIndex = i;
                return -1;
            }
        }
        //Draw
        if (emptyScaces == 0)
        {
            return 0;
        }
        return null;
    }

    private void ApplyEndConditions(int? result, int crossIndex)
    {
        if (result == 1)
        {
            crossings[crossIndex].enabled = true;
            SetWinDisplay();
        }
        else if (result == -1)
        {
            crossings[crossIndex].enabled = true;
            SetLossDisplay();
        }
        else if (result == 0)
        {
            msgText.text = "Draw";
        }
        else { return; }
        StartCoroutine(EndSequence(crossIndex));
    }

    private void SetWinDisplay()
    {
        msgText.text = "You win!";
        xScore++;
        scoreXText.text = xScore.ToString();
    }

    private void SetLossDisplay()
    {
        msgText.text = "You lost!";
        oScore++;
        scoreOText.text = oScore.ToString();
    }

    private IEnumerator EndSequence(int index)
    {
        DeactivateAllSpaces();
        gameEnded = true;
        yield return new WaitForSeconds(restartDelay);
        if (crossings[index].enabled) crossings[index].enabled = false;
        RoundReset();
    }

    private void RoundReset()
    {
        emptyScaces = 9;
        turnCount = 0;
        for (int i = 0; i < buttonArray.Length; i++)
        {
            buttonArray[i].GetComponent<Image>().sprite = null;
            ticksArray[i] = "";
        }
        gameEnded = false;
        whoPlaysFirst();
    }

    private void DeactivateAllSpaces()
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            buttonArray[i].interactable = false;
        }
    }

    private void ActivateFreeSpaces()
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            if (string.IsNullOrEmpty(ticksArray[i]))
            {
                buttonArray[i].interactable = true;
            }
        }
    }

}
