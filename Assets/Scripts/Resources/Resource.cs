using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Resource : MonoBehaviour
{
    public ResourceType ResourceType;
    public Color ResourceColor;
    public Sprite ResourceMiniSprite;
    public float ResourceAmount;
    public float ExtractionSpeed;
    public float ExtractionAmount;
    public bool IsAvailable;
}
