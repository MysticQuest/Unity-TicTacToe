﻿// using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    [SerializeField] AI aiHandler;
    [SerializeField] private Sprite[] xoSprites = new Sprite[] { null, null };

    private Button[] buttonArray;

    [SerializeField]
    private string[] ticksArray = new string[8];
    private string xTick = "x";
    private string oTick = "o";
    private int filledBoxes = 0;

    private GameObject gridImage;
    private Text msgText;
    private Text scoreXText;
    private Text scoreOText;
    private int xScore;
    private int oScore;

    [SerializeField] private Image[] crossings;

    [SerializeField] private bool playersTurn;
    private bool gameEnded = false;

    private float aiDelay;
    [SerializeField] float endDelay = 1.5f;

    private void Awake()
    {
        if (!aiHandler) { aiHandler = GameObject.Find("AI_Handler").GetComponent<AI>(); }
        if (!gridImage) { gridImage = GameObject.Find("Grid"); }
        if (!msgText) { msgText = GameObject.Find("MsgText").GetComponent<Text>(); }
        if (!scoreXText) { scoreXText = GameObject.Find("XText").GetComponent<Text>(); }
        if (!scoreOText) { scoreOText = GameObject.Find("OText").GetComponent<Text>(); }

        buttonArray = gridImage.GetComponentsInChildren<Button>();
        crossings = GameObject.Find("CrossingLine").GetComponentsInChildren<Image>();
    }

    private void Start()
    {
        whoPlaysFirst();
    }

    private void whoPlaysFirst()
    {
        playersTurn = (Random.Range(0, 2) == 0);
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
        DeactivateBoard();
        msgText.text = "Computer's turn..";
        aiDelay = Random.Range(0.5f, 1.5f);
        yield return new WaitForSeconds(aiDelay);
        Tick(aiHandler.SetMove(ticksArray));
    }

    //button function
    public void Tick(int i)
    {
        //pick sprite and register tick
        buttonArray[i].image.sprite = xoSprites[System.Convert.ToInt32(playersTurn)];
        buttonArray[i].interactable = false;
        if (playersTurn) { ticksArray[i] = xTick; } else { ticksArray[i] = oTick; };
        filledBoxes++;

        CheckEndConditions(filledBoxes);

        //after tick switch
        if (playersTurn && !gameEnded)
        {
            playersTurn = false;
            StartCoroutine(aiTurn()); // prin ta win conditions
        }
        else if (!playersTurn && !gameEnded)
        {
            playersTurn = true;
            PlayersTurn();
        }
    }

    private void CheckEndConditions(int filledBoxes)
    {
        string[] pWin = { xTick, xTick, xTick };
        string[] pLose = { oTick, oTick, oTick };

        string[] h1 = { ticksArray[0], ticksArray[1], ticksArray[2] };
        string[] h2 = { ticksArray[3], ticksArray[4], ticksArray[5] };
        string[] h3 = { ticksArray[6], ticksArray[7], ticksArray[8] };
        string[] v1 = { ticksArray[0], ticksArray[3], ticksArray[6] };
        string[] v2 = { ticksArray[1], ticksArray[4], ticksArray[7] };
        string[] v3 = { ticksArray[2], ticksArray[5], ticksArray[8] };
        string[] d1 = { ticksArray[0], ticksArray[4], ticksArray[8] };
        string[] d2 = { ticksArray[2], ticksArray[4], ticksArray[6] };

        string[][] winCombos = { h1, h2, h3, v1, v2, v3, d1, d2 };

        bool draw = true;
        for (int i = 0; i < winCombos.Length; i++)
        {
            //Win
            if (pWin.SequenceEqual(winCombos[i]))
            {
                crossings[i].enabled = true;
                SetWinDisplay();
                StartCoroutine(EndSequence(i));
                draw = false;
            }
            //Loss
            else if (pLose.SequenceEqual(winCombos[i]))
            {
                crossings[i].enabled = true;
                SetLossDisplay();
                StartCoroutine(EndSequence(i));
                draw = false;
            }
        }

        //Draw
        if (filledBoxes >= 9 && draw)
        {
            msgText.text = "Draw";
            StartCoroutine(EndSequence(0));
        }
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
        gameEnded = true;
        DeactivateBoard();
        yield return new WaitForSeconds(endDelay);
        if (crossings[index].enabled) crossings[index].enabled = false;
        RoundRestart();
    }

    private void DeactivateBoard()
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

    private void RoundRestart()
    {
        msgText.text = string.Empty;
        filledBoxes = 0;
        for (int i = 0; i < buttonArray.Length; i++)
        {
            buttonArray[i].GetComponent<Image>().sprite = null;
            // buttonArray[i].interactable = true;
            ticksArray[i] = "";
        }
        gameEnded = false;
        whoPlaysFirst();
    }


    private void DisableButtons()
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            buttonArray[i].enabled = false;
        }
    }

    private void EnableButtons()
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            buttonArray[i].enabled = true;
        }
    }

}
