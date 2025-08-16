using UnityEditor;
using UnityEngine;

public class ClickAndDrag : MonoBehaviour
{
    public GameObject selectedObject;
    Vector3 offset;

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

            if (targetObject)
            {
                selectedObject = targetObject.transform.gameObject;
                offset = selectedObject.transform.position - mousePosition;
            }
        }

        if (Input.mouseScrollDelta != Vector2.zero)
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

            if (targetObject)
            {
                if (Input.mouseScrollDelta.y > 0)
                    targetObject.transform.Rotate(Vector3.forward, 10f);
                else
                    targetObject.transform.Rotate(Vector3.forward, -10f);
            }
        }

        if (selectedObject)
        {
            selectedObject.transform.position = mousePosition + offset;
        }

        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject = null;
        }
    }
}
