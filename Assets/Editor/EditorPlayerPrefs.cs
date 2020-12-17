using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EditorPlayerPrefs
{
    [MenuItem("FFStudios/Delete PlayerPrefs")]
    static void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
