using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {

    [Header("Serialized Class References")]
    public GridScript grid;
    public AStarScript[] princesses;
    public BetManager betManager;

    [Header("Game Management UI Refs")]
    [SerializeField] TMP_Text statusText;
    [SerializeField] GameObject startButton;
    [SerializeField] Transform timerPanel;
    public TimerEntry[] timers;

    [Header("Game Management Vars")]
    [SerializeField] float timer;
    public List<TimerEntry> finishTimes = new List<TimerEntry>();
    [SerializeField] int payoutIndex;
    public bool top3Finished = false;

    private void Start() {
        startButton.SetActive(true);
        statusText.text = "START A NEW RACE";
    }

    // Called from scene button, opens bets, generates grid, initializes princesses and race vars
    public void StartRaceBetting() {
        grid.DestroyGrid();
        ResetLog();
        foreach (AStarScript princess in princesses)
           princess.InitAstar();

        top3Finished = false;
        betManager.ToggleBetPanel(0);
        startButton.SetActive(false);
        StartCoroutine(RaceBetting());
    }

    // Called when bets close, starts princess movement and race coroutine
    public void StartRace() {
        // Start princess movement
        foreach(AStarScript princess in princesses) 
            princess.StartRace();
        // Initialize finishTimes List
        finishTimes = new List<TimerEntry>();
        StartCoroutine(Race());
    }

    // Coroutine runs for 20 seconds while bets are open, updates default timerEntry UI
    IEnumerator RaceBetting() {
        statusText.text = "BETS CLOSE IN:";
        //Start bet timer, stops after 20 seconds
        StartCoroutine(Timer(20));
        while (timer < 20) {
            yield return new WaitForSecondsRealtime(1 / 60f);
            timers[2].UpdateTimeText(FormatTime(20 - timer));
        }
        // Bets close, start race
        betManager.PlaceBet();
        StartRace();
    }

    // Coroutine runs until top 3 finish, updates default timerEntry UI
    IEnumerator Race() {
        statusText.text = "GO!";     
        //Start race timer, stops when top 3 finish
        StartCoroutine(Timer());
        while (!top3Finished) {
            yield return new WaitForSecondsRealtime(1 / 60f);
            timers[2].UpdateTimeText(FormatTime(timer));             
        }
    }

    // Only a coroutine to delay the new race prompt, called from LogFinishTime when all 4 finish
    IEnumerator RaceFinished() {
        yield return new WaitForSecondsRealtime(2);
        statusText.text = "START A NEW RACE";
        betManager.ToggleBetPanel(2, payoutIndex);
        startButton.SetActive(true);
    }

    // Util coroutine, increases timer until specified limit or top 3 finish
    IEnumerator Timer(float limit = Mathf.Infinity) {
        timer = 0;
        while (timer < limit && !top3Finished) {
            yield return null;
            timer += Time.deltaTime;
        }
    }

    // Called from FollowAStarScript when princess reaches the end
    // Stores TimerEntries in List after updating their UI, does not store 4th place entry
    public void LogFinishTime(AStarScript princess) {
        //Store place string for UI
        string place = "";        
        switch(finishTimes.Count) {
            case 0:
                place = "1ST"; break;
            case 1:
                place = "2ND"; break;
            case 2:
                place = "3RD"; break;
            case 3:
                place = "4th"; break;
        }

        //Log finish time if top 3, update UI
        if (finishTimes.Count < 3) {
            timers[finishTimes.Count].gameObject.SetActive(true);
            timers[finishTimes.Count].ToggleLog(true);
            timers[finishTimes.Count].UpdateTimeText(FormatTime(timer));
            timers[finishTimes.Count].UpdatePlaceText(place);
            timers[finishTimes.Count].UpdatePrincessText(princess.princessNo);
            finishTimes.Add(timers[finishTimes.Count]);           
        }  

        //Check after logging if this princess was bet on by the player
        if (princess.princessNo == betManager.choosenPrincess) { 
            // Store place for payout, update payout UI
            payoutIndex = (top3Finished) ? 4 : finishTimes.Count;
            if (payoutIndex != 4)
                betManager.payoutText[finishTimes.Count - 1].transform.parent.GetComponent<Image>().color = Color.green;
        }

        // Check after logging if all 4 finished
        if (top3Finished) StartCoroutine(RaceFinished());
        // Check after logging if top 3 finished
        if (finishTimes.Count == 3) top3Finished = true;

        statusText.text = "PRINCESS " + princess.princessNo + " PLACED " + place;      
    }

    // Hide timerEntries, reset defaultTimer
    public void ResetLog() {
        timers[0].gameObject.SetActive(false);
        timers[1].gameObject.SetActive(false);
        timers[2].ToggleLog(false);
    }

    // pulled from unity answers
    public string FormatTime(float time) {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        int milliseconds = (int)(100 * (time - minutes * 60 - seconds));
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    // Util button function
    public void CloseBets() {
        timer += 20;
    }
}
