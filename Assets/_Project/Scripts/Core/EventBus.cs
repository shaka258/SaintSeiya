using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaintSeiya.Core
{
    /// <summary>
    /// 시스템 간 결합도를 낮추는 이벤트 버스
    /// 사용법: EventBus.Subscribe<BattleStartEvent>(OnBattleStart);
    ///         EventBus.Publish(new BattleStartEvent { ... });
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _handlers = new();

        public static void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!_handlers.ContainsKey(type))
                _handlers[type] = new List<Delegate>();
            _handlers[type].Add(handler);
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (_handlers.ContainsKey(type))
                _handlers[type].Remove(handler);
        }

        public static void Publish<T>(T eventData)
        {
            var type = typeof(T);
            if (!_handlers.ContainsKey(type)) return;

            foreach (var handler in _handlers[type].ToArray())
            {
                try { ((Action<T>)handler)?.Invoke(eventData); }
                catch (Exception e) { Debug.LogError($"[EventBus] {type.Name} 처리 오류: {e}"); }
            }
        }

        public static void Clear() => _handlers.Clear();
    }

    // ─── 이벤트 정의 ───────────────────────────────────────────

    public struct BattleStartEvent
    {
        public string EnemyId;
        public string SceneContext;
    }

    public struct BattleEndEvent
    {
        public bool IsVictory;
        public int ExpGained;
    }

    public struct CosmosChangedEvent
    {
        public float Current;
        public float Max;
    }

    public struct CosmosMaxEvent
    {
        public string CharacterId;
    }

    public struct QuestUpdatedEvent
    {
        public string QuestId;
        public string ObjectiveId;
        public int Progress;
    }

    public struct QuestCompletedEvent
    {
        public string QuestId;
    }

    public struct DialogueStartEvent
    {
        public string DialogueId;
    }

    public struct SceneTransitionEvent
    {
        public string FromScene;
        public string ToScene;
    }
}
