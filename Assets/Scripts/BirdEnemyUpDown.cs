using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class BirdEnemyUpDown : BirdEnemy
    {       

        protected override void Move()
        {
            Vector2 offset = new Vector2(0, Mathf.Sin(currentAngle) * radiusY);
            transform.position = initialPosition + offset;
        }
    }
}
