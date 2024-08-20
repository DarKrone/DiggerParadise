using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtSpriteController : MonoBehaviour, IComplitedConstruction
{
    [SerializeField] GameObject ArtSprite;
    [SerializeField] private GameObject _artTemplate;
    public void ConstructionCompleted()
    {
        ArtSprite.SetActive(true);
        _artTemplate.SetActive(false);
        gameObject.GetComponent<EnableCostume>().SetCostumeToEnable();
    }
}
