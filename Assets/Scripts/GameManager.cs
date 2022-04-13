using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text winLossMessage;

    static GameManager gameManager;
    public static GameManager GetGameManager()
    {
        return gameManager;
    }
    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else if(gameManager != this)
        {
            Destroy(this);
        }
        
    }
    public void DisplayWinMessage() 
    {
        winLossMessage.text = "You Win";
        winLossMessage.gameObject.SetActive(true);
    }
    public void DisplayLossMessage()
    {
        winLossMessage.text = "You Lose";
        winLossMessage.gameObject.SetActive(true);
    }
    void HideMessage()
    {
        winLossMessage.gameObject.SetActive(false);
    }
}
