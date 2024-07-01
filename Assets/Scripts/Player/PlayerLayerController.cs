using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerLayerController : MonoBehaviour
{
    [SerializeField] private int _defaultLayer = 0;
    [SerializeField] private int _overlapLayer = 0;
    [SerializeField] private float _offset = 0;

    SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultLayer = _spriteRenderer.sortingOrder;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("HidingCollider"))
        {
            if(transform.position.y + _offset > collision.ClosestPoint(transform.position + new Vector3(0,_offset,0)).y)
                _spriteRenderer.sortingOrder = collision.GetComponent<TilemapRenderer>().sortingOrder -1;
            else
                _spriteRenderer.sortingOrder = _overlapLayer;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _spriteRenderer.sortingOrder = _defaultLayer;
    }
}
