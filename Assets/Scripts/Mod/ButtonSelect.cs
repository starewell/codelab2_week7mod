using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to update serialized scene UI
public class ButtonSelect : MonoBehaviour
{
    public GameObject selectBox;
    public List<ButtonSelect> buttons;

    void Start() {
        ToggleSelectBox(false);
    }

    public void SelectThis() {
        ToggleSelectBox(true);
        foreach (ButtonSelect button in buttons)
            button.ToggleSelectBox(false);
    }

    public void ToggleSelectBox(bool state) {
        selectBox.SetActive(state);
    }
}
