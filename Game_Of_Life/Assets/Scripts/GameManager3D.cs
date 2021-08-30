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
    public static int depsCount = 10;

    public Color activeElementColor;
    public Color deactiveElementColor;
    public Shader activeElementShader;
    public Shader deactiveElementShader;
    public Vector3 activeElementScale = new Vector3(0.9f, 0.9f, 0.9f);
    public Vector3 deactiveElementScale = new Vector3(0.1f, 0.1f, 0.1f);

    public float frameSpeed = 0.5f;
    private float gameFrameStart;

    public bool mirrorTheMatrix = false;
    public GameObject parentTransform;
    public GameObject sampleElement;
    public Element[,,] elements;
    public Camera camera = new Camera();

    private bool runState = false;

    // Start is called before the first frame update
    void Start()
    {
        gameFrameStart = Time.time + frameSpeed;
        elements = new Element[rowsCount, colsCount, depsCount];

        for (int i = 0; i < rowsCount; i++)
        {
            for (int j = 0; j < colsCount; j++)
            {
                for (int k = 0; k < depsCount; k++)
                {
                    elements[i, j, k].value = false;
                    elements[i, j, k].gameObject = GameObject.Instantiate(sampleElement);
                    elements[i, j, k].gameObject.GetComponent<Renderer>().material = sampleElement.GetComponent<Renderer>().material;
                    elements[i, j, k].gameObject.transform.parent = parentTransform.transform;
                    elements[i, j, k].gameObject.transform.position = new Vector3(i, j, k);
                    setRenderer(i, j, k);
                    var scale = elements[i, j, k].value ? activeElementScale : deactiveElementScale;
                    elements[i, j, k].gameObject.transform.localScale = scale;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        setElementOnClick();
        if (Time.fixedTime >= gameFrameStart && runState)
        {
            updateGameOfLifeMatrixValues();
            gameFrameStart = Time.fixedTime + frameSpeed;
        }
    }

    private void updateGameOfLifeMatrixValues()
    {
        var newMatrix = new bool[rowsCount, colsCount, depsCount];
        for (int i = 0; i < rowsCount; i++)
        {
            for (int j = 0; j < colsCount; j++)
            {
                for (int k = 0; k < depsCount; k++)
                {
                    newMatrix[i, j, k] = gameRule(i, j, k);
                }
            }
        }
        for (int i = 0; i < rowsCount; i++)
        {
            for (int j = 0; j < colsCount; j++)
            {
                for (int k = 0; k < depsCount; k++)
                {
                    elements[i, j, k].value = newMatrix[i, j, k];
                    setRenderer(i, j, k);
                    var scale = newMatrix[i, j, k] ? activeElementScale : deactiveElementScale;
                    elements[i, j, k].gameObject.transform.localScale = scale;
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
                    localDep = (dep + k) < 0 ? depsCount + k : (dep + k >= depsCount) ? 0 : dep + k;
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
                    sum += (row + i >= 0) && (row + i < rowsCount) && (col + j >= 0) && (col + j < colsCount) && (dep + k >= 0) && (dep + k < depsCount) && elements[row + i, col + j, dep + k].value ? 1 : 0;
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

    private void toggle(int row, int col, int dep)
    {
        elements[row, col, dep].value = !elements[row, col, dep].value;
        setRenderer(row, col, dep);
        var scale = elements[row, col, dep].value ? activeElementScale : deactiveElementScale;
        elements[row, col, dep].gameObject.transform.localScale = scale;
    }

    private void setRenderer(int row, int col, int dep)
    {
        elements[row, col, dep].gameObject.GetComponent<Renderer>().material = sampleElement.GetComponent<Renderer>().material;
        if (elements[row, col, dep].value)
        {
            elements[row, col, dep].gameObject.GetComponent<Renderer>().enabled = true;
            activeElementColor.a = (float)((colsCount - col) + 1) / (float)colsCount;
            elements[row, col, dep].gameObject.GetComponent<Renderer>().material.color = activeElementColor;
            //elements[row, col, dep].gameObject.GetComponent<Renderer>().material.color = activeElementColor;
            //elements[row, col, dep].gameObject.GetComponent<Renderer>().material.shader = activeElementShader;
        }
        else
        {
            elements[row, col, dep].gameObject.GetComponent<Renderer>().enabled = false;
            //elements[row, col, dep].gameObject.GetComponent<Renderer>().material.color = deactiveElementColor;
            //elements[row, col, dep].gameObject.GetComponent<Renderer>().material.shader = deactiveElementShader;
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
                for (int k = 0; k < depsCount; k++)
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
                for (int k = 0; k < depsCount; k++)
                {
                    if (Random.Range(0, 2) == 1)
                    {
                        elements[i, j, k].value = true;
                        elements[i, j, k].gameObject.transform.localScale = activeElementScale;
                    }
                    else
                    {
                        elements[i, j, k].value = false;
                        elements[i, j, k].gameObject.transform.localScale = deactiveElementScale;
                    }
                    setRenderer(i, j, k);
                }
            }
        }
    }

    public void setFrameSpeed(float speed) {
        frameSpeed = speed;
    }
}
