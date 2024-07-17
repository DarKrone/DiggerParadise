using System.Collections;
using System.Collections.Generic;
using YG;
using UnityEngine;
using YG.Example;
using System;
using UnityEngine.UI;

public class RewardedAds : MonoBehaviour
{
    [SerializeField] private int _adId;
    [SerializeField] private float _btnCooldown = 120f;
    [SerializeField] private Button _upgBtn;
    private YandexGame _sdk;
    

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
    }

    void Rewarded(int id)
    {
        if (id == _adId)
        {
            ResourceManager.Instance.RewardedAdsUpgradeSpeedForPeriod(60f);
            StartCoroutine(ButtonCooldown(_btnCooldown));
        }
    }

    private IEnumerator ButtonCooldown(float cooldown)
    {
        _upgBtn.interactable = false;
        yield return new WaitForSeconds(cooldown);
        _upgBtn.interactable = true;
    }
}
