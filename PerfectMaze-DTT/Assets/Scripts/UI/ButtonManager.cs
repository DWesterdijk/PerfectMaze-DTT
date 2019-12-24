using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    void ReloadScene()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        this.transform.GetComponent<GenerateGrid>().StartGridGenerate();
    }
}
