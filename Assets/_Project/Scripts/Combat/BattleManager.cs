using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SaintSeiya.Characters;
using SaintSeiya.Data;

namespace SaintSeiya.Combat
{
    /// <summary>
    /// 전투 진행 매니저 — 턴제/실시간 혼합 방식
    /// </summary>
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance { get; private set; }

        public enum BattleState
        {
            Idle,
            PlayerTurn,
            EnemyTurn,
            SkillAnimation,
            Victory,
            Defeat
        }

        [Header("Battle State")]
        [SerializeField] private BattleState _state = BattleState.Idle;
        public BattleState State => _state;

        [Header("Combatants")]
        public CharacterStats Player;
        public List<CharacterStats> Enemies = new();

        [Header("Settings")]
        [SerializeField] private float _turnDelay = 0.5f;
        [SerializeField] private float _enemyTurnDelay = 1.2f;

        // 이벤트
        public event System.Action<BattleState> OnStateChanged;
        public event System.Action<CharacterStats, float> OnDamageDealt;
        public event System.Action<bool> OnBattleEnd; // true=승리

        void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        public void StartBattle(CharacterStats player, List<CharacterStats> enemies)
        {
            Player = player;
            Enemies = enemies;
            Player.cosmos.Reset();
            ChangeState(BattleState.PlayerTurn);
            Debug.Log("[BattleManager] 전투 시작!");
        }

        void ChangeState(BattleState newState)
        {
            _state = newState;
            OnStateChanged?.Invoke(newState);
        }

        // ─── 플레이어 액션 ─────────────────────────────────────

        public void PlayerAttack(CharacterStats target)
        {
            if (_state != BattleState.PlayerTurn) return;
            ExecuteNormalAttack(Player, target);
            StartCoroutine(AfterPlayerAction());
        }

        public void PlayerUseSkill(SkillData skill, CharacterStats target)
        {
            if (_state != BattleState.PlayerTurn) return;
            if (!Player.cosmos.ConsumeCosmos(skill.cosmosCost))
            {
                Debug.Log("[BattleManager] 코스모 부족!");
                return;
            }
            StartCoroutine(ExecuteSkillRoutine(Player, skill, target));
        }

        private IEnumerator ExecuteSkillRoutine(CharacterStats attacker, SkillData skill, CharacterStats target)
        {
            ChangeState(BattleState.SkillAnimation);

            // VFX 연출
            if (skill.vfxPrefab != null)
            {
                var vfx = Instantiate(skill.vfxPrefab, target.transform.position, Quaternion.identity);
                Destroy(vfx, 2f);
            }

            yield return new WaitForSeconds(skill.animationDuration);

            float damage = CalculateDamage(attacker, target, skill);
            ApplyDamage(target, damage);

            yield return new WaitForSeconds(_turnDelay);
            StartCoroutine(AfterPlayerAction());
        }

        private IEnumerator AfterPlayerAction()
        {
            if (CheckBattleEnd()) yield break;
            yield return new WaitForSeconds(_turnDelay);
            StartCoroutine(EnemyTurnRoutine());
        }

        // ─── 데미지 계산 ───────────────────────────────────────

        public float CalculateDamage(CharacterStats attacker, CharacterStats defender, SkillData skill = null)
        {
            float baseDmg = attacker.Attack;
            float multiplier = skill != null ? skill.powerMultiplier : 1f;
            float cosmosBonus = attacker.cosmos != null
                ? 1f + attacker.cosmos.CosmosRatio * 0.5f
                : 1f;

            float rawDmg = baseDmg * multiplier * cosmosBonus;
            float finalDmg = Mathf.Max(1f, rawDmg - defender.Defense);

            // 코스모 연소 중 데미지 2배
            if (attacker.cosmos != null && attacker.cosmos.IsBurning)
                finalDmg *= 2f;

            return Mathf.Round(finalDmg);
        }

        private void ExecuteNormalAttack(CharacterStats attacker, CharacterStats target)
        {
            float damage = CalculateDamage(attacker, target);
            ApplyDamage(target, damage);
            attacker.cosmos?.GainCosmos(10f); // 일반 공격 시 코스모 +10
        }

        private void ApplyDamage(CharacterStats target, float damage)
        {
            target.TakeDamage(damage);
            OnDamageDealt?.Invoke(target, damage);
            Core.EventBus.Publish(new BattleDamageEvent { Target = target.name, Damage = damage });
        }

        // ─── 적 턴 ─────────────────────────────────────────────

        private IEnumerator EnemyTurnRoutine()
        {
            ChangeState(BattleState.EnemyTurn);

            foreach (var enemy in Enemies)
            {
                if (enemy == null || enemy.IsDead) continue;
                yield return new WaitForSeconds(_enemyTurnDelay);

                float damage = CalculateDamage(enemy, Player);
                ApplyDamage(Player, damage);

                if (CheckBattleEnd()) yield break;
            }

            yield return new WaitForSeconds(_turnDelay);
            ChangeState(BattleState.PlayerTurn);
        }

        // ─── 승패 판정 ─────────────────────────────────────────

        private bool CheckBattleEnd()
        {
            if (Player.IsDead)
            {
                EndBattle(false);
                return true;
            }
            if (Enemies.TrueForAll(e => e.IsDead))
            {
                EndBattle(true);
                return true;
            }
            return false;
        }

        private void EndBattle(bool isVictory)
        {
            ChangeState(isVictory ? BattleState.Victory : BattleState.Defeat);
            OnBattleEnd?.Invoke(isVictory);
            Core.EventBus.Publish(new Core.BattleEndEvent
            {
                IsVictory = isVictory,
                ExpGained = isVictory ? CalculateTotalExp() : 0
            });
            Debug.Log($"[BattleManager] 전투 종료 — {(isVictory ? "승리" : "패배")}");
        }

        private int CalculateTotalExp()
        {
            int total = 0;
            foreach (var e in Enemies) total += e.ExpReward;
            return total;
        }
    }

    // 추가 이벤트 정의
    public struct BattleDamageEvent
    {
        public string Target;
        public float Damage;
    }
}
