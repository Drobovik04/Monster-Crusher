using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text hpText;

    private float maxHP;
    private float currentHP;

    public void Initialize(float maxHealth)
    {
        maxHP = maxHealth;
        currentHP = maxHealth;
        UpdateBar();
    }

    public void SetHealth(float current, float max)
    {
        currentHP = Mathf.Clamp(current, 0, max);
        maxHP = max;
        UpdateBar();
    }

    private void UpdateBar()
    {
        float fillValue = (float)currentHP / maxHP;
        fillImage.DOFillAmount(fillValue, 0.1f).SetEase(Ease.InCubic);
        hpText.text = $"{currentHP} / {maxHP}";
    }
}
