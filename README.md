# ⚔️ Saint Seiya Fan Game — Unity Project

> 세인트 세이야 원작 기반 팬게임 (비상업적 프로젝트)  
> Unity 2D RPG/어드벤처

---

## 🗂️ 프로젝트 구조

```
Assets/
└── _Project/
    ├── Scripts/
    │   ├── Core/        # GameManager, SaveManager, AudioManager, EventBus
    │   ├── Combat/      # BattleManager, CosmosSystem
    │   ├── Characters/  # CharacterStats
    │   ├── Data/        # ScriptableObjects (SkillData, CharacterData)
    │   ├── Quest/       # QuestManager
    │   ├── UI/          # (추가 예정)
    │   └── Utils/       # (추가 예정)
    ├── Prefabs/
    ├── Scenes/
    ├── Sprites/
    ├── Audio/
    └── Data/
```

## 🎮 핵심 시스템

| 시스템 | 파일 | 설명 |
|--------|------|------|
| 게임 매니저 | `Core/GameManager.cs` | 싱글톤, 씬 전환, 상태 관리 |
| 저장 시스템 | `Core/SaveManager.cs` | JSON 기반 저장/불러오기 |
| 이벤트 버스 | `Core/EventBus.cs` | 시스템 간 결합도 분리 |
| 코스모 시스템 | `Combat/CosmosSystem.cs` | 코스모 축적/소비/연소 |
| 전투 매니저 | `Combat/BattleManager.cs` | 턴제+실시간 혼합 전투 |
| 캐릭터 스탯 | `Characters/CharacterStats.cs` | HP, 공격/방어, 사망 처리 |
| 퀘스트 매니저 | `Quest/QuestManager.cs` | 퀘스트 수락/진행/완료 |

## 🛠️ 권장 Unity 패키지

- **DOTween** — 연출 애니메이션
- **Cinemachine** — 카메라 연출
- **TextMeshPro** — UI 텍스트
- **Newtonsoft.Json** — 저장 시스템
- **Unity Addressables** — 에셋 번들

## 📋 씬 구성

| 씬 | 용도 |
|----|------|
| `Boot` | 초기화, 저장 데이터 로드 |
| `MainMenu` | 타이틀 화면 |
| `WorldMap` | 지역 선택 |
| `Field_XXX` | 필드 탐색 |
| `Battle` | 전투 |
| `Dialogue` | 컷씬/대화 |

## ⚠️ 저작권 고지

이 프로젝트는 차학재(마사미 쿠라다)의 원작 만화 **세인트 세이야**를 기반으로 한 **비상업적 팬게임**입니다.  
상업적 이용을 일절 금지합니다.
