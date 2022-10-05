using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class BirdEnemyCircle : BirdEnemy
    {
        public float radiusX;
        public float radiusY;

        private float currentAngle;

        protected void CalculateAngleInCircle()
        {
            currentAngle += movementSpeed * Time.deltaTime;

            if (currentAngle >= twoPiRadians)
            {
                currentAngle -= twoPiRadians;
            }
        }

        protected void SetPositionInCircle()
        {
            float sineOfAngle = Mathf.Sin(currentAngle);
            float cosineOfAngle = Mathf.Cos(currentAngle);
            Vector2 offset = new Vector2(cosineOfAngle * radiusX, sineOfAngle * radiusY);
            transform.position = initialPosition + offset;
        }

        protected override void Move()
        {
            if ((radiusX == 0 && radiusY == 0) || movementSpeed == 0)
                return;

            CalculateAngleInCircle();

            SetPositionInCircle();
        }
    }
}
