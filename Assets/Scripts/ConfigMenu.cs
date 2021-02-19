using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigMenu : MonoBehaviour
{
    public GameObject widthInput;
    public GameObject heightInput;
    public GameObject depthInput;
    public GameObject controller;

    void Start()
    {
        // Set placeholders to current NodeGrid values
        //widthInput.transform.Find("Placeholder").GetComponent<Text>().text = controller.GetComponent<MakeGrid>().nodeGrid.ToString();
    }

    void Update()
    {
        //Debug.Log(widthInput.transform.Find("Placeholder").GetComponent<Text>().text);
    }
}
