using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Wave
{
    public float amplitude;
    public float frequency;
    public float lenght;
    public Vector3 origin;
}

public class Water : MonoBehaviour
{

    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float squareSize;

    [SerializeField] Wave[] waves;

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
    }

    void Update()
    {
        time += Time.deltaTime;

        CalculateWaves(mesh.vertices);
    }

    void MakeMesh(MeshFilter meshFilter)
    {
        int[] triangles = new int[width * height * 6];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                vertices[(x + y * width) * 6] = new Vector3(-(width * squareSize)/2 + x * squareSize, 0, height * squareSize / 2 - y * squareSize);
                vertices[(x + y * width) * 6 + 1] = new Vector3(-(width * squareSize) / 2 + x * squareSize + squareSize, 0, height * squareSize / 2 - y * squareSize);
                vertices[(x + y * width) * 6 + 2] = new Vector3(-(width * squareSize) / 2 + x * squareSize, 0, height * squareSize / 2 - y * squareSize - squareSize);

                vertices[(x + y * width) * 6 + 3] = new Vector3(-(width * squareSize) / 2 + x * squareSize, 0, height * squareSize / 2 - y * squareSize - squareSize);
                vertices[(x + y * width) * 6 + 4] = new Vector3(-(width * squareSize) / 2 + x * squareSize + squareSize, 0, height * squareSize / 2 - y * squareSize);
                vertices[(x + y * width) * 6 + 5] = new Vector3(-(width * squareSize) / 2 + x * squareSize + squareSize, 0, height * squareSize / 2 - y * squareSize - squareSize);

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

    void CalculateWaves(Vector3[] vertices)
    {
        /*for (int v = 0; v < vertices.Length; v++)
        {
            vertices[v].y = 0;

            for (int w = 0; w < waves.Length; w++)
            {
                vertices[v].y += waves[w].amplitude * Mathf.Sin(2 * Mathf.PI * (Vector3.Distance(waves[w].origin, vertices[v]) / waves[w].lenght - (float)time * waves[w].frequency));
            }
        }*/

        float yPos;

        for (int y = 0; y < height - 1; y++)
        {
            for (int x = 0; x < width - 1; x++)
            {
                yPos = 0;

                for (int w = 0; w < waves.Length; w++)
                {
                    yPos += waves[w].amplitude * Mathf.Sin(2 * Mathf.PI * (Mathf.Sqrt((waves[w].origin.x - vertices[(x + y * width) * 6 + 5].x)* (waves[w].origin.x - vertices[(x + y * width) * 6 + 5].x) + (waves[w].origin.z - vertices[(x + y * width) * 6 + 5].z)*(waves[w].origin.z - vertices[(x + y * width) * 6 + 5].z)) / waves[w].lenght - (float)time * waves[w].frequency));
                }

                vertices[(x + y * width) * 6 + 5].y = yPos;
                vertices[((x + 1) + y * width) * 6 + 2].y = yPos;
                vertices[((x + 1) + y * width) * 6 + 3].y = yPos;
                vertices[(x + (y + 1) * width) * 6 + 1].y = yPos;
                vertices[(x + (y + 1) * width) * 6 + 4].y = yPos;
                vertices[((x + 1) + (y + 1) * width) * 6].y = yPos;
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

    public float WaterLevel(Vector3 pos)
    {
        float y = 0;

        for (int w = 0; w < waves.Length; w++)
        {
            y += waves[w].amplitude * Mathf.Sin(2 * Mathf.PI * (Mathf.Sqrt((waves[w].origin.x - pos.x) * (waves[w].origin.x - pos.x) + (waves[w].origin.z - pos.z) * (waves[w].origin.z - pos.z)) / waves[w].lenght - (float)time * waves[w].frequency));
        }

        return y;
    }
}
