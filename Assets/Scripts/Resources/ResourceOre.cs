using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceOre : MonoBehaviour
{
    [SerializeField] private bool _debugMode;
    [SerializeField] private float _oreCooldown = 10f;
    [SerializeField] public float DefaultResourceAmount = 15f;
    [SerializeField] private float _resourceAmountUpgradeStep = 2f;
    [SerializeField] private float _higherAmountLimit = 100f;
    private Tilemap _tilemap;
    private Vector3Int _tilePos;
    public ResourceType ResourceType;
    public float ResourceAmount;
    public bool isFullyExtracted = false;
    private Collider2D _collider;

    private void Start()
    {
        SetResourceAmount();
        ResourceAmount = DefaultResourceAmount;
        _collider = GetComponent<BoxCollider2D>();
        _tilemap = GameObject.Find("Decor").GetComponent<Tilemap>();
        _tilePos = _tilemap.WorldToCell(transform.position);
    }

    public void SetResourceAmount()
    {
        ResourceAmount = DefaultResourceAmount;
    }

    public void AmountCheck(string tag)
    {
        if (ResourceAmount <= 0 && !isFullyExtracted)
        {
            isFullyExtracted = true;
            if(tag == "Player")
                RewardedAds.Instance.TryADSAfterResourceOreExtracting();
            _collider.enabled = false;
            _tilemap.SetColor(_tilePos, new Color(1f, 1f, 1f, 0.3f));
            StartCoroutine(OreCooldown());
            if (_debugMode)
                Debug.Log($"Resource {gameObject.name} has been full extracted");
        }
    }

    private IEnumerator OreCooldown()
    {
        yield return new WaitForSeconds(_oreCooldown);
        _tilemap.SetColor(_tilePos, new Color(1f, 1f, 1f, 1f));
        _collider.enabled = true;
        isFullyExtracted = false;
        if (DefaultResourceAmount < _higherAmountLimit)
            DefaultResourceAmount += _resourceAmountUpgradeStep;
        ResourceAmount = DefaultResourceAmount;
    }
}

