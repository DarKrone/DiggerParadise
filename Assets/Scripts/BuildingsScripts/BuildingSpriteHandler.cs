using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpriteHandler : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private ParticleSystem _particleSystem;
    private int curSpriteIndex = 0;

    public void UpdateBuildingSpriteToNext()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = _sprites[curSpriteIndex];
        _particleSystem.Play();
        curSpriteIndex++;
    }
}
