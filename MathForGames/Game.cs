using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using MathLibrary;
using Raylib_cs;

namespace MathForGames
{
    class Game
    {
        private static bool _gameOver = false;
        private static Scene[] _scenes;
        private static int _currentSceneIndex;

        public static int CurrentSceneIndex
        {
            get { return _currentSceneIndex; }
        } //Current Scene Index property

        public static ConsoleColor DefaultColor { get; set; } = ConsoleColor.White;

        /// <summary>
        /// Used to set the value of game over.
        /// </summary>
        /// <param name="value">If this value is true, the game will end</param>
        public static void SetGameOver(bool value)
        {
            _gameOver = value;
        } //Set Game Over function

        /// <summary>
        /// Returns the scene at the index given.
        /// Returns an empty scene if the index is out of bounds
        /// </summary>
        /// <param name="index">The index of the desired scene</param>
        /// <returns></returns>
        public static Scene GetScene(int index)
        {
            if (index < 0 || index >= _scenes.Length)
                return new Scene();

            return _scenes[index];
        } //Get Scene function

        /// <summary>
        /// Returns the scene that is at the index of the 
        /// current scene index
        /// </summary>
        /// <returns></returns>
        public static Scene GetCurrentScene()
        {
            return _scenes[_currentSceneIndex];
        } //Get Current Scene function

        /// <summary>
        /// Adds the given scene to the array of scenes.
        /// </summary>
        /// <param name="scene">The scene that will be added to the array</param>
        /// <returns>The index the scene was placed at. Returns -1 if
        /// the scene is null</returns>
        public static int AddScene(Scene scene)
        {
            //If the scene is null then return before running any other logic
            if (scene == null)
                return -1;

            //Create a new temporary array that one size larger than the original
            Scene[] tempArray = new Scene[_scenes.Length + 1];

            //Copy values from old array into new array
            for(int i = 0; i < _scenes.Length; i++)
            {
                tempArray[i] = _scenes[i];
            }

            //Store the current index
            int index = _scenes.Length;

            //Sets the scene at the new index to be the scene passed in
            tempArray[index] = scene;

            //Set the old array to the tmeporary array
            _scenes = tempArray;

            return index;
        } //Add Scene function

        /// <summary>
        /// Finds the instance of the scene given that inside of the array
        /// and removes it
        /// </summary>
        /// <param name="scene">The scene that will be removed</param>
        /// <returns>If the scene was successfully removed</returns>
        public static bool RemoveScene(Scene scene)
        {
            //If the scene is null then return before running any other logic
            if (scene == null)
                return false;

            bool sceneRemoved = false;

            //Create a new temporary array that is one less than our original array
            Scene[] tempArray = new Scene[_scenes.Length - 1];

            //Copy all scenes except the scene we don't want into the new array
            int j = 0;
            for(int i = 0; i < _scenes.Length; i++)
            {
                if (tempArray[i] != scene)
                {
                    tempArray[j] = _scenes[i];
                    j++;
                }
                else
                {
                    sceneRemoved = true;
                }
            }

            //If the scene was successfully removed set the old array to be the new array
            if(sceneRemoved)
                _scenes = tempArray;

            return sceneRemoved;
        } //Remove Scene by scene function

        /// <summary>
        /// Sets the current scene in the game to be the scene at the given index
        /// </summary>
        /// <param name="index">The index of the scene to switch to</param>
        public static void SetCurrentScene(int index)
        {
            //If the index is not within the range of the the array return
            if (index < 0 || index >= _scenes.Length)
                return;

            //Call end for the previous scene before changing to the new one
            if (_scenes[_currentSceneIndex].Started)
                _scenes[_currentSceneIndex].End();

            //Update the current scene index
            _currentSceneIndex = index;
        } //Set Current Scene

        /// <summary>
        /// Returns true while a key is being pressed
        /// </summary>
        /// <param name="key">The ascii value of the key to check</param>
        /// <returns></returns>
        public static bool GetKeyDown(int key)
        {
            return Raylib.IsKeyDown((KeyboardKey)key);
        } //Get Key Down function

        /// <summary>
        /// Returns true while if key was pressed once
        /// </summary>
        /// <param name="key">The ascii value of the key to check</param>
        /// <returns></returns>
        public static bool GetKeyPressed(int key)
        {
            return Raylib.IsKeyPressed((KeyboardKey)key);
        } //Get Key Pressed function

        public Game()
        {
            _scenes = new Scene[0];
        } //Game Constructor

