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


    [SerializeField] private bool _debugMode;
    private YandexGame _sdk;
    private void Start()
    {
        Instance = this;
    }
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
    private void Update()
    {
        if (!_rewardedADSUI.activeSelf)
            StopAllCoroutines();
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
        if (_rewardedADSUI.activeSelf)
            return;
        int roll = Random.Range(0, 100);
        int chance = 10;
        
        if (roll <= chance)
        {
            if (_debugMode)
                Debug.Log("Rolled succeful");
            //StartCoroutine(ShowObjectAtTime(_rewardedADSUI, _rewardCooldown));
            ShowObject(_rewardedADSUI);
        }
        else if (_debugMode)
            Debug.Log($"Rolled {roll} need <{chance}");
    }
    private void ShowObject(GameObject obj)
    {
        obj.SetActive(true);
    }
    private IEnumerator ShowObjectAtTime(GameObject obj, float time)
    {
        if(_debugMode)
            Debug.Log("Showed ads window");

        obj.SetActive(true);
        yield return new WaitForSeconds(time);
        if (obj.activeSelf)
            obj.SetActive(false);
        
        if (_debugMode)
            Debug.Log("Hided ads window");
    }
}
