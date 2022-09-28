using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ForceSelector : MonoBehaviour
{
    //自動選択されるボタン
    [SerializeField] private Selectable firstSelected;

    void OnEnable()
    {
        if (firstSelected == null)
        {
            firstSelected = GetComponentInChildren<Selectable>();
        }
    }

    void Update()
    {
        //EventSystemが何も選択していない場合、firstSelectedを選択
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (firstSelected != null && firstSelected.gameObject.activeInHierarchy && firstSelected.interactable)
            {
                EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
            }
        }
    }
}
