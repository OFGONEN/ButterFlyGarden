using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ButterFly : OccupyingEntity
{
    [HideInInspector]
    public Color color;
    public GamePhase movementPhase;

    private static int ColorShaderID = Shader.PropertyToID("_Color");

    public OccupyingEntitySet occupyingEntitySet;
    public ButterFlySet butterFlySet;
    public LevelCreationSettings creationSettings;
    public GameObject mergedButterFly;

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
        else if (platformEntity.occupingEntity is Bubble)
        {
            platformEntity.inComingEntity = null;

            var _bubble = platformEntity.occupingEntity as Bubble;

            attachedEntity = _bubble;
            _bubble.Attach(this);

            platformEntity.occupingEntity = this;
        }
        else if (platformEntity.occupingEntity is MergedButterFly)
        {
            platformEntity.inComingEntity = null;
            var _mergedButterFly = platformEntity.occupingEntity as MergedButterFly;


            gameObject.SetActive(false);

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
            var _mergedButterFly = GameObject.Instantiate(mergedButterFly, transform.position, transform.rotation, transform.parent);
            var _mergedButterFlyComponent = _mergedButterFly.GetComponent<MergedButterFly>();

            _mergedButterFlyComponent.mapCord = mapCord;
            _mergedButterFlyComponent.platformEntity = platformEntity;
            _mergedButterFlyComponent.SetData();

            _mergedButterFlyComponent.inputButterFlies.Add(_occupyingButterFly);
            _mergedButterFlyComponent.inputButterFlies.Add(this);
            _mergedButterFlyComponent.TryMerge();

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
