using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Chapter1_Home : MonoBehaviour, IChapterSetup
{
    [SerializeField] private Button buttonThuoc;
    [SerializeField] private Button buttonThu;
    [SerializeField] private Button buttonTien;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI selectedItemsText;

    private string selectedItem = null;
    private const int requiredItemCount = 1;

    void Start()
    {
        // Wait for UI_Common to be loaded before finding UI elements
        StartCoroutine(WaitForUIAndInitialize());
    }

    private IEnumerator WaitForUIAndInitialize()
    {
        // Wait a frame to ensure all scenes are loaded
        yield return null;

        FindUIElements();
        InitializeUI();
        SetupButtonListeners();
    }

    private void FindUIElements()
    {
        // Auto-find buttons if not assigned
        if (buttonThuoc == null)
            buttonThuoc = FindObjectWithName("Button_Thuoc")?.GetComponent<Button>();

        if (buttonThu == null)
            buttonThu = FindObjectWithName("Button_Thu")?.GetComponent<Button>();

        if (buttonTien == null)
            buttonTien = FindObjectWithName("Button_Tien")?.GetComponent<Button>();

        if (confirmButton == null)
            confirmButton = FindObjectWithName("ConfirmButton")?.GetComponent<Button>();

        if (selectedItemsText == null)
        {
            // Try multiple names
            GameObject textObj = FindObjectWithName("SelectedText");
            if (textObj == null)
                textObj = FindObjectWithName("SelectedItems");
            if (textObj == null)
                textObj = FindObjectWithName("SelectText");

            if (textObj != null)
                selectedItemsText = textObj.GetComponent<TextMeshProUGUI>();
        }

        // Log what's missing
        if (buttonThuoc == null) Debug.LogWarning("[Chapter1_Home] Button_Thuoc not found!");
        if (buttonThu == null) Debug.LogWarning("[Chapter1_Home] Button_Thu not found!");
        if (buttonTien == null) Debug.LogWarning("[Chapter1_Home] Button_Tien not found!");
        if (confirmButton == null) Debug.LogWarning("[Chapter1_Home] ConfirmButton not found!");
        if (selectedItemsText == null) Debug.LogWarning("[Chapter1_Home] SelectedText not found!");

        // Log what was found
        if (buttonThuoc != null) Debug.Log("[Chapter1_Home] ✓ Button_Thuoc found");
        if (buttonThu != null) Debug.Log("[Chapter1_Home] ✓ Button_Thu found");
        if (buttonTien != null) Debug.Log("[Chapter1_Home] ✓ Button_Tien found");
        if (confirmButton != null) Debug.Log("[Chapter1_Home] ✓ ConfirmButton found");
        if (selectedItemsText != null) Debug.Log("[Chapter1_Home] ✓ SelectedText found");
    }

    private GameObject FindObjectWithName(string name)
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (var obj in allObjects)
        {
            if (obj.name == name)
                return obj;
        }
        return null;
    }

    private void InitializeUI()
    {
        // Ensure ConfirmButton starts disabled
        if (confirmButton != null)
        {
            confirmButton.interactable = false;
        }

        // Update selected items display
        UpdateSelectedItemsDisplay();
    }

    private void SetupButtonListeners()
    {
        if (buttonThuoc != null)
            buttonThuoc.onClick.AddListener(() => OnItemSelected("Thuốc"));

        if (buttonThu != null)
            buttonThu.onClick.AddListener(() => OnItemSelected("Thư"));

        if (buttonTien != null)
            buttonTien.onClick.AddListener(() => OnItemSelected("Tiền"));

        if (confirmButton != null)
            confirmButton.onClick.AddListener(OnConfirmButtonClicked);
    }

    private void OnItemSelected(string itemName)
    {
        if (selectedItem == itemName)
        {
            // Deselect item
            selectedItem = null;
            Debug.Log($"[Chapter1_Home] Deselected: {itemName}");
        }
        else
        {
            // Select new item (automatically deselects previous)
            selectedItem = itemName;
            Debug.Log($"[Chapter1_Home] Selected: {itemName}");
        }

        UpdateSelectedItemsDisplay();
        UpdateConfirmButtonState();
    }

    private void UpdateSelectedItemsDisplay()
    {
        if (selectedItemsText != null)
        {
            if (selectedItem == null)
            {
                selectedItemsText.text = "Chọn: Chưa chọn";
            }
            else
            {
                selectedItemsText.text = $"Chọn: {selectedItem}";
            }
        }
    }

    private void UpdateConfirmButtonState()
    {
        if (confirmButton != null)
        {
            confirmButton.interactable = (selectedItem != null);
        }
    }

    private void OnConfirmButtonClicked()
    {
        Debug.Log($"[Chapter1_Home] Confirmed with item: {selectedItem}");

        // Save selected item for later use
        SaveSelectedItems();

        // Move to next stage (Inn investigation)
        MoveToNextStage();
    }

    private void SaveSelectedItems()
    {
        // Store selected item for later use
        if (selectedItem != null)
        {
            PlayerPrefs.SetString("Chapter1_SelectedItem", selectedItem);
            PlayerPrefs.Save();
            Debug.Log($"[Chapter1_Home] Saved selected item: {selectedItem}");
        }
    }

    private void MoveToNextStage()
    {
        // Hide home UI, show inn UI
        // This will be handled by a UI manager or directly here

        // For now, we'll disable this canvas section and enable inn section
        // Assuming there's a parent container or we'll add UI manager later

        GameObject chapter1HomeUI = transform.parent?.gameObject;
        if (chapter1HomeUI != null)
        {
            chapter1HomeUI.SetActive(false);
            Debug.Log("[Chapter1_Home] Hidden home UI, showing inn UI");
        }

        // TODO: Trigger inn investigation scene loading or show inn UI
    }

    public void OnChapterSetup(int chapterNumber)
    {
        if (chapterNumber == 1)
        {
            Debug.Log("[Chapter1_Home] Chapter 1 setup called");
            InitializeUI();
        }
    }
}
