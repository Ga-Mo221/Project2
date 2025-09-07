using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class SelectionBox : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Canvas canvas;                // Canvas chứa boxVisual
    [SerializeField] private RectTransform boxVisual;      // UI Image (RectTransform) làm box
    [SerializeField] private string selectableTag = "Selectable"; // tag các object có thể chọn

    private Vector2 startScreenPos;
    private Vector2 endScreenPos;
    public List<GameObject> chosen;

    void Awake()
    {
        if (canvas == null) canvas = GetComponentInParent<Canvas>();
        if (boxVisual) boxVisual.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startScreenPos = Input.mousePosition;
            endScreenPos = startScreenPos;
            if (boxVisual) boxVisual.gameObject.SetActive(true);
            UpdateVisual(startScreenPos, startScreenPos);
        }

        if (Input.GetMouseButton(0))
        {
            endScreenPos = Input.mousePosition;
            UpdateVisual(startScreenPos, endScreenPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (boxVisual) boxVisual.gameObject.SetActive(false);
            endScreenPos = Input.mousePosition;
            SelectObjects(startScreenPos, endScreenPos);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0; // nếu bạn muốn ở mặt phẳng 2D (z=0)
            if (chosen != null)
            {
                foreach (var obj in chosen)
                {
                    obj.GetComponent<PlayerAI>().ChangeTarget(worldPos);
                }
            }
        }
    }

    // Vẽ hộp trên Canvas: chuyển screen -> local của Canvas rồi set anchoredPosition/sizeDelta
    private void UpdateVisual(Vector2 screenStart, Vector2 screenEnd)
    {
        if (canvas == null || boxVisual == null) return;

        RectTransform canvasRect = canvas.transform as RectTransform;
        Camera cam = (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : canvas.worldCamera;

        Vector2 localStart, localEnd;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenStart, cam, out localStart);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenEnd, cam, out localEnd);

        Vector2 size = localEnd - localStart;
        Vector2 center = (localStart + localEnd) * 0.5f;

        // đảm bảo anchors/pivot phù hợp (đặt trung tâm để anchoredPosition = center)
        boxVisual.pivot = new Vector2(0.5f, 0.5f);
        boxVisual.anchorMin = boxVisual.anchorMax = new Vector2(0.5f, 0.5f);

        boxVisual.anchoredPosition = center;
        boxVisual.sizeDelta = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));
    }

    private void cleanListChosen()
    {
        foreach (var obj in chosen)
        {
            obj.GetComponent<PlayerAI>().isSetSelcted(false);
        }
        chosen = new List<GameObject>();
    }

    // Lấy các object có tag và kiểm tra bằng WorldToScreenPoint
    private void SelectObjects(Vector2 screenStart, Vector2 screenEnd)
    {
        Rect screenRect = GetScreenRect(screenStart, screenEnd);
        Camera cam = Camera.main;
        if (cam == null) { Debug.LogWarning("No Main Camera found for selection."); return; }

        GameObject[] selectable = GameObject.FindGameObjectsWithTag(selectableTag);
        cleanListChosen();

        foreach (var go in selectable)
        {
            Vector3 sp = cam.WorldToScreenPoint(go.transform.position);
            if (sp.z < 0) continue; // phía sau camera -> bỏ
            if (screenRect.Contains(new Vector2(sp.x, sp.y)))
            {
                chosen.Add(go);
                go.GetComponent<PlayerAI>().isSetSelcted(true);
            }
            else
            {
                go.GetComponent<PlayerAI>().isSetSelcted(false);
            }
        }

        Debug.Log($"Selected {chosen.Count} objects");
    }

    private Rect GetScreenRect(Vector2 a, Vector2 b)
    {
        Vector2 min = Vector2.Min(a, b);
        Vector2 max = Vector2.Max(a, b);
        return new Rect(min, max - min);
    }
}
