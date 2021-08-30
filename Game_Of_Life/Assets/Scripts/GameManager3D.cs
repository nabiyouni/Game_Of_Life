using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager3D : MonoBehaviour
{
    public struct Element
    {
        public bool value { get; set; }
        public GameObject gameObject { get; set; }
    }

    public static int rowsCount = 40;
    public static int colsCount = 20;
    public static int depthsCount = 10;

    public Color activeElementColor;
    public Color deactiveElementColor;
    private float colorAlphaOffset = 0.5f;

    public Shader activeElementShader;
    public Shader deactiveElementShader;

    private float frameSpeed = 0.5f;
    private float gameFrameStart;
    private float randomSparcity = 0.5f;

    public bool mirrorTheMatrix = false;
    public GameObject parentTransform;
    public GameObject sampleElement;
    public Element[,,] elements;
    public Camera camera;

    private bool runState = false;

    private int lastVisibleVerticalLayer;
    private int lastVisibleHorizontalLayer;
    private int lastVisibleDepthLayer;

    public FrameViewController frameViewController;

    // Start is called before the first frame update
    void Start()
    {
        frameViewController = this.GetComponent<FrameViewController>();
        gameFrameStart = Time.time + frameSpeed;
        elements = new Element[rowsCount, colsCount, depthsCount];

        for (int i = 0; i < rowsCount; i++)
        {
            for (int j = 0; j < colsCount; j++)
            {
                for (int k = 0; k < depthsCount; k++)
                {
                    elements[i, j, k].value = false;
                    sampleElement.SetActive(true);
                    elements[i, j, k].gameObject = GameObject.Instantiate(sampleElement);
                    elements[i, j, k].gameObject.GetComponent<Renderer>().material = sampleElement.GetComponent<Renderer>().material;
                    elements[i, j, k].gameObject.transform.parent = parentTransform.transform;
                    elements[i, j, k].gameObject.transform.position = new Vector3(i, j, k);
                    setRenderer(i, j, k);
                }
            }
        }

        lastVisibleDepthLayer = depthsCount - 1;
        lastVisibleVerticalLayer = rowsCount - 1;
        lastVisibleHorizontalLayer = colsCount - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.fixedTime >= gameFrameStart && runState)
        {
            updateGameOfLifeMatrixValues();
            gameFrameStart = Time.fixedTime + frameSpeed;
        }
        setElementOnClick();
        setVisibleLayer();
    }

    private void updateGameOfLifeMatrixValues()
    {
        var newMatrix = new bool[rowsCount, colsCount, depthsCount];
        for (int i = 0; i < rowsCount; i++)
        {
            for (int j = 0; j < colsCount; j++)
            {
                for (int k = 0; k < depthsCount; k++)
                {
                    newMatrix[i, j, k] = gameRule(i, j, k);
                }
            }
        }
        for (int i = 0; i < rowsCount; i++)
        {
            for (int j = 0; j < colsCount; j++)
            {
                for (int k = 0; k < depthsCount; k++)
                {
                    elements[i, j, k].value = newMatrix[i, j, k];
                    setRenderer(i, j, k);
                }
            }
        }
    }

    private bool gameRule(int row, int col, int dep)
    {
        int neighborsCount = mirrorTheMatrix ? countNeighborsMirror(row, col, dep) : countNeighbors(row, col, dep);
        if (elements[row, col, dep].value)
        {
            return (neighborsCount >= 5) && (neighborsCount <= 7) ? true : false;
        }
        else
        {
            return neighborsCount == 6 ? true : false;
        }
    }

    private int countNeighborsMirror(int row, int col, int dep)
    {
        int sum = 0;
        int localRow = 0;
        int localCol = 0;
        int localDep = 0;
        for (int i = -1; i < 2; i++)
        {
            localRow = (row + i) < 0 ? rowsCount + i : (row + i >= rowsCount) ? 0 : row + i;
            for (int j = -1; j < 2; j++)
            {
                localCol = (col + j) < 0 ? colsCount + j : (col + j >= colsCount) ? 0 : col + j;
                for (int k = -1; k < 2; k++)
                {
                    localDep = (dep + k) < 0 ? depthsCount + k : (dep + k >= depthsCount) ? 0 : dep + k;
                    sum += elements[localRow, localCol, localDep].value ? 1 : 0;
                }
            }
        }
        sum -= elements[row, col, dep].value ? 1 : 0;
        return sum;
    }

    private int countNeighbors(int row, int col, int dep)
    {
        int sum = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                for (int k = -1; k < 2; k++)
                {
                    sum += (row + i >= 0) && (row + i < rowsCount) && (col + j >= 0) && (col + j < colsCount) && (dep + k >= 0) && (dep + k < depthsCount) && elements[row + i, col + j, dep + k].value ? 1 : 0;
                }
            }
        }
        sum -= elements[row, col, dep].value ? 1 : 0;
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
                var col = (int)hit.transform.position.y;
                var dep = (int)hit.transform.position.z;
                toggle(row, col, dep);
            }
        }
    }

    private void setVisibleLayer()
    {
        int verticalDelta = 0;
        int horizontalDelta = 0;
        int depthDelta = 0;
        bool moveSubFrame = false;

        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && lastVisibleVerticalLayer < rowsCount - 1)
        {
            lastVisibleVerticalLayer++;
            verticalDelta = 1;
            moveSubFrame = true;
        }

        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && lastVisibleVerticalLayer > 1)
        {
            lastVisibleVerticalLayer--;
            verticalDelta = -1;
            moveSubFrame = true;
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && lastVisibleHorizontalLayer < colsCount - 1)
        {
            lastVisibleHorizontalLayer++;
            horizontalDelta = 1;
            moveSubFrame = true;
        }
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && lastVisibleHorizontalLayer > 1)
        {
            lastVisibleHorizontalLayer--;
            horizontalDelta = -1;
            moveSubFrame = true;
        }

        if ((Input.GetKeyDown(KeyCode.PageUp) || Input.GetKeyDown(KeyCode.E)) && lastVisibleDepthLayer < depthsCount - 1)
        {
            lastVisibleDepthLayer++;
            depthDelta = 1;
            moveSubFrame = true;
        }
        if ((Input.GetKeyDown(KeyCode.PageDown) || Input.GetKeyDown(KeyCode.Q
            )) && lastVisibleDepthLayer > 1)
        {
            lastVisibleDepthLayer--;
            depthDelta = -1;
            moveSubFrame = true;
        }

        renderLayers(verticalDelta, horizontalDelta, depthDelta);
        if (moveSubFrame) {
            frameViewController.subFrameViewer(lastVisibleVerticalLayer, lastVisibleHorizontalLayer, lastVisibleDepthLayer);
        }
    }

    private void toggle(int row, int col, int dep)
    {
        elements[row, col, dep].value = !elements[row, col, dep].value;
        setRenderer(row, col, dep);
    }

    private void setRenderer(int row, int col, int dep)
    {
        elements[row, col, dep].gameObject.GetComponent<Renderer>().material = sampleElement.GetComponent<Renderer>().material;
        if (elements[row, col, dep].value)
        {
            elements[row, col, dep].gameObject.GetComponent<Renderer>().enabled = true;

            float r = (float)(rowsCount - row) / (float)rowsCount;
            float g = (float)(colsCount - col) / (float)colsCount;
            float b = (float)(depthsCount - dep) / (float)depthsCount;
            float initialAlpha = (float)((colsCount - col) + 1) / (float)colsCount;
            float scaledAlpha = (initialAlpha * (1 - colorAlphaOffset)) + colorAlphaOffset;
            activeElementColor = new Color(r, g, b, scaledAlpha);
            elements[row, col, dep].gameObject.GetComponent<Renderer>().material.color = activeElementColor;
        }
        else
        {
            elements[row, col, dep].gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    private void renderLayers(int verticalDelta, int horizontalDelta, int depthDelta)
    {
        if (verticalDelta != 0)
        {
            for (int i = 0; i <= lastVisibleHorizontalLayer; i++)
            {
                for (int j = 0; j <= lastVisibleDepthLayer; j++)
                {
                    if (verticalDelta == 1)
                    {
                        elements[lastVisibleVerticalLayer, i, j].gameObject.SetActive(true);
                    }
                    else
                    {
                        elements[lastVisibleVerticalLayer + 1, i, j].gameObject.SetActive(false);
                    }
                }
            }
        }
        else if (horizontalDelta != 0)
        {
            for (int i = 0; i <= lastVisibleVerticalLayer; i++)
            {
                for (int j = 0; j <= lastVisibleDepthLayer; j++)
                {
                    if (horizontalDelta == 1)
                    {
                        elements[i, lastVisibleHorizontalLayer, j].gameObject.SetActive(true);
                    }
                    else
                    {
                        elements[i, lastVisibleHorizontalLayer + 1, j].gameObject.SetActive(false);
                    }
                }
            }
        }
        else if (depthDelta != 0)
        {
            for (int i = 0; i <= lastVisibleVerticalLayer; i++)
            {
                for (int j = 0; j <= lastVisibleHorizontalLayer; j++)
                {
                    if (depthDelta == 1)
                    {
                        elements[i, j, lastVisibleDepthLayer].gameObject.SetActive(true);
                    }
                    else
                    {
                        elements[i, j, lastVisibleDepthLayer + 1].gameObject.SetActive(false);
                    }
                }
            }
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
            for (int j = 0; j < colsCount; j++)
            {
                for (int k = 0; k < depthsCount; k++)
                {
                    elements[i, j, k].value = false;
                    setRenderer(i, j, k);
                }
            }
        }
    }

    public void randomizeScreen()
    {
        for (int i = 0; i < rowsCount; i++)
        {
            for (int j = 0; j < colsCount; j++)
            {
                for (int k = 0; k < depthsCount; k++)
                {
                    elements[i, j, k].value = Random.Range(0f, 1f) < randomSparcity ? true : false;
                    setRenderer(i, j, k);
                }
            }
        }
    }

    public void setFrameSpeed(float speed)
    {
        frameSpeed = speed;
    }

    public void setRandomSparcity(float sparcity)
    {
        randomSparcity = sparcity;
    }

    public void setMirrorGrid(bool mirror)
    {
        mirrorTheMatrix = mirror;
    }
}
