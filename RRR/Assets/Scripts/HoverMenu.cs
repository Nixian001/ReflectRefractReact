using Nixian.Waves;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverMenu : MonoBehaviour
{
    public TMP_Text titlebar;
    public TMP_Text description;
    public Image bg;
    public bool isHovering;

    private void Update()
    {
        Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
        
        if (hit.collider)
        {
            if (hit.collider.GetComponent<WaveMaker>())
            {
                titlebar.text = "Wave Emitter";
                description.text = "Emitts Light. Light waves emitted are:\n";
                foreach (Wave w in hit.collider.GetComponent<WaveMaker>().waves)
                {
                    description.text += w.color.ToString() + ", ";
                }
            }
            else if (hit.collider.GetComponent<WaveReciver>())
            {
                titlebar.text = "Wave Reciver";
                description.text = "Recives Light. To Complete this reciver, give:\n";
                foreach (Wave w in hit.collider.GetComponent<WaveReciver>().expected)
                {
                    description.text += w.color.ToString() + ", ";
                }
            }
            else if (hit.collider.CompareTag("Reflective"))
            {
                titlebar.text = "Mirror";
                description.text = "Reflects Light.";
                if (hit.collider.gameObject.layer == 3)
                {
                    description.text += "\n (Fixed)";
                }
            }
            else if (hit.collider.CompareTag("Solid"))
            {
                titlebar.text = "Block";
                description.text = "Blocks Light.";
                if (hit.collider.gameObject.layer == 3)
                {
                    description.text += "\n (Fixed)";
                }
            }
            else if (hit.collider.CompareTag("OneWayMirror"))
            {
                titlebar.text = "One Way Mirror";
                description.text = "One side Reflects light and the other lets it through\n - Dark: Lets light through\n - Light: Reflects Light";

            }
            else if (hit.collider.CompareTag("Reflective"))
            {
                titlebar.text = "Glass Block";
                description.text = "Light Refracts through it. This is because light travels slower slightly slower when going through glass and bends.";
            }

            isHovering = true;
        }
        else
        {
            isHovering = false;
        }
    }

    private void LateUpdate()
    {
        transform.position = new Vector2(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width - 335 - 255), Mathf.Clamp(Input.mousePosition.y, 135, Screen.height));

        if (isHovering)
        {
            bg.CrossFadeAlpha(1f, 0.1f, true);
            titlebar.CrossFadeAlpha(1f, 0.1f, true);
            description.CrossFadeAlpha(1f, 0.1f, true);
        }
        else
        {
            bg.CrossFadeAlpha(0f, 0.1f, true);
            titlebar.CrossFadeAlpha(0f, 0.1f, true);
            description.CrossFadeAlpha(0f, 0.1f, true);
        }
    }
}
