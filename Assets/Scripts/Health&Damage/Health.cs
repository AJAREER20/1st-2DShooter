﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This class handles the health state of a game object.
/// 
/// Implementation Notes: 2D Rigidbodies must be set to never sleep for this to interact with trigger stay damage
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Team Settings")]
    [Tooltip("The team associated with this damage")]
    public int teamId = 0;

    [Header("Health Settings")]
    [Tooltip("The default health value")]
    public int defaultHealth = 1;
    [Tooltip("The maximum health value")]
    public int maximumHealth = 1;
    [Tooltip("The current in game health value")]
    public int currentHealth = 1;
    [Tooltip("Invulnerability duration, in seconds, after taking damage")]
    public float invincibilityTime = 3f;
    [Tooltip("Whether or not this health is always invincible")]
    public bool isAlwaysInvincible = false;

    [Header("Lives settings")]
    [Tooltip("Whether or not to use lives")]
    public bool useLives = false;
    [Tooltip("Current number of lives this health has")]
    public int currentLives = 3;
    [Tooltip("The maximum number of lives this health can have")]
    public int maximumLives = 5;


	private bool Toggled = false;
	public bool getToggled(){
			return Toggled;
	}
	public void setToggled(bool set){
			Toggled = set;
	}
	public void setLives(int Lives){
			currentLives = Lives;
			maximumLives = Lives + (Lives/2);  
	}
	public void setHealth(int health,string pe){
			if(pe == "Player"){
				maximumHealth = health;
				currentHealth = health;
				defaultHealth = health;
			}
			else if(pe == "Enemy"){
				maximumHealth = health;
				currentHealth = health;
				defaultHealth = health;
			} else if( pe == "Boss" ) {
				maximumHealth = (health*100);
				currentHealth = health*100;
				defaultHealth = health*100;
			}


	}
	public float getHealthPercentage(){
			return (float)currentHealth/(float)defaultHealth;
	}
    /// <summary>
    /// Description:
    /// Standard unity funciton called before the first frame update
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void Start()
    {
        SetRespawnPoint(transform.position);
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once per frame
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void Update()
    {
        InvincibilityCheck();
    }

    // The specific game time when the health can be damged again
    private float timeToBecomeDamagableAgain = 0;
    // Whether or not the health is invincible
    private bool isInvincableFromDamage = false;

    /// <summary>
    /// Description:
    /// Checks against the current time and the time when the health can be damaged again.
    /// Removes invicibility if the time frame has passed
    /// Inputs:
    /// None
    /// Returns:
    /// void (no return)
    /// </summary>
    private void InvincibilityCheck()
    {
        if (timeToBecomeDamagableAgain <= Time.time)
        {
            isInvincableFromDamage = false;
        }
    }

    // The position that the health's gameobject will respawn at if lives are being used
    private Vector3 respawnPosition;
    /// <summary>
    /// Description:
    /// Changes the respawn position to a new position
    /// Inputs:
    /// Vector3 newRespawnPosition
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="newRespawnPosition">The new position to respawn at</param>
    public void SetRespawnPoint(Vector3 newRespawnPosition)
    {
        respawnPosition = newRespawnPosition;
    }

    /// <summary>
    /// Description:
    /// Repositions the health's game object to the respawn position and resets the health to the default value
    /// Inputs:
    /// None
    /// Returns:
    /// void (no return)
    /// </summary>
    void Respawn()
    {
		GameObject.Find("Player").GetComponent<ShootingController>().setLevel(1);
        transform.position = respawnPosition;
        currentHealth = defaultHealth;
		Image HealthBar = GameObject.Find("HealthBar").GetComponent<Image>();
		HealthBar.fillAmount = 1;
		SetHealthBarColor(Color.white,HealthBar);

		GameObject HealthText = GameObject.Find("Health");
		string TextToSet = string.Format("Health: {0}",currentHealth);
		HealthText.GetComponent<UnityEngine.UI.Text>().text = TextToSet;

		GameObject LivesText = GameObject.Find("Lives");
		TextToSet = string.Format("Lives: {0}", currentLives);
		LivesText.GetComponent<UnityEngine.UI.Text>().text = TextToSet;
    }

    /// <summary>
    /// Description:
    /// Applies damage to the health unless the health is invincible.
    /// Inputs:
    /// int damageAmount
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="damageAmount">The amount of damage to take</param>
    public void TakeDamage(int damageAmount)
    {
        if (isInvincableFromDamage || isAlwaysInvincible)
        {
            return;
        }
        else
        {
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, transform.rotation, null);
            }
            timeToBecomeDamagableAgain = Time.time + invincibilityTime;
            isInvincableFromDamage = true;
            currentHealth -= damageAmount;
			if(this.name == "Player"){
				Image HealthBar = GameObject.Find("HealthBar").GetComponent<Image>();
				HealthBar.fillAmount = getHealthPercentage();

				GameObject HealthText = GameObject.Find("Health");
				string TextToSet = string.Format("Health: {0}",currentHealth);
				HealthText.GetComponent<UnityEngine.UI.Text>().text = TextToSet;

				GameObject LivesText = GameObject.Find("Lives");
				TextToSet = string.Format("Lives: {0}", currentLives);
				LivesText.GetComponent<UnityEngine.UI.Text>().text = TextToSet;

				if(HealthBar.fillAmount < 0.2f)
		        {
		            SetHealthBarColor(Color.red,HealthBar);
		        }
		        else if(HealthBar.fillAmount < 0.4f)
		        {
		            SetHealthBarColor(Color.yellow,HealthBar);
		        }
		        else
		        {
		            SetHealthBarColor(Color.green,HealthBar);
		        }

			} else if(this.name == "Boss"){
				Image HealthBar = GameObject.Find("BossHealthBar").GetComponent<Image>();
				HealthBar.fillAmount = getHealthPercentage();

				GameObject HealthText = GameObject.Find("BossHealth");
				string TextToSet = string.Format("Boss: {0}",currentHealth);
				HealthText.GetComponent<UnityEngine.UI.Text>().text = TextToSet;
				if(HealthBar.fillAmount < 0.2f)
		        {
		            SetHealthBarColor(Color.red,HealthBar);
		        }
		        else if(HealthBar.fillAmount < 0.4f)
		        {
		            SetHealthBarColor(Color.yellow,HealthBar);
		        }
		        else
		        {
		            SetHealthBarColor(Color.green,HealthBar);
		        }
			}

            CheckDeath();
        }
		if(this.name == "Boss" && (currentHealth==defaultHealth*0.2)){
			invincibilityTime = 10;	
		}
		if(this.name == "Boss" && currentHealth < ((defaultHealth*0.2)-5)){
			invincibilityTime = 0;
			for(int i = 1; i != 8; i++){
				string ob = string.Format("SCSpawner{0}",i);
				GameObject edob = GameObject.Find(ob);
				edob.GetComponent<EnemySpawner>().spawnDelay = 4;
			}
			GameObject gun1 = GameObject.Find("GunOne");
			GameObject gun2 = GameObject.Find("GunTwo");
			GameObject gun3 = GameObject.Find("GunThree");


			gun1.GetComponent<ShootingController>().fireRate = 0.3f;
			gun2.GetComponent<ShootingController>().fireRate = 0.3f;
			gun3.GetComponent<ShootingController>().fireRate = 0.3f;

			gun1.GetComponent<ShootingController>().projectilePrefab.GetComponent<Projectile>().projectileSpeed = 20;
			gun2.GetComponent<ShootingController>().projectilePrefab.GetComponent<Projectile>().projectileSpeed = 20;
			gun3.GetComponent<ShootingController>().projectilePrefab.GetComponent<Projectile>().projectileSpeed = 20;

			gun1.GetComponent<ShootingController>().setLevel(2);
			gun2.GetComponent<ShootingController>().setLevel(2);
			gun3.GetComponent<ShootingController>().setLevel(2);
			
			GameObject.Find(this.name).GetComponent<Enemy>().moveSpeed = 6;
		}
    }
	 public static void SetHealthBarColor(Color healthColor,Image HealthBar)
     {
        HealthBar.color = healthColor;
     }

    /// <summary>
    /// Description:
    /// Applies healing to the health, capped out at the maximum health.
    /// Inputs:
    /// int healingAmount
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="healingAmount">How much healing to apply</param>
    public void ReceiveHealing(int healingAmount)
    {
        currentHealth += healingAmount;
        if (currentHealth > maximumHealth)
        {
            currentHealth = maximumHealth;
        }
        CheckDeath();
    }

    [Header("Effects & Polish")]
    [Tooltip("The effect to create when this health dies")]
    public GameObject deathEffect;
    [Tooltip("The effect to create when this health is damaged")]
    public GameObject hitEffect;

    /// <summary>
    /// Description:
    /// Checks if the health is dead or not. If it is, true is returned, false otherwise.
    /// Calls Die() if the health is dead.
    /// Inputs:
    /// none
    /// Returns:
    /// bool
    /// </summary>
    /// <returns>Bool: true or false value representing if the health has died or not (true for dead)</returns>
    bool CheckDeath()
    {
        if (currentHealth <= 0)
        {
            Die();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Description:
    /// Handles the death of the health. If a death effect is set, it is created. If lives are being used, the health is respawned.
    /// If lives are not being used or the lives are 0 then the health's game object is destroyed.
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    public void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, transform.rotation, null);
        }

        if (useLives)
        {
            HandleDeathWithLives();
        }
        else
        {
            HandleDeathWithoutLives();
        }      
    }

    /// <summary>
    /// Description:
    /// Handles the death of the health when lives are being used
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void HandleDeathWithLives()
    {
        currentLives -= 1;
        if (currentLives > 0)
        {
            Respawn();
        }
        else
        {
            if (gameObject.tag == "Player" && GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }
            if (gameObject.GetComponent<Enemy>() != null)
            {
                gameObject.GetComponent<Enemy>().DoBeforeDestroy(GameObject.Find("Player"));
            }
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Description:
    /// Handles death when lives are not being used
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void HandleDeathWithoutLives()
    {
        if (gameObject.tag == "Player" && GameManager.instance != null)
        {
            GameManager.instance.GameOver();
        }
        if (gameObject.GetComponent<Enemy>() != null)
        {
            gameObject.GetComponent<Enemy>().DoBeforeDestroy(GameObject.Find("Player"));
        }
        Destroy(this.gameObject);
    }
}
