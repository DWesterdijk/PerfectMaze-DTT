using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCells : MonoBehaviour
{
    public List<Transform> adjacents;
    public Vector2 gridPosition;
    public int weight;
    public int openAdj;
}
