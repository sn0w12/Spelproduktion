using UnityEngine;
using UnityEngine.UI;

public class EnemyS : MonoBehaviour
{
    public Transform raycast;
    public LayerMask raycastmask;
    public bool alive = true;
    private bool isHit = false;
    public float attackDistanse;
    public float moveSpeed;
    public float timer;
    public float aggrorange;
    public float cooldown;
    public int maxHealth = 10;
    public int currentHealth;
    Rigidbody2D rb;
    public GameObject target;
    public EnemyStatDIsplayS healthBar;

    private float distance;
    private bool inrange = false;
    //private Animation animation

    private void Awake()
    {
        //get animation
        rb = GetComponent<Rigidbody2D>();
        healthBar.MaxHealth(maxHealth);
        currentHealth = maxHealth;
    }

    private void Update()
    {     
        if (isHit == false)
            EnemyLogick();

        timer -= 0.1f;

        if (timer <= 0)
            timer = 0;

        if (inrange == false)
        {
            //stop attack animation
            StopAttack();
        }

    }

    void EnemyLogick()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);
        if (distance <= aggrorange)
        {
            if (distance > attackDistanse)
            {
                Move();
                StopAttack();
            }
            if (distance <= attackDistanse && timer == 0)
                Attack();

            if (timer == 0)
            {
                StopAttack();
                //stop Attack animation
            }
        }

    }

    void Move()
    {
        //run waling animation
        Vector2 direktion = (target.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(direktion.x, 0) * moveSpeed;
    }

    void Attack()
    {
        Debug.Log("is attacking");
        timer += cooldown;
        PlayerS.instance.TakeDamige(2);
        //stop walking animation
        //run attack animation
    }

    void StopAttack()
    {
        //stop attackanimation
    }

    //TakeDamige funktion som visar Helthbar när skadad
    public void TakeDamige(int damige)
    {
        currentHealth -= damige;
        isHit = true;
        healthBar.SetHelth(currentHealth);
        //få localscale från fiende som attakerat 
        Debug.Log(currentHealth);
        if (transform.localScale.x == -2)
            rb.velocity += new Vector2(18, 6);
        else if (transform.localScale.x == 2)
            rb.velocity += new Vector2(18, 6);
        
        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        currentHealth = maxHealth;
        alive = true;
        Debug.Log(gameObject.name + " is dead");
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Trap")
            isHit = false;
        if (collision.gameObject.tag == "Trap")
            Die();
    }

}

