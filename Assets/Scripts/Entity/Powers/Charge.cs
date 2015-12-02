using UnityEngine;
using System.Collections;

public class Charge : Power {

	// Use this for initialization
	void Start () {
	
	}

    public override void Execute()
    {
        base.Execute();

        bool hasHit = false;

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

    // Update is called once per frame
    void Update () {
	
	}
}
