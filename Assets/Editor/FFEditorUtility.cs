using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class FFEditorUtility
{
    [MenuItem("FFStudios/TakeScreenShot")]
    public static void TakeScreenShot()
    {
        ScreenCapture.CaptureScreenshot("ScreenShot.png");
    }
}
