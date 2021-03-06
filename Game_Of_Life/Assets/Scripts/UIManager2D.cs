using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// manages UI for the 2D Game of Life
// connects UI elements to Game Manager
public class UIManager2D : MonoBehaviour
{
    public GameManager2D gameManager;

    public UnityEngine.UI.Button startStopButton;
    public UnityEngine.UI.Slider simulationSpeedSlider;
    public UnityEngine.UI.Slider randomSparcitySlider;

    public UnityEngine.UI.Toggle mirrorGridToggle;

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

    public void clearScreen()
    {
        gameManager.clearScreen();
    }

    public void randomizeScreen()
    {
        gameManager.randomizeScreen();
    }

    public void setFrameSpeed()
    {
        gameManager.setFrameSpeed(simulationSpeedSlider.value);
    }

    public void setRandomSparcity(float sparcity)
    {
        gameManager.setRandomSparcity(randomSparcitySlider.value);
    }

    public void setMirrorGrid()
    {
        gameManager.setMirrorGrid(mirrorGridToggle.isOn);
    }
}