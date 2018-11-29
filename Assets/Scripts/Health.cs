using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour{

    public int currentHealth;
    public int maxHealth;

    public int currentArmor;
    public int maxArmor;

    public int startingHealthMin;
    public int startingHealthMax;
    public int startingArmorMin;
    public int startingArmorMax;

    public RectTransform healthBar;
    public RectTransform armorBar;

    void Start ()
    {
        if(tag != "Player") {
            currentHealth = Random.Range(startingHealthMin, startingHealthMax);
            currentArmor = Random.Range(startingArmorMin, startingArmorMax);
        }
    }

    //Apply the amount of damage defined and return the new health value
    public int takeDamage(int damage) {
        //Armor absorbs half of incoming damage
        int armorDamage = Mathf.CeilToInt((float)damage * .5f);
        int healthDamage = damage;
        if(currentArmor > 0) {
            currentArmor -= armorDamage;
            if (currentArmor < 0)
                currentArmor = 0;

            healthDamage -= armorDamage;
        }
        currentHealth -= healthDamage;

        //Check for negative damage applied
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        updateBars();
        return currentHealth;
    }

    //Add the amount of health to restore and return the result
    public int heal(int amount) {
        currentHealth += amount;
        
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        updateBars();
        return currentHealth;
    }

    public int healArmor(int amount) {
        currentArmor += amount;

        if(currentArmor > maxArmor) {
            currentArmor = maxArmor;
        }
        updateBars();
        return currentArmor;
    }

    private void updateBars() {
        if (healthBar != null) {
            healthBar.sizeDelta = new Vector2((float)currentHealth / (float)maxHealth, healthBar.sizeDelta.y);
        }
        if (armorBar!= null) {
            armorBar.sizeDelta = new Vector2((float)currentArmor / (float)maxArmor, armorBar.sizeDelta.y);
        }
    }
}
