using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class EditorAssetLibraryUtility
{
    public static EditorAssetLibrary assetLibrary;
    public static string levelDataSavePath = "Assets/ScriptableObjects/LevelData/";

    static EditorAssetLibraryUtility()
    {
        CreateFolders();

        assetLibrary = AssetDatabase.LoadAssetAtPath("Assets/Editor/EditorAssetLibrary.asset", typeof(EditorAssetLibrary)) as EditorAssetLibrary;

        if (assetLibrary == null)
        {
            assetLibrary = ScriptableObject.CreateInstance<EditorAssetLibrary>();
            AssetDatabase.CreateAsset(assetLibrary, "Assets/Editor/EditorAssetLibrary.asset");
        }
    }

    static void CreateFolders()
    {
        bool _isExits = false;

        _isExits = AssetDatabase.IsValidFolder("Assets/ScriptableObjects");

        if (!_isExits)
            AssetDatabase.CreateFolder("Assets", "ScriptableObjects");

        _isExits = AssetDatabase.IsValidFolder("Assets/ScriptableObjects/LevelData");

        if (!_isExits)
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", "LevelData");
    }

    [MenuItem("FFStudios/Asset/Track Asset")]
    public static void TrackAsset()
    {
        var _activeGO = Selection.activeGameObject;

        if (_activeGO == null)
        {
            Debug.LogError("Select a PreFab to track");
            EditorApplication.Beep();
            return;
        }

        if (!_activeGO.name.Contains("_"))
        {
            Debug.LogError("PreFab name convention should be PreFab_1 to track");
            EditorApplication.Beep();
            return;
        }
        else
        {
            var _index = _activeGO.name.IndexOf('_');

            if (_activeGO.name.Length == _index + 1 || _activeGO.name[_index + 1] != '1')
            {
                Debug.LogError("Always track the first variant of a Prefab." +
                "After '_' character there should be '1'");
                EditorApplication.Beep();

                return;
            }
        }

        assetLibrary.TrackAsset(_activeGO);

        EditorUtility.SetDirty(assetLibrary);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("FFStudios/Asset/UnTrack Asset")]
    public static void UnTrackAsset()
    {
        var _activeGO = Selection.activeGameObject;

        if (_activeGO == null)
        {
            Debug.LogError("Select a PreFab to track");
            EditorApplication.Beep();
            return;
        }

        assetLibrary.UnTrackAsset(_activeGO);

        EditorUtility.SetDirty(assetLibrary);
        AssetDatabase.SaveAssets();
    }

}
