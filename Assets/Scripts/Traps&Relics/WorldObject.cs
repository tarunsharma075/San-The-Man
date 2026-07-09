using UnityEngine;

public enum RelicState
{
    Uncollected,
    Collected
}

public enum TrapState
{
    Activated,
    Deactivated
}

public abstract class WorldObject : GenericMonoSingleton<WorldObject>
{
    [Header("Components")]
    protected Rigidbody2D rb;
    protected Animator anim;

    [Header("Trap")]
    [SerializeField] protected float damage;
    protected TrapState currentTrapState = TrapState.Activated;

    [Header("Relic")]
    protected RelicState currentRelicState = RelicState.Uncollected;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.Log("No rb");
        }
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.Log("No anim");
        }
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    #region Trap

    public void ActivateTrap()
    {
        currentTrapState = TrapState.Activated;
    }

    public void DeactivateTrap()
    {
        currentTrapState = TrapState.Deactivated;
    }

    public TrapState GetTrapState()
    {
        return currentTrapState;
    }



    #endregion

    #region Relic

    protected void CollectRelic()
    {
        currentRelicState = RelicState.Collected;
    }

    public RelicState GetRelicState()
    {
        return currentRelicState;
    }

    #endregion

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }
}