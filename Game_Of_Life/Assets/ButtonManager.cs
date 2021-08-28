using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    public GameManager2D gameManager;

    public UnityEngine.UI.Button startStopButton;
    private string startString = "Start";
    private string stopString = "Stop";
    public UnityEngine.UI.Button clearButton;
    public UnityEngine.UI.Button randomButton;

    void Start()
    {
        startStopButton.GetComponentInChildren<TextMeshProUGUI>().text = startString;
        gameManager.setRunState(false);
    }

    public void toggleState()
    {
        bool newState = startStopButton.GetComponentInChildren<TextMeshProUGUI>().text == startString;
        startStopButton.GetComponentInChildren<TextMeshProUGUI>().text = newState ? stopString : startString;
        gameManager.setRunState(newState);
    }

}
