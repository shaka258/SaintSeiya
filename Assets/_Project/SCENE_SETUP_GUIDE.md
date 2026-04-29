# 🎮 Saint Seiya Fan Game — Unity 씬 배치 가이드

Unity 에디터를 열고 아래 순서대로 따라하면 첫 실행이 가능한 상태가 됩니다.

---

## ✅ 사전 준비 — 패키지 설치 확인

**Window → Package Manager** 에서 아래 패키지가 설치되어 있는지 확인하세요.

| 패키지 | 용도 |
|--------|------|
| TextMeshPro | UI 텍스트 |
| Input System | 플레이어 입력 |
| Newtonsoft Json | 저장 시스템 |
| Cinemachine | 카메라 |

---

## 1️⃣ Boot 씬 설정

> **File → Open Scene → Assets/Scenes/Boot.unity**

### Hierarchy 구성

```
Boot [씬]
├── GameManager          ← 빈 오브젝트
│   └── GameManager.cs 부착
│   └── SaveManager.cs 부착
├── AudioManager         ← 빈 오브젝트
│   └── AudioManager.cs 부착
│   └── Audio Source x2  (BGM용, SFX용)
├── InventoryManager     ← 빈 오브젝트
│   └── InventoryManager.cs 부착
├── DialogueManager      ← 빈 오브젝트
│   └── DialogueManager.cs 부착
│   └── DialogueLoader.cs 부착
├── BootLoader           ← 빈 오브젝트
│   └── BootLoader.cs 부착
└── Canvas (UI)
    └── LoadingText      ← TextMeshPro
```

### Inspector 연결

**GameManager 오브젝트 선택:**
- `Save Manager` 필드 → GameManager 오브젝트 드래그
- `Audio Manager` 필드 → AudioManager 오브젝트 드래그

**AudioManager 오브젝트 선택:**
- `Bgm Source` → AudioSource (루프 ON) 드래그
- `Sfx Source` → AudioSource (루프 OFF) 드래그

**BootLoader 오브젝트 선택:**
- `Minimum Load Time` → 1.5

---

## 2️⃣ MainMenu 씬 설정

> **File → Open Scene → Assets/Scenes/MainMenu.unity**

### Hierarchy 구성

```
MainMenu [씬]
├── Canvas
│   ├── MainMenuUI       ← MainMenuUI.cs 부착
│   │   ├── TitleText    ← TextMeshPro "SAINT SEIYA"
│   │   ├── NewGameBtn   ← Button
│   │   ├── ContinueBtn  ← Button
│   │   ├── SettingsBtn  ← Button
│   │   └── QuitBtn      ← Button
│   └── SettingsPanel    ← 비활성화 상태
└── EventSystem
```

### Inspector 연결

**MainMenuUI 선택:**
- 각 Button 필드에 해당 버튼 드래그
- `Settings Panel` 필드에 SettingsPanel 오브젝트 드래그

---

## 3️⃣ Field_Sanctuary 씬 설정

> **File → Open Scene → Assets/Scenes/Field_Sanctuary.unity**

### Hierarchy 구성

