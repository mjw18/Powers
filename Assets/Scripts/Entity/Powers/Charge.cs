using UnityEngine;
using System.Collections;

public class Charge : Power {

    public float chargeForce = 1f;
    public float speed = 5f;

    private bool m_Charging = false;

    public Easing.VectorEasingFunction easeFunction = Easing.VectorEaseIn;
    public EasingType easeType = EasingType.Cubic;

    void Awake()
    {
        base.Init();
    }

    public override void Execute()
    {
        base.Execute();

        m_Charging = true;

        //Default charge direction is forward
        Vector3 rayToTarget = Vector3.right * player.facing;

        //Get ray to chosen target
        if (targetSelector.targets.Count > 0)
        {
            Debug.Log(targetSelector.targets[0]);
            rayToTarget = targetSelector.targets[0].transform.position - m_Player.transform.position;
        }
        GameObject hitObj = null;;
        //Change to Crcle Cast to avoid glitches where player warps through walls?
        RaycastHit2D hit = Physics2D.Raycast(player.shootPosition.position, rayToTarget, powerConfig.range);
        if (hit) hitObj = hit.collider.gameObject;

        //Strings
        //If there is an object between the player and target
        if(!hitObj)
        {
            StartCoroutine(MoveToTarget(m_Player, Vector3.Normalize(rayToTarget) * powerConfig.range));
        }
        else if (!hitObj.CompareTag("Enemy"))
        {
            StartCoroutine(MoveToTarget(m_Player, hit.point, hitObj));
            //Move to Visual Effects?
            EventManager.PostMessage(EventManager.MessageKey.ShakeCamera);
        }
        else
        {
            StartCoroutine(MoveToTarget(m_Player, hit.point, hitObj));
        }

        /* RaycastHit2D[] hits = Physics2D.CircleCastAll(m_Player.transform.position, 3f, Vector2.right * m_Player.facing, ChargeData.chargeAttackDistance);

         foreach (RaycastHit2D hit in hits)
         {
             Debug.Log("Entered for each");
             if (hit.collider.tag == "Enemy" && !hasHit)
             {
                 Debug.Log("Charge Attack Enemy at " + hit.point);
                 Debug.Log(hit.normal);

              //   StartCoroutine(SmoothMove(hit));

                 hasHit = true;
                 EventManager.PostMessage(EventManager.MessageKey.ChargeHit);
             }
         }*/
    }

    void PostCharge(GameObject hitObj, Vector3 rayToTarget)
    {

        playerRigidbody.velocity = Vector2.zero;

        if(hitObj && hitObj.CompareTag("Enemy"))
        {
            hitObj.GetComponent<Enemy>().ApplyDamage(powerConfig.damage);
            hitObj.GetComponent<Rigidbody2D>().AddForce(Vector3.Normalize(rayToTarget) * chargeForce, ForceMode2D.Impulse);
        }

        EventManager.PostMessage(EventManager.MessageKey.ChargeHit);
    }

    //It would be great to not hae to keep writing this over and over (look into retunr values/out params)
    IEnumerator MoveToTarget(GameObject a, Vector3 target, GameObject hitObj = null)
    {
        float t = 0f;

        Vector3 tempPos = a.transform.position;
        //The rate to add to the t value (1/s)
        float rate = speed / Vector3.Magnitude(a.transform.position - target);

        while (t <= 1f)
        {
            playerRigidbody.MovePosition(Easing.InterpolateVector(tempPos, target, t, easeType, easeFunction));

            //m_Player.transform.position = Easing.InterpolateVector(tempPos, target, t, easeType, easeFunction);
            //Real DeltaTime?
            t += rate * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        //Post has hit Message here?
        PostCharge(hitObj, target - tempPos);
        
        yield return new WaitForSeconds(1f);
    }

}
