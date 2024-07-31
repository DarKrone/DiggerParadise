using System.Collections;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        StartTimer(60f);
    }
    private void StartTimer(float duration)
    {
        StartCoroutine(Timer(duration));
    }
    private IEnumerator Timer(float duration)
    {
        while(0 <= duration)
        {
            _text.text = duration.ToString();
            yield return new WaitForSeconds(1f);
            duration--;
        }
    }
}
