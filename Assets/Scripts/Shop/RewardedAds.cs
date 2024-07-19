using System.Collections;
using System.Collections.Generic;
using YG;
using UnityEngine;
using UnityEngine.UI;

public class RewardedAds : MonoBehaviour
{
    public static RewardedAds Instance;
    [SerializeField] private int _adId;
    [SerializeField] private float _btnCooldown = 120f;
    [SerializeField] private Button _upgBtn;

    [SerializeField] private GameObject _rewardedADSMenu;

    [SerializeField] private GameObject _rewardedIcon;

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
            StartCoroutine(ShowObjectAtTime(_rewardedIcon, 60f));
            StartCoroutine(ButtonCooldown(_btnCooldown));
        }
    }
    private IEnumerator ButtonCooldown(float cooldown)
    {
        _upgBtn.interactable = false;
        yield return new WaitForSeconds(cooldown);
        _upgBtn.interactable = true;
    }
    public void TryADSAfterResourceOreExtracting()
    {
        if (_rewardedADSMenu.activeSelf || _rewardedIcon.activeSelf)
            return;
        int roll = Random.Range(0, 100);
        int chance = 25;
        
        if (roll <= chance)
        {
            if (_debugMode)
                Debug.Log("Rolled succeful");
            //StartCoroutine(ShowObjectAtTime(_rewardedADSMenu, 60f));
            ShowObject(_rewardedADSMenu);
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
