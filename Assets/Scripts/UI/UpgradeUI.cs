using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject _ui;

    private void Start()
    {
        _ui.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        OpenUpgradeUI();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        CloseUpgradeUI();
    }

    public void OpenUpgradeUI()
    {
        _ui.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        _ui.SetActive(false);
    }

    protected void DisableThisUpgradeBtn()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
    }
}