```
Field_Sanctuary [씬]
├── --- MANAGERS ---
│   ├── QuestManager     ← QuestManager.cs 부착
│   └── SoundManager     ← (AudioManager는 DontDestroy로 Boot에서 유지)
│
├── --- PLAYER ---
│   └── Player           ← Tag: "Player"
│       ├── SpriteRenderer
│       ├── Rigidbody2D  (Gravity Scale: 0, Freeze Rotation Z)
│       ├── CapsuleCollider2D
│       ├── Animator
│       ├── PlayerInput  ← PlayerInputActions 에셋 연결
│       ├── PlayerController.cs
│       │     - Move Speed: 5
│       │     - Dash Speed: 12
│       │     - Interact Layer: NPC, Item 레이어
│       ├── CharacterStats.cs
│       │     - Data: Char_PegasusSeiya 에셋 연결
│       ├── CosmosSystem.cs
│       └── LevelSystem.cs
│
├── --- CAMERA ---
│   └── CM vcam1         ← Cinemachine Virtual Camera
│       - Follow: Player 드래그
│       - Dead Zone: 0.1, 0.1
│
├── --- ENEMIES ---
│   └── Enemy_01         ← Tag: "Enemy"
│       ├── SpriteRenderer
│       ├── Rigidbody2D  (Gravity Scale: 0)
│       ├── CapsuleCollider2D
│       ├── Animator
│       ├── EnemyController.cs
│       │     - Enemy Id: "enemy_dark_saint_01"
│       │     - Detect Range: 4
│       │     - Battle Range: 1.2
│       └── CharacterStats.cs
│             - Data: (적 캐릭터 데이터 연결)
│
├── --- NPC ---
│   └── Elder_NPC        ← Tag: "NPC"
│       ├── SpriteRenderer
│       ├── CapsuleCollider2D (IsTrigger: ON)
│       ├── NPCInteractable.cs
│       │     - Npc Name: "장로"
│       │     - Dialogue Lines 추가
│       └── InteractPrompt   ← 자식 오브젝트 (! 이미지)
│
├── --- FIELD ITEMS ---
│   └── FieldItem_Potion ← Tag: "Item"
│       ├── SpriteRenderer
│       ├── CircleCollider2D (IsTrigger: ON)
│       └── FieldItem.cs
│             - Item Data: Item_SmallHealPotion 에셋 연결
│             - Amount: 1
│
├── --- UI ---
│   └── HUD_Canvas
│       ├── HUDController.cs 부착
│       ├── HP_Slider     ← Slider
│       ├── HP_Text       ← TextMeshPro
│       ├── CosmosGauge   ← CosmosGaugeUI.cs 부착
│       ├── PlayerName    ← TextMeshPro
│       └── LevelText     ← TextMeshPro
│   └── Inventory_Canvas
│       └── InventoryUI.cs 부착
│   └── Dialogue_Canvas
│       └── DialogueUI.cs 부착
│
└── --- TILEMAP ---
    └── Grid
        ├── Ground       ← Tilemap (Tilemap Collider 2D)
        └── Walls        ← Tilemap (Composite Collider 2D)
```

### ⚠️ 중요 설정

**Player Rigidbody2D:**
- Gravity Scale: **0** (탑다운 2D)
- Freeze Rotation: **Z 체크**
- Collision Detection: **Continuous**

**레이어 설정 (Edit → Project Settings → Tags and Layers):**

| 레이어 | 번호 | 용도 |
|--------|------|------|
| Player | 8 | 플레이어 |
| Enemy | 9 | 적 |
| NPC | 10 | NPC |
| Item | 11 | 필드 아이템 |
| Ground | 12 | 지형 |

**PlayerController Interact Layer:**
- NPC(10) + Item(11) 체크

---

## 4️⃣ Battle 씬 설정

```
Battle [씬]
├── BattleManager        ← BattleManager.cs 부착
├── DialogueManager      ← (전투 전 대사용)
│
├── --- PLAYER ---
│   └── PlayerBattle
│       ├── CharacterStats.cs (Char_PegasusSeiya 연결)
│       └── CosmosSystem.cs
│
├── --- ENEMIES ---
│   └── EnemyBattle_01
│       └── CharacterStats.cs
│
└── --- UI ---
    └── BattleUI_Canvas
        ├── HUDController.cs
        ├── CosmosGaugeUI.cs
        ├── BurnButton      ← Button
        ├── AttackButton    ← Button
        ├── SkillButton_01  ← SkillButtonUI.cs
        ├── SkillButton_02  ← SkillButtonUI.cs
        ├── DamagePopup_Prefab (프리팹으로 만들기)
        └── BattleResultUI.cs
```

---

## 5️⃣ Build Settings 확인

**File → Build Settings:**

| 순서 | 씬 |
|------|----|
| 0 | Assets/Scenes/Boot.unity |
| 1 | Assets/Scenes/MainMenu.unity |
| 2 | Assets/Scenes/WorldMap.unity |
| 3 | Assets/Scenes/Field_Sanctuary.unity |
| 4 | Assets/Scenes/Battle.unity |

---

## 6️⃣ 첫 실행 체크리스트

- [ ] Boot 씬이 Build Settings 0번인지 확인
- [ ] Player 태그 설정 확인
- [ ] PlayerInput에 PlayerInputActions 연결 확인
- [ ] CharacterStats에 CharacterData 에셋 연결 확인
- [ ] HUDController에 Player CharacterStats 연결 확인
- [ ] Boot 씬에서 Play 버튼 눌러 MainMenu로 자동 전환되는지 확인
