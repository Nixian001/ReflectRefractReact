using UnityEngine;

namespace Nixian.Waves 
{
    public class WaveMaker : MonoBehaviour
    {
        public float hz;
        public float marchDistance = 0.1f;
        public Vector2 startPos;
        public Vector2 direction = new Vector2(1, 0);
        public LineRenderer lineRenderer;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            startPos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 tracePosition = startPos;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPos);
            
            while (IsPointInScreen(tracePosition))
            {
                RaycastHit2D hit = Physics2D.Raycast(tracePosition, direction, marchDistance);

                if (hit.collider != null)
                {
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, tracePosition);
                    lineRenderer.positionCount++;
                }
                tracePosition += direction * marchDistance;
            }

            lineRenderer.SetPosition(lineRenderer.positionCount - 1, tracePosition);
        }

        private bool IsPointInScreen(Vector2 point) 
        {
            return (point.x > -20 && point.x < 20 && point.y > -15 && point.y < 15);
        }
    }
}