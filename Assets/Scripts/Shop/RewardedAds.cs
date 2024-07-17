using System.Collections;
using System.Collections.Generic;
using YG;
using UnityEngine;
using YG.Example;
using UnityEngine.UI;

public class RewardedAds : MonoBehaviour
{
    public static RewardedAds Instance;
    [SerializeField] private int _adId;
    [SerializeField] private float _btnCooldown = 120f;
    [SerializeField] private Button _upgBtn;
    [SerializeField] private GameObject _rewardIcon;

    [SerializeField] private GameObject _rewardedADSUI;

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
        _rewardIcon.SetActive(true);
        yield return new WaitForSeconds(cooldown);
        _upgBtn.interactable = true;
        _rewardIcon.SetActive(false);
    }
    public void TryADSAfterResourceOreExtracting()
    {
        if (Random.Range(0, 10) <= 2)
        {
            ShowObjectAtTime(_rewardedADSUI, 10f);
        }
    }
    private IEnumerator ShowObjectAtTime(GameObject obj, float time)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(time);
        if (obj.activeSelf)
            obj.SetActive(false);
    }
}
