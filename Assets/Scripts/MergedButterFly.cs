using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FFStudio;

public class MergedButterFly : ButterFly
{
    public ParticleSystem particles;
    public GamePhase encounterPhase;
    public MergedButterFlySet mergedButterFlySet;
    public NewCreatedObjectsSet newCreatedObjectSet;
    public SoundEvent sound_merge_correct;
    public SoundEvent sound_merge_wrong;
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

        newCreatedObjectSet.AddDictionary(mapCord, this);
        newCreatedObjectSet.AddList(this);

        graphicTransform = transform.GetChild(0);
    }

    private void Start()
    {
        graphicTransform.SetParent(platformEntity.transform.GetChild(0));
    }

    private void OnParticleSystemStopped()
    {
        encounterPhase.RemoveWait();
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
        newCreatedObjectSet.RemoveList(this);
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
        else if (attachedEntity is Bubble && platformEntity.occupingEntity is Bubble) // ButterFly already has a bubble 
        {
            (platformEntity.occupingEntity as Bubble).Pop();
            (attachedEntity as Bubble).Pop();

            platformEntity.inComingEntity = null;
            platformEntity.occupingEntity = this;
        }
        else if (platformEntity.occupingEntity is Bubble)
        {
            platformEntity.inComingEntity = null;

            var _bubble = platformEntity.occupingEntity as Bubble;

            _bubble.Attach(this);

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

            if (_mergedButterFly.attachedEntity == null && attachedEntity is Bubble)
            {
                (attachedEntity as Bubble).Attach(_mergedButterFly);
            }

            graphicTransform.SetParent(transform);
            gameObject.SetActive(false);
        }
        else if (platformEntity.occupingEntity is ButterFly && platformEntity.occupingEntity != this)
        {
            var _butterFly = platformEntity.occupingEntity as ButterFly;

            platformEntity.inComingEntity = null;
            platformEntity.occupingEntity = this;

            inputButterFlies.Add(_butterFly);

            if (attachedEntity == null && _butterFly.attachedEntity is Bubble)
            {
                (attachedEntity as Bubble).Attach(this);
            }

            TryMerge();

            _butterFly.graphicTransform.SetParent(_butterFly.transform);
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

    public void Merge(int dataIndex, bool canMerge)
    {
        TargetButterFlyData _targetButterFlyData;

        if (canMerge)
        {
            _targetButterFlyData = currentLevelData.levelData.targetButterFlyDatas[dataIndex];
            RearrangeInputButterFlies(_targetButterFlyData);

            sound_merge_correct.Raise();
            particles.Play();
            encounterPhase.AddWait();
        }
        else
        {
            _targetButterFlyData = currentLevelData.levelData.wrongTargetData;
            sound_merge_wrong.Raise();
        }


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

    public void TryMerge()
    {
        var _targetButterFlies = currentLevelData.levelData.targetButterFlyDatas;

        int _targetButterFlyDataIndex;

        var _canMerge = FindTarGetButterFly(_targetButterFlies, out _targetButterFlyDataIndex);

        TargetButterFlyData _targetButterFlyData;

        if (_canMerge)
        {
            _targetButterFlyData = _targetButterFlies[_targetButterFlyDataIndex];
            RearrangeInputButterFlies(_targetButterFlyData);
            sound_merge_correct.Raise();
            particles.Play();
        }
        else
        {
            _targetButterFlyData = currentLevelData.levelData.wrongTargetData;
            sound_merge_wrong.Raise();
        }

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


    void RearrangeInputButterFlies(TargetButterFlyData targetButterFlyData)
    {
        List<ButterFly> temp = new List<ButterFly>(inputButterFlies.Count);
        List<Color> colors = new List<Color>(inputButterFlies.Count);

        for (int i = 0; i < inputButterFlies.Count; i++)
        {
            colors.Add(inputButterFlies[i].color);
        }

        for (int i = 0; i < targetButterFlyData.butterFlyColors.Count; i++)
        {
            int _index;
            var _find = colors.FindSameColor(targetButterFlyData.butterFlyColors[i], out _index);

            if (_find) temp.Add(inputButterFlies[_index]);

        }

        inputButterFlies.Clear();
        inputButterFlies.AddRange(temp);
    }
    bool FindTarGetButterFly(List<TargetButterFlyData> targetButterFlies, out int dataIndex)
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
