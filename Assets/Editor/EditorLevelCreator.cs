using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class EditorLevelCreator
{
    static void CreateFolders()
    {
        bool _isExits = AssetDatabase.IsValidFolder("Assets/Editor");

        if (_isExits) return;

        AssetDatabase.CreateFolder("Assets", "Editor");
    }

    [MenuItem("FFStudios/Asset/Create LevelData Asset %&d")]
    static void CreateLevelData()
    {
        ClearLog();

        if (EditorAssetLibraryUtility.assetLibrary.trackedAssets.Count < 1)
        {
            Debug.LogError("There is no tracked Asset");
            return;
        }

        var _selection = Selection.assetGUIDs;

        foreach (var guid in _selection)
        {
            var _path = AssetDatabase.GUIDToAssetPath(guid);

            if (!_path.Contains(".png")) continue;

            CreateLevelData(_path, false);
        }
    }
    [MenuItem("FFStudios/Asset/Update LevelData Asset %&u")]
    static void UpdateLevelData()
    {
        ClearLog();

        if (EditorAssetLibraryUtility.assetLibrary.trackedAssets.Count < 1)
        {
            Debug.LogError("There is no tracked Asset");
            return;
        }

        var _selection = Selection.assetGUIDs;

        foreach (var guid in _selection)
        {
            var _path = AssetDatabase.GUIDToAssetPath(guid);

            if (!_path.Contains(".png")) continue;

            CreateLevelData(_path, true);
        }
    }
    [MenuItem("FFStudios/Asset/Create All LevelData Asset %&a")]
    static void CreateAllLevelData()
    {
        ClearLog();

        if (EditorAssetLibraryUtility.assetLibrary.trackedAssets.Count < 1)
        {
            Debug.LogError("There is no tracked Asset");
            return;
        }

        var _guids = AssetDatabase.FindAssets("t:texture2D", new string[] { "Assets/LevelData" });

        foreach (var _guid in _guids)
        {
            var _path = AssetDatabase.GUIDToAssetPath(_guid);
            CreateLevelData(_path, false);
        }
    }

    static void CreateLevelData(string path, bool respectOldData)
    {
        var _fileName = Path.GetFileNameWithoutExtension(path);
        var _texture = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
        var _levelData = ScriptableObject.CreateInstance<LevelData>();
        var _assetLib = EditorAssetLibraryUtility.assetLibrary;


        Vector2 _leftDown = new Vector2(_texture.width, _texture.height);
        Vector2 _rightUp = Vector2.zero;

        for (int x = 0; x < _texture.width; x++)
        {
            for (int y = 0; y < _texture.height; y++)
            {
                var _pixel = _texture.GetPixel(x, y);

                if (_pixel.a == 0) continue;

                // Debug.Log($"Pixel:({x},{y}) r:{_pixel.r * 255} g:{_pixel.g * 255} b:{_pixel.b * 255}");

                if (x <= _leftDown.x)
                    _leftDown.x = x;

                if (y <= _leftDown.y)
                    _leftDown.y = y;

                if (x >= _rightUp.x)
                    _rightUp.x = x;

                if (y >= _rightUp.y)
                    _rightUp.y = y;
            }
        }

        for (int x = 0; x < _texture.width; x++)
        {
            for (int y = 0; y < _texture.height; y++)
            {
                var _pixel = _texture.GetPixel(x, y);

                if (_pixel.a != 1) continue;

                int _pixelRed = (int)(_pixel.r * 255);
                int _assetIndex = _pixelRed / 40;

                // Add lily for every object
                var _mapPosition = new Vector2(x, y) - _leftDown;
                var _worldPosition = new Vector2(_mapPosition.x, _mapPosition.y);
                float _worldDirection = _pixel.b * 255 * 90;

                _levelData.lilyDatas.Add(new LilyData()
                {
                    mapCord = _mapPosition,
                    levelObject = _assetLib.trackedAssets[0],
                    position = _worldPosition,
                    direction = _worldDirection
                });

                if (_assetIndex >= 1 && _assetIndex < _assetLib.trackedAssets.Count)
                {
                    var _levelObject = _assetLib.trackedAssets[_assetIndex];

                    if (_levelObject.name.ToLower().Contains("butterfly"))
                    {
                        _levelData.butterFlyDatas.Add(new ButterFlyData()
                        {
                            mapCord = _mapPosition,
                            levelObject = _levelObject,
                            position = _worldPosition,
                            direction = _worldDirection
                        });
                    }
                    else if (_levelObject.name.ToLower().Contains("frog"))
                    {
                        _levelData.frogDatas.Add(new FrogData()
                        {
                            mapCord = _mapPosition,
                            levelObject = _levelObject,
                            position = _worldPosition,
                            direction = _worldDirection,
                        });
                    }
                    else if (_levelObject.name.ToLower().Contains("bubble"))
                    {
                        _levelData.bubbleDatas.Add(new BubbleData()
                        {
                            mapCord = _mapPosition,
                            levelObject = _levelObject,
                            position = _worldPosition,
                            direction = _worldDirection,
                        });
                    }
                    // else if (_levelObject.name.ToLower().Contains("rhino"))
                    // {
                    //     _levelData.rhinoDatas.Add(new RhinoData()
                    //     {
                    //         mapCord = _mapPosition,
                    //         levelObject = _levelObject,
                    //         position = _worldPosition,
                    //         direction = _worldDirection
                    //     });
                    // }
                }
            }
        }

        _levelData.leftDown = _leftDown;
        _levelData.size = _rightUp + Vector2.one - _leftDown;

        var _startIndex = _fileName.IndexOf('_');
        var _levelDataFileName = "LevelData" + _fileName.Substring(_startIndex) + ".asset";
        var _createPath = EditorAssetLibraryUtility.levelDataSavePath + _levelDataFileName;

        if (respectOldData)
        {
            var _oldAsset = AssetDatabase.LoadAssetAtPath<LevelData>(_createPath);

            if (_oldAsset != null)
            {
                RetrieveDataFromOldData(_levelData, _oldAsset);
            }
        }

        AssetDatabase.CreateAsset(_levelData, _createPath);
    }

    private static void RetrieveDataFromOldData(LevelData newData, LevelData oldData)
    {
        for (int i = 0; i < newData.butterFlyDatas.Count; i++)
        {
            for (int x = 0; x < oldData.butterFlyDatas.Count; x++)
            {
                if (newData.butterFlyDatas[i].mapCord == oldData.butterFlyDatas[x].mapCord)
                {
                    var _newData = newData.butterFlyDatas[i];
                    _newData.butterFlyColor = oldData.butterFlyDatas[x].butterFlyColor;

                    newData.butterFlyDatas[i] = _newData;
                }
            }

        }

        for (int i = 0; i < newData.frogDatas.Count && i < oldData.frogDatas.Count; i++)
        {
            for (int x = 0; x < oldData.frogDatas.Count; x++)
            {
                if (newData.frogDatas[i].mapCord == oldData.frogDatas[x].mapCord)
                {
                    var _newData = newData.frogDatas[i];
                    _newData.frogColor = oldData.frogDatas[x].frogColor;

                    newData.frogDatas[i] = _newData;
                }
            }

        }


        // for (int i = 0; i < newData.rhinoDatas.Count && i < oldData.rhinoDatas.Count; i++)
        // {

        //     for (int x = 0; x < oldData.rhinoDatas.Count; x++)
        //     {
        //         if (newData.rhinoDatas[i].mapCord == oldData.rhinoDatas[x].mapCord)
        //         {
        //             var _newData = newData.rhinoDatas[i];
        //             _newData.rhinoColor = oldData.rhinoDatas[x].rhinoColor;

        //             newData.rhinoDatas[i] = _newData;
        //         }
        //     }
        // }

        newData.targetButterFlyDatas.AddRange(oldData.targetButterFlyDatas);
        newData.wrongTargetData = oldData.wrongTargetData;
        newData.levelIntroductionData = oldData.levelIntroductionData;
        newData.uiTutorial = oldData.uiTutorial;
    }

    private static void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}
