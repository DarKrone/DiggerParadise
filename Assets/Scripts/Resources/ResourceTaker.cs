using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class NeededResource
    {
        public ResourceType ResourceType;
        [Range(1f, 1000f)]
        public float ResourceAmountNeeded;
    }
public class ResourceTaker : MonoBehaviour
{

    [SerializeField] private bool _debugMode = false;
    public List<NeededResource> NeededResources;
    [SerializeField] private GameObject _canvasToSpawnTexts;
    [SerializeField] private GameObject _resourceNeededPrefab;
    [SerializeField] private float _takingSpeed = 3f;
    private List<NeededResource> _neededResourcesToDelete;
    private List<GameObject> _neededResourceObjects;

    private Coroutine _takingCoroutine;
    private bool _needUpdatingResourcesList = false;
    public bool IsDoneTaking = false;

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

        IsDoneTaking = NeededResources.Count <= 0;

        if (IsDoneTaking)
            DoneTaking();
        else
            UpdateNeededResources();
    }

    private void FixedUpdate()
    {
        if (NeededResources.Count == 0 && !IsDoneTaking)
        {
            IsDoneTaking = true;
            DoneTaking();
            StopCoroutine(_takingCoroutine);
        }
    }

    private void UpdateNeededResources()
    {
        _needUpdatingResourcesList = false;
        ClearAllNeededResourceObjects();
        
        for (int i = 0; i < NeededResources.Count; i++)
        {
            if (NeededResources[i].ResourceAmountNeeded > 0)
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
            NeededResources.Remove(_neededResourcesToDelete[i]);
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
        TextMeshProUGUI neededResourceText = _neededResourceObjects[currentObjectIndex].GetComponent<ResourceInfoUI>().ResourceAmountText;
        neededResourceText.color = ResourceManager.Instance.GetResourceByType(NeededResources[currentObjectIndex].ResourceType).ResourceColor;
    }

    private void UpdateNeededTextByIndex(int currentObjectIndex)
    {
        ResourceInfoUI resource = _neededResourceObjects[currentObjectIndex].GetComponent<ResourceInfoUI>();
        resource.ResourceAmountText.text = NeededResources[currentObjectIndex].ResourceAmountNeeded.ToString();
    }

    private void ConfigureNeededImageByIndex(int currentObjectIndex)
    {
        Image neededResourceImage = _neededResourceObjects[currentObjectIndex].GetComponent<ResourceInfoUI>().ResourceImage;
        neededResourceImage.sprite = ResourceManager.Instance.GetResourceByType(NeededResources[currentObjectIndex].ResourceType).ResourceMiniSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || IsDoneTaking)
            return;
        if (_debugMode)
            Debug.Log($"Start taking - {gameObject}");
        _takingCoroutine = StartCoroutine(TakingResource());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || IsDoneTaking)
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
            for (int i = 0; i < NeededResources.Count; i++)
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
        float amountToRemove = ResourceManager.Instance.GetResourceByType(NeededResources[currentResourceIndex].ResourceType).ExtractionAmount;
        if (ResourceManager.Instance.GetResourceByType(NeededResources[currentResourceIndex].ResourceType).ResourceAmount >= amountToRemove)
        {
            if (amountToRemove > NeededResources[currentResourceIndex].ResourceAmountNeeded)
            {
                amountToRemove = NeededResources[currentResourceIndex].ResourceAmountNeeded;
            }
        }
        else
        {
            amountToRemove = ResourceManager.Instance.GetResourceByType(NeededResources[currentResourceIndex].ResourceType).ResourceAmount;
        }

        if (amountToRemove == 0)
            return;

        _audioSource.Play();
        NeededResources[currentResourceIndex].ResourceAmountNeeded -= amountToRemove;
        Resource resource = ResourceManager.Instance.GetResourceByType(NeededResources[currentResourceIndex].ResourceType);
        resource.ResourceAmount -= amountToRemove;
        if (!resource.IsAvailable)
        {
            GameManager.Instance.UpdateResourcesList();
        }
        GameManager.Instance.UpdateUI();
        NotificationHandler.Instance.ShowNotification(PlayerMovement.Instance.gameObject, NeededResources[currentResourceIndex].ResourceType, -amountToRemove);
        if (NeededResources[currentResourceIndex].ResourceAmountNeeded <= 0)
        {
            _neededResourcesToDelete.Add(NeededResources[currentResourceIndex]);
            _needUpdatingResourcesList = true;
        }
    }

    public void DoneTaking()
    {
        if (_debugMode)
            Debug.Log($"Done taking - {gameObject}");
        try
        {
            _collider.enabled = false;
            _spriteRenderer.enabled = false;
        }
        catch{ }
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
