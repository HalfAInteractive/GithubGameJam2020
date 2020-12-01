using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Tooltip("Max health set by the designer in editor.")]
    [SerializeField] int maxHealth = 3;

    public int CurrHealth { get; private set; } = 0;

    const int minHealth = 0;

    private void OnValidate()
    {
        if (maxHealth <= 1) maxHealth = 1;
    }

    private void Awake()
    {
        CurrHealth = maxHealth;
    }

    public bool IsDead => CurrHealth <= minHealth;

    public void TakeDamage(int amount)
    {
        CurrHealth = CurrHealth - amount < minHealth ? minHealth : CurrHealth - amount;
    }

    public void Heal(int amount)
    {
        CurrHealth = CurrHealth + amount > maxHealth ? maxHealth : CurrHealth + amount;
    }
}
