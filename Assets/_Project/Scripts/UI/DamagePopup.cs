using UnityEngine;
using TMPro;
<<<<<<< HEAD
using DG.Tweening;

namespace SaintSeiya.UI
{
    /// <summary>
    /// 데미지/힐 숫자가 캐릭터 위에 떠오르는 팝업 텍스트
    /// Object Pool과 함께 사용 권장
    /// </summary>
    public class DamagePopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        [Header("Colors")]
        [SerializeField] private Color _damageColor  = Color.red;
        [SerializeField] private Color _healColor    = Color.green;
        [SerializeField] private Color _cosmosColor  = new Color(0.3f, 0.5f, 1f);
        [SerializeField] private Color _critColor    = Color.yellow;

        [Header("Animation")]
        [SerializeField] private float _floatHeight  = 1.5f;
        [SerializeField] private float _duration     = 1.2f;

        public static DamagePopup Create(GameObject prefab, Vector3 worldPos, float amount,
                                         DamageType type = DamageType.Damage)
        {
            var go = Instantiate(prefab, worldPos, Quaternion.identity);
            var popup = go.GetComponent<DamagePopup>();
            popup.Show(amount, type);
            return popup;
=======

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
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
        }

        public void Show(float amount, DamageType type)
        {
<<<<<<< HEAD
            string prefix = "";
            Color color = _damageColor;

            switch (type)
            {
                case DamageType.Damage:
                    prefix = "-";
                    color = _damageColor;
                    break;
                case DamageType.Heal:
                    prefix = "+";
                    color = _healColor;
                    break;
                case DamageType.Cosmos:
                    prefix = "✦";
                    color = _cosmosColor;
                    break;
                case DamageType.Critical:
                    prefix = "!!";
                    color = _critColor;
                    _text.fontSize *= 1.3f;
                    break;
            }

            _text.text = $"{prefix}{Mathf.RoundToInt(amount)}";
            _text.color = color;

            // 위로 떠오르며 사라지는 애니메이션
            var seq = DOTween.Sequence();
            seq.Append(transform.DOMoveY(transform.position.y + _floatHeight, _duration)
                .SetEase(Ease.OutCubic));
            seq.Join(_text.DOFade(0f, _duration).SetDelay(_duration * 0.5f));
            seq.OnComplete(() => Destroy(gameObject));
        }
    }

    public enum DamageType { Damage, Heal, Cosmos, Critical }
=======
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
>>>>>>> 85d6086137a2cfa6b961a0149bcb69432042ea76
}
