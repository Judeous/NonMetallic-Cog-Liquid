using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace MathForGames
{

    /// <summary>
    /// This is the base class for all objects that will 
    /// be moved or interacted with in the game
    /// </summary>
    class Actor
    {
        protected char _icon = ' ';
        protected ConsoleColor _color;
        protected Color _rayColor;
        protected Matrix3 _transform;
        protected Vector2 _velocity;

        public bool Started { get; private set; } //Started property

        public Vector2 Forward
        {
            get { return new Vector2(_transform.m11, _transform.m22); }
            set { _transform.m11 = value.X;   _transform.m22 = value.Y; }
        } //Forward property

        public Vector2 Position
        {
            get { return new Vector2(_transform.m31, _transform.m32); }
            set { _transform.m31 = value.X; _transform.m32 = value.Y; }
        } //Position property

        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        } //Velocity property

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn</param>
        public Actor(float y, float x, char icon = ' ', ConsoleColor color = ConsoleColor.White)
        {
            _rayColor = Color.WHITE;
            _icon = icon;
            _color = color;
            _transform = new Matrix3();
            Position = new Vector2(x, y);
            Velocity = new Vector2();
            Forward = new Vector2(1, 0);
        } //Actor Constructor

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Actor(float x, float y, Color rayColor, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : this((char)x, y, icon, color)
        {
            _rayColor = rayColor;
        } //Overload Constructor with Raylib

        /// <summary>
        /// Updates the actors forward vector to be
        /// the last direction it moved in
        /// </summary>
        private void UpdateFacing()
        {
            if (Velocity.Magnitude <= 0)
                return;

            Forward = Velocity.Normalized;
        } //Update Facing function

        public virtual void Start()
        {
            Started = true;
        } //Start
        
        public virtual void Update(float deltaTime)
        {
            //Before the actor is moved, update the direction it's facing
            UpdateFacing();

            //Increase position by the current velocity
            Position += Velocity * deltaTime;

            //Makes sure position stays within bounds
            _transform.m31 = Math.Clamp(_transform.m31, 0, Raylib.GetScreenWidth()/32 - 1);
            _transform.m32 = Math.Clamp(_transform.m32, 0, Raylib.GetScreenHeight()/32 - 1);
        } //Update

        public virtual void Draw()
        {
            //Draws the actor and a line indicating it facing to the raylib window.
            //Scaled to match console movement
            Raylib.DrawText(_icon.ToString(), (int)(Position.X * 32), (int)(Position.Y * 32), 32, _rayColor);
            Raylib.DrawLine(
                (int)(Position.X * 32),
                (int)(Position.Y * 32),
                (int)((Position.X + Forward.X) * 32),
                (int)((Position.Y + Forward.Y) * 32),
                Color.WHITE);

            //Changes the color of the console text to be this actors color
            Console.ForegroundColor = _color;

            //Only draws the actor on the console if it is within the bounds of the window
            if(Position.X >= 0 && Position.X < Console.WindowWidth 
                && Position.Y >= 0  && Position.Y < Console.WindowHeight)
            {
                Console.SetCursorPosition((int)Position.X, (int)Position.Y);
                Console.Write(_icon);
            }
            
            //Reset console text color to be default color
            Console.ForegroundColor = Game.DefaultColor;
        } //Draw

        public virtual void End()
        {
            Started = false;
        } //End
    } //Actor
} //Math For Games