using UnityEngine;

public class Highlight : MonoBehaviour
{
    public SpriteRenderer  objectSpriteRenderer;
    public SpriteRenderer highlightSpriteRenderer;

    private BoxCollider2D highlightCollider;

    private float timeToHighlight = 0.5f;
    private float timeHighlighted = 0.0f;
    private float maxAlpha = 0.5f;

    private bool isHighlighted = false;

    private void Start()
    {
        highlightSpriteRenderer.sprite = objectSpriteRenderer.sprite;
        highlightSpriteRenderer.sortingOrder = objectSpriteRenderer.sortingOrder + 1;
        // //hideFlags = HideFlags.HideInInspector;
        // // highlightSpriteRenderer.enabled = true;

        var alpha = highlightSpriteRenderer.color;
        alpha.a =   0.0f;
        highlightSpriteRenderer.color = alpha;

        transform.localScale = objectSpriteRenderer.transform.localScale * 1.1f;
        highlightCollider = gameObject.AddComponent<BoxCollider2D>();
        highlightCollider.size = objectSpriteRenderer.bounds.size;
        highlightCollider.isTrigger = true;
        highlightCollider.hideFlags = HideFlags.HideInInspector;
    }
    

    private void Update()
    {
        if(isHighlighted)
        {
            timeHighlighted += Time.deltaTime;
            if(timeHighlighted >= timeToHighlight)
            {
                var alpha = highlightSpriteRenderer.color;
                alpha.a = maxAlpha;
                highlightSpriteRenderer.color = alpha;
                timeHighlighted = timeToHighlight;
            } else
            {
                var alpha = highlightSpriteRenderer.color;
                alpha.a = maxAlpha * (timeHighlighted / timeToHighlight);
                highlightSpriteRenderer.color = alpha;
            }
        }
       else
       {
           timeHighlighted -= Time.deltaTime;
           if(timeHighlighted <= 0.0f)
           {
               var alpha = highlightSpriteRenderer.color;
               alpha.a = 0.0f;
               highlightSpriteRenderer.color = alpha;
               timeHighlighted = 0.0f;
           } 
           else
           {
                var alpha = highlightSpriteRenderer.color;
                alpha.a = maxAlpha * (timeHighlighted / timeToHighlight);
                highlightSpriteRenderer.color = alpha;
           }
       }
    }


    void OnMouseOver()
    {
        
    }

    void OnMouseEnter()
    {
        Debug.Log("Mouse enter");
        isHighlighted = true;
    }


    void OnMouseExit()
    {
        Debug.Log("Mouse exit");
        isHighlighted = false;
    }

}
