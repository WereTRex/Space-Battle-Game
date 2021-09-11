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

    private Action onCloseAction;

    public void Close()
    {
        onCloseAction?.Invoke();
        ClosePanel();
    }

    void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }
}
