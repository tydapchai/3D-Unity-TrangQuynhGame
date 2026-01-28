using UnityEngine;

/// <summary>
/// NPC Base Class - Cho tất cả NPCs
/// </summary>
public class NPC : MonoBehaviour
{
    [SerializeField] protected string npcName = "NPC";
    [SerializeField] protected string[] dialogueLines;
    [SerializeField] protected float interactionRange = 3f;
    [SerializeField] protected bool hasQuest = false;
    
    protected bool hasInteracted = false;
    protected Animator animator;
    
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public virtual void Interact()
    {
        if (!hasInteracted)
        {
            hasInteracted = true;
            ShowDialogue();
        }
    }
    
    protected virtual void ShowDialogue()
    {
        Debug.Log($"[{npcName}] Started dialogue");
        // TODO: Trigger UI dialogue system
        // DialogueManager.Instance.ShowDialogue(npcName, dialogueLines);
    }
    
    public string GetNPCName() => npcName;
    public bool HasQuest() => hasQuest;
}

/// <summary>
/// Merchant NPC - Bán hàng
/// </summary>
public class Merchant : NPC
{
    [SerializeField] private ItemData[] shopItems;
    
    public override void Interact()
    {
        base.Interact();
        ShowShop();
    }
    
    private void ShowShop()
    {
        Debug.Log($"[{npcName}] Opening shop...");
        // TODO: Show shop UI
        // ShopUI.Instance.ShowShop(shopItems);
    }
}

/// <summary>
/// Guard NPC - Security
/// </summary>
public class Guard : NPC
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolSpeed = 2f;
    
    private int currentPatrolIndex = 0;
    
    protected override void Start()
    {
        base.Start();
        if (patrolPoints.Length == 0)
        {
            Debug.LogWarning($"[{npcName}] No patrol points assigned!");
        }
    }
    
    private void Update()
    {
        Patrol();
    }
    
    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;
        
        Transform targetPoint = patrolPoints[currentPatrolIndex];
        transform.position = Vector3.Lerp(
            transform.position, 
            targetPoint.position, 
            patrolSpeed * Time.deltaTime
        );
        
        // Chuyển point khi đến
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }
}
