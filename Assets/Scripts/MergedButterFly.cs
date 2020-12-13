using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FFStudio;

public class MergedButterFly : ButterFly
{
    public CurrentLevelData currentLevelData;
    public MergedButterFlySet mergedButterFlySet;
    public NewCreatedObjectsSet newCreatedObjectSet;
    public Texture2D patternTexture;
    public List<ButterFly> inputButterFlies;

    private static readonly int inputButterflyColorsShaderID = Shader.PropertyToID("inputButterflyColors");
    private static readonly int numberOfInputButterfliesShaderID = Shader.PropertyToID("numberOfInputButterflies");

    private void OnEnable()
    {

        if (!hasData) return;

        materialPropertyBlock = new MaterialPropertyBlock();

        renderer.GetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.
        materialPropertyBlock.SetTexture("_MainTex", patternTexture);

        // Array sizes are determined the first time they are set, so set it to max beforehand.
        materialPropertyBlock.SetFloatArray(inputButterflyColorsShaderID, new float[3 * 10]);
        renderer.SetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.


        occupyingEntitySet.AddDictionary(mapCord, this);
        occupyingEntitySet.AddList(this);

        butterFlySet.AddDictionary(mapCord, this);
        butterFlySet.AddList(this);

        mergedButterFlySet.AddDictionary(mapCord, this);
        mergedButterFlySet.AddList(this);

        newCreatedObjectSet.AddDictionary(mapCord, gameObject);
        newCreatedObjectSet.AddList(gameObject);
    }
    private void OnDisable()
    {
        occupyingEntitySet.RemoveList(this);
        occupyingEntitySet.RemoveDictionary(mapCord);

        butterFlySet.RemoveList(this);
        butterFlySet.RemoveDictionary(mapCord);

        mergedButterFlySet.RemoveList(this);
        mergedButterFlySet.RemoveDictionary(mapCord);
    }
    private void OnDestroy()
    {
        newCreatedObjectSet.RemoveDictionary(mapCord);
        newCreatedObjectSet.RemoveList(gameObject);
    }
    public override void SetData()
    {
        hasData = true;
        OnEnable();
    }
    public override void Encounter()
    {
        if (platformEntity.occupingEntity == null)
        {
            platformEntity.inComingEntity = null;
            platformEntity.occupingEntity = this;
        }
        else if (platformEntity.occupingEntity is MergedButterFly && platformEntity.occupingEntity != this)
        {
            platformEntity.inComingEntity = null;
            var _mergedButterFly = platformEntity.occupingEntity as MergedButterFly;

            foreach (var _butterFly in inputButterFlies)
            {
                _mergedButterFly.inputButterFlies.Add(_butterFly);
            }

            _mergedButterFly.TryMerge();

            gameObject.SetActive(false);
        }
        else if (platformEntity.occupingEntity is ButterFly && platformEntity.occupingEntity != this)
        {
            var _butterFly = platformEntity.occupingEntity as ButterFly;

            platformEntity.inComingEntity = null;
            platformEntity.occupingEntity = this;

            inputButterFlies.Add(_butterFly);
            TryMerge();

            _butterFly.gameObject.SetActive(false);
        }
    }
    public void Merge()
    {
        renderer.GetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.
        materialPropertyBlock.SetTexture("_MainTex", patternTexture);
        materialPropertyBlock.SetFloatArray(inputButterflyColorsShaderID,
                                                  inputButterFlies.Select(bfGraphics => bfGraphics.GetColor())
                                                                  .SelectMany(color => new[] { color.r, color.g, color.b })
                                                                  .ToArray());
        materialPropertyBlock.SetInt(numberOfInputButterfliesShaderID, inputButterFlies.Count);
        renderer.SetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.
    }

    public void TryMerge()
    {
        var _targetButterFlies = currentLevelData.levelData.targetButterFlyDatas;

        int _targetButterFlyDataIndex;

        var _canMerge = FindTarGetButterFly(_targetButterFlies, out _targetButterFlyDataIndex);


        if (_canMerge)
        {

            var _targetButterFlyData = _targetButterFlies[_targetButterFlyDataIndex];

            if (inputButterFlies.Count <= _targetButterFlyData.butterFlyPatterns.Count)
            {
                patternTexture = _targetButterFlyData.butterFlyPatterns[inputButterFlies.Count - 2];
            }
            else
            {
                patternTexture = _targetButterFlyData.finalPattern;
            }

            Merge();
        }
        else
        {
            Debug.Log("False merge");
        }
    }


    public bool FindTarGetButterFly(List<TargetButterFlyData> targetButterFlies, out int dataIndex)
    {
        for (int i = 0; i < targetButterFlies.Count; i++)
        {
            var _find = true;

            var _targetButterFlyData = targetButterFlies[i];

            for (int x = 0; x < inputButterFlies.Count; x++)
            {
                _find &= _targetButterFlyData.butterFlyColors.FindSameColor(inputButterFlies[x].color);
            }

            if (_find)
            {
                dataIndex = i;
                return true;
            }
        }

        dataIndex = -1;
        return false;
    }

}
