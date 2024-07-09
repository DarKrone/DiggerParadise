using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpriteHandler : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private int curSpriteIndex = 0;

    public void UpdateBuildingSpriteToNext()
    {
        if (curSpriteIndex != _sprites.Count - 1)
            _particleSystem.Play();
        if (curSpriteIndex < _sprites.Count - 1)
            curSpriteIndex++;
        gameObject.GetComponent<SpriteRenderer>().sprite = _sprites[curSpriteIndex];
    }
}
