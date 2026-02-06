using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// CHẶNG 2: QUÁN TRỌ - AI LÀ KẺ TRỘM?
///
/// Bối cảnh: Nửa đêm. Chủ quán phát hiện mất tiền. Cửa quán bị khóa.
///
/// Nghi phạm:
/// - Thương buôn: Giày khô, áo sạch → VÔ TỘI
/// - Thầy bói: Đèn dầu trước mặt vẫn ấm → VÔ TỘI
/// - Người gánh nước: Giày ướt, tiền dính bùn → CÓ TỘI
///
/// Kết quả:
/// - Buộc tội Thương buôn → BỊ ĐÁNH CHẾT (Game Over)
/// - Buộc tội Thầy bói → BỊ ĐUỔI KHỎI QUÁN (Game Over)
/// - Buộc tội Người gánh nước → QUA MÀN ✓
/// </summary>
public class Chapter2_Inn : MonoBehaviour, IChapterSetup
{
    [Header("=== MAIN UI PANELS ===")]
    [SerializeField] private GameObject innPanel;           // Panel chính của quán trọ
    [SerializeField] private GameObject investigationPanel; // Panel điều tra
    [SerializeField] private GameObject accusationPanel;    // Panel buộc tội
    [SerializeField] private GameObject gameOverPanel;      // Panel game over
    [SerializeField] private GameObject victoryPanel;       // Panel chiến thắng

    [Header("=== NPC BUTTONS (HỎI) ===")]
    [SerializeField] private Button askMerchantButton;      // Hỏi Thương buôn
    [SerializeField] private Button askFortuneTellerButton; // Hỏi Thầy bói
    [SerializeField] private Button askWaterCarrierButton;  // Hỏi Người gánh nước
    [SerializeField] private Button askInnkeeperButton;     // Hỏi Chủ quán

    [Header("=== EXAMINE BUTTONS (SOI) ===")]
    [SerializeField] private Button examineMerchantButton;      // Soi Thương buôn
    [SerializeField] private Button examineFortuneTellerButton; // Soi Thầy bói
    [SerializeField] private Button examineWaterCarrierButton;  // Soi Người gánh nước

    [Header("=== ACCUSATION BUTTONS ===")]
    [SerializeField] private Button accuseMerchantButton;       // Buộc tội Thương buôn
    [SerializeField] private Button accuseFortuneTellerButton;  // Buộc tội Thầy bói
    [SerializeField] private Button accuseWaterCarrierButton;   // Buộc tội Người gánh nước
    [SerializeField] private Button openAccusationButton;       // Mở panel buộc tội

    [Header("=== TEXT DISPLAYS ===")]
    [SerializeField] private TextMeshProUGUI dialogueText;      // Lời thoại NPC
    [SerializeField] private TextMeshProUGUI cluesText;         // Danh sách bằng chứng
    [SerializeField] private TextMeshProUGUI gameOverText;      // Text game over
    [SerializeField] private TextMeshProUGUI victoryText;       // Text chiến thắng

