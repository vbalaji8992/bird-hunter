using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class BirdEnemyLeftRight : BirdEnemy
    {     

        protected override void Move()
        {
            float sineOfAngle = Mathf.Sin(currentAngle);
            Vector2 offset = new Vector2(sineOfAngle * radiusX, 0);
            transform.position = initialPosition + offset;

            CheckDirectionAndFlipSprite();

        }

        private void CheckDirectionAndFlipSprite()
        {
            float radians = currentAngle * Mathf.Rad2Deg;

            if (radians >= 90f && radians <= 270f)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
    }
}
