using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMode : MonoBehaviour
{
    [SerializeField]
    private GameObject console;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            console.SetActive(!console.activeSelf);
        }
    }
}
