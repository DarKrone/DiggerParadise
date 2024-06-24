using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Start()
    {
        Instance = this;
    }

    [SerializeField] private TextMeshProUGUI _copperText;

    public void UpdateUI()
    {
        _copperText.text = "Copper: " + Storage.CopperResource;
    }
}