    [Header("=== RESTART/CONTINUE ===")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextChapterButton;

    [Header("=== AUDIO ===")]
    [SerializeField] private AudioClip bgmMystery;
    [SerializeField] private AudioClip sfxCorrect;
    [SerializeField] private AudioClip sfxWrong;

    // ========== DATA ==========
    private HashSet<string> collectedClues = new HashSet<string>();
    private HashSet<string> askedNPCs = new HashSet<string>();
    private HashSet<string> examinedNPCs = new HashSet<string>();

    // Lời thoại khi HỎI
    private Dictionary<string, string> npcDialogues = new Dictionary<string, string>()
    {
        { "Innkeeper", "Chủ quán: \"Tiền của ta mất rồi! Trong quán này, chắc chắn có kẻ trộm!\"" },
        { "Merchant", "Thương buôn: \"Tôi buôn bán đàng hoàng, tiền còn nhiều hơn ông chủ quán!\"" },
        { "FortuneTeller", "Thầy bói: \"Ta biết trước đêm nay có họa… Nhưng biết không có nghĩa là làm.\"" },
        { "WaterCarrier", "Người gánh nước: \"Tôi nghèo… nhưng không trộm.\"" }
    };

    // Bằng chứng khi SOI
    private Dictionary<string, List<string>> npcClues = new Dictionary<string, List<string>>()
    {
        { "Merchant", new List<string> { "Giày khô", "Áo sạch" } },
        { "FortuneTeller", new List<string> { "Đèn dầu trước mặt vẫn ấm" } },
        { "WaterCarrier", new List<string> { "Giày ướt", "Tiền dính bùn" } }
    };

    // ========== UNITY LIFECYCLE ==========
    void Start()
    {
        StartCoroutine(WaitForUIAndInitialize());
    }

    private IEnumerator WaitForUIAndInitialize()
    {
        // Đợi UI_Common scene load
        yield return new WaitForSeconds(0.5f);

        FindUIElements();
        SetupButtonListeners();
        InitializeGame();
    }

    // ========== UI SETUP ==========
    private void FindUIElements()
    {
        // Main panels
        if (innPanel == null) innPanel = FindObjectWithName("Chapter2_Inn_Panel");
        if (investigationPanel == null) investigationPanel = FindObjectWithName("Investigation_Panel");
        if (accusationPanel == null) accusationPanel = FindObjectWithName("Accusation_Panel");
        if (gameOverPanel == null) gameOverPanel = FindObjectWithName("GameOver_Panel");
        if (victoryPanel == null) victoryPanel = FindObjectWithName("Victory_Panel");

        // Ask buttons
        if (askMerchantButton == null) askMerchantButton = FindButtonByName("Ask_Merchant_Button");
        if (askFortuneTellerButton == null) askFortuneTellerButton = FindButtonByName("Ask_FortuneTeller_Button");
        if (askWaterCarrierButton == null) askWaterCarrierButton = FindButtonByName("Ask_WaterCarrier_Button");
        if (askInnkeeperButton == null) askInnkeeperButton = FindButtonByName("Ask_Innkeeper_Button");

        // Examine buttons
        if (examineMerchantButton == null) examineMerchantButton = FindButtonByName("Examine_Merchant_Button");
        if (examineFortuneTellerButton == null) examineFortuneTellerButton = FindButtonByName("Examine_FortuneTeller_Button");
        if (examineWaterCarrierButton == null) examineWaterCarrierButton = FindButtonByName("Examine_WaterCarrier_Button");

        // Accusation buttons
        if (accuseMerchantButton == null) accuseMerchantButton = FindButtonByName("Accuse_Merchant_Button");
        if (accuseFortuneTellerButton == null) accuseFortuneTellerButton = FindButtonByName("Accuse_FortuneTeller_Button");
        if (accuseWaterCarrierButton == null) accuseWaterCarrierButton = FindButtonByName("Accuse_WaterCarrier_Button");
        if (openAccusationButton == null) openAccusationButton = FindButtonByName("Open_Accusation_Button");

        // Text elements
        if (dialogueText == null) dialogueText = FindTMPByName("Dialogue_Text");
        if (cluesText == null) cluesText = FindTMPByName("Clues_Text");
        if (gameOverText == null) gameOverText = FindTMPByName("GameOver_Text");
        if (victoryText == null) victoryText = FindTMPByName("Victory_Text");

        // Control buttons
        if (restartButton == null) restartButton = FindButtonByName("Restart_Button");
        if (nextChapterButton == null) nextChapterButton = FindButtonByName("NextChapter_Button");

        LogUIStatus();
    }

    private void LogUIStatus()
    {
        Debug.Log($"[Chapter2_Inn] UI Elements found:");
        Debug.Log($"  - innPanel: {innPanel != null}");
        Debug.Log($"  - dialogueText: {dialogueText != null}");
        Debug.Log($"  - askMerchantButton: {askMerchantButton != null}");
    }

    private void SetupButtonListeners()
    {
        // === HỎI BUTTONS ===
        if (askInnkeeperButton != null)
            askInnkeeperButton.onClick.AddListener(() => OnAskNPC("Innkeeper"));
        if (askMerchantButton != null)
            askMerchantButton.onClick.AddListener(() => OnAskNPC("Merchant"));
        if (askFortuneTellerButton != null)
            askFortuneTellerButton.onClick.AddListener(() => OnAskNPC("FortuneTeller"));
        if (askWaterCarrierButton != null)
            askWaterCarrierButton.onClick.AddListener(() => OnAskNPC("WaterCarrier"));

        // === SOI BUTTONS ===
        if (examineMerchantButton != null)
            examineMerchantButton.onClick.AddListener(() => OnExamineNPC("Merchant"));
        if (examineFortuneTellerButton != null)
            examineFortuneTellerButton.onClick.AddListener(() => OnExamineNPC("FortuneTeller"));
        if (examineWaterCarrierButton != null)
            examineWaterCarrierButton.onClick.AddListener(() => OnExamineNPC("WaterCarrier"));

        // === BUỘC TỘI BUTTONS ===
        if (openAccusationButton != null)
            openAccusationButton.onClick.AddListener(OpenAccusationPanel);
        if (accuseMerchantButton != null)
            accuseMerchantButton.onClick.AddListener(() => OnAccuse("Merchant"));
        if (accuseFortuneTellerButton != null)
            accuseFortuneTellerButton.onClick.AddListener(() => OnAccuse("FortuneTeller"));
        if (accuseWaterCarrierButton != null)
            accuseWaterCarrierButton.onClick.AddListener(() => OnAccuse("WaterCarrier"));

        // === CONTROL BUTTONS ===
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartChapter);
        if (nextChapterButton != null)
            nextChapterButton.onClick.AddListener(LoadNextChapter);
    }

