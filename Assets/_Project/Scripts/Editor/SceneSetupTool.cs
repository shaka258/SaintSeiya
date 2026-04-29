#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class SceneSetupTool
{
    [MenuItem("SaintSeiya/Setup/① Configure Tags")]
    public static void SetupTags()
    {
        foreach (var tag in new[] { "Player", "Enemy", "NPC", "Item" }) AddTag(tag);
        Debug.Log("✅ 태그 설정 완료!");
    }

    [MenuItem("SaintSeiya/Setup/② Configure Build Settings")]
    public static void SetupBuildSettings()
    {
        var paths = new[]{ "Assets/Scenes/Boot.unity","Assets/Scenes/MainMenu.unity","Assets/Scenes/WorldMap.unity","Assets/Scenes/Field_Sanctuary.unity","Assets/Scenes/Battle.unity" };
        var scenes = new EditorBuildSettingsScene[paths.Length];
        for (int i = 0; i < paths.Length; i++) scenes[i] = new EditorBuildSettingsScene(paths[i], true);
        EditorBuildSettings.scenes = scenes;
        Debug.Log("✅ Build Settings 완료!");
    }

    [MenuItem("SaintSeiya/Setup/③ Boot Scene — Create Managers")]
    public static void SetupBootScene()
    {
        if (!Confirm("Boot")) return;
        CreateEmpty("GameManager"); CreateEmpty("AudioManager");
        CreateEmpty("InventoryManager"); CreateEmpty("DialogueManager"); CreateEmpty("BootLoader");
        var canvas = CreateCanvas("UI_Canvas");
        var lt = new GameObject("LoadingText"); lt.transform.SetParent(canvas.transform,false);
        var tmp = lt.AddComponent<TextMeshProUGUI>(); tmp.text="Loading..."; tmp.alignment=TextAlignmentOptions.Center;
        CreateEventSystem();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Debug.Log("✅ Boot 씬 완료!");
    }

    [MenuItem("SaintSeiya/Setup/④ MainMenu Scene — Create UI")]
    public static void SetupMainMenuScene()
    {
        if (!Confirm("MainMenu")) return;
        var canvas = CreateCanvas("MainMenu_Canvas");
        var title = MakeTMPText(canvas.transform,"TitleText","SAINT SEIYA");
        title.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,150);
        title.GetComponent<TextMeshProUGUI>().fontSize=60;
        MakeButton(canvas.transform,"NewGameBtn","새 게임",new Vector2(0,50));
        MakeButton(canvas.transform,"ContinueBtn","계속하기",new Vector2(0,-20));
        MakeButton(canvas.transform,"SettingsBtn","설정",new Vector2(0,-90));
        MakeButton(canvas.transform,"QuitBtn","종료",new Vector2(0,-160));
        var sp=new GameObject("SettingsPanel"); sp.transform.SetParent(canvas.transform,false); sp.AddComponent<RectTransform>(); sp.SetActive(false);
        CreateEventSystem();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Debug.Log("✅ MainMenu 씬 완료!");
    }

    [MenuItem("SaintSeiya/Setup/⑤ Field Scene — Create Player & UI")]
    public static void SetupFieldScene()
    {
        if (!Confirm("Field")) return;
        CreateEmpty("QuestManager");
        var player=new GameObject("Player"); player.tag="Player";
        player.AddComponent<SpriteRenderer>();
        var rb=player.AddComponent<Rigidbody2D>(); rb.gravityScale=0f; rb.constraints=RigidbodyConstraints2D.FreezeRotation;
        var col=player.AddComponent<CapsuleCollider2D>(); col.size=new Vector2(0.6f,0.9f);
        player.AddComponent<Animator>(); player.AddComponent<UnityEngine.InputSystem.PlayerInput>();
        Undo.RegisterCreatedObjectUndo(player,"Create Player");
        var enemy=new GameObject("Enemy_01"); enemy.tag="Enemy";
        enemy.AddComponent<SpriteRenderer>(); var erb=enemy.AddComponent<Rigidbody2D>(); erb.gravityScale=0f;
        enemy.AddComponent<CapsuleCollider2D>(); enemy.transform.position=new Vector3(3f,0f,0f);
        Undo.RegisterCreatedObjectUndo(enemy,"Create Enemy");
        CreateCanvas("HUD_Canvas"); CreateCanvas("Inventory_Canvas"); CreateCanvas("Dialogue_Canvas");
        var grid=new GameObject("Grid"); grid.AddComponent<Grid>();
        new GameObject("Ground").transform.SetParent(grid.transform);
        new GameObject("Walls").transform.SetParent(grid.transform);
        Undo.RegisterCreatedObjectUndo(grid,"Create Grid");
        CreateEventSystem();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Debug.Log("✅ Field 씬 완료!");
    }

    [MenuItem("SaintSeiya/Setup/⑥ Battle Scene — Create Battle Objects")]
    public static void SetupBattleScene()
    {
        if (!Confirm("Battle")) return;
        CreateEmpty("BattleManager"); CreateEmpty("DialogueManager");
        var player=new GameObject("PlayerBattle"); player.tag="Player";
        player.AddComponent<SpriteRenderer>(); player.transform.position=new Vector3(-3f,0f,0f);
        Undo.RegisterCreatedObjectUndo(player,"Create PlayerBattle");
        var enemy=new GameObject("EnemyBattle_01"); enemy.tag="Enemy";
        enemy.AddComponent<SpriteRenderer>(); enemy.transform.position=new Vector3(3f,0f,0f);
        Undo.RegisterCreatedObjectUndo(enemy,"Create EnemyBattle");
        CreateCanvas("BattleUI_Canvas");
        CreateEventSystem();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Debug.Log("✅ Battle 씬 완료!");
    }

    [MenuItem("SaintSeiya/Setup/⑦ Create Test Scene (빠른 테스트)")]
    public static void CreateTestScene()
    {
        if (!Confirm("TestScene")) return;

        var player=new GameObject("Player"); player.tag="Player";
        player.AddComponent<SpriteRenderer>();
        var rb=player.AddComponent<Rigidbody2D>(); rb.gravityScale=0f; rb.constraints=RigidbodyConstraints2D.FreezeRotation;
        var col=player.AddComponent<CapsuleCollider2D>(); col.size=new Vector2(0.6f,0.9f);
        player.AddComponent<Animator>(); player.AddComponent<UnityEngine.InputSystem.PlayerInput>();
        Undo.RegisterCreatedObjectUndo(player,"Create Player");

        var enemy=new GameObject("Enemy_01"); enemy.tag="Enemy";
        enemy.AddComponent<SpriteRenderer>();
        var erb=enemy.AddComponent<Rigidbody2D>(); erb.gravityScale=0f; erb.constraints=RigidbodyConstraints2D.FreezeRotation;
        enemy.AddComponent<CapsuleCollider2D>(); enemy.transform.position=new Vector3(3f,0f,0f);
        Undo.RegisterCreatedObjectUndo(enemy,"Create Enemy");

        CreateEmpty("BattleManager"); CreateEmpty("QuestManager");
        CreateCanvas("HUD_Canvas"); CreateCanvas("Dialogue_Canvas");
        CreateEmpty("TestSceneInitializer");
        CreateEventSystem();

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Debug.Log("✅ 테스트 씬 생성 완료!\n" +
                  "1. Player에 스크립트 부착: PlayerController, CharacterStats, CosmosSystem, LevelSystem\n" +
                  "2. Enemy에 스크립트 부착: EnemyController, CharacterStats\n" +
                  "3. TestSceneInitializer에 AutoSprite, TestSceneInitializer.cs 부착 후 Player/Enemy 드래그\n" +
                  "4. Play 버튼!");
    }

    static GameObject CreateEmpty(string name){var go=new GameObject(name);Undo.RegisterCreatedObjectUndo(go,$"Create {name}");return go;}
    static Canvas CreateCanvas(string name){var go=CreateEmpty(name);var c=go.AddComponent<Canvas>();c.renderMode=RenderMode.ScreenSpaceOverlay;var s=go.AddComponent<CanvasScaler>();s.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;s.referenceResolution=new Vector2(1920,1080);go.AddComponent<GraphicRaycaster>();return c;}
    static GameObject MakeTMPText(Transform p,string name,string text){var go=new GameObject(name);go.transform.SetParent(p,false);var t=go.AddComponent<TextMeshProUGUI>();t.text=text;t.alignment=TextAlignmentOptions.Center;return go;}
    static GameObject MakeButton(Transform p,string name,string label,Vector2 pos){var go=new GameObject(name);go.transform.SetParent(p,false);var rt=go.AddComponent<RectTransform>();rt.sizeDelta=new Vector2(220,55);rt.anchoredPosition=pos;var img=go.AddComponent<Image>();img.color=new Color(0.15f,0.15f,0.3f,0.9f);go.AddComponent<Button>();var txt=MakeTMPText(go.transform,"Text",label);var trt=txt.GetComponent<RectTransform>();trt.anchorMin=Vector2.zero;trt.anchorMax=Vector2.one;trt.sizeDelta=Vector2.zero;txt.GetComponent<TextMeshProUGUI>().fontSize=24;return go;}
    static void CreateEventSystem(){if(Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>()!=null)return;var es=CreateEmpty("EventSystem");es.AddComponent<UnityEngine.EventSystems.EventSystem>();es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();}
    static void AddTag(string tag){var so=new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);var p=so.FindProperty("tags");for(int i=0;i<p.arraySize;i++)if(p.GetArrayElementAtIndex(i).stringValue==tag)return;p.InsertArrayElementAtIndex(p.arraySize);p.GetArrayElementAtIndex(p.arraySize-1).stringValue=tag;so.ApplyModifiedProperties();}
    static bool Confirm(string name)=>EditorUtility.DisplayDialog($"{name} Scene Setup",$"현재 씬에 {name} 기본 오브젝트를 생성합니다.\n계속하시겠습니까?","생성","취소");
}
#endif
