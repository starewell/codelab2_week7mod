using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Class to update serialized scene UI
public class TimerEntry : MonoBehaviour
{

    public TMP_Text timeText;
    public TMP_Text placeText;
    public TMP_Text princessText;

    public void UpdateTimeText(string value) {
        timeText.text = value;
    }
    public void UpdatePlaceText(string value) {
        placeText.text = value;
    }
    public void UpdatePrincessText(string value) {
        princessText.text = value;
    }

    public void ToggleLog(bool state) {
        placeText.transform.parent.gameObject.SetActive(state);
        princessText.transform.parent.gameObject.SetActive(state);
    }

}
