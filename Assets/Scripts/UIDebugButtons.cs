using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIDebugButtons : MonoBehaviour
{
    GameObject water;
    public void ToggleWater()
    {
        if (water == null)
            water = GameObject.Find("Water");

        if (water != null)
        {
            water.SetActive(!water.activeInHierarchy);
        }
    }

    public void LoadNextLevel()
    {
        DOTween.KillAll();

        var _level = PlayerPrefs.GetInt("Level");
        PlayerPrefs.SetInt("Level", _level + 1);

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void LoadPrevLevel()
    {
        DOTween.KillAll();

        var _level = PlayerPrefs.GetInt("Level");
        PlayerPrefs.SetInt("Level", _level - 1);

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
