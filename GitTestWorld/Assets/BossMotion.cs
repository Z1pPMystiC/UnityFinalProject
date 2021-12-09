using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class BossMotion : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsPlayer;

    public Animator animator;

    public int currentHealth;
    public int maxHealth;

    public HealthBarScript healthBar;

    public float attackDelay;

    public playerMotor playerMotor;

    public int damageToPlayer;

    public float damageDelay;

    public int projectileDamage;

    public Slider bossHealth;
    public TextMeshProUGUI bossNameText;
    public TextMeshProUGUI bossHealthText;

    public string bossName;

    public bool playerInBossArena = false;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float attackRange;
    public bool playerInAttackRange;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        bossHealth.value = 0;
        bossNameText.SetText("");
        bossHealthText.SetText("");
    }

    private void Awake()
    {
        player = GameObject.Find("PlayerObject").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        attackDelay = playerMotor.attackDelayCurrent;

        if (!playerInAttackRange && animator.GetBool("isAttacking") == false)
        {
            ChasePlayer();
        }
            
        if (playerInAttackRange)
        {
            AttackPlayer();
        }

        if (playerInBossArena)
        {
            bossHealth.value = currentHealth;
            bossNameText.SetText(bossName);
            bossHealthText.SetText(currentHealth + " / " + maxHealth + " HP");
        }
        
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);


        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

        if (!alreadyAttacked)
        {
            Invoke("TakeDamage", damageDelay);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void DestroyBoss()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Boss"))
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

        if (currentHealth <= 0 && tag == "Boss")
        {
            currentHealth = 0;
            playerMotor.SetBossDead(true);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AttackEnemy(projectileDamage);
        playerMotor.enemyHit = true;
    }

    public void TakeDamage() {
        healthBar.TakeDamage(damageToPlayer);
    }

    public void SetPlayerInBossArena(bool boolean) {
        playerInBossArena = boolean;
    }

}