    private void InitializeGame()
    {
        // Reset state
        collectedClues.Clear();
        askedNPCs.Clear();
        examinedNPCs.Clear();

        // Hide all panels except main
        if (accusationPanel != null) accusationPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (investigationPanel != null) investigationPanel.SetActive(true);

        // Initial dialogue
        ShowDialogue("Chủ quán: \"Tiền của ta mất rồi! Trong quán này, chắc chắn có kẻ trộm!\"\n\n[Hãy hỏi và soi xét từng người để tìm ra thủ phạm]");

        // Disable accusation button until enough clues
        if (openAccusationButton != null)
            openAccusationButton.interactable = false;

        UpdateCluesDisplay();

        // Play BGM
        if (bgmMystery != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBGM(bgmMystery);
        }

        Debug.Log("[Chapter2_Inn] Game initialized");
    }

    // ========== GAME ACTIONS ==========

    /// <summary>
    /// HỎI - Nghe lời khai của nghi phạm
    /// </summary>
    private void OnAskNPC(string npcName)
    {
        Debug.Log($"[Chapter2_Inn] Asking: {npcName}");

        if (npcDialogues.TryGetValue(npcName, out string dialogue))
        {
            ShowDialogue(dialogue);
            askedNPCs.Add(npcName);
        }

        CheckCanAccuse();
    }

    /// <summary>
    /// SOI - Kiểm tra giày, đồ vật của nghi phạm
    /// </summary>
    private void OnExamineNPC(string npcName)
    {
        Debug.Log($"[Chapter2_Inn] Examining: {npcName}");

        if (npcClues.TryGetValue(npcName, out List<string> clues))
        {
            string examineResult = $"[Soi xét {GetNPCDisplayName(npcName)}]\n";

            foreach (string clue in clues)
            {
                if (!collectedClues.Contains(clue))
                {
                    collectedClues.Add(clue);
                    examineResult += $"→ {clue}\n";
                }
            }

            ShowDialogue(examineResult);
            examinedNPCs.Add(npcName);
            UpdateCluesDisplay();
        }

        CheckCanAccuse();
    }

    /// <summary>
    /// BUỘC TỘI - Chỉ ra kẻ trộm
    /// </summary>
    private void OnAccuse(string suspectName)
    {
        Debug.Log($"[Chapter2_Inn] Accusing: {suspectName}");

        switch (suspectName)
        {
            case "Merchant":
                // ❌ SAI → Bị đánh chết
                OnGameOver(GameOverType.BeatenToDeath);
                break;

            case "FortuneTeller":
                // ❌ SAI → Bị đuổi khỏi quán
                OnGameOver(GameOverType.KickedOut);
                break;

            case "WaterCarrier":
                // ✅ ĐÚNG → Qua màn
                OnVictory();
                break;
        }
    }

    // ========== GAME STATE ==========

    private void CheckCanAccuse()
    {
        // Có thể buộc tội khi đã thu thập đủ bằng chứng
        // (ít nhất 3 clues hoặc đã soi xét 2 người)
        bool canAccuse = collectedClues.Count >= 3 || examinedNPCs.Count >= 2;

        if (openAccusationButton != null)
        {
            openAccusationButton.interactable = canAccuse;

            if (canAccuse)
            {
                Debug.Log("[Chapter2_Inn] Can now accuse!");
            }
        }
    }

    private void OpenAccusationPanel()
    {
        if (accusationPanel != null)
        {
            accusationPanel.SetActive(true);
            ShowDialogue("Hãy chỉ ra kẻ trộm!\n\n⚠️ Cẩn thận! Buộc tội sai sẽ có hậu quả nghiêm trọng!");
        }
    }

    // ========== GAME OVER ==========

    private enum GameOverType
    {
        BeatenToDeath,  // Buộc tội Thương buôn
        KickedOut       // Buộc tội Thầy bói
    }

