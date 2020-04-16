using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    //sprite fields
    [SerializeField] private Sprite[] xoSprites = new Sprite[] { null, null };

    [SerializeField] private Button[] buttonArray;

    //get grid image
    [SerializeField] private GameObject gridImage;
    [SerializeField] private Text msgText;

    [SerializeField] private bool playerTurn;

    private void Awake()
    {
        if (!gridImage)
        {
            gridImage = GameObject.Find("Grid");
        }
        buttonArray = gridImage.GetComponentsInChildren<Button>(true);

        if (!msgText)
        {
            msgText = GameObject.Find("MsgText").GetComponent<Text>();
        }
    }

    private void Start()
    {
        RoundReset();
        whoPlaysFirst();
    }

    private void whoPlaysFirst()
    {
        playerTurn = (Random.Range(0, 2) == 0);
        if (playerTurn)
        {
            PlayersTurn();
        }
        else
        {
            aiTurn();
        }
    }

    private void PlayersTurn()
    {
        msgText.text = "Your turn!";
        Debug.Log("Player's Turn");
    }

    private void aiTurn()
    {
        msgText.text = string.Empty;
        Debug.Log("AI's Turn");
    }

    private void RoundReset()
    {
        msgText.text = string.Empty;
        for (int i = 0; i < buttonArray.Length; i++)
        {
            buttonArray[i].GetComponent<Image>().sprite = null;
            buttonArray[i].interactable = true;
        }
    }

    //button function
    public void Tick(int i)
    {
        buttonArray[i].image.sprite = xoSprites[System.Convert.ToInt32(playerTurn)];
        buttonArray[i].interactable = false;

        // ColorBlock tickedColor = buttonArray[i].GetComponent<Button>().colors;


        if (playerTurn)
        {
            // tickedColor.normalColor = Color.red;
            playerTurn = false;
        }
        else
        {
            // tickedColor.normalColor = Color.green;
            playerTurn = true;
        }

        EndCondition();
    }

    private void EndCondition()
    {

    }
}
