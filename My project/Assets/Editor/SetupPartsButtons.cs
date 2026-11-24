using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.Events;

public class SetupPartsButtons : Editor
{
    [MenuItem("Tools/パーツボタンの自動設定 (1〜15)")]
    public static void Setup()
    {
        // 1. シーン内の PatternSettings を検索
        PatternSettings patternSettings = Object.FindAnyObjectByType<PatternSettings>();
        if (patternSettings == null)
        {
            Debug.LogError("[SetupPartsButtons] シーン内に PatternSettings が見つかりません！");
            return;
        }

        // 2. color3 を検索
        GameObject color3Obj = GameObject.Find("color3");
        if (color3Obj == null)
        {
            Debug.LogError("[SetupPartsButtons] 'color3' オブジェクトが見つかりません！");
            return;
        }

        // Content を検索
        Transform contentTr = color3Obj.transform.Find("Pattern/parts/Scroll View/Viewport/Content");
        if (contentTr == null)
        {
            Button[] allButtons = color3Obj.GetComponentsInChildren<Button>(true);
            foreach (var b in allButtons)
            {
                if (b.transform.parent != null && b.transform.parent.name == "Content")
                {
                    contentTr = b.transform.parent;
                    break;
                }
            }
        }

        if (contentTr == null)
        {
            Debug.LogError("[SetupPartsButtons] 'Content' が見つかりません！");
            return;
        }

        // 3. colordata3 の Parts1.png 〜 Parts15.png を読み込み
        Sprite[] partSprites = new Sprite[15];
        for (int i = 1; i <= 15; i++)
        {
            string path = $"Assets/colordata3/Parts{i}.png";
            partSprites[i - 1] = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (partSprites[i - 1] == null)
            {
                Debug.LogWarning($"[SetupPartsButtons] {path} が読み込めませんでした。");
            }
        }

        // 4. Content の各子ボタンに設定
        int childCount = contentTr.childCount;
        int setupCount = 0;

        for (int i = 0; i < childCount; i++)
        {
            Transform btnTr = contentTr.GetChild(i);
            Button btn = btnTr.GetComponent<Button>();
            Image img = btnTr.GetComponent<Image>();

            if (i < 15 && partSprites[i] != null)
            {
                btnTr.gameObject.SetActive(true);

                // ボタンのアイコン画像を設定
                if (img != null)
                {
                    Undo.RecordObject(img, "Set Button Image");
                    img.sprite = partSprites[i];
                    img.color = Color.white;
                }

                // OnClick に SetPart(sprite) を登録
                if (btn != null)
                {
                    Undo.RecordObject(btn, "Set Button OnClick");
                    while (btn.onClick.GetPersistentEventCount() > 0)
                    {
                        UnityEventTools.RemovePersistentListener(btn.onClick, 0);
                    }

                    Sprite spriteArg = partSprites[i];
                    UnityEngine.Events.UnityAction<Sprite> action = new UnityEngine.Events.UnityAction<Sprite>(patternSettings.SetPart);
                    UnityEventTools.AddObjectPersistentListener(btn.onClick, action, spriteArg);
                    setupCount++;
                }
            }
            else
            {
                // 16個目以降の余剰ボタンは非表示にする
                btnTr.gameObject.SetActive(false);
            }
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(patternSettings.gameObject.scene);
        Debug.Log($"<color=green>★【設定完了】{setupCount}個のパーツボタンに Parts1〜Parts15 を設定しました！シーンを保存してください。</color>");
    }
}
