using UnityEngine;
using UnityEngine.EventSystems;

public class PartsChange : MonoBehaviour
{
    private Camera mainCamera;
    private Plane dragPlane;
    private Vector3 offset;
    private bool isDragging = false;
    private Collider myCollider;
    private SpriteRenderer mySpriteRenderer;

    [Header("大きさ変更の設定")]
    [Tooltip("最小サイズ倍率")]
    [SerializeField] private float minScale = 0.05f;

    [Tooltip("最大サイズ倍率")]
    [SerializeField] private float maxScale = 1.5f;

    [Tooltip("ホイール感度")]
    [SerializeField] private float scrollSensitivity = 0.08f;

    private void Start()
    {
        mainCamera = Camera.main;
        myCollider = GetComponent<Collider>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null) return;
        }

        Vector2 mousePos = GetMousePosition();

        // 1. マウス右クリックで「そのパーツだけ削除」
        if (GetMouseButtonDown1())
        {
            Ray ray = mainCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider == myCollider || hit.transform == transform)
                {
                    Debug.Log("<color=red>パーツを削除しました: " + gameObject.name + "</color>");
                    Destroy(gameObject);
                    return;
                }
            }
        }

        // 2. マウス左クリックが押された瞬間の判定（パーツを掴む）
        if (GetMouseButtonDown0())
        {
            Ray ray = mainCamera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider == myCollider || hit.transform == transform)
                {
                    isDragging = true;

                    // 掴んだパーツを手前（最前面）に表示する
                    if (mySpriteRenderer != null)
                    {
                        PatternSettings.topSortingOrder++;
                        mySpriteRenderer.sortingOrder = PatternSettings.topSortingOrder;
                    }

                    // カメラに向かう法線（-forward）で平面を作成
                    dragPlane = new Plane(-mainCamera.transform.forward, transform.position);

                    if (dragPlane.Raycast(ray, out float enter))
                    {
                        Vector3 hitPoint = ray.GetPoint(enter);
                        offset = transform.position - hitPoint;
                    }

                    Debug.Log("<color=cyan>★パーツをつかみました: " + gameObject.name + "</color>");
                }
            }
        }

        // 3. ドラッグ中の移動処理
        if (isDragging && GetMouseButton0())
        {
            Ray ray = mainCamera.ScreenPointToRay(mousePos);

            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                transform.position = hitPoint + offset;
            }
        }

        // 4. マウスを離した時
        if (isDragging && GetMouseButtonUp0())
        {
            isDragging = false;
            Debug.Log("<color=yellow>★パーツを離しました: " + gameObject.name + "</color>");
        }

        // 5. ドラッグ中にマウスホイールで拡大・縮小
        if (isDragging)
        {
            float scroll = GetScrollDelta();
            if (Mathf.Abs(scroll) > 0.0001f)
            {
                Vector3 newScale = transform.localScale + Vector3.one * (scroll * scrollSensitivity);
                float clamped = Mathf.Clamp(newScale.x, minScale, maxScale);
                transform.localScale = new Vector3(clamped, clamped, clamped);
                Debug.Log($"サイズ変更: {clamped:F2}");
            }
        }
    }

    // 新旧Input System両対応の入力ヘルパー
    private Vector2 GetMousePosition()
    {
#if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Mouse.current != null)
            return UnityEngine.InputSystem.Mouse.current.position.ReadValue();
#endif
        return Input.mousePosition;
    }

    private bool GetMouseButtonDown0()
    {
#if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Mouse.current != null)
            return UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame;
#endif
        return Input.GetMouseButtonDown(0);
    }

    private bool GetMouseButton0()
    {
#if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Mouse.current != null)
            return UnityEngine.InputSystem.Mouse.current.leftButton.isPressed;
#endif
        return Input.GetMouseButton(0);
    }

    private bool GetMouseButtonUp0()
    {
#if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Mouse.current != null)
            return UnityEngine.InputSystem.Mouse.current.leftButton.wasReleasedThisFrame;
#endif
        return Input.GetMouseButtonUp(0);
    }

    private bool GetMouseButtonDown1()
    {
#if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Mouse.current != null)
            return UnityEngine.InputSystem.Mouse.current.rightButton.wasPressedThisFrame;
#endif
        return Input.GetMouseButtonDown(1);
    }

    private float GetScrollDelta()
    {
#if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Mouse.current != null)
            return UnityEngine.InputSystem.Mouse.current.scroll.ReadValue().y * 0.01f;
#endif
        return Input.GetAxis("Mouse ScrollWheel");
    }
}
