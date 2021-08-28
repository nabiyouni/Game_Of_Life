using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2D : MonoBehaviour
{
    public struct Element
    {
        public bool value { get; set; }
        public GameObject gameObject { get; set; }
    }

    public int rowsCount = 38;
    public int colsCount = 23;

    public Color activeElementColor = Color.white;
    private Material activeElementMaterial;
    private Material deactiveElementMaterial;

    public float gameFrameLatency = 0.5f;
    private float gameFrameStart;

    public bool mirrorTheMatrix = false;
    public GameObject parentTransform;
    public GameObject sampleElement;
    public Element[,] elements;
    public Camera camera = new Camera();

    private bool runState = false;

    // Start is called before the first frame update
    void Start()
    {
        gameFrameStart = Time.time + gameFrameLatency;
        elements = new Element[rowsCount, colsCount];

        for (int i = 0; i < rowsCount; i++)
        {
            for (int k = 0; k < colsCount; k++)
            {
                elements[i, k].gameObject = GameObject.Instantiate(sampleElement);
                elements[i, k].gameObject.GetComponent<Renderer>().material = sampleElement.GetComponent<Renderer>().material;
                elements[i, k].gameObject.transform.parent = parentTransform.transform;
                elements[i, k].gameObject.transform.position = new Vector3(i, 0, k);
                setRenderer(i, k);
            }
        }
    }

    void Update()
    {
        setElementOnClick();
        if (Time.fixedTime >= gameFrameStart && runState)
        {
            updateGameOfLifeMatrixValues();
            gameFrameStart = Time.fixedTime + gameFrameLatency;
        }
    }

    private void updateGameOfLifeMatrixValues()
    {
        var newMatrix = new bool[rowsCount, colsCount];
        for (int i = 0; i < rowsCount; i++)
        {
            for (int k = 0; k < colsCount; k++)
            {
                newMatrix[i, k] = gameRule(i, k);
            }
        }
        for (int i = 0; i < rowsCount; i++)
        {
            for (int k = 0; k < colsCount; k++)
            {
                elements[i, k].value = newMatrix[i, k];
                setRenderer(i, k);
            }
        }
    }

    private bool gameRule(int row, int col)
    {
        int neighborsCount = mirrorTheMatrix ? countNeighborsMirror(row, col) : countNeighbors(row, col);
        if (elements[row, col].value)
        {
            return neighborsCount == 2 || neighborsCount == 3 ? true : false;
        }
        else
        {
            return neighborsCount == 3 ? true : false;
        }
    }

    private int countNeighborsMirror(int row, int col)
    {
        int sum = 0;
        int localRow = 0;
        int localCol = 0;
        for (int i = -1; i < 2; i++)
        {
            localRow = (row + i) < 0 ? rowsCount + i : (row + i >= rowsCount) ? 0 : row + i;
            for (int k = -1; k < 2; k++)
            {
                localCol = (col + k) < 0 ? colsCount + k : (col + k >= colsCount) ? 0 : col + k;
                sum += elements[localRow, localCol].value ? 1 : 0;
            }
        }
        sum -= elements[row, col].value ? 1 : 0;
        return sum;
    }

    private int countNeighbors(int row, int col)
    {
        int sum = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int k = -1; k < 2; k++)
            {
                sum += (row + i >= 0) && (row + i < rowsCount) && (col + k >= 0) && (col + k < colsCount) && elements[row + i, col + k].value ? 1 : 0;
            }
        }
        sum -= elements[row, col].value ? 1 : 0;
        return sum;
    }

    private void setElementOnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var row = (int)hit.transform.position.x;
                var col = (int)hit.transform.position.z;
                toggle(row, col);
            }
        }
    }

    private void toggle(int row, int col)
    {
        elements[row, col].value = !elements[row, col].value;
        setRenderer(row, col);
    }

    private void setRenderer(int row, int col) {
        if (elements[row, col].value)
        {
            elements[row, col].gameObject.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            elements[row, col].gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    public void setRunState(bool state)
    {
        runState = state;
    }

    public void clearScreen()
    {
        for (int i = 0; i < rowsCount; i++)
        {
            for (int k = 0; k < colsCount; k++)
            {
                elements[i, k].value = false;
                setRenderer(i, k);
            }
        }
    }

    public void randomizeScreen()
    {
        for (int i = 0; i < rowsCount; i++)
        {
            for (int k = 0; k < colsCount; k++)
            {
                if (Random.Range(0, 2) == 1)
                {
                    elements[i, k].value = true;
                    setRenderer(i, k);
                }
                else
                {
                    elements[i, k].value = false;
                    setRenderer(i, k);
                }

            }
        }
    }
}
