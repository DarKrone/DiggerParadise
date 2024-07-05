using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceTaker : MonoBehaviour
{
    [Serializable]
    private class NeededResource
    {
        public ResourceType ResourceType;
        [Range(1f, 1000f)]
        public float ResourceAmountNeeded;
    }

    [Serializable]
    private class NeededTextSettings
    {
        public float SPAWN_Y_OFFSET = 0.6f;
        public float FONT_SIZE = 0.21f;
        public float WIDTH = 2f;
        public float HEIGHT = 0.5f;
    }


    [SerializeField] private bool _debugMode = false;
    [SerializeField] private List<NeededResource> _neededResources;
    [SerializeField] private GameObject _canvasToSpawnTexts;
    [SerializeField] private float _takingSpeed = 3f;
    [SerializeField] private NeededTextSettings _textSettings;
    private List<NeededResource> _neededResourcesToDelete;
    private List<TextMeshProUGUI> _neededTexts;

    private Coroutine _takingCoroutine;
    private bool _needUpdatingResourcesList = false;
    private bool _doneTaking = false;

    private AudioSource _audioSource;

    private void Start()
    {
        _neededTexts = new List<TextMeshProUGUI>();
        _neededResourcesToDelete = new List<NeededResource>();
        _audioSource = GetComponent<AudioSource>();
        UpdateNeededResources();
    }

    private void UpdateNeededResources()
    {
        _needUpdatingResourcesList = false;
        ClearAllNeededResourceObjects();
        
        for (int i = 0; i < _neededResources.Count; i++)
        {
            if (_neededResources[i].ResourceAmountNeeded > 0)
                GenerateNeededTextObjectByIndex(i);
        }
    }

    private void ClearAllNeededResourceObjects()
    {
        while (_canvasToSpawnTexts.transform.childCount > 0)
        {
            DestroyImmediate(_canvasToSpawnTexts.transform.GetChild(0).gameObject);
        }
        for (int i = 0; i < _neededResourcesToDelete.Count; i++)
        {
            _neededResources.Remove(_neededResourcesToDelete[i]);
        }
        _neededResourcesToDelete.Clear();
        _neededTexts.Clear();
    }

    private void GenerateNeededTextObjectByIndex(int currentObjectIndex)
    {
        GameObject neededResource = new GameObject($"{_neededResources[currentObjectIndex].ResourceType} text");
        neededResource.transform.SetParent(_canvasToSpawnTexts.transform);
        neededResource.transform.localScale = Vector3.one;
        neededResource.AddComponent<CanvasRenderer>();
        _neededTexts.Add(neededResource.AddComponent<TextMeshProUGUI>());
        neededResource.transform.position = GetNeededTextGameObjectPosByIndex(currentObjectIndex);
        ConfigureNeededTextByIndex(currentObjectIndex);
    }

    private Vector3 GetNeededTextGameObjectPosByIndex(int currentObjectIndex)
    {
        Vector3 spawnPos = _canvasToSpawnTexts.transform.position;
        spawnPos += new Vector3(0, _textSettings.SPAWN_Y_OFFSET * currentObjectIndex, 0);
        return spawnPos;
    }

    private void ConfigureNeededTextByIndex(int currentObjectIndex)
    {
        UpdateNeededTextByIndex(currentObjectIndex);
        TextMeshProUGUI neededResourceText = _neededTexts[currentObjectIndex];
        neededResourceText.color = Storage.Instance.GetResourceColorByType(_neededResources[currentObjectIndex].ResourceType);
        neededResourceText.fontSize = _textSettings.FONT_SIZE;
        neededResourceText.alignment = TextAlignmentOptions.Center;
        neededResourceText.rectTransform.sizeDelta = new Vector2(_textSettings.WIDTH, _textSettings.HEIGHT);
    }

    private void UpdateNeededTextByIndex(int currentObjectIndex)
    {
        _neededTexts[currentObjectIndex].text = $"Need {_neededResources[currentObjectIndex].ResourceType} " +
                                           $": {_neededResources[currentObjectIndex].ResourceAmountNeeded}";
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
            for (int i = 0; i < _neededResources.Count; i++)
            {
                allResourcesDone = false;
                TakeResourceByIndex(i);
                UpdateNeededTextByIndex(i);
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

    private void TakeResourceByIndex(int currentResourceIndex)
    {
        float amountToRemove = Storage.Instance.GetExtractionAmountByType(_neededResources[currentResourceIndex].ResourceType);
        if (Storage.Instance.CheckResourceAmount(_neededResources[currentResourceIndex].ResourceType) >= amountToRemove)
        {
            if (amountToRemove > _neededResources[currentResourceIndex].ResourceAmountNeeded)
            {
                amountToRemove = _neededResources[currentResourceIndex].ResourceAmountNeeded;
            }
            _neededResources[currentResourceIndex].ResourceAmountNeeded -= amountToRemove;
            _audioSource.Play();
        }
        else
        {
            amountToRemove = Storage.Instance.CheckResourceAmount(_neededResources[currentResourceIndex].ResourceType);
        }

        if (amountToRemove == 0)
            return;

        Storage.Instance.RemoveFromStorage(amountToRemove, _neededResources[currentResourceIndex].ResourceType);
        NotificationHandler.Instance.ShowNotification(PlayerMovement.Instance.gameObject, _neededResources[currentResourceIndex].ResourceType, -amountToRemove);
        if (_neededResources[currentResourceIndex].ResourceAmountNeeded <= 0)
        {
            _neededResourcesToDelete.Add(_neededResources[currentResourceIndex]);
            _needUpdatingResourcesList = true;
        }
    }

    protected virtual void DoneTaking()
    {
        if (_debugMode)
            Debug.Log($"Done taking - {gameObject}");
    }
}
