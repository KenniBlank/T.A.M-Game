using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exit : MonoBehaviour
{
    public Canvas pauseCanvas;
    private bool isCanvasActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCanvas();
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void ToggleCanvas()
    {
        if (pauseCanvas != null)
        {
            isCanvasActive = !isCanvasActive;
            pauseCanvas.gameObject.SetActive(isCanvasActive);
        }
    }
}
