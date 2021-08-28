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
    public Color deactiveElementColor = Color.black;
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

        deactiveElementColor.a = 0.3f;
        activeElementColor.a = 0.5f;

        for (int i = 0; i < rowsCount; i++)
        {
            for (int k = 0; k < colsCount; k++)
            {
                elements[i, k].gameObject = GameObject.Instantiate(sampleElement);
                elements[i, k].gameObject.GetComponent<Renderer>().material = sampleElement.GetComponent<Renderer>().material;
                elements[i, k].gameObject.transform.parent = parentTransform.transform;
                elements[i, k].gameObject.transform.position = new Vector3(i, 0, k);
                var color = elements[i, k].value ? activeElementColor : deactiveElementColor;
                elements[i, k].gameObject.GetComponent<Renderer>().material.color = color;
            }
        }
    }
}