        //Called when the game begins. Use this for initialization.
        public void Start()
        {
            DisplayControls();

            //Creates a new window for raylib
            Raylib.InitWindow(1024, 760, "Math For Games");
            Raylib.SetTargetFPS(24);

            //Set up console window
            Console.CursorVisible = false;
            Console.Title = "Math For Games";

            //Create a new scene for our actors to exist in
            Scene scene1 = new Scene();
            Scene scene2 = new Scene();

            //Create the actors to add to our scene
            Enemy enemy1 = new Enemy(2, -5, Color.GREEN, new Vector2(0,5), new Vector2(30, 5), '■', ConsoleColor.Green);
            Enemy enemy2 = new Enemy(10, 10, Color.GREEN, new Vector2(0, 10), new Vector2(30, 10), '■', ConsoleColor.Green);
            Enemy enemy3 = new Enemy(.1f, 7, Color.GREEN, new Vector2(0, 20), new Vector2(30, 20), '■', ConsoleColor.Green);
            Player player = new Player(0, 0, Color.BLUE, '@', ConsoleColor.Red);
            Goal goal = new Goal(11, 16, Color.GREEN, player, 'G', ConsoleColor.Green);

            //Initialize the enemies' starting values
            enemy1.Speed = 2;
            enemy2.Speed = 2;
            enemy3.Speed = 2;

            //Set player's starting speed
            player.Speed = 5;

            goal.AddChild(enemy1);
            goal.AddChild(enemy2);
            goal.AddChild(enemy3);

            //Add actors to the scenes
            scene1.AddActor(player);
            scene1.AddActor(enemy1);
            scene1.AddActor(enemy2);
            scene1.AddActor(enemy3);
            scene1.AddActor(goal);
            scene2.AddActor(player);
            scene2.AddActor(goal);
            
            //Sets the targets of the enemies to be the Player
            enemy1.Target = player;
            enemy2.Target = player;
            enemy3.Target = player;
            //Sets the starting scene index and adds the scenes to the scenes array
            int startingSceneIndex = AddScene(scene1);
            AddScene(scene2);

            //Sets the current scene to be the starting scene index
            SetCurrentScene(startingSceneIndex);
        } //Start

        /// <summary>
        /// Called every frame
        /// </summary>
        /// <param name="deltaTime">The time between each frame</param>
        public void Update(float deltaTime)
        {
            if (!_scenes[_currentSceneIndex].Started)
                _scenes[_currentSceneIndex].Start();

            _scenes[_currentSceneIndex].Update(deltaTime);
        } //Update

        /// <summary>
        /// Used to display objects and other info on the screen.
        /// </summary>
        public void Draw()
        {
            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.BLACK);
            Console.Clear();
            _scenes[_currentSceneIndex].Draw();

            Raylib.EndDrawing();
        } //Draw

        /// <summary>
        /// Called when the game ends.
        /// </summary>
        public void End()
        {
            if (_scenes[_currentSceneIndex].Started)
                _scenes[_currentSceneIndex].End();
        } //End

        /// <summary>
        /// Handles all of the main game logic including the main game loop.
        /// </summary>
        public void Run()
        {
            //Call start for all objects in game
            Start();

            //Loops the game until either the game is set to be over or the window closes
            while(!_gameOver && !Raylib.WindowShouldClose())
            {
                //Stores the current time between frames
                float deltaTime = Raylib.GetFrameTime();
                //Call update for all objects in game
                Update(deltaTime);
                //Call draw for all objects in game
                Draw();
                //Clear the input stream for the console window
                while (Console.KeyAvailable)
                    Console.ReadKey(true);
            } //While game isn't over and Window shouldn't close

            End();
        } //Run

        public void DisplayControls()
        {
            Console.WriteLine("Controls:");
            Console.WriteLine("---------");
            Console.WriteLine("W: Move Up");
            Console.WriteLine("S: Move Down");
            Console.WriteLine("A: Move Left");
            Console.WriteLine("D: Move Right");
            Console.WriteLine("");
            Console.WriteLine("Left Arrow: Slow Down");
            Console.WriteLine("Right Arrow: Speed Up");
            Console.WriteLine("");
            Console.WriteLine("Up Arrow: Scale Up");
            Console.WriteLine("Down Arrow: Scale Down");
            Console.WriteLine("");
            Console.WriteLine("Esc: Exit Game");
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey();
        } //Display Controls function
    } //Game
} //Math For Games
