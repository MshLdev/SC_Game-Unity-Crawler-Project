using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[RequireComponent(typeof(Outline))]
public class Enemy : MonoBehaviour
{
    //Components
    private CharacterController     controller;
    private Animator                anim;
    private Rigidbody               rb;
    private BoxCollider             entityColider;
    //Components

    ///Scripts Dependencies
    ///
    private Navigation              navigation;
    private AudioMenager            audioM;
    ///Scripts Dependencies

    //UI references
    public GameObject               UI;
    public Image                    Filler;
    public Outline                  outline;
    //UI references

    ///Events --------------------
    public static event System.Action<bool>     MouseOnEnemy;
    public static event System.Action<Color>    EnemyHit;
    ///Events --------------------
    

    //Abilities
    public bool         isAlive       =   true;
    public string       sName         =   "Roach";
    public int          level         =   1;
    public float        hitPoints     =   100f;
    public float        maxHitPoints  =   100f;
    public float        resistance    =   10f;
    public float        speedWalk     =   3f;
    public float        speedRun      =   7f;
    public float        speedRotation =   20f;
    //Abilities


    /// for DEMO -----------------
    float statecoolfown = 0.6f;
    float statetimer = 0f;
    float Idlefactor = 1f;
    /// for Demo -----------------


    private AgentState  agentState     = AgentState.IDLE;
    private Vector3     moveVector     = Vector3.zero;
    private float       gravityFactor  = -10f;
    

    void Start()
    {
        controller      = transform.parent.GetComponent<CharacterController>();
        rb              = gameObject.GetComponent<Rigidbody>();
        anim            = gameObject.GetComponent<Animator>();
        entityColider   = gameObject.GetComponent<BoxCollider>();
        navigation      = GameObject.FindWithTag("_GAME").GetComponent<Navigation>();
        audioM          = GameObject.FindWithTag("_GAME").GetComponent<AudioMenager>();
    }

    void Update()
    {
        if(isAlive)
        {
            healthBar();
            AI();
            movement();
            transform.parent.GetComponent<LineRenderer>().SetPositions(new Vector3[] {transform.position, navigation.getClosestPoint(transform.position)});
        }
        //animationstaging();
    }

    void healthBar()
    {
        //fill
        Filler.fillAmount = hitPoints/maxHitPoints;
        //rotation
        UI.transform.rotation = Camera.main.transform.rotation;
        //Scaling
        float scaleFactor = Vector3.Distance(transform.position, Camera.main.transform.position) / 35.0f;
        scaleFactor = Mathf.Clamp(scaleFactor, 0.05f, 0.15f);
        UI.transform.localScale =  Vector3.one * scaleFactor;
    }


    void AI()
    {
        switch(agentState)
        {
            case AgentState.IDLE:
                break;

            case AgentState.PATROL:
                break;

            case AgentState.TAUNT:
                break;

            case AgentState.CHASE:
                break;

            case AgentState.ATTACK:
                break;
        }
    }


    //Apply desired movement direction, Gravitation
    void movement()
    {
        //Apply gravity and move
        moveVector.y = gravityFactor * Time.deltaTime;
        controller.Move(moveVector);

        //not sure if needed, just in case
        moveVector = Vector3.zero;
    }

    void lookAtPlayer()
    {
            Vector3 D = navigation.PlayerLocation - transform.position;  
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(D), speedRotation * Time.deltaTime);
            //Apply the rotation 
            transform.rotation = rot; 
            // put 0 on the axys you do not want for the rotation object to rotate
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); 
    }

    //Animation Demo for future, looks promising
    void animationstaging()
    {
        if(statetimer > statecoolfown)
        {
            anim.SetInteger("State", (int)Random.Range(0f, 2.4f));
            Idlefactor = Random.Range(0f, 1f);
            statetimer = 0f;
        }
        else
        statetimer += Time.deltaTime;

        anim.SetFloat("blend_IDLE", Idlefactor, 0.2f, Time.deltaTime);
    }



    public void DealDamage(float dmg)
    {
        hitPoints -= (dmg - (resistance * 0.1f));
        if(hitPoints <= 0 && isAlive)
            Die();
        else
            if(EnemyHit != null)
                EnemyHit?.Invoke(Color.yellow);
    }



    void Die()
    {
        //enemyEvent with color(colors not implemented yet)
        if(EnemyHit != null)
            EnemyHit?.Invoke(Color.red);

        audioM.AudioAtPlayer(AudioMenager.clips.Score);
        isAlive = false;
        SwitchRagdoll();
        OnMouseExit();
        Destroy(UI);
        Destroy(GetComponent<Enemy>());
    }



    public void SwitchRagdoll() 
    {
        entityColider.enabled = !entityColider.enabled;
        anim.enabled = !anim.enabled;

        foreach ( Rigidbody rb in transform.GetComponentsInChildren<Rigidbody>() ) 
            rb.isKinematic = !rb.isKinematic;

        foreach ( CapsuleCollider cc in transform.GetComponentsInChildren<CapsuleCollider>() ) 
            cc.enabled = !cc.enabled;
        
        
        float eyeglow = 77f;
        if(!anim.enabled)
            eyeglow = 0f;

        foreach ( Renderer go in GetComponentsInChildren<Renderer>() ) 
            go.material.SetFloat("_EyeGlow", eyeglow);
     }


    private void OnMouseEnter()
    {
        outline.enabled = true;
        if(MouseOnEnemy != null && isAlive)
            MouseOnEnemy?.Invoke(true);
    }
    private void OnMouseExit()
    {
        outline.enabled = false;
        if(MouseOnEnemy != null)
            MouseOnEnemy?.Invoke(false);
    }

}


enum AgentState
{
    IDLE,   /// -- Do Nothing, check, if there is PATROL possible or maybe Player to harras
    PATROL, /// -- Walk to the random Point, then IDLE
    TAUNT,  /// -- when Spoted Player, then CHASE 
    CHASE,  /// -- follow Player, if found ATTACK, random chance for TAUNT.
    ATTACK, /// -- try to hit Player, then CHASE or ATTACK or TAUNT.
}
