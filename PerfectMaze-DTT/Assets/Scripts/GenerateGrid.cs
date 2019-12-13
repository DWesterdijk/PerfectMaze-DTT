using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    [Header("Size of Grid")]
    public Vector2 gridSize;

    [Header("Cell prefab")]
    public Transform cellObj;



    private Transform[,] _GridArray;
    private int weight;

    void Start()
    {
        GenerateGridFunc((int)gridSize.x, (int)gridSize.y);
        setAdjecents((int)gridSize.x, (int)gridSize.y);
        SetRandomWeight();
    }

    void SetRandomWeight()
    {
        foreach (Transform children in transform)
        {
            weight = Random.Range(0, 10);
            children.GetComponent<ManageCells>().weight = weight;
        }
    }

    void GenerateGridFunc(int gridX, int gridY)
    {
        _GridArray = new Transform[gridX, gridY];

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                Transform nextCell;
                nextCell = (Transform)Instantiate(cellObj, new Vector2(x, y), Quaternion.identity);

                nextCell.parent = transform;
                nextCell.name = "Cell" + "(" + x + ", " + y +  ", " + "W:" + weight + ")";

                nextCell.GetComponent<ManageCells>().gridPosition = new Vector2(x, y);

                _GridArray[x, y] = nextCell;
            }
        }
    }

    void setAdjecents(int gridX, int gridY)
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                Transform adjCell;
                adjCell = _GridArray[x, y];

                ManageCells manageCells = adjCell.GetComponent<ManageCells>();


                //Remake this in a switch statement
                if (x - 1 >= 0)
                {
                    manageCells.adjecents.Add(_GridArray[x - 1, y]);
                }
                if (x + 1 < gridX)
                {
                    manageCells.adjecents.Add(_GridArray[x + 1, y]);
                }
                if (y - 1 >= 0)
                {
                    manageCells.adjecents.Add(_GridArray[x, y - 1]);
                }
                if (y + 1 < gridY)
                {
                    manageCells.adjecents.Add(_GridArray[x, y + 1]);
                }
            }
        }
    }

    private void Update()
    {
        //Reload scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
