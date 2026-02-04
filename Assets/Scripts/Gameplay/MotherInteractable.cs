using UnityEngine;
using UnityEngine.InputSystem;

public class MotherInteractable : MonoBehaviour
{
    [SerializeField] private float interactionRange = 2f;
    private Transform playerTransform;
    private bool hasInteracted = false;

    void Start()
    {
        // Find player
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("[MotherInteractable] Player not found!");
        }

        // Add collider as trigger for interaction
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    void Update()
    {
        // Check if player is in range
        if (playerTransform != null && !hasInteracted)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            Debug.Log($"[MotherInteractable] Distance to player: {distance}, Range: {interactionRange}");

            if (distance <= interactionRange)
            {
                Debug.Log("[MotherInteractable] Player in range! Press E to interact");

                // Check for interaction input (E key)
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    InteractWithMother();
                }
            }
        }
        else if (playerTransform == null)
        {
            Debug.LogError("[MotherInteractable] Player transform is null!");
        }
    }

    private void InteractWithMother()
    {
        Debug.Log("[MotherInteractable] Interacting with Mother!");
        hasInteracted = true;

        // Show Chapter1_Home_UI
        ShowHomeUI();
    }

    private void ShowHomeUI()
    {
        Debug.Log("[MotherInteractable] Looking for Chapter1_Home_UI...");

        // Method 1: Find by GameObject name
        GameObject homeUIObj = GameObject.Find("Chapter1_Home_UI");
        if (homeUIObj != null)
        {
            homeUIObj.SetActive(true);
            Debug.Log("[MotherInteractable] Chapter1_Home_UI found and enabled!");
            return;
        }

        Debug.Log("[MotherInteractable] GameObject.Find didn't work, trying Canvas search...");

        // Method 2: Find via Canvas
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        Debug.Log($"[MotherInteractable] Found {canvases.Length} canvases");

        foreach (var canvas in canvases)
        {
            Debug.Log($"[MotherInteractable] Canvas: {canvas.name}");

            if (canvas.name == "Chapter1_Home_UI")
            {
                canvas.gameObject.SetActive(true);
                Debug.Log("[MotherInteractable] Canvas Chapter1_Home_UI enabled!");
                return;
            }
        }

        Debug.LogWarning("[MotherInteractable] Chapter1_Home_UI not found by any method!");
    }

    private Transform FindTransformRecursive(Transform parent, string name)
    {
        if (parent.name == name)
            return parent;

        foreach (Transform child in parent)
        {
            Transform result = FindTransformRecursive(child, name);
            if (result != null)
                return result;
        }

        return null;
    }

    public void ResetInteraction()
    {
        hasInteracted = false;
    }
}
