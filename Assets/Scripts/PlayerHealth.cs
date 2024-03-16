using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public Image healthContent;
    public GameObject screenGradient;

    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        if (healthContent == null)
        {
            healthContent = GameObject.FindGameObjectWithTag("HPContent").GetComponent<Image>();
        }
        if (screenGradient == null)
        {
            screenGradient = GameObject.FindGameObjectWithTag("ScreenGradient");
        }

        currentHealth = maxHealth;

        screenGradient.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        ChangeHealth(-damage);
    }

    public void ChangeHealth(int change)
    {
        currentHealth = Mathf.Clamp(currentHealth + change, 0, maxHealth);
        healthContent.fillAmount = (float)currentHealth / (float)maxHealth;
        if (currentHealth == 0)
        {
            screenGradient.SetActive(true);
            Invoke("Respawn", 2f);
        }
    }

    private void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
