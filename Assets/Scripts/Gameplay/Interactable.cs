using UnityEngine;

/// <summary>
/// Item Data - Dữ liệu cho items
/// </summary>
[System.Serializable]
public class ItemData
{
    public string itemName = "Item";
    public string itemDescription = "";
    public int quantity = 1;
    public Sprite itemIcon;
}

/// <summary>
/// Interactable Base Class
/// </summary>
public class Interactable : MonoBehaviour
{
    [SerializeField] protected string interactableName = "Interactable";
    [SerializeField] protected string interactPrompt = "Press F to interact";
    [SerializeField] protected float interactionRange = 2f;
    [SerializeField] protected Animator animator;
    
    protected bool isInteracted = false;
    
    protected virtual void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }
    
    public virtual void Interact()
    {
        Debug.Log($"[{interactableName}] Interacted!");
    }
    
    public string GetInteractPrompt() => interactPrompt;
    public float GetInteractionRange() => interactionRange;
}

/// <summary>
/// Chest - Khối hàng chứa items
/// </summary>
public class Chest : Interactable
{
    [SerializeField] private ItemData[] items;
    [SerializeField] private bool isLocked = false;
    [SerializeField] private string requiredKeyItem = "";
    
    public override void Interact()
    {
        if (isInteracted) return;
        
        if (isLocked && !string.IsNullOrEmpty(requiredKeyItem))
        {
            Debug.Log($"[{interactableName}] Chest is locked! Need {requiredKeyItem}");
            return;
        }
        
        base.Interact();
        isInteracted = true;
        
        if (animator != null)
            animator.SetTrigger("Open");
        
        DropItems();
    }
    
    private void DropItems()
    {
        foreach (ItemData item in items)
        {
            // TODO: Instantiate item in world
            Debug.Log($"Dropped {item.quantity}x {item.itemName}");
        }
    }
    
    public void Unlock()
    {
        isLocked = false;
        Debug.Log($"[{interactableName}] Unlocked!");
    }
}

/// <summary>
/// Door - Cửa có thể mở
/// </summary>
public class Door : Interactable
{
    [SerializeField] private bool isLocked = false;
    [SerializeField] private bool isOpen = false;
    
    public override void Interact()
    {
        if (isLocked)
        {
            Debug.Log($"[{interactableName}] Door is locked!");
            return;
        }
        
        ToggleDoor();
    }
    
    public void ToggleDoor()
    {
        isOpen = !isOpen;
        
        if (animator != null)
            animator.SetBool("IsOpen", isOpen);
        
        Debug.Log($"[{interactableName}] Door {(isOpen ? "opened" : "closed")}");
    }
    
    public void Unlock()
    {
        isLocked = false;
        Debug.Log($"[{interactableName}] Door unlocked!");
    }
}

/// <summary>
/// Lever - Cần để kích hoạt sự kiện
/// </summary>
public class Lever : Interactable
{
    [SerializeField] private Interactable linkedObject;
    [SerializeField] private bool isActivated = false;
    
    public override void Interact()
    {
        isActivated = !isActivated;
        
        if (animator != null)
            animator.SetBool("IsActivated", isActivated);
        
        if (linkedObject != null && isActivated)
        {
            if (linkedObject is Door door)
            {
                door.ToggleDoor();
            }
        }
        
        Debug.Log($"[{interactableName}] Lever {(isActivated ? "activated" : "deactivated")}");
    }
}
