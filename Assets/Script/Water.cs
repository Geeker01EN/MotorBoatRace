using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{

    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float squareSize;

    [SerializeField] float amplitude;
    [SerializeField] float frequency;
    [SerializeField] float lenght;
    [SerializeField] Vector3 origin;

    Vector3[] vertices;
    Mesh mesh;
    MeshFilter meshFilter;

    double time = 0;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        vertices = new Vector3[width * height * 6];
        MakeMesh(meshFilter);

        GetComponent<BoxCollider>().size = new Vector3(width * squareSize, 1, height * squareSize);
    }

    void Update()
    {
        time += Time.deltaTime;

        CalculateWave(mesh.vertices);
    }

    void MakeMesh(MeshFilter meshFilter)
    {
        int[] triangles = new int[width * height * 6];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                vertices[(x + y * width) * 6] = new Vector3(-(width * squareSize)/2 + x * squareSize, 0, height * squareSize / 2 - y * squareSize);
                vertices[(x + y * width) * 6 + 1] = new Vector3(-(width * squareSize) / 2 + x * squareSize + width, 0, height * squareSize / 2 - y * squareSize);
                vertices[(x + y * width) * 6 + 2] = new Vector3(-(width * squareSize) / 2 + x * squareSize, 0, height * squareSize / 2 - y * squareSize - height);

                vertices[(x + y * width) * 6 + 3] = new Vector3(-(width * squareSize) / 2 + x * squareSize, 0, height * squareSize / 2 - y * squareSize - height);
                vertices[(x + y * width) * 6 + 4] = new Vector3(-(width * squareSize) / 2 + x * squareSize + width, 0, height * squareSize / 2 - y * squareSize);
                vertices[(x + y * width) * 6 + 5] = new Vector3(-(width * squareSize) / 2 + x * squareSize + width, 0, height * squareSize / 2 - y * squareSize - height);

                triangles[(x + y * width) * 6] = (x + y * width) * 6;
                triangles[(x + y * width) * 6 + 1] = (x + y * width) * 6 + 1;
                triangles[(x + y * width) * 6 + 2] = (x + y * width) * 6 + 2;
                triangles[(x + y * width) * 6 + 3] = (x + y * width) * 6 + 3;
                triangles[(x + y * width) * 6 + 4] = (x + y * width) * 6 + 4;
                triangles[(x + y * width) * 6 + 5] = (x + y * width) * 6 + 5;
            }
        }

        mesh.MarkDynamic();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

    void CalculateWave(Vector3[] vertices)
    {
        for (int v = 0; v < vertices.Length; v++)
        {
            vertices[v].y = amplitude * Mathf.Sin(2 * Mathf.PI * (Vector3.Distance(origin, vertices[v]) / lenght - (float)time * frequency));
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }
}
