using UnityEngine;
using TMPro;

namespace SaintSeiya.UI
{
    public enum DamageType { Damage, Heal, Cosmos, Critical }

    public class DamagePopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Color _damageColor = Color.red;
        [SerializeField] private Color _healColor   = Color.green;
        [SerializeField] private Color _cosmosColor = new Color(0.3f, 0.5f, 1f);
        [SerializeField] private Color _critColor   = Color.yellow;
        [SerializeField] private float _floatHeight = 1.5f;
        [SerializeField] private float _duration    = 1.2f;

        public static DamagePopup Create(GameObject prefab, Vector3 worldPos, float amount, DamageType type = DamageType.Damage)
        {
            var go = Instantiate(prefab, worldPos, Quaternion.identity);
            go.GetComponent<DamagePopup>()?.Show(amount, type);
            return go.GetComponent<DamagePopup>();
        }

        public void Show(float amount, DamageType type)
        {
            string prefix = type == DamageType.Heal ? "+" : type == DamageType.Cosmos ? "✦" : type == DamageType.Critical ? "!!" : "-";
            Color color   = type == DamageType.Heal ? _healColor : type == DamageType.Cosmos ? _cosmosColor : type == DamageType.Critical ? _critColor : _damageColor;
            if (_text != null) { _text.text = $"{prefix}{Mathf.RoundToInt(amount)}"; _text.color = color; }
            StartCoroutine(AnimateAndDestroy());
        }

        System.Collections.IEnumerator AnimateAndDestroy()
        {
            float elapsed = 0f;
            Vector3 startPos = transform.position;
            while (elapsed < _duration)
            {
                elapsed += Time.deltaTime;
                transform.position = startPos + Vector3.up * (_floatHeight * elapsed / _duration);
                if (_text != null) _text.alpha = 1f - (elapsed / _duration);
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
