using UnityEngine;

namespace XiheFramework {
    public static class GizmosUtil {
        //implement rotation later
        public static void DrawCircle(Vector3 center, float radius, int segment) {
            Vector3[] points = new Vector3[segment];
            float unit = 360f / segment;

            for (int i = 0; i < segment; i++) {
                float angle = unit * i;
                var horizontal = GetPointOnCircle(angle) * radius;
                points[i] = new Vector3(center.x + horizontal.x, center.y, center.z + horizontal.y);

                if (i > 0) {
                    Gizmos.DrawLine(points[i - 1], points[i]);
                }

                //Gizmos.DrawSphere(points[i], 0.1f);
            }

            Gizmos.DrawLine(points[segment - 1], points[0]);
        }

        //radius = 1
        static Vector2 GetPointOnCircle(float angle) {
            var rad = Mathf.Deg2Rad * angle;
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);

            return new Vector2(x, y);
        }
    }
}