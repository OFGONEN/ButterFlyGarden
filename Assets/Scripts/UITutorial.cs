using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UITutorial : ScriptableObject
{
    [HideInInspector]
    public UIHelperManager uIHelperManager;
    public UITutorial nextTutorial;
    public delegate void TutorialEnded(bool success);
    public TutorialEnded tutorialEnded;

    public abstract void StartTutorial();
    public abstract void SetHelperAssetDatas();
    public abstract void EndTutorial(bool success);
    public abstract void ForseEndTutorial();
    public abstract UIHelperAsset GetHelperAsset(string assetName);

}
