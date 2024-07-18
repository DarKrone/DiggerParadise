using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OreFinder : MonoBehaviour
{
    [SerializeField] private float _maxFinderRaduis = 5f;
    private GameObject ClosestOreObject = null;
    private CircleCollider2D _circleCollider;

    private void Start()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
    }
    public void FindClosestPoint(ref GameObject findedPoint)
    {
        if (ClosestOreObject == null)
            ColliderExpansion();

        findedPoint = ClosestOreObject;
        ClosestOreObject = null;
    }
    private void ColliderExpansion()
    {
        try
        {
            _circleCollider.radius += 1f;
            _circleCollider.radius = Mathf.Clamp(_circleCollider.radius, 0, _maxFinderRaduis);
        }
        catch { }
    }
    private void ColliderToNormal()
    {
        _circleCollider.radius = 0.1f;
        //ClosestOrePoint = Vector2.zero;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Resource"))
        {
            ClosestOreObject = collision.gameObject;
            ColliderToNormal();
        }
    }
}
