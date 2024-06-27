using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceTaker : MonoBehaviour
{
    [Serializable]
    public class NeededResource
    {
        public ResourceType ResourceType;
        public float ResourceAmountNeeded;
    }

    [SerializeField] private bool _debugMode = false;
    [SerializeField] private List<NeededResource> _neededResource;
    [SerializeField] private GameObject _canvasToSpawn;
    [SerializeField] private float _takingSpeed = 3f;
    private List<TextMeshProUGUI> _neededTexts;

    private const float SPAWN_Y_OFFSET = 0.6f;
    private const float FONT_SIZE = 0.15f;
    private const float WIDTH = 2f;
    private const float HEIGHT = 0.5f;

    private Coroutine _takingCoroutine;
    private float _removeAmount = 1f;
    private bool _doneTaking = false;

    private void Start()
    {
        _neededTexts = new List<TextMeshProUGUI>();
        UpdateNeededResources();
    }

    private void UpdateNeededResources()
    {
        RemoveAllNeededResourcesObjects();
        for (int i = 0; i < _neededResource.Count; i++)
        {
            GenerateNeededTextObject(i);
        }
    }

    private void RemoveAllNeededResourcesObjects()
    {
        while (_canvasToSpawn.transform.childCount > 0)
             DestroyImmediate(_canvasToSpawn.transform.GetChild(0).gameObject);
        _neededTexts.Clear();
    }

    private void GenerateNeededTextObject(int currentObjectIndex)
    {
        GameObject neededResource = new GameObject($"{_neededResource[currentObjectIndex].ResourceType} text");
        neededResource.transform.SetParent(_canvasToSpawn.transform);
        neededResource.transform.localScale = Vector3.one;
        neededResource.AddComponent<CanvasRenderer>();
        ConfigureNeededText(currentObjectIndex, neededResource.AddComponent<TextMeshProUGUI>());
        SetNeededTextGameObjectPos(currentObjectIndex, neededResource);
    }


    private void ConfigureNeededText(int currentObjectIndex, TextMeshProUGUI neededResourceText)
    {
        neededResourceText.text = $"Need {_neededResource[currentObjectIndex].ResourceType} " +
                                  $": {_neededResource[currentObjectIndex].ResourceAmountNeeded}";
        neededResourceText.fontSize = FONT_SIZE;
        neededResourceText.alignment = TextAlignmentOptions.Center;
        neededResourceText.rectTransform.localPosition = Vector3.zero;
        neededResourceText.rectTransform.sizeDelta = new Vector2(WIDTH, HEIGHT);
        _neededTexts.Add(neededResourceText);
    }

    private void SetNeededTextGameObjectPos(int currentObjectIndex, GameObject textObject)
    {
        Vector3 spawnPos = _canvasToSpawn.transform.position;
        spawnPos += new Vector3(0, SPAWN_Y_OFFSET * currentObjectIndex, 0);
        textObject.transform.position = spawnPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || _doneTaking)
            return;
        if (_debugMode)
            Debug.Log($"Start taking - {gameObject}");
        _takingCoroutine = StartCoroutine(TakingResource());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || _doneTaking)
            return;
        if (_debugMode)
            Debug.Log($"Stop taking - {gameObject}");
        StopCoroutine(_takingCoroutine);
    }

    private IEnumerator TakingResource()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / _takingSpeed);
            bool allResourcesDone = true;
            for (int i = 0; i < _neededResource.Count; i++)
            {
                allResourcesDone = false;
                TakeResource(i);
            }

            if (allResourcesDone)
            {
                _doneTaking = true;
                DoneTaking();
                StopCoroutine(_takingCoroutine);
            }
        }
    }

    private void TakeResource(int currentResourceIndex)
    {
        if (Storage.CheckResourceAmount(_neededResource[currentResourceIndex].ResourceType) >= _removeAmount)
        {
            _neededResource[currentResourceIndex].ResourceAmountNeeded -= _removeAmount;
            Storage.RemoveFromStorage(_removeAmount, _neededResource[currentResourceIndex].ResourceType);
            if (_neededResource[currentResourceIndex].ResourceAmountNeeded <= 0)
            {
                _neededResource.RemoveAt(currentResourceIndex);
                UpdateNeededResources();
            }
            UpdateNeededAmounts();
        }
    }

    private void UpdateNeededAmounts()
    {
        for (int i = 0; i < _neededResource.Count; i++)
        {
            _neededTexts[i].text = $"Need {_neededResource[i].ResourceType} : {_neededResource[i].ResourceAmountNeeded}";
        }
    }

    private void DoneTaking()
    {
        if (_debugMode)
            Debug.Log($"Done taking - {gameObject}");
    }
}
