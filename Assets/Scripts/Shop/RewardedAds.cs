using System.Collections;
using YG;
using UnityEngine;

public class RewardedAds : MonoBehaviour
{
    public static RewardedAds Instance;
    [SerializeField] private int _adId;
    [SerializeField] private float _upgradeChance = 5f;

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
        }
    }

    public void TryADSAfterResourceOreExtracting()
    {
        if (_rewardedADSMenu.activeSelf || _rewardedIcon.activeSelf)
            return;
        int roll = Random.Range(0, 100);

        
        if (roll <= _upgradeChance)
        {
            if (_debugMode)
                Debug.Log("Rolled succeful");
            ShowObject(_rewardedADSMenu);
        }
        else if (_debugMode)
            Debug.Log($"Rolled {roll} need <{_upgradeChance}");
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
