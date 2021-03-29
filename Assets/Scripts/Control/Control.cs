using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Camera and program control
public class Control : MonoBehaviour
{
    bool linesEnabled = true;
    bool valuesEnabled = true;
    //bool menuEnabled = false;

    GameObject[] lines;
    GameObject[] values;
    //GameObject menu;
    GameObject cam;
    public Text snapshotStatus;
    public Text consoleText;

    public GameObject ui;

    public void SetupNewCamera()
    {
        Instantiate(ui);

        lines = GameObject.FindGameObjectsWithTag("Line");
        values = GameObject.FindGameObjectsWithTag("Value");
        //menu = GameObject.FindGameObjectWithTag("Menu");
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        snapshotStatus = GameObject.FindGameObjectWithTag("Status").GetComponent<Text>();
        consoleText = GameObject.FindGameObjectWithTag("Console").GetComponent<Text>();

        // Close menu
        //menu.SetActive(false);

        // Enable camera control
        cam.GetComponent<FreeFlyCamera>().enabled = true;

        //menuEnabled = false;
    }

    void Update()
    {
        // Toggle lines
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Disable
            if (linesEnabled)
            {
                foreach (GameObject line in lines)
                {
                    line.SetActive(false);
                }

                linesEnabled = false;
            }
            // Enable
            else
            {
                foreach (GameObject line in lines)
                {
                    line.SetActive(true);
                }

                linesEnabled = true;
            }
        }

        // Toggle values
        if (Input.GetKeyDown(KeyCode.V))
        {
            // Disable
            if (valuesEnabled)
            {
                foreach(GameObject value in values)
                {
                    value.SetActive(false);
                }

                valuesEnabled = false;
            }
            // Enable
            else
            {
                foreach(GameObject value in values)
                {
                    value.SetActive(true);
                }

                valuesEnabled = true;
            }
        }

        // Toggle menu
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    // Disable
        //    if (menuEnabled)
        //    {
        //        menu.SetActive(false);

        //        // Enable camera control
        //        cam.GetComponent<FreeFlyCamera>().enabled = true;

        //        menuEnabled = false;
        //    }
        //    // Enable
        //    else
        //    {
        //        menu.SetActive(true);

        //        // Release and make cursor visible
        //        Cursor.lockState = CursorLockMode.None;
        //        Cursor.visible = true;

        //        // Disable camera control
        //        cam.GetComponent<FreeFlyCamera>().enabled = false;

        //        menuEnabled = true;
        //    }
        //}
    }
}
