using System.Runtime.InteropServices;
using FMODUnity;
using UnityEditor;
using UnityEngine;

public class ClickAndDrag : MonoBehaviour
{
    public GameObject selectedObject;
    Vector3 offset;

    public static ClickAndDrag Instance;
    public bool canDrag;

    private void Start()
    {
        Instance = this;
        canDrag = true;
    }

    void Update()
    {
        if (canDrag)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                Collider2D[] targetObjects = Physics2D.OverlapPointAll(mousePosition);

                foreach (Collider2D col in targetObjects)
                {
                    if (col.gameObject.layer != 3)
                    {
                        selectedObject = col.transform.gameObject;
                        offset = selectedObject.transform.position - mousePosition;

                        RuntimeManager.PlayOneShot("event:/SFX/Game/Glass Up");

                        break;
                    }
                }
            }

            if (Input.mouseScrollDelta != Vector2.zero)
            {
                Collider2D[] targetObjects = Physics2D.OverlapPointAll(mousePosition);

                foreach (Collider2D col in targetObjects)
                {
                    if (col.gameObject.layer != 3)
                    {
                        if (Input.mouseScrollDelta.y > 0)
                            col.transform.Rotate(Vector3.forward, 15f);
                        else
                            col.transform.Rotate(Vector3.forward, -15f);
                        RuntimeManager.PlayOneShot("event:/SFX/Game/Glass Up");
                        RuntimeManager.PlayOneShot("event:/SFX/Game/Glass Down");

                    }
                }
            }

            if (selectedObject)
            {
                selectedObject.transform.position = mousePosition + offset;
            }

            if (Input.GetMouseButtonUp(0) && selectedObject)
            {
                selectedObject = null;
                RuntimeManager.PlayOneShot("event:/SFX/Game/Glass Down");
            }
        }
        else
        {
            selectedObject = null;
        }
    }
}
