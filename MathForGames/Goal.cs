using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace MathForGames
{
    /// <summary>
    /// This is the goal the player must reach to end the game. 
    /// </summary>
    class Goal : Actor
    {
        private Actor _player;
        private float _rotate = 0;

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn</param>
        public Goal(float x, float y, Actor player, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base((char)x, y, icon, color)
        {
            _player = player;
        } //Goal Constructor

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Goal(float x, float y, Color rayColor, Actor player, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, icon, color)
        {
            _player = player;
        } //Overload Constructor

        /// <summary>
        /// Checks to see if the player is in range of the goal.
        /// </summary>
        /// <returns></returns>
        private bool CheckPlayerDistance()
        {
            float distance = (_player.GlobalPosition - GlobalPosition).Magnitude;
            return distance <= 1;
        } //Check PlayerDistance function

        public override void Update(float deltaTime)
        {
            _rotate += .07f;
            SetRotation(_rotate);

            UpdateLocalTransform();
            UpdateGlobalTransform();

            //Increase position by the current velocity
            LocalPosition += Velocity * deltaTime;
        } //Update

        public override void OnCollision(Actor actor)
        {
            base.OnCollision(actor);

            if (actor is Player)
                Game.SetGameOver(true);
        } //On Collision override
    } //Goal
} //Math For Games
