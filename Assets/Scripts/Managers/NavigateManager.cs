using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigateManager : MonoBehaviour
{
    public void navigate(int sceneID)
    {
        Application.targetFrameRate = 60;
        SceneManager.LoadScene(sceneID);
    }



}
