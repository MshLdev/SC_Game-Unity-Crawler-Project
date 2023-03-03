using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    Animator        anim;
    Rigidbody       rb;
    BoxCollider     entityColider;
    Navigation      navigation;
    

    //Abilities
    public bool         isAlive     =   true;
    public string       sName       =   "Roach";
    public int          level       =   1;
    public float        hitPoints   =   100;
    public float        maxHitPoints =  100;
    public float        resistance  =   10;


    //UI references
    public GameObject   UI;
    public Image        Filler;
    public Outline      outline;


    /// for DEMO -----------------
    float statecoolfown = 0.6f;
    float statetimer = 0f;
    float Idlefactor = 1f;
    /// for Demo -----------------

    ///Events --------------------
    public static event System.Action<bool> MouseOnEnemy;
    public static event System.Action<Color> EnemyHit;


    void Start()
    {
        rb              = gameObject.GetComponent<Rigidbody>();
        anim            = gameObject.GetComponent<Animator>();
        entityColider   = gameObject.GetComponent<BoxCollider>();
        navigation      = GameObject.FindWithTag("_GAME").GetComponent<Navigation>();
    }

    void Update()
    {
        if(isAlive)
        {
            healthBar();
        }
        //animationstaging();
    }

    //physical based code
    void FixedUpdate()
    {
        addGravity();
    }

    void healthBar()
    {
        //fill
        Filler.fillAmount = hitPoints/maxHitPoints;
        //rotation
        UI.transform.rotation = Camera.main.transform.rotation;
        //Scaling
        float scaleFactor = Vector3.Distance(transform.position, Camera.main.transform.position) / 5.0f;
        scaleFactor = Mathf.Clamp(scaleFactor, 0.1f, 2.1f);
        UI.transform.localScale =  Vector3.one * scaleFactor;
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
        hitPoints -= dmg;
        if(hitPoints <= 0 && isAlive)
        {
            isAlive = false;
            SwitchRagdoll();
            //UI.SetActive(false);
            Destroy(UI);
            OnMouseExit();
            Destroy(GetComponent<Enemy>());

            if(EnemyHit != null)
                EnemyHit?.Invoke(Color.red);
        }
        else
        {
            if(EnemyHit != null)
                EnemyHit?.Invoke(Color.yellow);
        }
            
    }



    public void SwitchRagdoll() 
    {
        entityColider.enabled = !entityColider.enabled;
        anim.enabled = !anim.enabled;

        foreach ( Rigidbody rb in transform.GetComponentsInChildren<Rigidbody>() ) 
            rb.isKinematic = !rb.isKinematic;
        
        
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


    private void addGravity()
    {
        rb.MovePosition(Vector3.zero * Time.deltaTime);
    }
}
