using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceOre : MonoBehaviour
{
    [SerializeField] private bool _debugMode;
    [SerializeField] private float _oreCooldown = 10f;
    [SerializeField] private float _defaultResourceAmount = 15f;
    public ResourceType ResourceType;
    public float ResourceAmount;
    public bool isFullyExtracted = false;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        ResourceAmount = _defaultResourceAmount;
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (ResourceAmount <= 0 && !isFullyExtracted)
        {
            isFullyExtracted = true;
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
            _collider.enabled = false;
            _spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);
            StartCoroutine(OreCooldown());
            if (_debugMode)
                Debug.Log($"Resource {gameObject.name} has been full extracted");
        }
    }

    private IEnumerator OreCooldown()
    {
        yield return new WaitForSeconds(_oreCooldown);
        _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        _collider.enabled = true;
        isFullyExtracted = false;
        ResourceAmount = _defaultResourceAmount;
    }
}

