using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public AICommand command;
    public TMP_Text commandButtonText;
    private Vector3 originalPosition;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip failSound;
    [SerializeField] private AudioClip successSound;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.position;
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponentInParent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        audioSource.PlayOneShot(successSound);

        originalPosition = rectTransform.position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out Vector2 localPoint);
        rectTransform.position = canvas.transform.TransformPoint(localPoint);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out localPoint))
        {
            rectTransform.position = canvas.transform.TransformPoint(localPoint);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = eventData.position
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        bool success = false;

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Node"))
            {
                Debug.Log("hit! " + result.gameObject.name);
                var circuitNode = result.gameObject.GetComponent<CircuitNode>();
                circuitNode.ChangeCommand(command);
                success = true;
                break;
            }
        }
        rectTransform.position = originalPosition;

        if (success)
        {
            audioSource.PlayOneShot(successSound);
        }
        else
        {
            audioSource.PlayOneShot(failSound);
        }

    }
}
