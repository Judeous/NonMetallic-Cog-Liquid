﻿using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace MathForGames
{
    /// <summary>
    /// An enemy is an actor that is able to "see" other actors.
    /// When given a target, an enemy will repeatedly check if it
    /// is in its sight range. 
    /// </summary>
    class Enemy : Actor
    {
        private Actor _target;
        private Color _alertColor;
        private Vector2 _patrolPointA;
        private Vector2 _patrolPointB;
        private Vector2 _currentPoint;
        private float _speed = 1;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        } //Speed property

        public Actor Target
        {
            get { return _target; }
            set { _target = value; }
        } //Target property

        public Vector2 PatrolPointA
        {
            get { return _patrolPointA; }
            set { _patrolPointA = value; }
        } //Patrol Point A property

        public Vector2 PatrolPointB
        {
            get { return _patrolPointB; }
            set { _patrolPointB = value; }
        } //Patrol Poing B property

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn</param>
        public Enemy(float x, float y, Vector2 patrolPointA, Vector2 patrolPointB, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base((char)x, y, icon, color)
        {
            PatrolPointA = patrolPointA;
            PatrolPointB = patrolPointB;
            _currentPoint = PatrolPointA;
        } //Enemy constructor

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Enemy(float x, float y, Color rayColor, Vector2 patrolPointA, Vector2 patrolPointB, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, icon, color)
        {
            _sprite = new Sprite("Images/enemy.png");

            _alertColor = Color.RED;
            PatrolPointA = patrolPointA;
            PatrolPointB = patrolPointB;
            _currentPoint = PatrolPointA;
        } //Enemy constructor overload for Raylib

        /// <summary>
        /// Checks to see if the target is within the given angle
        /// and within the given distance. Returns false if no
        /// target has been set. Both the angle and the distance are inclusive.
        /// </summary>
        /// <param name="maxAngle">The maximum angle (in radians) 
        /// that the target can be detected.</param>
        /// <param name="maxDistance">The maximum distance that the target can be detected.</param>
        /// <returns></returns>
        public bool CheckTargetInSight(float maxAngle, float maxDistance)
        {
            //Checks if the target has a value before continuing
            if (Target == null)
                return true;

            //Find the vector representing the distance between the actor and its target
            Vector2 direction = Target.GlobalPosition - GlobalPosition;
            //Get the magnitude of the distance vector
            float distance = direction.Magnitude;
            //Use the inverse cosine to find the angle of the dot product in radians
            float angle = (float)Math.Acos(Vector2.DotProduct(Forward, direction.Normalized));

            //Return true if the angle and distance are in range
            if (angle <= maxAngle && distance <= maxDistance)
                return true;

            return false;
        } //Check Target In Sight function

        /// <summary>
        /// Updates the current location the enemy is traveling to
        /// once its reached a patrol point.
        /// </summary>
        private void UpdatePatrolLocation()
        {
            //Calculate the distance between the current patrol point and the current position
            Vector2 direction = _currentPoint - LocalPosition;
            float distance = direction.Magnitude;

            //Switch to the new patrol point if the enemy is within distance of the current one
            if (_currentPoint == PatrolPointA && distance <= 1)
                _currentPoint = PatrolPointB;
            else if (_currentPoint == PatrolPointB && distance <= 1)
                _currentPoint = PatrolPointA;

            //Calcute new velocity to travel to the next waypoint
            direction = _currentPoint - LocalPosition;
            Velocity = direction.Normalized * Speed;
        } //Update Patrol location function

        public override void Update(float deltaTime)
        {
            //If the target can be seen change the color to red and reset the player's position
            //If the target can't be seen change the color to blue
            if(CheckTargetInSight(0.75f, 5))
            {
                _rayColor = Color.RED;
                //Target.LocalPosition = new Vector2();
            }
            else
                _rayColor = Color.BLUE;

            //UpdatePatrolLocation();
            base.Update(deltaTime);
        } //Update
    } //Enemy
} //Math For Games