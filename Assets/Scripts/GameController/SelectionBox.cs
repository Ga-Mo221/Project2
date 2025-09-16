using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.Experimental.GraphView;

public class SelectionBox : MonoBehaviour
{
    public static SelectionBox Instance { get; private set; }


    [Header("Setup")]
    [SerializeField] private Canvas canvas;                // Canvas chứa boxVisual
    [SerializeField] private RectTransform boxVisual;      // UI Image (RectTransform) làm box
    [SerializeField] private GameObject _moveTo;
    private bool _singleSelected = false;

    private Vector2 startScreenPos;
    private Vector2 endScreenPos;
    public List<GameObject> chosen;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (canvas == null) canvas = GetComponentInParent<Canvas>();
        if (boxVisual) boxVisual.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !CursorManager.Instance.ChoseUI)
        {
            _singleSelected = false;
            startScreenPos = Input.mousePosition;
            endScreenPos = startScreenPos;
            if (boxVisual) boxVisual.gameObject.SetActive(true);
            UpdateVisual(startScreenPos, startScreenPos);
            if (CursorManager.Instance.Select)
            {
                SelectObject();
            }
        }

        if (Input.GetMouseButton(0) && !CursorManager.Instance.ChoseUI)
        {
            endScreenPos = Input.mousePosition;
            UpdateVisual(startScreenPos, endScreenPos);
        }

        if (Input.GetMouseButtonUp(0) && !CursorManager.Instance.ChoseUI)
        {
            if (boxVisual) boxVisual.gameObject.SetActive(false);
            endScreenPos = Input.mousePosition;
            if (_singleSelected) return;
            SelectObjects(startScreenPos, endScreenPos);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0; // nếu bạn muốn ở mặt phẳng 2D (z=0)
            if (chosen.Count != 0)
            {
                GameObject _co = Instantiate(_moveTo, worldPos, Quaternion.identity);
                _co.GetComponent<MoveTo>().SetChosen(chosen);
                foreach (var obj in chosen)
                {
                    obj.GetComponent<PlayerAI>().setTarget(worldPos, true);
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
            obj.GetComponent<PlayerAI>().isSetSelected(false);
        }
        chosen = new List<GameObject>();
    }

    // Lấy các object có tag và kiểm tra bằng WorldToScreenPoint
    private void SelectObjects(Vector2 screenStart, Vector2 screenEnd)
    {
        if (CursorManager.Instance.ChoseUI) return;
        Rect screenRect = GetScreenRect(screenStart, screenEnd);
        Camera cam = Camera.main;
        if (cam == null) { Debug.LogWarning("No Main Camera found for selection."); return; }

        GameObject[] selectable = getUnitClass();
        cleanListChosen();

        foreach (var go in selectable)
        {
            Vector3 sp = cam.WorldToScreenPoint(go.transform.position);
            if (sp.z < 0) continue; // phía sau camera -> bỏ
            if (screenRect.Contains(new Vector2(sp.x, sp.y)))
            {
                chosen.Add(go);
                go.GetComponent<PlayerAI>().isSetSelected(true);
            }
            else
            {
                go.GetComponent<PlayerAI>().isSetSelected(false);
            }
        }

        //Debug.Log($"[{transform.name}] [SelectionBox] Selected {chosen.Count} objects");
    }

    private GameObject[] getUnitClass()
    {
        List<GameObject> selectable = new List<GameObject>();

        if (Castle.Instance._Q)
            selectable.AddRange(GameObject.FindGameObjectsWithTag("Warrior"));
        if (Castle.Instance._W)
            selectable.AddRange(GameObject.FindGameObjectsWithTag("Archer"));
        if (Castle.Instance._E)
            selectable.AddRange(GameObject.FindGameObjectsWithTag("Lancer"));
        if (Castle.Instance._A)
            selectable.AddRange(GameObject.FindGameObjectsWithTag("Healer"));
        if (Castle.Instance._S)
            selectable.AddRange(GameObject.FindGameObjectsWithTag("TNT"));

        // Nếu không bấm phím nào thì lấy tất cả
        if (selectable.Count == 0)
        {
            selectable.AddRange(GameObject.FindGameObjectsWithTag("Warrior"));
            selectable.AddRange(GameObject.FindGameObjectsWithTag("Archer"));
            selectable.AddRange(GameObject.FindGameObjectsWithTag("Lancer"));
            selectable.AddRange(GameObject.FindGameObjectsWithTag("Healer"));
            selectable.AddRange(GameObject.FindGameObjectsWithTag("TNT"));
        }

        return selectable.ToArray();
    }

    private void SelectObject()
    {
        GameObject _obj = CursorManager.Instance._hoverGameobject;
        if (_obj != null)
        {
            if (_obj.tag == "Warrior"
            || _obj.tag == "Archer"
            || _obj.tag == "Lancer"
            || _obj.tag == "Healer"
            || _obj.tag == "TNT")
            {
                _singleSelected = true;
                chosen.Add(_obj);
                _obj.GetComponent<PlayerAI>().isSetSelected(true);
            }
        }
    }

    private Rect GetScreenRect(Vector2 a, Vector2 b)
    {
        Vector2 min = Vector2.Min(a, b);
        Vector2 max = Vector2.Max(a, b);
        return new Rect(min, max - min);
    }
}
