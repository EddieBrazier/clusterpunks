﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WarrenWarriorsGame
{
    /// <summary>
    /// Warren Warriors
    /// John, Liam, Eddie, Noah
    /// handles the movement of players
    /// 3/8/2019
    /// </summary>
    public class PlayerHandler
	{
		SpriteFont text;

		int selectedChar; //holds the position of the selected char
		SelectedState Swap = SelectedState.deselected; //gamestate used for if youre swapping characters

        //Eddie: Changed Unit array to PlayerChar array due to problems accessing abstract properties
		PlayerChar[] Units = new PlayerChar[3]; //holds the units that will be displayed on screen

        //the players inventory
		Inventory playerInv;
        //buttons that handle moveing players
        Button[] playerButtons = new Button[3];

        //accessor for player units
        public PlayerChar[] PlayerParty
        {
            get
            {
                return Units;
            }
        }


		public PlayerHandler(SpriteFont font,Game g)
		{
			//initializes the inventory of the player
			playerInv = new Inventory(g);

			//initializes the base units
			Units[0] = new PlayerChar(font, g, CharType.Heavy);
			Units[1] = new PlayerChar(font, g, CharType.Medium);
			Units[2] = new PlayerChar(font, g, CharType.Light);


            //initializes buttons for player controls
            playerButtons[0] = new Button(g.Content.Load<Texture2D>("btnNormal"), g.Content.Load<Texture2D>("btnHovered"), g.Content.Load<Texture2D>("btnClicked"), new Rectangle(70,240,50,50));
            playerButtons[1] = new Button(g.Content.Load<Texture2D>("btnNormal"), g.Content.Load<Texture2D>("btnHovered"), g.Content.Load<Texture2D>("btnClicked"), new Rectangle(200, 240, 50, 50));
            playerButtons[2] = new Button(g.Content.Load<Texture2D>("btnNormal"), g.Content.Load<Texture2D>("btnHovered"), g.Content.Load<Texture2D>("btnClicked"), new Rectangle(330, 240, 50, 50));
            //*Temporary*
            text = font;
		}

        //playerhandler's update now takes a gametime object (also takes an enemy object for testing and will likely be changed later)
        public void Update(KeyboardState kbState,KeyboardState PrevkbState,MouseState mState, MouseState prevMsState, GameTime gameTime, Enemy enemy)
		{

			switch (Swap)
			{
				case SelectedState.deselected: //if no characters are selected and the user selects a character update the code to match
                                               //cannot swap if either character is currently attacking or dead
                    if (Config.SingleKeyPress(Keys.NumPad1, kbState, PrevkbState) || Config.SingleKeyPress(Keys.D1, kbState, PrevkbState) && Units[0].IsAttacking == false && Units[0].Health > 0)
					{
						selectedChar = 0;
						Swap = SelectedState.selected;
					}
					if (Config.SingleKeyPress(Keys.NumPad2, kbState, PrevkbState) || Config.SingleKeyPress(Keys.D2, kbState, PrevkbState) && Units[1].IsAttacking == false && Units[1].Health > 0)
					{
						selectedChar = 1;
						Swap = SelectedState.selected;
					}
					if (Config.SingleKeyPress(Keys.NumPad3, kbState, PrevkbState) || Config.SingleKeyPress(Keys.D3, kbState, PrevkbState) && Units[2].IsAttacking == false && Units[2].Health > 0)
					{
						selectedChar = 2;
						Swap = SelectedState.selected;
					}

                    if(Swap == SelectedState.selected)
                    {
                        playerButtons[selectedChar].Select();
                    }

                    for (int i = 0; i < playerButtons.Length; i++) //handles button conrols
                    {
                        if(playerButtons[i].Update(mState) && Units[i].IsAttacking == false && Units[i].Health > 0)
                        {
                            selectedChar = i;
                            Swap = SelectedState.selected;

                        }

                    }


					break;
				case SelectedState.selected://if a character is selected and the user selects a character swap those two characters and their inventorys
                                            //cannot swap if either character is currently attacking

					if ((Config.SingleKeyPress(Keys.NumPad1, kbState, PrevkbState) || Config.SingleKeyPress(Keys.D1, kbState, PrevkbState)) && Units[0].IsAttacking == false && Units[0].Health > 0)
					{
						SwapUnits(selectedChar, 0);
						playerInv.CharSwap(selectedChar, 0);
						Swap = SelectedState.deselected;
					}
					if ((Config.SingleKeyPress(Keys.NumPad2, kbState, PrevkbState) || Config.SingleKeyPress(Keys.D2, kbState, PrevkbState)) && Units[1].IsAttacking == false && Units[1].Health > 0)
					{
						SwapUnits(selectedChar, 1);
						playerInv.CharSwap(selectedChar, 1);
						Swap = SelectedState.deselected;
					}
					if ((Config.SingleKeyPress(Keys.NumPad3, kbState, PrevkbState) || Config.SingleKeyPress(Keys.D3, kbState, PrevkbState)) && Units[2].IsAttacking == false && Units[2].Health > 0)
					{
						SwapUnits(selectedChar, 2);
						playerInv.CharSwap(selectedChar, 2);
						Swap = SelectedState.deselected;
					}

                    for (int i = 0; i < playerButtons.Length; i++) //handles button control
                    {
                        if (playerButtons[i].Update(mState) && Units[i].IsAttacking == false && Units[i].Health > 0)
                        {
                            SwapUnits(selectedChar,i);
                            playerInv.CharSwap(selectedChar, i);
                            playerButtons[i].Deselect();
                            playerButtons[selectedChar].Deselect();
                            Swap = SelectedState.deselected;

                        }

                    }


                    break;


			}

            //run code for inventory interaction
            //now takes in the Units array
			playerInv.Update(kbState, PrevkbState, mState, prevMsState, Units);

            //loop that runs player attacks when initiated
            for (int i = 0; i < Units.Length; i++)
            {
                if(Units[i].IsAttacking == true)
                {
                    Units[i].Atk.PlayerUpdate(gameTime, enemy, Units[i]);
                }
            }
        }

		public void Draw(SpriteBatch spriteBatch,UI GameUI)
		{
            //draws the  buttons
            for (int j = 0; j < playerButtons.Length; j++)
            {
                playerButtons[j].Draw(spriteBatch);
            }

            //draw the health icon
            for (int j = 0; j < 3; j++)
            {
                spriteBatch.Draw(GameUI.GameUI[0], new Rectangle(10 +130*j, 240, 50, 50), Color.White);
            }

            //draws the players numbers
            for (int j = 0; j < Units.Length; j++)
			{
				Color drawcolor = Color.Black;

				Units[j].Draw(spriteBatch, j);

				if (j == selectedChar && Swap == SelectedState.selected)
				{
					drawcolor = Color.MonoGameOrange;
				}

				spriteBatch.DrawString(text, string.Format("{0}:   ", j + 1), j * 5 * Config.LineSpacing, drawcolor);
                spriteBatch.DrawString(text, (j + 1).ToString(), new Vector2(95 + j * 130, 265), drawcolor);

			}



			playerInv.Draw(spriteBatch, text,GameUI);
		}



		private void SwapUnits(int pos1, int pos2)
		{
            playerButtons[pos1].Deselect();
            playerButtons[pos2].Deselect();

			PlayerChar temp = Units[pos1];
			Units[pos1] = Units[pos2];
			Units[pos2] = temp;

		}


	}
}
