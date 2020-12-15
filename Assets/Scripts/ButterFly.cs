using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ButterFly : OccupyingEntity
{
    [HideInInspector]
    public Color color;
    public GamePhase movementPhase;
    public CurrentLevelData currentLevelData;

    private static int ColorShaderID = Shader.PropertyToID("_Color");

    public OccupyingEntitySet occupyingEntitySet;
    public ButterFlySet butterFlySet;
    public SoundEvent sound_butterfly_movement;
    public LevelCreationSettings creationSettings;

    [HideInInspector]
    public ButterFlyData butterFlyData;

    private bool forcedIdle = false;
    private WaitForSeconds waitForNewIdle;
    private Coroutine randomIdleCoroutine;

    private void OnEnable()
    {
        if (!hasData) return;


        occupyingEntitySet.AddDictionary(mapCord, this);
        occupyingEntitySet.AddList(this);

        butterFlySet.AddDictionary(mapCord, this);
        butterFlySet.AddList(this);
    }
    private void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();

        renderer.GetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.
        materialPropertyBlock.SetColor(ColorShaderID, color);
        renderer.SetPropertyBlock(materialPropertyBlock, 0); // Don't care about the 2nd material of wings.

        waitForNewIdle = new WaitForSeconds(Random.Range(creationSettings.butterFlyIdleAnimRepeatMin, creationSettings.butterFlyIdleAnimRepeatMax));
        RandomIdle(); // Move this to levelmanager
        randomIdleCoroutine = StartCoroutine(RandomIdleCoroutine());
    }
    private void OnDisable()
    {
        occupyingEntitySet.RemoveList(this);
        occupyingEntitySet.RemoveDictionary(mapCord);

        butterFlySet.RemoveList(this);
        butterFlySet.RemoveDictionary(mapCord);
    }
    public override void SetData()
    {
        hasData = true;
        OnEnable();
    }

    public override void ResetToDefault()
    {
        // if has a protection vs. 
        entityAnimator.enabled = false;
        entityAnimator.enabled = true;
    }
    public bool MoveToPlatform(Vector2 additiveCord)
    {
        var _newCord = mapCord + additiveCord;

        PlatformEntity _platform = platformEntity.GetNeighborPlatform(additiveCord);

        if (_platform == null || _platform.occupingEntity is Frog) return false;

        movementPhase.AddWait();
        sound_butterfly_movement.Raise();

        var _targetPosition = _platform.transform.position;
        _targetPosition.y = creationSettings.butterFlyDistanceToLily;

        // transform.rotation = Quaternion.Euler(0, Vector2.SignedAngle(additiveCord, Vector2.right) + 90, 0);
        transform.DORotate(new Vector3(0, Vector2.SignedAngle(additiveCord, Vector2.right) + 90, 0), creationSettings.butterFlyFlyDuration / 2);

        transform.DOMoveX(_targetPosition.x, creationSettings.butterFlyFlyDuration).OnComplete(() => OccupyPlatform(_newCord, _platform));
        transform.DOMoveZ(_targetPosition.z, creationSettings.butterFlyFlyDuration);
        transform.DOMoveY(_targetPosition.y + 0.75f, creationSettings.butterFlyFlyDuration / 2).SetLoops(2, LoopType.Yoyo);

        entityAnimator.SetTrigger("Fly");

        return true;
    }
    public void OccupyPlatform(Vector2 newMapCord, PlatformEntity platform)
    {

        entityAnimator.SetFloat("IdleBlend", 1);
        entityAnimator.SetTrigger("Idle");
        forcedIdle = true;

        platformEntity.occupingEntity = null;

        mapCord = newMapCord;
        platform.inComingEntity = this;
        platformEntity = platform;

        movementPhase.RemoveWait();
    }
    public virtual void Encounter()
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
        else if (platformEntity.occupingEntity is MergedButterFly)
        {
            platformEntity.inComingEntity = null;
            var _mergedButterFly = platformEntity.occupingEntity as MergedButterFly;


            gameObject.SetActive(false);

            if (_mergedButterFly.attachedEntity == null && attachedEntity is Bubble)
            {
                (attachedEntity as Bubble).Attach(_mergedButterFly);
            }

            _mergedButterFly.inputButterFlies.Add(this);
            _mergedButterFly.TryMerge();
        }
        else if (platformEntity.occupingEntity is ButterFly && platformEntity.occupingEntity != this)
        {
            platformEntity.inComingEntity = null;
            var _occupyingButterFly = platformEntity.occupingEntity as ButterFly;

            _occupyingButterFly.gameObject.SetActive(false);
            gameObject.SetActive(false);

            //create merged butterfly
            int _dataIndex;

            var _canMerge = currentLevelData.FindTarGetButterFly(_occupyingButterFly.color,
            color,
            out _dataIndex);

            GameObject _mergedObject;
            if (_canMerge)
                _mergedObject = currentLevelData.levelData.targetButterFlyDatas[_dataIndex].butterFlyObject;
            else
                _mergedObject = currentLevelData.levelData.wrongTargetData.butterFlyObject;


            var _mergedButterFly = GameObject.Instantiate(_mergedObject,
            transform.position,
            transform.rotation,
            transform.parent);

            var _mergedButterFlyComponent = _mergedButterFly.GetComponent<MergedButterFly>();

            _mergedButterFlyComponent.mapCord = mapCord;
            _mergedButterFlyComponent.platformEntity = platformEntity;
            _mergedButterFlyComponent.SetData();

            if (attachedEntity is Bubble)
            {
                (attachedEntity as Bubble).Attach(_mergedButterFlyComponent);
            }
            else if (_occupyingButterFly.attachedEntity is Bubble)
            {
                (_occupyingButterFly.attachedEntity as Bubble).Attach(_mergedButterFlyComponent);
            }

            _mergedButterFlyComponent.inputButterFlies.Add(_occupyingButterFly);
            _mergedButterFlyComponent.inputButterFlies.Add(this);
            _mergedButterFlyComponent.Merge(_dataIndex, _canMerge);

            platformEntity.occupingEntity = _mergedButterFlyComponent;
        }
    }

    public void RandomIdle()
    {
        var _random = Random.Range(0, 3);

        entityAnimator.SetFloat("IdleBlend", _random);
        entityAnimator.SetTrigger("Idle");
    }

    public IEnumerator RandomIdleCoroutine()
    {
        while (true)
        {
            yield return waitForNewIdle;

            if (forcedIdle)
                forcedIdle = false;
            else
                RandomIdle();
        }
    }
    public Color GetColor()
    {
        return color;
    }
}
