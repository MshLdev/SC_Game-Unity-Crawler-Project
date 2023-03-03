using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    void Start()
    {
         Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartCampaign()
    {
        SceneManager.LoadScene("Campaign", LoadSceneMode.Single);
    }

    public void StartArena()
    {
        SceneManager.LoadScene("Arena", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
