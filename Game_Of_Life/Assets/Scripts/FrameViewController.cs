using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameViewController : MonoBehaviour
{
    private const int edgeCount = 12;
    private const int subFrameEdgeCount = 12;
    private const float elementThickness = 1f;

    private GameObject[] edges;
    public const float edgeThickness = 0.04f;
    public const float subFrameEdgeThickness = 0.06f;
    public Color edgeColor;
    public Material edgeMaterial;
    private GameObject[] subFrameEdges;
    public Color subFrameEdgeColor;
    public GameObject parentTransform;

    public GameObject sampleEdge;

    // Start is called before the first frame update
    void Start()
    {
        //changed to shorter name for code readability
        float e = edgeThickness;
        float l = elementThickness;
        //offset so that the edge line is not colliding with elements
        float o = (l + e) / 2;
        //changed to shorter name for code readability
        float x = GameManager3D.rowsCount - 1;
        float y = GameManager3D.colsCount - 1;
        float z = GameManager3D.depthsCount - 1;

        sampleEdge.SetActive(true);

        // create a sub frame for partial grid using primitive cubes
        subFrameEdges = new GameObject[subFrameEdgeCount];
        for (int i = 0; i < subFrameEdgeCount; i++)
        {
            subFrameEdges[i] = GameObject.Instantiate(sampleEdge);
            subFrameEdges[i].gameObject.transform.SetParent(parentTransform.transform);
            subFrameEdges[i].gameObject.GetComponent<Renderer>().material = edgeMaterial;
            subFrameEdges[i].gameObject.GetComponent<Renderer>().material.color = subFrameEdgeColor;
        }

        subFrameViewer((int)x, (int)y, (int)z);

        // create a frame for the grid to imporve spatial understanding 

        edges = new GameObject[edgeCount];
        for (int i = 0; i < edgeCount; i++)
        {
            edges[i] = GameObject.Instantiate(sampleEdge);
            edges[i].gameObject.transform.SetParent(parentTransform.transform);
            edges[i].gameObject.GetComponent<Renderer>().material = edgeMaterial;
            edges[i].gameObject.GetComponent<Renderer>().material.color = edgeColor;
        }

        //Lines in x direction
        edges[0].transform.localScale = new Vector3(x + 2 * o, e, e);
        edges[0].transform.transform.position = new Vector3(x / 2, -o, -o);

        edges[1].transform.localScale = new Vector3(x + 2 * o, e, e);
        edges[1].transform.transform.position = new Vector3(x / 2, y + o, -o);

        edges[2].transform.localScale = new Vector3(x + 2 * o, e, e);
        edges[2].transform.transform.position = new Vector3(x / 2, y + o, z + o);

        edges[3].transform.localScale = new Vector3(x + 2 * o, e, e);
        edges[3].transform.transform.position = new Vector3(x / 2, -o, z + o);

        //Lines in y direction
        edges[4].transform.localScale = new Vector3(e, y + 2 * o, e);
        edges[4].transform.transform.position = new Vector3(-o, y / 2, -o);

        edges[5].transform.localScale = new Vector3(e, y + 2 * o, e);
        edges[5].transform.transform.position = new Vector3(x + o, y / 2, -o);

        edges[6].transform.localScale = new Vector3(e, y + 2 * o, e);
        edges[6].transform.transform.position = new Vector3(x + o, y / 2, z + o);

        edges[7].transform.localScale = new Vector3(e, y + 2 * o, e);
        edges[7].transform.transform.position = new Vector3(-o, y / 2, z + o);

        //Lines in z direction
        edges[8].transform.localScale = new Vector3(e, e, z + 2 * o);
        edges[8].transform.transform.position = new Vector3(-o, -o, z / 2);

        edges[9].transform.localScale = new Vector3(e, e, z + 2 * o);
        edges[9].transform.transform.position = new Vector3(x + o, -o, z / 2);

        edges[10].transform.localScale = new Vector3(e, e, z + 2 * o);
        edges[10].transform.transform.position = new Vector3(x + o, y + o, z / 2);

        edges[11].transform.localScale = new Vector3(e, e, z + 2 * o);
        edges[11].transform.transform.position = new Vector3(-o, y + o, z / 2);
    }

    // set the subframe for any (x,y,z) size of the subframe
    public void subFrameViewer(int x, int y, int z)
    {
        float e = subFrameEdgeThickness;
        //offset so that the subFrame edge line is not colliding with elements
        float o = (elementThickness + e) / 2;

        //Lines in x direction
        subFrameEdges[0].transform.localScale = new Vector3(x + 2 * o, e, e);
        subFrameEdges[0].transform.transform.position = new Vector3((float)x / 2, -o, -o);

        subFrameEdges[1].transform.localScale = new Vector3(x + 2 * o, e, e);
        subFrameEdges[1].transform.transform.position = new Vector3((float)x / 2, y + o, -o);

        subFrameEdges[2].transform.localScale = new Vector3(x + 2 * o, e, e);
        subFrameEdges[2].transform.transform.position = new Vector3((float)x / 2, y + o, z + o);

        subFrameEdges[3].transform.localScale = new Vector3(x + 2 * o, e, e);
        subFrameEdges[3].transform.transform.position = new Vector3((float)x / 2, -o, z + o);

        //Lines in y direction
        subFrameEdges[4].transform.localScale = new Vector3(e, y + 2 * o, e);
        subFrameEdges[4].transform.transform.position = new Vector3(-o, (float)y / 2, -o);

        subFrameEdges[5].transform.localScale = new Vector3(e, y + 2 * o, e);
        subFrameEdges[5].transform.transform.position = new Vector3(x + o, (float)y / 2, -o);

        subFrameEdges[6].transform.localScale = new Vector3(e, y + 2 * o, e);
        subFrameEdges[6].transform.transform.position = new Vector3(x + o, (float)y / 2, z + o);

        subFrameEdges[7].transform.localScale = new Vector3(e, y + 2 * o, e);
        subFrameEdges[7].transform.transform.position = new Vector3(-o, (float)y / 2, z + o);

        //Lines in z direction
        subFrameEdges[8].transform.localScale = new Vector3(e, e, z + 2 * o);
        subFrameEdges[8].transform.transform.position = new Vector3(-o, -o, (float)z / 2);

        subFrameEdges[9].transform.localScale = new Vector3(e, e, z + 2 * o);
        subFrameEdges[9].transform.transform.position = new Vector3(x + o, -o, (float)z / 2);

        subFrameEdges[10].transform.localScale = new Vector3(e, e, z + 2 * o);
        subFrameEdges[10].transform.transform.position = new Vector3(x + o, y + o, (float)z / 2);

        subFrameEdges[11].transform.localScale = new Vector3(e, e, z + 2 * o);
        subFrameEdges[11].transform.transform.position = new Vector3(-o, y + o, (float)z / 2);
    }
}
