using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour
{
    private float smoothing = 3f;

    private RectTransform rectTransform;
    private Button button;

    private Vector3 originalWidth;

    private Vector3 oldWidth;
    private Vector3 newWidth;
    private Vector3 currentWidth;

    private bool startLerp;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        
        originalWidth = new Vector3(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (startLerp)
        {
            startLerp = false;
            StartCoroutine(transition());
        }
    }

    IEnumerator transition()
    {
        while(Mathf.Abs(rectTransform.sizeDelta.x - newWidth.x) > 1)
        {
            rectTransform.sizeDelta = Vector3.Lerp(rectTransform.sizeDelta, newWidth, smoothing * Time.fixedDeltaTime);
        
            yield return null;
        }

        rectTransform.sizeDelta = newWidth;
    }

    public void onHover()
    {
        oldWidth = originalWidth;
        newWidth = new Vector3(500f, oldWidth.y,0);

        startLerp = true;
    }

    public void onLeave()
    {
        oldWidth = new Vector3(500f, oldWidth.y, 0);
        newWidth = originalWidth;

        startLerp = true;
    }
}
