using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIDebugButtons : MonoBehaviour
{
    GameObject water;
    GameObject[] foams;

    public EventListenerDelegateResponse levelStartEventListener;

    private void OnEnable()
    {
        levelStartEventListener.OnEnable();
    }

    private void OnDisable()
    {
        levelStartEventListener.OnDisable();
    }

    private void Start()
    {
        levelStartEventListener.response = DisableFoams;
    }

    void DisableFoams()
    {
        foams = GameObject.FindGameObjectsWithTag("Foam");

        for (int i = 0; i < foams.Length; i++)
        {
            foams[i].SetActive(false);
        }
    }
    public void ToggleWater()
    {
        if (water == null)
            water = GameObject.Find("Water");

        if (foams == null)
            foams = GameObject.FindGameObjectsWithTag("Foam");

        if (water != null)
            water.SetActive(!water.activeInHierarchy);

        for (int i = 0; i < foams.Length; i++)
        {
            foams[i].SetActive(!foams[i].activeInHierarchy);
        }
    }

    public void LoadNextLevel()
    {
        DOTween.KillAll();

        water = null;
        foams = null;

        var _level = PlayerPrefs.GetInt("Level");
        PlayerPrefs.SetInt("Level", _level + 1);

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void LoadPrevLevel()
    {
        DOTween.KillAll();

        water = null;
        foams = null;

        var _level = PlayerPrefs.GetInt("Level");
        PlayerPrefs.SetInt("Level", _level - 1);

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
