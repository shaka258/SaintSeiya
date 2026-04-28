#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace SaintSeiya.Editor
{
    public static class SceneSetupTool
    {
        [MenuItem("SaintSeiya/Setup/① Boot Scene Setup")]
        public static void SetupBootScene()
        {
            if (!Confirm("Boot")) return;
            var gm = Create("GameManager"); gm.AddComponent<Core.GameManager>(); gm.AddComponent<Core.SaveManager>();
            var am = Create("AudioManager"); am.AddComponent<Core.AudioManager>();
            Create("InventoryManager").AddComponent<Inventory.InventoryManager>();
            var dm = Create("DialogueManager"); dm.AddComponent<Dialogue.DialogueManager>(); dm.AddComponent<Dialogue.DialogueLoader>();
            Create("BootLoader").AddComponent<Core.BootLoader>();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("✅ Boot 씬 셋업 완료!");
        }

        [MenuItem("SaintSeiya/Setup/② MainMenu Scene Setup")]
        public static void SetupMainMenuScene()
        {
            if (!Confirm("MainMenu")) return;
            var canvas = MakeCanvas("MainMenu_Canvas");
            canvas.gameObject.AddComponent<UI.MainMenuUI>();
            MakeButton(canvas.transform, "NewGameBtn",  "새 게임",   new Vector2(0,  50));
            MakeButton(canvas.transform, "ContinueBtn", "계속하기",  new Vector2(0, -20));
            MakeButton(canvas.transform, "SettingsBtn", "설정",      new Vector2(0, -90));
            MakeButton(canvas.transform, "QuitBtn",     "종료",      new Vector2(0,-160));
            MakeEventSystem();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("✅ MainMenu 씬 셋업 완료!");
        }

        [MenuItem("SaintSeiya/Setup/③ Field Scene Setup")]
        public static void SetupFieldScene()
        {
            if (!Confirm("Field")) return;
            Create("QuestManager").AddComponent<Quest.QuestManager>();
            var player = new GameObject("Player"); player.tag = "Player";
            player.AddComponent<SpriteRenderer>();
            var rb = player.AddComponent<Rigidbody2D>(); rb.gravityScale = 0f; rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            player.AddComponent<CapsuleCollider2D>(); player.AddComponent<Animator>();
            player.AddComponent<UnityEngine.InputSystem.PlayerInput>();
            player.AddComponent<Characters.PlayerController>();
            player.AddComponent<Characters.CharacterStats>();
            player.AddComponent<Combat.CosmosSystem>();
            player.AddComponent<Characters.LevelSystem>();
            MakeCanvas("HUD_Canvas").gameObject.AddComponent<UI.HUDController>();
            MakeCanvas("Inventory_Canvas").gameObject.AddComponent<Inventory.InventoryUI>();
            MakeCanvas("Dialogue_Canvas").gameObject.AddComponent<Dialogue.DialogueUI>();
            var grid = new GameObject("Grid"); grid.AddComponent<Grid>();
            new GameObject("Ground").transform.SetParent(grid.transform);
            new GameObject("Walls").transform.SetParent(grid.transform);
            MakeEventSystem();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("✅ Field 씬 셋업 완료!\n⚠️ Player에 CharacterData, PlayerInputActions 연결 필요");
        }

        [MenuItem("SaintSeiya/Setup/④ Battle Scene Setup")]
        public static void SetupBattleScene()
        {
            if (!Confirm("Battle")) return;
            Create("BattleManager").AddComponent<Combat.BattleManager>();
            Create("DialogueManager").AddComponent<Dialogue.DialogueManager>();
            var player = new GameObject("PlayerBattle"); player.tag = "Player";
            player.AddComponent<SpriteRenderer>(); player.AddComponent<Characters.CharacterStats>(); player.AddComponent<Combat.CosmosSystem>();
            var enemy = new GameObject("EnemyBattle_01"); enemy.tag = "Enemy";
            enemy.AddComponent<SpriteRenderer>(); enemy.AddComponent<Characters.CharacterStats>();
            var battleCanvas = MakeCanvas("BattleUI_Canvas");
            battleCanvas.gameObject.AddComponent<UI.HUDController>();
            battleCanvas.gameObject.AddComponent<UI.BattleResultUI>();
            MakeEventSystem();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("✅ Battle 씬 셋업 완료!");
        }

        [MenuItem("SaintSeiya/Setup/⑤ Configure Tags")]
        public static void SetupTags()
        {
            foreach (var tag in new[]{"Player","Enemy","NPC","Item"}) AddTag(tag);
            Debug.Log("✅ 태그 설정 완료! (Player, Enemy, NPC, Item)");
        }

        [MenuItem("SaintSeiya/Setup/⑥ Configure Build Settings")]
        public static void SetupBuildSettings()
        {
            var paths = new[]{ "Assets/Scenes/Boot.unity","Assets/Scenes/MainMenu.unity","Assets/Scenes/WorldMap.unity","Assets/Scenes/Field_Sanctuary.unity","Assets/Scenes/Battle.unity" };
            var scenes = new EditorBuildSettingsScene[paths.Length];
            for (int i = 0; i < paths.Length; i++) scenes[i] = new EditorBuildSettingsScene(paths[i], true);
            EditorBuildSettings.scenes = scenes;
            Debug.Log("✅ Build Settings 씬 순서 설정 완료!");
        }

        private static GameObject Create(string name) { var go = new GameObject(name); Undo.RegisterCreatedObjectUndo(go, $"Create {name}"); return go; }
        private static Canvas MakeCanvas(string name) { var go = Create(name); var c = go.AddComponent<Canvas>(); c.renderMode = RenderMode.ScreenSpaceOverlay; go.AddComponent<CanvasScaler>(); go.AddComponent<GraphicRaycaster>(); return c; }
        private static GameObject MakeButton(Transform parent, string name, string label, Vector2 pos) { var go = new GameObject(name); go.transform.SetParent(parent,false); var rt = go.AddComponent<RectTransform>(); rt.sizeDelta = new Vector2(200,50); rt.anchoredPosition = pos; go.AddComponent<Image>(); go.AddComponent<Button>(); var txt = new GameObject("Text"); txt.transform.SetParent(go.transform,false); var tmp = txt.AddComponent<TextMeshProUGUI>(); tmp.text = label; tmp.alignment = TextAlignmentOptions.Center; var trt = txt.GetComponent<RectTransform>(); trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one; trt.sizeDelta = Vector2.zero; return go; }
        private static void MakeEventSystem() { if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() != null) return; var es = Create("EventSystem"); es.AddComponent<UnityEngine.EventSystems.EventSystem>(); es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>(); }
        private static void AddTag(string tag) { var so = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]); var p = so.FindProperty("tags"); for (int i=0;i<p.arraySize;i++) if (p.GetArrayElementAtIndex(i).stringValue==tag) return; p.InsertArrayElementAtIndex(p.arraySize); p.GetArrayElementAtIndex(p.arraySize-1).stringValue=tag; so.ApplyModifiedProperties(); }
        private static bool Confirm(string name) => EditorUtility.DisplayDialog($"{name} Scene Setup",$"현재 씬에 {name} 기본 오브젝트를 생성합니다.\n계속하시겠습니까?","생성","취소");
    }
}
#endif
