using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using NaughtyAttributes;

public class FFAnalitics : MonoBehaviour
{
    private void Awake()
    {
        if (!FB.IsInitialized)
            FB.Init(OnFacebookInitialized, OnHideUnity);
        else
            FB.ActivateApp();
    }

    void OnFacebookInitialized()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            Debug.Log("[FFAnalitic] Facebook initiliazed");
        }
        else
            Debug.Log("[FFAnalitic] Failed to initialize Facebook SDK");


        DontDestroyOnLoad(gameObject);

    }

    void OnHideUnity(bool hide)
    {

    }
}
