using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerLayerController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _shadow;
    [SerializeField] private int _defaultLayer = 0;
    [SerializeField] private int _overlapLayer = 0;
    [SerializeField] private float _offset = 0;

    SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultLayer = _spriteRenderer.sortingOrder;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("HidingCollider"))
        {
            if(transform.position.y + _offset > collision.ClosestPoint(transform.position + new Vector3(0, _offset, 0)).y)
            {
                TilemapRenderer tileMap;
                if(collision.TryGetComponent<TilemapRenderer>(out tileMap))
                {
                    _spriteRenderer.sortingOrder = tileMap.sortingOrder - 1;
                    _shadow.sortingOrder = tileMap.sortingOrder - 2;
                }
            }
            else
            {
                _spriteRenderer.sortingOrder = _overlapLayer;
                _shadow.sortingOrder = _overlapLayer - 1;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _spriteRenderer.sortingOrder = _defaultLayer;
        _shadow.sortingOrder = _defaultLayer - 1;
    }
}
