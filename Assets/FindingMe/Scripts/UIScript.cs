using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    public void ClickReturn()
    {
        SceneManager.LoadScene(0);
    }

    public void ClickExit()
    {
        Application.Quit();
    }

}
