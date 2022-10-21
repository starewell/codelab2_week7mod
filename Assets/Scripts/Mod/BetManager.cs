using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BetManager : MonoBehaviour
{   
    [Header("Wager Panel UI")]
    public TMP_Text cashText;
    public GameObject wagerPanel;
    public TMP_Text wagerText;
    public GameObject payoutPanel;
    public TMP_Text[] payoutText;

    [Header("Bet Panel UI")]
    public TMP_Text betTitle;
    public Transform buttonLayout;
    public GameObject closeBetButton;

    [Header("Wager Vars")]
    public int currentCash;
    public int currentBet = 0;
    public int[] payout = { 0, 0, 0, 0 };
    public int choosenPrincess;

    public void Start() {
        DefaultWager();
        ToggleBetPanel(1);
        wagerPanel.SetActive(true);
        cashText.text = "$" + currentCash;
    }

    //Initialize wager panel, zero out wager and reset payout and bet buttons
    public void DefaultWager() {
        for (int i = buttonLayout.childCount - 1; i >= 0; i--)
            buttonLayout.GetChild(i).GetComponent<ButtonSelect>().ToggleSelectBox(false);
        choosenPrincess = 0;
        currentBet = 0;
        ChangeWager(0);
        payoutText[0].transform.parent.GetComponent<Image>().color = Color.white;
        payoutText[1].transform.parent.GetComponent<Image>().color = Color.white;
        payoutText[2].transform.parent.GetComponent<Image>().color = Color.white;
    }

    // Called from scene buttons
    // Append wager by value, calculate payout and update UI
    public void ChangeWager(int amount) {
        // Player has that much cash and wager is greater than 0
        if (currentCash >= currentBet + amount
            && currentBet + amount >= 0) { 
            currentBet += amount;
            wagerText.text = "$" + currentBet;
        }
        payout = new int[] { 0, 0, 0, 0 };
        payout[0] = (int)(currentBet * 2.5);
        payoutText[0].text = "$" + payout[0];
        payout[1] = (int)(currentBet * 1.5f);
        payoutText[1].text = "$" + payout[1];
        payout[2] = (int)(currentBet * 0.5f);
        payoutText[2].text = "$" + payout[2];
    }

    // Called from scene buttons
    public void ChoosePrincess(int number) {
        choosenPrincess = number;
    }

    //Called from GameManager when bets close, take money from player if betting
    public void PlaceBet() {
        ToggleBetPanel(1); // Close bets, stop player from changing wager
        if (choosenPrincess != 0) { 
            currentCash -= currentBet;
            cashText.text = "$" + currentCash;
        } else { // If the player did not bet on a princess
            payoutPanel.SetActive(false);
            payout[0] = 0; payout[1] = 0; payout[2] = 0;
        }
    }

    // Award the player the payout corresponding to choosenPrincess' place
    public int Payout(int place) {
        if (choosenPrincess != 0) { 
            currentCash += payout[place];
            cashText.text = "$" + currentCash;
            return payout[place];
        } else return 0;
    }

    // Update UI by index, bets open, closed, and payout
    public void ToggleBetPanel(int index, int place = 0) {
        if (index == 0) {
            betTitle.text = "PLACE YOUR BET!";
            //Show betting buttons
            closeBetButton.SetActive(true);
            for (int i = buttonLayout.childCount - 1; i >= 0; i--) 
                buttonLayout.GetChild(i).gameObject.SetActive(true);
            
        } else if (index == 1) {
            betTitle.text = "BETS CLOSED.";
            // Hide wager panels
            wagerPanel.SetActive(false);
            closeBetButton.SetActive(false);
            // Hide all princess buttons but choosenPrincess
            for (int i = buttonLayout.childCount - 1; i >= 0; i--) {
                if (i != choosenPrincess - 1)
                    buttonLayout.GetChild(i).gameObject.SetActive(false);
            }       
        } else if (index == 2) {
            betTitle.text = "RACE FINISHED. YOU EARNED $" + Payout(place - 1) + ".";
            //Reset wager panels
            wagerPanel.SetActive(true);
            payoutPanel.SetActive(true);
            DefaultWager();
            //Hide betting buttons
            for (int i = buttonLayout.childCount - 1; i >= 0; i--)
                buttonLayout.GetChild(i).gameObject.SetActive(false);
        }
    }
}
