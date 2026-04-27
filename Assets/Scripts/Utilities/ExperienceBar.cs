using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    private int expToLevelUp;
    private int currentExp;

    public void Initialize(int expToLevelUp)
    {
        this.expToLevelUp = expToLevelUp;
        currentExp = 0;
        UpdateBar();
    }

    public void SetExp(int exp, int expToLevel)
    {
        currentExp = Mathf.Clamp(exp, 0, expToLevel);
        this.expToLevelUp = expToLevel;
        UpdateBar();
    }

    private void UpdateBar()
    {
        float fillValue = (float)currentExp / expToLevelUp;
        fillImage.DOFillAmount(fillValue, 0.1f).SetEase(Ease.InCubic);
    }
}
