using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResourceTaker : MonoBehaviour
{
    [Serializable]
    private class NeededResource
    {
        public ResourceType ResourceType;
        [Range(1f, 1000f)]
        public float ResourceAmountNeeded;
    }

    [SerializeField] private bool _debugMode = false;
    [SerializeField] private List<NeededResource> _neededResources;
    [SerializeField] private GameObject _canvasToSpawnTexts;
    [SerializeField] private GameObject _resourceNeededPrefab;
    [SerializeField] private float _takingSpeed = 3f;
    private List<NeededResource> _neededResourcesToDelete;
    private List<GameObject> _neededResourceObjects;

    private Coroutine _takingCoroutine;
    private bool _needUpdatingResourcesList = false;
    private bool _doneTaking = false;

    private AudioSource _audioSource;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _neededResourceObjects = new List<GameObject>();
        _neededResourcesToDelete = new List<NeededResource>();
        _audioSource = GetComponent<AudioSource>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateNeededResources();
    }

    private void FixedUpdate()
    {
        if (_neededResources.Count == 0 && !_doneTaking)
        {
            _doneTaking = true;
            DoneTaking();
            StopCoroutine(_takingCoroutine);
        }
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
        _neededResourceObjects.Clear();
    }

    private void GenerateNeededTextObjectByIndex(int currentObjectIndex)
    {
        Vector3 spawnPos = Vector3.zero;
        GameObject spawnedObj = Instantiate(_resourceNeededPrefab, spawnPos, _resourceNeededPrefab.transform.rotation, _canvasToSpawnTexts.transform);
        _neededResourceObjects.Add(spawnedObj);
        ConfigureNeededTextByIndex(currentObjectIndex);
        ConfigureNeededImageByIndex(currentObjectIndex);
    }

    private void ConfigureNeededTextByIndex(int currentObjectIndex)
    {
        UpdateNeededTextByIndex(currentObjectIndex);
        TextMeshProUGUI neededResourceText = _neededResourceObjects[currentObjectIndex].GetComponent<ResourceNeeded>().NeededText;
        neededResourceText.color = Storage.Instance.GetResourceColorByType(_neededResources[currentObjectIndex].ResourceType);
    }

    private void UpdateNeededTextByIndex(int currentObjectIndex)
    {
        _neededResourceObjects[currentObjectIndex].GetComponent<ResourceNeeded>().NeededText.text = _neededResources[currentObjectIndex].ResourceAmountNeeded.ToString();
    }

    private void ConfigureNeededImageByIndex(int currentObjectIndex)
    {
        Image neededResourceImage = _neededResourceObjects[currentObjectIndex].GetComponent<ResourceNeeded>().ResourceImage;
        neededResourceImage.sprite = Storage.Instance.GetResourceSpriteByType(_neededResources[currentObjectIndex].ResourceType);
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
            for (int i = 0; i < _neededResources.Count; i++)
            {
                TakeResourceByIndex(i);
                UpdateNeededTextByIndex(i);
            }
            if (_needUpdatingResourcesList)
                UpdateNeededResources();

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
        }
        else
        {
            amountToRemove = Storage.Instance.CheckResourceAmount(_neededResources[currentResourceIndex].ResourceType);
        }

        if (amountToRemove == 0)
            return;

        _audioSource.Play();
        Storage.Instance.RemoveFromStorage(amountToRemove, _neededResources[currentResourceIndex].ResourceType);
        NotificationHandler.Instance.ShowNotification(PlayerMovement.Instance.gameObject, _neededResources[currentResourceIndex].ResourceType, -amountToRemove);
        if (_neededResources[currentResourceIndex].ResourceAmountNeeded <= 0)
        {
            _neededResourcesToDelete.Add(_neededResources[currentResourceIndex]);
            _needUpdatingResourcesList = true;
        }
    }

    private void DoneTaking()
    {
        if (_debugMode)
            Debug.Log($"Done taking - {gameObject}");
        _collider.enabled = false;
        _spriteRenderer.enabled = false;
        if (gameObject.TryGetComponent<IComplitedConstruction>(out IComplitedConstruction completedObject))
        {
            completedObject.ConstructionCompleted();
        }
        else
        {
            Debug.LogError($"Didnt find IComplitedConstruction on object - {gameObject.name}");
        }
    }
}