    private void OnGameOver(GameOverType type)
    {
        Debug.Log($"[Chapter2_Inn] GAME OVER: {type}");

        // Play SFX
        if (sfxWrong != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(sfxWrong);
        }

        // Hide investigation panels
        if (investigationPanel != null) investigationPanel.SetActive(false);
        if (accusationPanel != null) accusationPanel.SetActive(false);

        // Show game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            string gameOverMessage = "";

            switch (type)
            {
                case GameOverType.BeatenToDeath:
                    gameOverMessage =
                        "❌ SAI RỒI!\n\n" +
                        "Thương buôn giàu có, quen biết nhiều người.\n" +
                        "Ông ta tức giận vì bị vu oan...\n\n" +
                        "Đám người nhà thương buôn đã đánh bạn đến chết.\n\n" +
                        "GAME OVER";
                    break;

                case GameOverType.KickedOut:
                    gameOverMessage =
                        "❌ SAI RỒI!\n\n" +
                        "Thầy bói được nhiều người kính trọng.\n" +
                        "Ông ta nguyền rủa bạn trước mặt mọi người...\n\n" +
                        "Bạn bị đuổi khỏi quán trong đêm tối.\n\n" +
                        "GAME OVER";
                    break;
            }

            if (gameOverText != null)
            {
                gameOverText.text = gameOverMessage;
            }
        }
    }

    // ========== VICTORY ==========

    private void OnVictory()
    {
        Debug.Log("[Chapter2_Inn] VICTORY!");

        // Play SFX
        if (sfxCorrect != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(sfxCorrect);
        }

        // Hide investigation panels
        if (investigationPanel != null) investigationPanel.SetActive(false);
        if (accusationPanel != null) accusationPanel.SetActive(false);

        // Show victory panel
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);

            string victoryMessage =
                "✅ CHÍNH XÁC!\n\n" +
                "Người gánh nước - giày ướt, tiền dính bùn.\n" +
                "Hắn đã ra ngoài trộm tiền rồi quay về!\n\n" +
                "Chủ quán: \"Đúng rồi! Cảm ơn đã tìm ra thủ phạm!\"\n\n" +
                "★ CHẶNG 2 HOÀN THÀNH ★";

            if (victoryText != null)
            {
                victoryText.text = victoryMessage;
            }
        }

        // Save progress
        SaveChapterProgress();
    }

    private void SaveChapterProgress()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.CompleteChapter(2);
        }

        PlayerPrefs.SetInt("Chapter2_Completed", 1);
        PlayerPrefs.Save();
        Debug.Log("[Chapter2_Inn] Chapter 2 progress saved!");
    }

    // ========== NAVIGATION ==========

    private void RestartChapter()
    {
        Debug.Log("[Chapter2_Inn] Restarting chapter...");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartChapter();
        }
        else
        {
            // Fallback: reinitialize
            InitializeGame();
        }
    }

    private void LoadNextChapter()
    {
        Debug.Log("[Chapter2_Inn] Loading next chapter...");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.NextChapter();
        }
    }

    // ========== UI HELPERS ==========

    private void ShowDialogue(string text)
    {
        if (dialogueText != null)
        {
            dialogueText.text = text;
        }
    }

    private void UpdateCluesDisplay()
    {
        if (cluesText == null) return;

        string display = "📋 BẰNG CHỨNG THU THẬP:\n\n";

        if (collectedClues.Count == 0)
        {
            display += "(Chưa có bằng chứng)\n\nHãy SOI XÉT nghi phạm!";
        }
        else
        {
            foreach (string clue in collectedClues)
            {
                display += $"• {clue}\n";
            }
        }

        display += $"\n\n[Đã thu thập: {collectedClues.Count} bằng chứng]";

        cluesText.text = display;
    }

    private string GetNPCDisplayName(string npcKey)
    {
        return npcKey switch
        {
            "Innkeeper" => "Chủ quán",
            "Merchant" => "Thương buôn",
            "FortuneTeller" => "Thầy bói",
            "WaterCarrier" => "Người gánh nước",
            _ => npcKey
        };
    }

    // ========== FIND HELPERS ==========

    private GameObject FindObjectWithName(string name)
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (var obj in allObjects)
        {
            if (obj.name == name)
                return obj;
        }
        Debug.LogWarning($"[Chapter2_Inn] GameObject not found: {name}");
        return null;
    }

    private Button FindButtonByName(string name)
    {
        GameObject obj = FindObjectWithName(name);
        return obj?.GetComponent<Button>();
    }

    private TextMeshProUGUI FindTMPByName(string name)
    {
        GameObject obj = FindObjectWithName(name);
        return obj?.GetComponent<TextMeshProUGUI>();
    }

    // ========== INTERFACE ==========

    public void OnChapterSetup(int chapterNumber)
    {
        if (chapterNumber == 2)
        {
            Debug.Log("[Chapter2_Inn] Chapter 2 setup called via interface");
            StartCoroutine(WaitForUIAndInitialize());
        }
    }
}
