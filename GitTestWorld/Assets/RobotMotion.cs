using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class RobotMotion : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public GameObject playerBody;

    public LayerMask whatIsGround, whatIsPlayer;

    public Animator animator;

    public int currentHealth;

    public HealthBarScript healthBar;

    public int playerDamage;

    public Rigidbody rigidBody;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float attackRange;
    public bool playerInAttackRange;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = 100;
        rigidBody.freezeRotation = true;
    }

    private void Awake()
    {
        player = GameObject.Find("PlayerObject").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInAttackRange) ChasePlayer();
        if (playerInAttackRange)
        {
            AttackPlayer();
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                AttackEnemy(playerDamage);
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            DestroyEnemies();
        }

        if(healthBar.IsDead())
        {
            DestroyEnemies();
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);
    }

    private void AttackPlayer() {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", true);

        if (!alreadyAttacked)
        {
            healthBar.TakeDamage(2);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void DestroyEnemies()
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
        
    }

    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
    }

    public void AttackEnemy(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            currentHealth = 100;
        }
    }
}
