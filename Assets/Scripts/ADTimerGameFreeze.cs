using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YG;

public class ADTimerGameFreeze : MonoBehaviour
{
    [SerializeField] private GameObject _eventSystem;
    [SerializeField] private PlayerMovement _movement;

    public void FreezeGame()
    {
        Time.timeScale = 0f;
        _movement.enabled = false;
    }

    public void UnfreezeGame()
    {
        if (!YandexGame.nowFullAd)
        {
            Time.timeScale = 1f;
            _movement.enabled = true;
        }
    }

    private void Start()
    {
        YandexGame.OpenFullAdEvent += FreezeGame;
        YandexGame.CloseFullAdEvent += UnfreezeGame;
    }
}
