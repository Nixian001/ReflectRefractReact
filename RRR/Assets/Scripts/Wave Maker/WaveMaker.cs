using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Nixian.Waves 
{
    public class WaveMaker : MonoBehaviour
    {
        public Wave[] waves;
        public float marchDistance = 0.1f;
        public float amplitude;
        public int sineRes = 10007;
        public Vector2 startPos;
        public Vector2 direction = new Vector2(1, 0);
        public LineRenderer lineRenderer;
        public LineRenderer sineRenderer;

        public bool isThroughGlass = false;

        private float airRI = 1.000277f;
        private float glassRI = 1.5234f;

        private LightColor[] colors = {
            new() { name = "violet", GHz = 0.750f, nm = 400, hue=new Color() { r=131,g=0  ,b=181} },
            new() { name = "indigo", GHz = 0.675f, nm = 445, hue=new Color() { r=0  ,g=40 ,b=255} },
            new() { name = "blue"  , GHz = 0.630f, nm = 475, hue=new Color() { r=0  ,g=192,b=255} },
            new() { name = "green" , GHz = 0.590f, nm = 510, hue=new Color() { r=0  ,g=255,b=0  } },
            new() { name = "yellow", GHz = 0.525f, nm = 570, hue=new Color() { r=225,g=255,b=0  } },
            new() { name = "orange", GHz = 0.510f, nm = 590, hue=new Color() { r=255,g=223,b=0  } },
            new() { name = "red"   , GHz = 0.460f, nm = 650, hue=new Color() { r=255,g=0  ,b=0  } },
        };

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            startPos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 tracePosition = startPos;
            Vector2 traceDir = direction;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPos);

            int i = 0;

            Color c = new();

            if (waves.Length > 1)
            {
                for (int j = 0; j < waves.Length; j++)
                {
                    if (j == 0)
                    {
                        c = colors[(int)waves[0].color].hue;
                    }
                    else
                    {
                        c = (c + colors[(int)waves[j].color].hue) / 2;
                    }
                }
            }
            else
            {
                c = colors[(int)waves[0].color].hue;
            }

            c /= 255;
            c.a = 1f;

            sineRenderer.startColor = c;
            sineRenderer.endColor = c;

            while (IsPointInScreen(tracePosition))
            {
                i++;
                if (i > 10000)
                {
                    break;
                }


                RaycastHit2D hit = Physics2D.Raycast(tracePosition, direction, marchDistance);

                if (isThroughGlass && !hit.collider)
                {
                    RaycastHit2D hit2 = Physics2D.Raycast(tracePosition, direction, -marchDistance);
                    isThroughGlass = false;
                    traceDir = LightRefraction.CalculateRefraction(traceDir, -hit2.normal, glassRI, airRI);
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, tracePosition);
                    lineRenderer.positionCount++;
                }

                if (hit.collider != null)
                {
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, tracePosition);

                    if (hit.collider.CompareTag("Solid"))
                    {
                        break;
                    }
                    else if (hit.collider.CompareTag("Reflective"))
                    {
                        GameObject mirror = hit.collider.gameObject;
                        Vector2 normal = hit.normal;
                        Debug.DrawLine(hit.point, -normal + hit.point, Color.magenta);

                        traceDir = traceDir - 2 * Vector2.Dot(traceDir, normal) * normal;

                        Debug.DrawLine(hit.point, traceDir + hit.point, Color.cyan);

                        lineRenderer.positionCount++;
                    }
                    else if (hit.collider.CompareTag("OneWayMirror"))
                    {
                        Vector2 mirrorDir = hit.collider.transform.right;

                        if (Vector2.Dot(traceDir, mirrorDir) < 0)
                        {
                            GameObject mirror = hit.collider.gameObject;
                            Vector2 normal = hit.normal;
                            Debug.DrawLine(hit.point, -normal + hit.point, Color.magenta);

                            traceDir = traceDir - 2 * Vector2.Dot(traceDir, normal) * normal;

                            Debug.DrawLine(hit.point, traceDir + hit.point, Color.cyan);

                            lineRenderer.positionCount++;
                        }
                        else
                        {
                            lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                            lineRenderer.positionCount++;

                            tracePosition = hit.point + traceDir * 0.001f;
                            Debug.DrawLine(hit.point, tracePosition);
                        }
                    }
                    else if (hit.collider.CompareTag("Reciver"))
                    {
                        hit.collider.GetComponent<WaveReciver>().given.AddRange(waves);
                        break;
                    }
                    else if (hit.collider.CompareTag("Refractive") && !isThroughGlass)
                    {
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, tracePosition);
                        lineRenderer.positionCount++;

                        isThroughGlass = true;
                        traceDir = LightRefraction.CalculateRefraction(traceDir, hit.normal, airRI, glassRI);
                    }
                }
                tracePosition += traceDir * marchDistance;
            }

            lineRenderer.SetPosition(lineRenderer.positionCount - 1, tracePosition);

            DrawCompundWave();
        }

        private void DrawCompundWave()
        {
            float totalLength = 0f;
            for (int i = 0; i < lineRenderer.positionCount - 1; i++)
            {
                totalLength += Vector2.Distance(lineRenderer.GetPosition(i), lineRenderer.GetPosition(i + 1));
            }

            sineRenderer.positionCount = sineRes;
            float cumulativeLength = 0f;
            int pointIndex = 0;

            for (int l = 0; l < lineRenderer.positionCount - 1; l++)
            {
                Vector2 start = lineRenderer.GetPosition(l);
                Vector2 finish = lineRenderer.GetPosition(l + 1);
                float segmentLength = Vector2.Distance(start, finish);

                if (segmentLength == 0)
                    continue;

                Vector2 direction = (finish - start).normalized;
                Vector2 normal = new Vector2(-direction.y, direction.x);

                int segmentPoints = Mathf.FloorToInt((segmentLength / totalLength) * sineRes);
                if (segmentPoints == 0)
                    continue;

                for (int p = 0; p < segmentPoints; p++)
                {
                    float prog = (float)p / (segmentPoints - 1);
                    Vector2 pointOnLine = Vector2.Lerp(start, finish, prog);

                    float segmentProgressLength = prog * segmentLength;
                    float waveInput = cumulativeLength + segmentProgressLength;

                    float waveOffsetValue = GetCompundWave(waveInput);

                    Vector2 waveOffset = normal * waveOffsetValue;

                    Vector2 finalPoint = pointOnLine + waveOffset;

                    sineRenderer.SetPosition(pointIndex, new Vector3(finalPoint.x, finalPoint.y, 4));
                    pointIndex++;
                }

                cumulativeLength += segmentLength;
            }
            sineRenderer.positionCount = pointIndex;
        }

        private float GetCompundWave(float x)
        {
            float wavey = 0;
            foreach (Wave w in waves)
            {
                wavey += Mathf.Sin(colors[(int)w.color].GHz * 2 * Mathf.PI * (x - Time.timeSinceLevelLoad * 2));
            }
            return wavey * amplitude;
        }

        private bool IsPointInScreen(Vector2 point) 
        {
            return (point.x > -20 && point.x < 20 && point.y > -15 && point.y < 15);
        }
    }

    [Serializable]
    public struct Wave
    {
        public colors color;

        public static bool operator ==(Wave w1, Wave w2)
        {
            return w1.color == w2.color;
        }

        public static bool operator !=(Wave w1, Wave w2)
        {
            return !(w1 == w2);
        }
    };

    public enum colors
    {
        violet, indigo, blue, green, yellow, orange, red
    }

    [Serializable]
    public struct LightColor
    {
        public string name;
        public float GHz;
        public int nm;
        public Color hue;
    }
}