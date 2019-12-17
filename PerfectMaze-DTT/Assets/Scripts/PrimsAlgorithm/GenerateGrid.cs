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
    private List<Transform> _set;
    private List<List<Transform>> _setAdj;
    private Material _mat;
    private int weight;

    void Start()
    {
        GenerateGridFunc((int)gridSize.x, (int)gridSize.y);
        setAdjecents((int)gridSize.x, (int)gridSize.y);
        SetRandomWeight();
        SetStart();
        FindNext();
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
        Camera.main.transform.position = new Vector3((gridX / 2) - 0.5f, (gridY / 2) - 0.5f, -10);
        Camera.main.orthographicSize = Mathf.Max(gridX, gridY) / 2f;
    }
    void SetRandomWeight()
    {
        foreach (Transform children in transform)
        {
            weight = Random.Range(0, 10);
            children.GetComponent<ManageCells>().weight = weight;
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
                    manageCells.adjacents.Add(_GridArray[x - 1, y]);
                }
                if (x + 1 < gridX)
                {
                    manageCells.adjacents.Add(_GridArray[x + 1, y]);
                }
                if (y - 1 >= 0)
                {
                    manageCells.adjacents.Add(_GridArray[x, y - 1]);
                }
                if (y + 1 < gridY)
                {
                    manageCells.adjacents.Add(_GridArray[x, y + 1]);
                }

                manageCells.adjacents.Sort(SortByWeight);
            }
        }
    }

    int SortByWeight(Transform inA, Transform inB)
    {
        int a = inA.GetComponent<ManageCells>().weight;
        int b = inB.GetComponent<ManageCells>().weight;
        return a.CompareTo(b);
    }

    void SetStart()
    {
        _set = new List<Transform>();
        _setAdj = new List<List<Transform>>();

        for (int i = 0; i < 10; i++)
        {
            _setAdj.Add(new List<Transform>());
        }

        _GridArray[0, 0].GetComponent<Renderer>().material.color = Color.green;

        AddSet(_GridArray[0, 0]);
    }

    void FindNext()
    {
        Transform next;

        do
        {
            bool empty = true;
            int lowestList = 0;

            for (int i = 0; i < 10; i++)
            {
                lowestList = i;
                if (_setAdj[i].Count > 0)
                {
                    empty = false;
                    break;
                }
            }

            if (empty)
            {
                CancelInvoke("FindNext");
                _set[_set.Count - 1].GetComponent<Renderer>().material.color = Color.red;

                foreach (Transform cell in _GridArray)
                {
                    if (!_set.Contains(cell))
                    {
                        cell.GetComponent<Renderer>().material.color = Color.black;
                    }
                }
                return;
            }
            next = _setAdj[lowestList][0];
            _setAdj[lowestList].Remove(next);
        } while (next.GetComponent<ManageCells>().openAdj >= 2);
        next.GetComponent<Renderer>().material.color = Color.white;
        AddSet(next);

        Invoke("FindNext", 0);
    }

    void AddSet(Transform addedCell)
    {
        _set.Add(addedCell);

        foreach(Transform adj in addedCell.GetComponent<ManageCells>().adjacents)
        {
            adj.GetComponent<ManageCells>().openAdj++;
            if (!_set.Contains(adj) && !(_setAdj[adj.GetComponent<ManageCells>().weight].Contains(adj)))
            {
                _setAdj[adj.GetComponent<ManageCells>().weight].Add(adj);
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
