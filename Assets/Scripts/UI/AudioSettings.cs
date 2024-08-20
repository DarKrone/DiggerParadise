using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Sprite _audioOn;
    [SerializeField] private Sprite _audioOff;
    [SerializeField] private Image _image;
    private bool _isAudioOn = true;

    public void TurnAudio()
    {
        if (_isAudioOn)
        {
            _isAudioOn = false;
            _image.sprite = _audioOff;
            _audioMixer.SetFloat("MasterVolume", -80f);
        }
        else
        {
            _isAudioOn = true;
            _image.sprite = _audioOn;
            _audioMixer.SetFloat("MasterVolume", 0f);
        }
    }
}
