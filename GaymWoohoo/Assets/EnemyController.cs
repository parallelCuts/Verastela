using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health;
    public float damage;
    public PlayerController pCScript;
    public Animator animator;
    public GameObject enemy;
    public TurnSystem tSScript;
    public string lCName;
    public string cCName;
    public string lcCName;
    public bool plsStopIBeg = true;
    public bool isDead = false;
    public int pITW;
    public int tW;
    public int lOAP = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log((tSScript.turnNum % (tW + 1) == pITW).ToString() + " " + (plsStopIBeg == false).ToString() + " " + (health > 0f).ToString());
        if (health <= 0f)
        {
            Death();
        }
        if ((tSScript.turnNum%(tW+1) == pITW) && (lOAP == 0) && (health > 0f))
        {
            pCScript.Hurt(damage);
            string dialogue = "The " + lCName + " attacks you for " + damage.ToString() + " damage.";
            tSScript.UpdateTurn(dialogue);
            lOAP++;
        }
    }
    public void Hurt(float damage)
    {
        animator.SetBool("IsHurt", true);
        health -= damage;
        Invoke("Idle", 1f);
    }
    public void Idle()
    {
        animator.SetBool("IsHurt", false);
    }
    public void Death()
    {
        if ((tSScript.text.text == "") && (isDead == false))
        {
            string deathMsg = "The " + lCName + " has died.";
            tSScript.DeathTurn(deathMsg);
            isDead = true;
            animator.SetBool("IsDead", true);
            Destroy(enemy, 1f);
        }
    }
}
