using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using SaintSeiya.Data;

namespace SaintSeiya.UI
{
    /// <summary>
    /// 전투 씬 스킬 버튼 하나를 담당하는 컴포넌트
    /// SkillData를 받아 아이콘/이름/코스모 비용을 표시하고
    /// 클릭 시 BattleManager에 스킬 실행 요청
    /// </summary>
    public class SkillButtonUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Image _cooldownOverlay; // 코스모 부족 시 어둡게

        private SkillData _skill;
        private System.Action<SkillData> _onSkillSelected;

        public void Setup(SkillData skill, System.Action<SkillData> onSelected)
        {
            _skill = skill;
            _onSkillSelected = onSelected;

            _icon?.sprite != null.Equals(_icon.sprite = skill.icon);
            _nameText?.SetText(skill.skillName);
            _costText?.SetText($"코스모 {skill.cosmosCost:F0}");

            _button?.onClick.RemoveAllListeners();
            _button?.onClick.AddListener(OnClick);
        }

        public void SetInteractable(bool interactable)
        {
            if (_button != null) _button.interactable = interactable;

            if (_cooldownOverlay != null)
            {
                _cooldownOverlay.gameObject.SetActive(!interactable);
                _cooldownOverlay.color = new Color(0, 0, 0, 0.6f);
            }
        }

        private void OnClick()
        {
            if (_skill == null) return;

            // 클릭 연출
            transform.DOPunchScale(Vector3.one * 0.15f, 0.2f, 5, 0.5f);
            _onSkillSelected?.Invoke(_skill);
        }
    }
}
