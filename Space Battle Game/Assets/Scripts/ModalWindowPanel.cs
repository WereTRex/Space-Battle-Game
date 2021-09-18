using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModalWindowPanel : MonoBehaviour
{
    [Header("Header")]

    [SerializeField] Transform headerArea;
    [SerializeField] TextMeshProUGUI titleField;

    [Space(20)]
    [Header("Content")]

    [SerializeField] Transform contentArea;
    [SerializeField] Transform verticalLayoutArea;
    [SerializeField] Transform horizontalLayoutArea;

    [Space(20)]
    [Header("Footer")]

    [SerializeField] Transform footerArea;
    [SerializeField] Button closeButton;


    //Events
    public delegate void CloseAction(GameObject modalWindow);
    public static event CloseAction OnCloseAction;

    private void Awake()
    {
        ModalWindowPanel.OnCloseAction += ClosePanel;
    }


    public void Close()
    {
        OnCloseAction?.Invoke(this.gameObject);
    }

    void ClosePanel(GameObject modalWindow)
    {
        this.gameObject.SetActive(false);
    }
}
