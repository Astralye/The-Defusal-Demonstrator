using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Loader : MonoBehaviour
{
    public enum Scene
    {
        MAIN,
        OFFICE,
        HOTEL
    }

    [SerializeField]
    public PauseMenu pauseMenu;
    public Scene sceneType;
    void Start()
    {
    }

    public void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public void LoadScene()
    {
        if (pauseMenu != null)
        {
            pauseMenu.closeMenu();
        }
        Load(sceneType);
    }
}
