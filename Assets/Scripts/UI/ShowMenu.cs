using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenu : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private bool _isOpen = false;

    public void ToogleMenu()
    {
        _isOpen = !_isOpen;
        _animator.SetBool("Menu", _isOpen);
    }
}
