using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameViewController : MonoBehaviour
{
    private const int edgeCount = 12;
    private const float elementThickness = 1f ;

    private GameObject[] edges;
    public const float edgeThickness = 0.05f;
    public Color edgeColor;
    public Material edgeMaterial;
    public GameObject parentTransform;

    // Start is called before the first frame update
    void Start()
    {
        //changed to shorter name for code readability
        float e = edgeThickness;
        float l = elementThickness;
        //offset so that the edge line is not colliding with elements
        float o = (l + e) / 2;
        //changed to shorter name for code readability
        float x = GameManager3D.rowsCount-1;
        float y = GameManager3D.colsCount-1;
        float z = GameManager3D.depsCount-1;

        edges = new GameObject[edgeCount];
        for (int i = 0; i < edgeCount; i++) {
            edges[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            edges[i].gameObject.transform.SetParent(parentTransform.transform);
            edges[i].gameObject.GetComponent<Renderer>().material = edgeMaterial;
            edges[i].gameObject.GetComponent<Renderer>().material.color = edgeColor;
        }
        //Lines in x direction
        edges[0].transform.localScale = new Vector3(x + 2 * o, e, e);
        edges[0].transform.transform.position = new Vector3(x / 2, -o, -o);

        edges[1].transform.localScale = new Vector3(x + 2 * o, e, e);
        edges[1].transform.transform.position = new Vector3(x / 2, y+o, -o);

        edges[2].transform.localScale = new Vector3(x + 2 * o, e, e);
        edges[2].transform.transform.position = new Vector3(x / 2, y+o, z+o);

        edges[3].transform.localScale = new Vector3(x + 2 * o, e, e);
        edges[3].transform.transform.position = new Vector3(x / 2, -o, z+o);

        //Lines in y direction
        edges[4].transform.localScale = new Vector3(e, y + 2 * o, e);
        edges[4].transform.transform.position = new Vector3(-o, y / 2, -o);

        edges[5].transform.localScale = new Vector3(e, y + 2 * o, e);
        edges[5].transform.transform.position = new Vector3(x + o, y / 2, -o);

        edges[6].transform.localScale = new Vector3(e, y + 2 * o, e);
        edges[6].transform.transform.position = new Vector3(x+o, y / 2, z+o);

        edges[7].transform.localScale = new Vector3(e, y + 2 * o, e);
        edges[7].transform.transform.position = new Vector3(-o, y / 2, z+o);

        //Lines in z direction
        edges[8].transform.localScale = new Vector3(e, e, z + 2 * o);
        edges[8].transform.transform.position = new Vector3(-o, -o, z / 2);

        edges[9].transform.localScale = new Vector3(e, e, z + 2 * o);
        edges[9].transform.transform.position = new Vector3(x+o, -o, z / 2);

        edges[10].transform.localScale = new Vector3(e, e, z + 2 * o);
        edges[10].transform.transform.position = new Vector3(x+o, y+o, z / 2);

        edges[11].transform.localScale = new Vector3(e, e, z + 2 * o);
        edges[11].transform.transform.position = new Vector3(-o, y+o, z / 2);


    }
}
