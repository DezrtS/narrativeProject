using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalPage : MonoBehaviour
{
    [SerializeField] private bool disableOnStart = false;
    [SerializeField] private TextMeshProUGUI pageTitle;
    [SerializeField] private TextMeshProUGUI pageContent;
    [SerializeField] private TextMeshProUGUI pageNumber;

    [SerializeField] private Image leftGradientImage;
    [SerializeField] private Image rightGradientImage;

    private RectTransform pageRectTransform;
    private Image pageImage;

    private void Start()
    {
        pageRectTransform = GetComponent<RectTransform>();
        pageImage = GetComponent<Image>();

        if (disableOnStart)
        {
            DisablePage();
        }
    }

    public void EnablePage()
    {
        pageImage.enabled = true;
        pageTitle.enabled = true;
        pageContent.enabled = true;
        pageNumber.enabled = true;
        leftGradientImage.enabled = true;
        rightGradientImage.enabled = true;
    }

    public void DisablePage()
    {
        pageImage.enabled = false;
        pageTitle.enabled = false;
        pageContent.enabled = false;
        pageNumber.enabled = false;
        leftGradientImage.enabled = false;
        rightGradientImage.enabled = false;
    }

    public void LoadJournalPageContent(string pageTitle, string pageContent, string pageNumber)
    {
        EnablePage();

        this.pageTitle.text = pageTitle;
        this.pageContent.text = pageContent;
        this.pageNumber.text = pageNumber;
    }

    public void ScalePage(float scale)
    {
        pageRectTransform.localScale = new Vector3(scale, 1, 1);
    }

    public float GetPageScale()
    {
        return pageRectTransform.localScale.x;
    }

    public void SwitchPageSide(bool leftSide)
    {
        int side = 0;

        if (!leftSide)
        {
            side = 1;
        }

        pageRectTransform.pivot = new Vector2(side, 0.5f);
        pageRectTransform.localPosition = new Vector3(0, pageRectTransform.localPosition.y, pageRectTransform.localPosition.z);
    }
}
