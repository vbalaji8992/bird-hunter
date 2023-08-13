using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts
{
    class BirdEnemyFree : BirdEnemy
    {
        private Vector2 pos1;
        private Vector2 pos2;

        private Vector3 newPos;
        private float time = 0.0f;
        private float destroyTime = 0.0f;

        public virtual string Type { get; protected set; } = "Hittable";

        protected override void InitializePositions()
        {
            pos1 = GameObject.Find("Limit-1").transform.position;
            pos2 = GameObject.Find("Limit-2").transform.position;

            newPos = new Vector3(Random.Range(pos1.x, pos2.x), Random.Range(pos1.y, pos2.y));
        }

        protected override void Move()
        {
            var diff = (Vector2)(newPos - transform.position);

            if (diff.magnitude > 1.05)
            {
                rb2d.MovePosition((Vector2)transform.position + diff.normalized * Time.deltaTime * movementSpeed);
            }
            else if (time > Random.Range(2.0f, 5.0f))
            {
                newPos = new Vector3(Random.Range(pos1.x, pos2.x), Random.Range(pos1.y, pos2.y));
                time = 0.0f;
            }
            else
            {
                time += Time.deltaTime;
            }
        }

        protected override void HandleDeadBird()
        {
            destroyTime += Time.deltaTime;
            if (destroyTime > 3.0f)
            {
                Destroy(gameObject);
                Destroy(deathArrow);
            }            
        }

        protected override void UpdateKillCount()
        {
            gameControl.FreePlayKills += 1;
            gameControl.CalculateScore();
        }
    }
}
