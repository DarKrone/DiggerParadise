using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class InstructionStart : MonoBehaviour
{
    [SerializeField] private GameObject _firstInstructionWindow;
    [SerializeField] private bool _startInFirstSession;
    private void Start()
    {
        if (YandexGame.SDKEnabled)
        {
            StartInstruction();
        }
    }
    private void OnEnable()
    {
        YandexGame.GetDataEvent += StartInstruction;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= StartInstruction;
    }

    private void StartInstruction()
    {
        if(!_startInFirstSession)
            _firstInstructionWindow.SetActive(true);
        else if (YandexGame.savesData.isFirstSession)
            _firstInstructionWindow.SetActive(true);
    }
}
