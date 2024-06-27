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
        [Range(1f, 10000f)]
        public float ResourceAmountNeeded;
    }

    [SerializeField] private bool _debugMode = false;
    [SerializeField] private List<NeededResource> _neededResource;
    [SerializeField] private GameObject _canvasToSpawn;
    [SerializeField] private float _takingSpeed = 3f;
    private List<NeededResource> _neededResourcedToDelete;
    private List<TextMeshProUGUI> _neededTexts;

    private const float SPAWN_Y_OFFSET = 0.6f;
    private const float FONT_SIZE = 0.15f;
    private const float WIDTH = 2f;
    private const float HEIGHT = 0.5f;

    private Coroutine _takingCoroutine;
    private bool _needUpdatingResourcesList = false;
    private float _removeAmount = 1f;
    private bool _doneTaking = false;

    private void Start()
    {
        _neededTexts = new List<TextMeshProUGUI>();
        _neededResourcedToDelete = new List<NeededResource>();
        UpdateNeededResources();
    }

    private void UpdateNeededResources()
    {
        _needUpdatingResourcesList = false;
        ClearAllNeededResourceObjects();
        
        for (int i = 0; i < _neededResource.Count; i++)
        {
            if (_neededResource[i].ResourceAmountNeeded > 0)
                GenerateNeededTextObject(i);
        }
    }

    private void ClearAllNeededResourceObjects()
    {
        while (_canvasToSpawn.transform.childCount > 0)
        {
            DestroyImmediate(_canvasToSpawn.transform.GetChild(0).gameObject);
        }
        for (int i = 0; i < _neededResourcedToDelete.Count; i++)
        {
            _neededResource.Remove(_neededResourcedToDelete[i]);
        }
        _neededResourcedToDelete.Clear();
        _neededTexts.Clear();
    }

    private void GenerateNeededTextObject(int currentObjectIndex)
    {
        GameObject neededResource = new GameObject($"{_neededResource[currentObjectIndex].ResourceType} text");
        neededResource.transform.SetParent(_canvasToSpawn.transform);
        neededResource.transform.localScale = Vector3.one;
        neededResource.AddComponent<CanvasRenderer>();
        _neededTexts.Add(neededResource.AddComponent<TextMeshProUGUI>());
        neededResource.transform.position = GetNeededTextGameObjectPos(currentObjectIndex);
        ConfigureNeededText(currentObjectIndex);
    }

    private void ConfigureNeededText(int currentObjectIndex)
    {
        UpdateNeededText(currentObjectIndex);
        TextMeshProUGUI neededResourceText = _neededTexts[currentObjectIndex];
        neededResourceText.fontSize = FONT_SIZE;
        neededResourceText.alignment = TextAlignmentOptions.Center;
        neededResourceText.rectTransform.sizeDelta = new Vector2(WIDTH, HEIGHT);
    }

    private Vector3 GetNeededTextGameObjectPos(int currentObjectIndex)
    {
        Vector3 spawnPos = _canvasToSpawn.transform.position;
        spawnPos += new Vector3(0, SPAWN_Y_OFFSET * currentObjectIndex, 0);
        return spawnPos;
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
                UpdateNeededText(i);
            }
            if (_needUpdatingResourcesList)
                UpdateNeededResources();
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
                _neededResourcedToDelete.Add(_neededResource[currentResourceIndex]);
                _needUpdatingResourcesList = true;
            }
        }
    }

    private void UpdateNeededText(int currentObjectIndex)
    {
        _neededTexts[currentObjectIndex].text = $"Need {_neededResource[currentObjectIndex].ResourceType} " +
                                           $": {_neededResource[currentObjectIndex].ResourceAmountNeeded}";
    }

    protected virtual void DoneTaking()
    {
        if (_debugMode)
            Debug.Log($"Done taking - {gameObject}");
    }
}
