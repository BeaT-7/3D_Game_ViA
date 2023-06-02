using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class QuitGame : MonoBehaviour
{

    public void quitGame()
    {
        Debug.Log("Quit");
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
