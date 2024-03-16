using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // degrees the enemy rotates per second
    public float rotationSpeed = 30f;
    public float moveSpeed = 4f;
    public float closeInDistance = 1f;

    public float attackRange;
    public int attackDamage;
    // the amount of seconds between each attack
    public float attackRate;

    private GameObject player;

    private PlayerHealth playerHealth;

    private CharacterController controller;

    private Vector3 playerLastSeenAt;

    private float attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();

        controller = GetComponent<CharacterController>();

        playerLastSeenAt = transform.position;

        attackTimer = attackRate;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

        Vector3 playerDisplacement = player.transform.position - transform.position;
        Vector3 playerDir = playerDisplacement.normalized;

        if (!Physics.CapsuleCast(transform.position + Vector3.up * 0.5f, transform.position + Vector3.down * 0.5f, 1.2f, playerDir, playerDisplacement.magnitude - 2f))
        {
            playerLastSeenAt = player.transform.position;
        }

        if (Vector3.Distance(transform.position, playerLastSeenAt) >= closeInDistance)
        {
            controller.Move(playerDir * moveSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackRate)
            {
                attackTimer = 0f;
                playerHealth.TakeDamage(attackDamage);
            }
        } else
        {
            attackTimer = attackRate;
        }
    }
}
