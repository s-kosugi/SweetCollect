using UnityEngine;

public class ItemPudding : ItemBase
{
    private Rigidbody2D rb = null;
    // Start is called before the first frame update
    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        if (m_GameMainManager.state != GameMainManager.STATE.MAIN)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
        base.Update();
    }

}