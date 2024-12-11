using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class ChargeTowardsPoint : MonoBehaviour
{
    [SerializeField] float chargeForce;
    [SerializeField] float rateOfCharges;
    [SerializeField] float chargeRange;

    float chargeTimer;

    Rigidbody2D rb;
    Transform playerTrans;
    [SerializeField] LayerMask obstacleLayer;
    Vector3 targetPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTrans = GameObject.Find("Character").transform;
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = playerTrans.position - transform.position;

        DrawLOS(targetPos);

        // Can charge.
        if (chargeTimer <= 0 && targetPos.magnitude < chargeRange)
        {
            // If has line of sight.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPos, targetPos.magnitude, obstacleLayer);
            if (hit.collider == null)
            {
                Charge();
            }
        }
        else chargeTimer -= Time.deltaTime;
    }

    void Charge()
    {
        chargeTimer = rateOfCharges;

        targetPos = playerTrans.position - transform.position;
        rb.AddForce(targetPos * chargeForce);
    }

    // Suboptimal as resuses logic but is self contained.
    void DrawLOS(Vector3 targetPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPos, targetPos.magnitude, obstacleLayer);

        // In range.
        if (targetPos.magnitude < chargeRange)
        {
            // Has Line of Sight.
            if (hit.collider == null)
            {
                Debug.DrawRay(transform.position, targetPos, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, targetPos, Color.red);
            }
        }
    }
}
