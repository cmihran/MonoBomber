﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoBomber.MacOS
{
    /// <summary>
    /// A player that is capable of placing bombs
    /// </summary>
    public class Player : Sprite
    {
        // how long it takes for bombs to recharge
        public const int BOMB_COOLDOWN_TIME = 50 + Explosion.LINGER_TIME;
        private const int SPEED = 8;

        // how long this player has left until it can bomb again
        private int bombCooldownLeft;

        // keys the player uses
        private readonly Keys up;
        private readonly Keys left;
        private readonly Keys down;
        private readonly Keys right;
        private readonly Keys bomb;

        public Player(Texture2D texture, Vector2 pos, Color color, 
                      Keys up, Keys left, Keys down, Keys right, Keys bomb, MonoBomberGame game) : base(texture, pos, color, game) {
            // bombs
            this.bombCooldownLeft = 0;

            // keys
            this.up = up;
            this.left = left;
            this.down = down;
            this.right = right;
            this.bomb = bomb;
        }

        public new void Draw() {
            // draw the player
            base.Draw();
        }

        public override void Update() {
            if(getTile().hasExplosion()) {
                //Console.WriteLine("u died " + pos);
            }

            // lower the bomb cooldown
            if(bombCooldownLeft > 0) {
                bombCooldownLeft--;
            }
                
            // movement
            KeyboardState state = Keyboard.GetState();

            // check up/down
            if (state.IsKeyDown(up)) {
                if (pos.Y > 0) {
                    pos.Y -= SPEED;
                }
            } else if (state.IsKeyDown(down)) {
       
                if (pos.Y + texture.Height < game.graphics.GraphicsDevice.Viewport.Height) {
                    pos.Y += SPEED;
                }
            }

            // check left/right
            if (state.IsKeyDown(left)) {
                if (pos.X > 0) {
                    pos.X -= SPEED;
                }
            } else if (state.IsKeyDown(right)) {
                if (pos.X + texture.Width < game.graphics.GraphicsDevice.Viewport.Width)
                {
                    pos.X += SPEED;
                }
            }

            // check bomb
            if (state.IsKeyDown(bomb)) {
                if(HasBomb()) {
                    PlaceBomb();
                }
            }
        }

        // true if this player can place a bomb
        public Boolean HasBomb() {
            return bombCooldownLeft == 0;
        }

        // places a bomb to the right of this player
        public void PlaceBomb() {
            if(bombCooldownLeft == 0) {
                Tile tile = getTile();
                if(!tile.isOccupied()) {
                    tile.PlaceSprite(new Bomb(this, tile.pos, color, game));
                    bombCooldownLeft = BOMB_COOLDOWN_TIME;
                }
            }
        }

    }
}
