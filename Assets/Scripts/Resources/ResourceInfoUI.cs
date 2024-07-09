using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceInfoUI : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI ResourceAmountText;
    [SerializeField] public Image ResourceImage;
    public ResourceType ResourceType;
}
