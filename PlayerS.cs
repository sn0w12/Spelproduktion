using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerS : MonoBehaviour
{
    public static PlayerS instance;
    public float speed;
    public int jump;
    public float fall;
    public float knockBack;
    public float slide;
    public float timer;
    public float attackRange = 0.5f;
    public float grabWall = 0;
    const float wallRadius = 0.2f;
    public float attackTimer;
    public int jumplock = 0;
    private int falllock = 0;
    public int maxHealth = 10;
    public int currentHealth;
    bool isgrounded = false;
    public bool alive = true;
    private bool isHit = false;
    private bool trapHit;
    bool canWalljump = false;
    public LayerMask wall;
    public LayerMask enemyLayer;
    public Transform colideWall;
    public Transform attackPoint;
    private Rigidbody2D body;
    public HealthBar healthBar;
    //private GameObject attackArea = default;
    public bool sliding = false;
    bool wallJumping = false;
    public float xWallJumpforce;
    public float yWallJumpforce;
    public float wallJumpTime;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        healthBar.MaxHealth(maxHealth);
        currentHealth = maxHealth;
        //attackArea = transform.GetChild(0).gameObject;
    }

    public void Update()
    {
        float horImput = Input.GetAxis("Horizontal");
        if (isHit != true)
        {
            body.velocity += new Vector2(horImput * speed, 0);
            if (body.velocity.x >= 9)
                body.velocity = new Vector2(9, body.velocity.y);
            else if (body.velocity.x <= -9)
                body.velocity = new Vector2(-9, body.velocity.y);
        }

        timer -= 0.1f;
        if (timer <= 0)
            timer = 0;
        grabWall -= 0.01f;
        if (grabWall <= 0)
            grabWall = 0;

        if (horImput > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horImput < -0.001f)
            transform.localScale = new Vector3(-1, 1, 1);

        if (jumplock != 3)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                body.velocity = new Vector2(body.velocity.x, jump);
                jumplock++;
            }
            if (Input.GetKeyDown(KeyCode.W) && canWalljump == true || Input.GetKeyDown(KeyCode.Space) && canWalljump == true || Input.GetKeyDown(KeyCode.UpArrow) && canWalljump == true)
            {
                wallJumping = true;
                Invoke("SetTozero", wallJumpTime);
            }
            if (wallJumping == true)
            {
                body.velocity = new Vector2(xWallJumpforce * -horImput, yWallJumpforce);
            }
        }

        if (timer == 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E))
                Attack();
        }

        if (falllock != 1)
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                body.velocity = new Vector2(body.velocity.x, -fall);
                falllock++;
            }
        }
        if (grabWall == 0)
        {
            WallChek();
        }
    }

    private void Attack()
    {
        attackPoint.position = new Vector2(attackPoint.position.x + (attackRange/2), transform.position.y);
        Collider2D[] hitEnemys = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRange, 2), enemyLayer);
        attackPoint.position = new Vector2(attackPoint.position.x - (attackRange / 2), transform.position.y);
        foreach (Collider2D enemys in hitEnemys)
        {
            try
            {
                enemys.GetComponent<EnemyS>().TakeDamige(2);
            }
            catch
            {

            }
        }
        timer += attackTimer;
    }

    public void TakeDamige(int damige)
    {
        float horImput = Input.GetAxis("Horizontal");
        currentHealth -= damige;
        healthBar.SetHelth(currentHealth);
        isHit = true;
        //få localscale från fiende som attakerat 

        if (trapHit == true)        
            body.velocity += new Vector2(knockBack - 4 * -gameObject.transform.localScale.x, 6);        
        else        
            body.velocity += new Vector2(knockBack * -gameObject.transform.localScale.x, 6);       

        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        currentHealth = maxHealth;
        healthBar.SetHelth(currentHealth);
        alive = true;
        Debug.Log("Dead"); 
        SceneManager.LoadScene(sceneName: "Game");
    }

    private void WallChek()
    {
        float horImputs = Input.GetAxis("Horizontal");
        if (Physics2D.OverlapCircle(colideWall.position, wallRadius, wall) && Mathf.Abs(horImputs) > 0 && body.velocity.y < 0 && isgrounded == false)
        {
            sliding = true;
            Vector2 v = body.velocity;
            v.y = -slide;
            body.velocity = v;
        }
    }

    private void SetTozero()
    {
        wallJumping = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            falllock = 0;
            jumplock = 0;
            isgrounded = true;
            body.rotation = 1; 
            sliding = false;
            canWalljump = false;
        }
        else if (collision.gameObject.tag != "Floor")
            isgrounded = false;
        if (collision.gameObject.tag == "Trap")
        {
            jumplock = 3;
            trapHit = true;
            TakeDamige(1);
            trapHit = false;
        }
        if (collision.gameObject.tag != "Trap")
            isHit = false;
        if (collision.gameObject.tag == "Wall")
            canWalljump = true;

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trap")
        {
            if (transform.localScale.x == 1)
            {
                body.velocity += new Vector2(-6, 10);
            }
            else if (transform.localScale.x == -1)
            {
                body.velocity += new Vector2(6, 10);
            }
        }
        if (collision.gameObject.tag == "Floor")        
            canWalljump = false;        
        if (collision.gameObject.tag == "Wall")
            canWalljump = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}