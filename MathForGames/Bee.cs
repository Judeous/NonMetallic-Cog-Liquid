using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathForGames
{
    class Bee : Actor
    {
        public Bee(float x, float y, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base((char)x, y, icon, color)
        { }

        public Bee(float x, float y, Color rayColor, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, icon, color)
        {
            _sprite = new Sprite("Images/player.png");
        }


    } //Bee
} //Math For Games
