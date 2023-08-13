using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    class BirdEnemyRedFree : BirdEnemyFree
    {
        public override string Type { get; protected set; } = "NonHittable";

        protected override void UpdateKillCount()
        {
            gameControl.DisplayPoint(-5);

            if (gameControl.FreePlayKills >= 5)
                gameControl.FreePlayKills -= 5;
            else
                gameControl.FreePlayKills = 0;

            gameControl.CalculateScore();
        }
    }
}
