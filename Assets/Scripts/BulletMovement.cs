using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public Vector3 bulletDir;
    [SerializeField] float shotSpeed;
    public float damage;

    // Multiplier for bullet momentum.
    [SerializeField] float momentumPos;
    [SerializeField] float momentumNeg;

    private void Start()
    {
        //// Set bulletDir based off shootDir.
        //GameObject head = GameObject.Find("Head");
        //IsaacShoot isaacShoot = head.GetComponent<IsaacShoot>();
        //GameObject isaac = GameObject.Find("Isaac");
        //PlayerMovement move = isaac.GetComponent<PlayerMovement>();

        //bulletDir = isaacShoot.shootDir;
        //bulletDir = bulletDir.normalized;

        //// Increase speed when moving with. Limit speed when moving against.
        //float dirMultiplier = bulletDir == move.movement ? momentumPos : momentumNeg;
        //bulletDir.x += move.movement.x * dirMultiplier;
        //bulletDir.y += move.movement.y * momentumPos;

        //// Increase bullet 
        //bulletTimer = bulletTimerLength; //* (1 + dirMultiplier);

        //// Change sorting order for each eye.
        //SpriteRenderer spr = GetComponent<SpriteRenderer>();
        //if (bulletDir.y == 0)
        //{
        //    spr.sortingOrder = isaacShoot.usingRightEye ? 2 : 0;
        //}
    }

    private void FixedUpdate()
    {
        transform.position += bulletDir * shotSpeed * Time.fixedDeltaTime;
    }

    public void SetDirection(Vector3 newBulletDir)
    {
        bulletDir = newBulletDir;
    }

    public void SetDirection(Vector2 newBulletDir)
    {

        bulletDir = new Vector3(newBulletDir.x, newBulletDir.y, 0);
    }
}
