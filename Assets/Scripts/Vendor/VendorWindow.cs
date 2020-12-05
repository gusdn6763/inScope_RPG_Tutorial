﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendorWindow : MonoBehaviour
{
    [SerializeField] private VendorButton[] vendorButtons;
    [SerializeField] private Text pageNumber;

    //페이지의 아이템의 리스트들을 전부 저장하는 변수 -> 모든 페이지의 정보를 가지고 있다.  
    private List<List<VendorItem>> pages = new List<List<VendorItem>>();
    private CanvasGroup canvasGroup;

    private int pageIndex = 0;

    private Vendor vendor;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void CreatePages(VendorItem[] items)
    {
        List<VendorItem> page = new List<VendorItem>();

        for (int i = 0; i < items.Length; i++)
        {
            page.Add(items[i]);
            if (page.Count == 10 || i == items.Length - 1)
            {
                pages.Add(page);
                page = new List<VendorItem>();
            }
        }
        AddItems();
    }

    public void AddItems()
    {
        pageNumber.text = pageIndex + 1 + "/" + pages.Count;
        if (pages.Count > 0)
        {
            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if (pages[pageIndex][i] != null)
                {
                    vendorButtons[i].AddItem(pages[pageIndex][i]);
                }
            }
        }
    }

    public void NextPage()
    {
        if (pageIndex < pages.Count - 1)
        {
            ClearButtons();
            pageIndex++;
            AddItems();
        }
    }

    public void PreviousPage()
    {
        if (pageIndex > 0)
        {
            ClearButtons();
            pageIndex--;
            AddItems();
        }
    }

    public void ClearButtons()
    {
        foreach (VendorButton btn in vendorButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }

    public void Open(Vendor vendor)
    {
        this.vendor = vendor;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void Close()
    {
        vendor.IsOpen = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        vendor = null;
    }
}
