﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/*Warren Warriors Group Project
 *Cluster Punks
 *Enemy class
 */

namespace WarrenWarriorsGame
{
    class Enemy: Unit
    {
        public Enemy(SpriteFont enemyFont, EnemyType type)
        {
            //set font based on the spritefont that's passed in
            font = enemyFont;

            //switch statement to determine enemy stats based on type
            switch (type)
            {
                case EnemyType.Buckshot:

                    //set non-randomized stats for buckshot, a set enemy type
                    Health = 100;
                    atk = new Attack(10, 10);
                    name = "Buckshot";

                    break;
                case EnemyType.SewCrow:

                    //set non-randomized stats for sewcrow, a set enemy type
                    Health = 60;
                    atk = new Attack(1, 3);
                    name = "Sew Crow";

                    break;
                case EnemyType.Bandit:

                    //set non-randomized stats for bandit, a set enemy type
                    Health = 75;
                    atk = new Attack(8, 6);
                    name = "Bandit";

                    break;
                case EnemyType.Custom:

                    //here's where code for enemies created using the external tool
                    //will go, reading in an enemy file and setting stats based on it 

                    break;
            }

            //set isAttacking bool to false by default
            isAttacking = false;
        }

        //required methods based on abstract parent, not currently in use
        public override void Draw(SpriteBatch sb, int position)
        {
            throw new NotImplementedException();
        }

        public override void Update(KeyboardState kbState, KeyboardState PrevkbState, GameTime time)
        {
            throw new NotImplementedException();
        }
    }
}
