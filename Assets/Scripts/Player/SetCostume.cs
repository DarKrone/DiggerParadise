using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCostume : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController _toSetAnimator;
    [SerializeField] private ParticleSystem _particleSystemToChange;
    [SerializeField] private Color _particlesColor;
    private Animator _playerAnimator;

    private void Start()
    {
        _playerAnimator = PlayerMovement.Instance.GetComponent<Animator>();
    }

    public void SetAnimatorToPlayer()
    {
        if (_playerAnimator.runtimeAnimatorController == _toSetAnimator)
            return;
        _playerAnimator.runtimeAnimatorController = _toSetAnimator;
        ParticleSystem.MainModule main = _particleSystemToChange.main;
        main.startColor = _particlesColor;
        _particleSystemToChange.Play();
    }
}
