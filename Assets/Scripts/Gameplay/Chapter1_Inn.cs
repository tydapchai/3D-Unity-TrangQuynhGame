using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Chapter1_Inn : MonoBehaviour, IChapterSetup
{
    [SerializeField] private Button merchantButton;
    [SerializeField] private Button fortuneTellerButton;
    [SerializeField] private Button waterCarrierButton;
    [SerializeField] private Button accuseButton;

    [SerializeField] private TextMeshProUGUI cluesText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private HashSet<string> examinedClues = new HashSet<string>();
    private int correctAccusations = 0;

    // Track which suspects have been questioned
    private Dictionary<string, bool> suspectQuestioned = new Dictionary<string, bool>
    {
        { "Merchant", false },
        { "FortuneTeller", false },
        { "WaterCarrier", false }
    };

    // Clues for each suspect
    private Dictionary<string, string> suspectClues = new Dictionary<string, string>
    {
        { "Merchant", "Giày khô, Áo sạch" },
        { "FortuneTeller", "Đèn dầu ấm" },
        { "WaterCarrier", "Giày ướt, Tiền dính bùn" }
    };

    void Start()
    {
        FindUIElements();
        InitializeUI();
        SetupButtonListeners();
    }

    private void FindUIElements()
    {
        // Auto-find UI elements if not assigned
        if (merchantButton == null)
            merchantButton = FindObjectWithName("Merchant_Button")?.GetComponent<Button>();

        if (fortuneTellerButton == null)
            fortuneTellerButton = FindObjectWithName("FortuneTeller_Button")?.GetComponent<Button>();

        if (waterCarrierButton == null)
            waterCarrierButton = FindObjectWithName("WaterCarrier_Button")?.GetComponent<Button>();

        if (accuseButton == null)
            accuseButton = FindObjectWithName("AccuseButton")?.GetComponent<Button>();

        if (cluesText == null)
            cluesText = FindObjectWithName("CluesText")?.GetComponent<TextMeshProUGUI>();

        if (dialogueText == null)
            dialogueText = FindObjectWithName("DialogueText")?.GetComponent<TextMeshProUGUI>();

        if (merchantButton == null) Debug.LogWarning("[Chapter1_Inn] Merchant_Button not found!");
        if (fortuneTellerButton == null) Debug.LogWarning("[Chapter1_Inn] FortuneTeller_Button not found!");
        if (waterCarrierButton == null) Debug.LogWarning("[Chapter1_Inn] WaterCarrier_Button not found!");
        if (accuseButton == null) Debug.LogWarning("[Chapter1_Inn] AccuseButton not found!");
        if (cluesText == null) Debug.LogWarning("[Chapter1_Inn] CluesText not found!");
        if (dialogueText == null) Debug.LogWarning("[Chapter1_Inn] DialogueText not found!");
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
        // Ensure AccuseButton starts disabled
        if (accuseButton != null)
        {
            accuseButton.interactable = false;
        }

        if (dialogueText != null)
        {
            dialogueText.text = "Chủ quán: \"Tiền của ta mất rồi! Ai là thủ phạm?\"";
        }

        UpdateCluesDisplay();
    }

    private void SetupButtonListeners()
    {
        if (merchantButton != null)
            merchantButton.onClick.AddListener(() => OnSuspectSelected("Merchant"));

        if (fortuneTellerButton != null)
            fortuneTellerButton.onClick.AddListener(() => OnSuspectSelected("FortuneTeller"));

        if (waterCarrierButton != null)
            waterCarrierButton.onClick.AddListener(() => OnSuspectSelected("WaterCarrier"));

        if (accuseButton != null)
            accuseButton.onClick.AddListener(OnAccuseButtonClicked);
    }

    private void OnSuspectSelected(string suspectName)
    {
        Debug.Log($"[Chapter1_Inn] Selected suspect: {suspectName}");
        suspectQuestioned[suspectName] = true;

        // Show clues for this suspect
        if (suspectClues.ContainsKey(suspectName))
        {
            string clues = suspectClues[suspectName];

            if (dialogueText != null)
            {
                dialogueText.text = $"{suspectName} has: {clues}";
            }

            // Add clues to examined list
            foreach (var clue in clues.Split(','))
            {
                string trimmedClue = clue.Trim();
                if (!examinedClues.Contains(trimmedClue))
                {
                    examinedClues.Add(trimmedClue);
                }
            }

            UpdateCluesDisplay();
            CheckIfCanAccuse();
        }
    }

    private void UpdateCluesDisplay()
    {
        if (cluesText != null)
        {
            string cluesList = "Bằng chứng tìm được:\n";

            if (examinedClues.Count == 0)
            {
                cluesList += "☐ Giày khô\n☐ Áo sạch\n☐ Đèn dầu ấm\n☐ Giày ướt\n☐ Tiền dính bùn";
            }
            else
            {
                cluesList += "☐ Giày khô" + (examinedClues.Contains("Giày khô") ? " ✓" : "") + "\n";
                cluesList += "☐ Áo sạch" + (examinedClues.Contains("Áo sạch") ? " ✓" : "") + "\n";
                cluesList += "☐ Đèn dầu ấm" + (examinedClues.Contains("Đèn dầu ấm") ? " ✓" : "") + "\n";
                cluesList += "☐ Giày ướt" + (examinedClues.Contains("Giày ướt") ? " ✓" : "") + "\n";
                cluesList += "☐ Tiền dính bùn" + (examinedClues.Contains("Tiền dính bùn") ? " ✓" : "");
            }

            cluesText.text = cluesList;
        }
    }

    private void CheckIfCanAccuse()
    {
        // Can accuse if examined at least 3 clues
        if (examinedClues.Count >= 3)
        {
            if (accuseButton != null)
            {
                accuseButton.interactable = true;
            }
        }
    }

    private void OnAccuseButtonClicked()
    {
        Debug.Log("[Chapter1_Inn] Accuse button clicked");

        // Check if water carrier clues are examined (correct answer)
        bool hasWaterCarrierClues = examinedClues.Contains("Giày ướt") &&
                                     examinedClues.Contains("Tiền dính bùn");

        if (hasWaterCarrierClues)
        {
            OnCorrectAccusation();
        }
        else
        {
            OnIncorrectAccusation();
        }
    }

    private void OnCorrectAccusation()
    {
        correctAccusations++;
        Debug.Log("[Chapter1_Inn] CORRECT! Water carrier is the thief!");

        if (dialogueText != null)
        {
            dialogueText.text = "Chủ quán: \"Đúng rồi! Kẻ gánh nước chính là thủ phạm!\" Cảm ơn bạn!";
        }

        // Disable accuse button after accusation
        if (accuseButton != null)
        {
            accuseButton.interactable = false;
        }

        // Save progress
        SaveChapterProgress();

        // TODO: Load next chapter or show completion screen
    }

    private void OnIncorrectAccusation()
    {
        Debug.Log("[Chapter1_Inn] INCORRECT! Wrong suspect!");

        if (dialogueText != null)
        {
            dialogueText.text = "Chủ quán: \"Không, đó không phải kẻ trộm! Tìm lại bằng chứng khác.\"";
        }

        // Re-enable accuse button to try again
        if (accuseButton != null)
        {
            accuseButton.interactable = true;
        }
    }

    private void SaveChapterProgress()
    {
        // Save that chapter 1 is completed
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.CompleteChapter(1);
        }

        PlayerPrefs.SetInt("Chapter1_Completed", 1);
        PlayerPrefs.Save();
        Debug.Log("[Chapter1_Inn] Progress saved");
    }

    public void OnChapterSetup(int chapterNumber)
    {
        if (chapterNumber == 1)
        {
            Debug.Log("[Chapter1_Inn] Chapter 1 setup called");
            InitializeUI();
        }
    }
}
