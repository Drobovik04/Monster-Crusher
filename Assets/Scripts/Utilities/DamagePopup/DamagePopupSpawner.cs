using Cinemachine;
using UnityEngine;

public class DamagePopupSpawner : MonoBehaviour
{
    public static DamagePopupSpawner Instance { get; private set; }

    public GameObject damagePopupPrefab;
    public Canvas worldCanvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ShowDamage(float damage, Vector3 worldPosition, Color color)
    {
        GameObject popup = Instantiate(damagePopupPrefab, worldCanvas.transform, true);
        popup.transform.position = worldPosition;

        popup.GetComponent<DamagePopup>().Setup(damage, color);
    }
}
