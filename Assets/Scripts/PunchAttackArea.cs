using UnityEngine;

public class PunchAttackArea : MonoBehaviour
{
    /// <summary>
    /// ƒpƒ“ƒ`‚Ì“–‚½‚è”»’èİ’è
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FireBall fireBall = collision.GetComponent<FireBall>();
        if(fireBall != null )
        {
            fireBall.Break();
        }
    }
}
