using UnityEngine;
using ExtendedEvents;
using System.Collections;

public class Charge : Power {

    public float chargeForce = 1f;
    public float speed = 5f;

    private Transform m_CastPosition;

    private bool m_Charging = false;

    public Easing.VectorEasingFunction easeFunction = Easing.VectorEaseIn;
    public EasingType easeType = EasingType.Cubic;

    void Awake()
    {
        base.Init();
        m_CastPosition = player.shootPosition;
    }

    public override void Execute()
    {
        base.Execute();

        m_Charging = true;

        //Default charge direction is forward
        Vector3 rayToTarget = GetChargeDirection();

        GameObject hitObj = null;
        //Change to Crcle Cast to avoid glitches where player warps through walls?
        RaycastHit2D hit = Physics2D.Raycast(player.shootPosition.position, rayToTarget, powerConfig.range);

        if (hit) hitObj = hit.collider.gameObject;
        //If the casted object is null (no object hit) then return
        if (hitObj == null) return;

        //If there is an object between the player and target
        if (!hitObj.CompareTag(Tags.enemy))
        {
            StartCoroutine(MoveToTarget(m_Player, hit.point, hitObj));
        }
        else
        {
            StartCoroutine(MoveToTarget(m_Player, hit.point, hitObj));
        }
    }

    Vector3 GetChargeDirection()
    {
        //Default charge direction is forward
        Vector3 rayToTarget = Vector3.right * player.facing;

        //Get ray to chosen target, change to use DamageType
        if (targetSelector.targets.Count == 0) return Vector3.zero;

        //Store target in power, why, I don't know
        target = targetSelector.targets[0].GetComponent<Transform>();

        return target.position - m_CastPosition.position;
    }

    void PostCharge(GameObject hitObj, Vector3 rayToTarget)
    {

        playerRigidbody.velocity = Vector2.zero;

        if(hitObj.CompareTag(Tags.enemy))
        {
            hitObj.GetComponent<Enemy>().ApplyDamage(powerConfig.damage);
            hitObj.GetComponent<Rigidbody2D>().AddForce(Vector3.Normalize(rayToTarget) * chargeForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("Hit a wall?");
        }
    }

    IEnumerator MoveToTarget(GameObject a, Vector3 target, GameObject hitObj = null)
    {
        float t = 0f;

        Vector3 tempPos = a.transform.position;
        //The rate to add to the t value (1/s)
        float rate = speed / Vector3.Magnitude(a.transform.position - target);

        while (t <= 1f)
        {
            playerRigidbody.MovePosition(Easing.InterpolateVector(tempPos, target, t, easeType, easeFunction));

            t += rate * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        //Post has hit Message here?
        PostCharge(hitObj, target - tempPos);
        EventManager.PostMessage<ChargeHitMessage>(new ChargeHitMessage(0.5f));

        yield return new WaitForSeconds(2f);
    }

}
