using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceTaker : MonoBehaviour
{
    [SerializeField] private bool _debugMode = false;
    [SerializeField] private List<ResourceType> _resourceType;
    [SerializeField] private List<float> _resourceNeeded;
    [SerializeField] private GameObject _canvasToSpawn;
    [SerializeField] private float _takingSpeed = 3f;
    private List<TextMeshProUGUI> _neededTexts;

    private float _spawnYOffset = 0.6f;
    private float _fontSize = 0.15f;
    private Vector2 _wightAndHeight = new Vector2(2f, 0.5f);

    private Coroutine _takingCoroutine;
    private float _removeAmount = 1f;
    private bool _doneTaking = false;

    private void Start()
    {
        if (_resourceType.Count != _resourceNeeded.Count)
            Debug.LogError($"The number of resource types does not match the number of requested quantity at - {gameObject}");

        _neededTexts = new List<TextMeshProUGUI>();

        for (int i = 0; i < _resourceType.Count; i++)
        {
            GameObject textObject = new GameObject($"{_resourceType[i]} text");
            textObject.transform.SetParent(_canvasToSpawn.transform);
            textObject.transform.localScale = Vector3.one;
            textObject.AddComponent<CanvasRenderer>();
            TextMeshProUGUI resourceNeededText = textObject.AddComponent<TextMeshProUGUI>();
            resourceNeededText.text = $"Need {_resourceType[i]} : {_resourceNeeded[i]}";
            resourceNeededText.fontSize = _fontSize;
            resourceNeededText.alignment = TextAlignmentOptions.Center;
            resourceNeededText.rectTransform.localPosition = Vector3.zero;
            resourceNeededText.rectTransform.sizeDelta = _wightAndHeight;
            Vector3 spawnPos = _canvasToSpawn.transform.position;
            spawnPos += new Vector3(0, _spawnYOffset * i, 0);
            textObject.transform.position = spawnPos;
            //GameObject createdText = Instantiate(textObject, spawnPos, textObject.transform.rotation, _canvasToSpawn.transform);
            _neededTexts.Add(resourceNeededText);
        }
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
            bool allEmpty = true;
            for (int i = 0; i < _resourceType.Count; i++)
            {
                if (_resourceNeeded[i] > 0)
                {
                    allEmpty = false;
                    if (Storage.CheckResourceAmount(_resourceType[i]) >= _removeAmount)
                    {
                        _resourceNeeded[i] -= _removeAmount;
                        Storage.RemoveFromStorage(_removeAmount, _resourceType[i]);
                        UpdateNeededAmounts();
                    }
                }
            }

            if (allEmpty)
            {
                _doneTaking = true;
                DoneTaking();
                StopCoroutine(_takingCoroutine);
            }
        }
    }

    private void UpdateNeededAmounts()
    {
        for (int i = 0; i < _resourceType.Count; i++)
        {
            _neededTexts[i].text = $"Need {_resourceType[i]} : {_resourceNeeded[i]}";
        }
    }

    private void DoneTaking()
    {
        if (_debugMode)
            Debug.Log($"Done taking - {gameObject}");
    }
}
