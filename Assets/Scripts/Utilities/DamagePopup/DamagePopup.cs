using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    public float moveY = 1f;
    public float duration = 1f;
    public float scaleUp = 1.3f;

    private TextMeshProUGUI textMesh;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Setup(float damage, Color color)
    {
        textMesh.text = damage.ToString();
        textMesh.color = color;
        transform.localScale = Vector3.one * 0.7f;
        canvasGroup.alpha = 1f;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(Random.Range(-0.3f, 0.3f), moveY, 0f);

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(scaleUp, 0.2f).SetEase(Ease.OutBack))
           .Join(transform.DOMove(endPos, duration).SetEase(Ease.OutCubic))
           .Join(canvasGroup.DOFade(0, duration).SetEase(Ease.InQuad).SetDelay(duration * 0.5f))
           .AppendCallback(() => Destroy(gameObject));
    }
}

