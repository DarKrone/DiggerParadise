using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtSpriteController : MonoBehaviour, IComplitedConstruction
{
    [SerializeField] GameObject ArtSprite;
    public void ConstructionCompleted()
    {
        ArtSprite.SetActive(true);
    }
}
