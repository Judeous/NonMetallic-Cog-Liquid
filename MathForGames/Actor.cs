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
    /// 
    /// Create new matrices to transform the actors matrix. The user should be able to translate, rotate, and scale the actor.
    /// </summary>
    class Actor
    {
        protected char _icon = ' ';
        protected ConsoleColor _color;
        protected Color _rayColor;
        protected Sprite _sprite;

        protected Actor _parent;
        protected Actor[] _children = new Actor[0];

        protected Vector2 _velocity;

        protected Matrix3 _localTransform = new Matrix3();
        protected Matrix3 _globalTransform = new Matrix3();
        //Pieces of _transform matrix
        protected Matrix3 _translation = new Matrix3();
        protected Matrix3 _rotation = new Matrix3();
        protected Matrix3 _scale = new Matrix3();

        public bool Started { get; private set; } //Started property

        public Vector2 Forward
        {
            get { return new Vector2(_globalTransform.m11, _globalTransform.m21); }
            set { _globalTransform.m11 = value.X; _globalTransform.m21 = value.Y; }
        } //Forward property

        public Vector2 LocalPosition
        {
            get { return new Vector2(_localTransform.m13, _localTransform.m23); }
            set { _translation.m13 = value.X; _translation.m23 = value.Y; }
        } //Position property

        public Vector2 GlobalPosition
        {
            get { return new Vector2(_globalTransform.m13, _globalTransform.m23); }
            set { _translation.m13 = value.X; _translation.m23 = value.Y; }
        } //Position property

        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        } //Velocity property

        public void AddChild(Actor child)
        {
            Actor[] tempArray = new Actor[_children.Length + 1];

            for (int i = 0; i < _children.Length; i++)
                tempArray[i] = _children[i];

            tempArray[_children.Length] = child;
            _children = tempArray;
            child._parent = this;
        } //Add Child function

        public bool RemoveChild(Actor child)
        {
            if (child == null)
                return false;

            Actor[] tempArray = new Actor[_children.Length];
            bool childRemoved = false;

            Actor[] newArray = new Actor[_children.Length - 1];

            int j = 0;

            for (int i = 0; i < _children.Length; i++)
            {
                if (child != _children[i])
                {
                    newArray[j] = _children[i];
                    j++;
                }
                else
                    childRemoved = true;
            } //for every child

            _children = tempArray;
            child._parent = null;
            return childRemoved;
        } //Remove Child by Child function

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn</param>
        public Actor(float y, float x, char icon = ' ', ConsoleColor color = ConsoleColor.White)
        {
            _rayColor = Color.WHITE;
            _icon = icon;
            _color = color;
            _localTransform = new Matrix3();
            LocalPosition = new Vector2(x, y);
            Velocity = new Vector2();
            //Forward = new Vector2(1, 0);
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
            {

                return;
            }

        } //Update Facing function

        public virtual void Start()
        {
            Started = true;
        } //Start
        
        public virtual void Update(float deltaTime)
        {
            UpdateLocalTransform();
            UpdateGlobalTransform();

            //Increase position by the current velocity
            LocalPosition += Velocity * deltaTime;
        } //Update

        public virtual void Draw()
        {
            if(_sprite != null)
                _sprite.Draw(_globalTransform);

            //Draws the actor and a line indicating it facing to the raylib window.
            //Scaled to match console movement
            Raylib.DrawText(_icon.ToString(), (int)(GlobalPosition.X * 32), (int)(GlobalPosition.Y * 32), 32, _rayColor);
            Raylib.DrawLine(
                (int)(GlobalPosition.X * 32),
                (int)(GlobalPosition.Y * 32),
                (int)((GlobalPosition.X + Forward.X) * 32),
                (int)((GlobalPosition.Y + Forward.Y) * 32),
                Color.WHITE);

            //Changes the color of the console text to be this actors color
            Console.ForegroundColor = _color;

            //Only draws the actor on the console if it is within the bounds of the window
            if(GlobalPosition.X >= 0 && GlobalPosition.X < Console.WindowWidth 
                && GlobalPosition.Y >= 0  && GlobalPosition.Y < Console.WindowHeight)
            {
                Console.SetCursorPosition((int)GlobalPosition.X, (int)GlobalPosition.Y);
                Console.Write(_icon);
            }
            
            //Reset console text color to be default color
            Console.ForegroundColor = Game.DefaultColor;
        } //Draw

        public virtual void End()
        {
            Started = false;
        } //End

        public void SetTranslate(Vector2 position) 
        {
            _translation.m13 = position.X;
            _translation.m23 = position.Y;
        } //Set Translate function

        public void SetRotation(float radians)
        {
            _rotation.m11 = (float)(Math.Cos(radians));
            _rotation.m21 = (float)(-1*Math.Sin(radians));
            _rotation.m12 = (float)(Math.Sin(radians));
            _rotation.m22 = (float)(Math.Cos(radians));
        } //Set Rotation function

        public void SetScale(float x, float y)
        {
            _scale.m11 = x;
            _scale.m22 = y;
        } //Set Scale function

        private void UpdateLocalTransform()
        {
            SetRotation(-(float)Math.Atan2(Velocity.Y, Velocity.X));

            _localTransform = _translation * _rotation * _scale;

        } //Update Transform function

        private void UpdateGlobalTransform()
        {
            if (_parent != null)
                _globalTransform = _parent._globalTransform * _localTransform;
            else
                _globalTransform = _localTransform;
        } //Update Global Transform
    } //Actor
} //Math For Games