using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Shapes;
using System.Windows.Media;
using Microsoft.Xna.Framework;
//using System.Drawing;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using System.Runtime.Serialization.Json;
using System.IO.IsolatedStorage;
using Windows.Storage.Streams;
using Newtonsoft.Json;


namespace PhoneApp4
{
    public partial class PlayLevel : PhoneApplicationPage
    {
        GameTimer gameTime;
        bool goalReached;
        int jump;
        List<GameObject> GObjs; // List for all game objects
        GameObject goal;
        //List<Enemy> Enemies;
        Actor Player;
        Actor PlayerTemp;
        //Enemy EnemyTemp;
        bool enemyNoGravity;
        bool playerNoGravity;
        bool doubleJumpAvaivable;
        int topTemp;
        int leftTemp;
        bool kbLeft;
        bool kbRight;
        string levelName;
        public PlayLevel()
        {
            InitializeComponent();
            goalReached = false;
            gameTime = new GameTimer();
            gameTime.UpdateInterval = TimeSpan.FromSeconds(0.03);
            gameTime.Update += new EventHandler<GameTimerEventArgs>(gameTime_Update);
            levelName = "levelname";

            GObjs = new List<GameObject>();
            //deserializeJsonAsync();
            
            // Load Level to the canvas
            /*
            EnemyTemp = new Enemy();
            enemyNoGravity = false;
            Enemies = new List<Enemy>();
            Enemies.Add(new Enemy(10,10,10,10,10,1,1000,true,false));
            Enemies.Add(new Enemy(10,100,10,10,10,1,1000,false,true));
            foreach (Enemy e in Enemies)
            {
                Canvas.SetLeft(e.hitbox, e.Left);
                Canvas.SetTop(e.hitbox, e.Top);
                canvasWorld.Children.Add(e.hitbox);

            }
            */

            kbLeft = false;
            kbRight = false;
            topTemp = 0;
            jump = 0;
            doubleJumpAvaivable = true;

            /*
            //GObjs.Add(new GameObject(10,10,100,10));
            GObjs.Add(new GameObject(100, 10, 100, 10));
            GObjs.Add(new GameObject(150, 110, 100, 10));
            GObjs.Add(new GameObject(200, 210, 100, 10));
            GObjs.Add(new GameObject(250, 310, 100, 10));

            foreach (GameObject go in GObjs)
            {
                Canvas.SetLeft(go.hitbox, go.Left);
                Canvas.SetTop(go.hitbox, go.Top);
                canvasWorld.Children.Add(go.hitbox);
                
            }
            */

            //MessageBox.Show("Start Game");
            //gameTime.Start();

        }

        private void loadLevel()
        {
            JsonRead();

            BitmapImage bmi = LoadImageFromIsolatedStorage();
            if (bmi != null)
            {
                //canvasWorld.Width = bmi.DecodePixelWidth;
                //canvasWorld.Height = bmi.DecodePixelHeight;
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = bmi;
                canvasWorld.Background = brush;

            }

            Player = new Actor(0, 0, 10, 25, 1, 0, 100);
            PlayerTemp = new Actor(0, 0, 10, 25, 1, 0, 100);
            playerNoGravity = false;
            Canvas.SetLeft(Player.hitbox, 10);
            Canvas.SetTop(Player.hitbox, 10);
            canvasWorld.Children.Add(Player.hitbox);

            goal = new GameObject(400, 650, 30, 30);
            goal.Trigger = 1;

            //GObjs.Add(goal);
            System.Windows.Shapes.Rectangle rec = new System.Windows.Shapes.Rectangle();
            rec.Width = goal.Width;
            rec.Height = goal.Height;
            rec.Fill = new SolidColorBrush(Colors.Red);
            Canvas.SetLeft(rec, goal.Left);
            Canvas.SetTop(rec, goal.Top);
            canvasWorld.Children.Add(rec);

            foreach (GameObject go in GObjs)
            {
                System.Windows.Shapes.Rectangle rec1 = new System.Windows.Shapes.Rectangle();
                rec.Width = 5;
                rec.Height = 5;
                rec.Fill = new SolidColorBrush(Colors.Red);
                Canvas.SetLeft(rec1, go.Left);
                Canvas.SetTop(rec1, go.Top);
                canvasWorld.Children.Add(rec1);

                System.Windows.Shapes.Rectangle rec2 = new System.Windows.Shapes.Rectangle();
                rec2.Width = 5;
                rec2.Height = 5;
                rec2.Fill = new SolidColorBrush(Colors.Blue);
                Canvas.SetLeft(rec2, go.Left + go.Width);
                Canvas.SetTop(rec2, go.Top + go.Height);
                canvasWorld.Children.Add(rec2);

                System.Windows.Shapes.Rectangle rec3 = new System.Windows.Shapes.Rectangle();
                rec3.Width = 5;
                rec3.Height = 5;
                rec3.Fill = new SolidColorBrush(Colors.Green);
                Canvas.SetLeft(rec3, go.Left);
                Canvas.SetTop(rec3, go.Top + go.Height);
                canvasWorld.Children.Add(rec3);
            }

            MessageBox.Show("Start Game");
            gameTime.Start();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            IDictionary<string, string> parameters = NavigationContext.QueryString;
            if (parameters.ContainsKey("levelname"))
            {
                levelName = parameters["levelname"];
                loadLevel();
            }
            else
            {
                MessageBox.Show("Failed to load level. Check level name!");
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        #region JSON


        private async void JsonRead()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            try
            {
                // Getting JSON from file if it exists, or file not found exception if it does not
                StorageFile textFile = await localFolder.GetFileAsync(levelName + ".json");

                using (IRandomAccessStream textStream = await textFile.OpenReadAsync())
                {
                    // Read text stream 
                    using (DataReader textReader = new DataReader(textStream))
                    {
                        //get size
                        uint textLength = (uint)textStream.Size;
                        await textReader.LoadAsync(textLength);
                        // read it
                        string jsonContents = textReader.ReadString(textLength);
                        // deserialize back to our products!
                        //I only had to change this following line in this function
                        GObjs = JsonConvert.DeserializeObject<IList<GameObject>>(jsonContents) as List<GameObject>;
                        // and show it
                        //MessageBox.Show("Json ladattu!!!");
                        //displayProduct();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
                //txtOut.Text = "Exception: " + ex.Message;
            }
        }




        private BitmapImage LoadImageFromIsolatedStorage()
        {
            // The image will be read from isolated storage into the following byte array 
            byte[] data;


            // Read the entire image in one go into a byte array 
            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    // Open the file - error handling omitted for brevity 
                    // Note: If the image does not exist in isolated storage the following exception will be generated: 
                    // System.IO.IsolatedStorage.IsolatedStorageException was unhandled  
                    // Message=Operation not permitted on IsolatedStorageFileStream  
                    using (IsolatedStorageFileStream isfs = isf.OpenFile(levelName + "_bg.jpg", FileMode.Open, FileAccess.Read))
                    {
                        // Allocate an array large enough for the entire file 
                        data = new byte[isfs.Length];
                        // Read the entire file and then close it 
                        isfs.Read(data, 0, data.Length);
                        isfs.Close();
                    }
                }


                // Create memory stream and bitmap 
                MemoryStream ms = new MemoryStream(data);
                BitmapImage bi = new BitmapImage();
                // Set bitmap source to memory stream 
                bi.SetSource(ms);
                return bi;
                // Create an image UI element Note: this could be declared in the XAML instead 
                //Image image = new Image();
                // Set size of image to bitmap size for this demonstration 
                //image.Height = bi.PixelHeight;
                //image.Width = bi.PixelWidth;
                // Assign the bitmap image to the imageÃ¢â‚¬â„¢s source 
                //image.Source = bi;
                // Add the image to the grid in order to display the bit map 
                //ContentPanel.Children.Add(image);
            }
            catch (Exception e)
            {
                // handle the exception 
                //Debug.WriteLine(e.Message);
                return null;
            }
        } 






        private const string JSONFILENAME = "levelname.json";

        private async Task readJsonAsync()
        {
            // Notice that the write **IS** identical ... except for the serializer.

            string content = String.Empty;

            var myStream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(JSONFILENAME);
            using (StreamReader reader = new StreamReader(myStream))
            {
                content = await reader.ReadToEndAsync();
            }
            MessageBox.Show("Json luettu");
            //resultTextBlock.Text = content;
        }

        private async Task deserializeJsonAsync()
        {
            string content = String.Empty;

            //List<GameObject> myCars;
            var jsonSerializer = new DataContractJsonSerializer(typeof(List<GameObject>));

            var myStream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(JSONFILENAME);
            myStream.ReadTimeout = 9999999;
            myStream.WriteTimeout = 9999999;
            GObjs = (List<GameObject>)jsonSerializer.ReadObject(myStream);
            //MessageBox.Show("Json deserialized");
            /*
            foreach (var car in myCars)
            {
                content += String.Format("ID: {0}, Make: {1}, Model: {2} ... ", car.Id, car.Make, car.Model);
            }

            resultTextBlock.Text = content;*/
        }

        #endregion
        private void gameTime_Update(object sender, GameTimerEventArgs e)
        {
            if (goalReached)
            {
                MessageBox.Show("Top " + Player.Top + " Left " + Player.Left);
                goalReached = false;
            }

            

            if (btnLEFT.IsPressed || kbLeft) 
            {
                PlayerTemp.Top = Player.Top;
                PlayerTemp.Left = Player.Left;
                PlayerTemp.Left = PlayerTemp.Left - 2;
                bool stopLeft = false;
                if (PlayerTemp.Collision(goal))
                {
                    //gameTime.Stop();
                    //MessageBox.Show("Goal Reached!");
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
                foreach (GameObject go in GObjs)
                {
                    if (PlayerTemp.Collision(go))
                    {
                        stopLeft = true;
                        leftTemp = go.Left + (int)go.Width;
                        
                    }
                }
                if (!stopLeft) Player.Left = PlayerTemp.Left;
                else
                {
                    Player.Left = leftTemp;
                }



                //////////////
                canvasWorld.Children.Remove(Player.hitbox);
                //Player.left = Player.left + 2;
                Canvas.SetLeft(Player.hitbox, Player.Left);
                Canvas.SetTop(Player.hitbox, Player.Top);
                canvasWorld.Children.Add(Player.hitbox);


                /*
                canvasWorld.Children.Remove(Player.hitbox);
                Player.left = Player.left -2;
                Canvas.SetLeft(Player.hitbox, Player.left);
                Canvas.SetTop(Player.hitbox, Player.top);
                canvasWorld.Children.Add(Player.hitbox);
                 */
                /*
                foreach (System.Windows.Shapes.Rectangle p in canvasWorld.Children) {
                    if (p.Tag != null && p.Tag.ToString() == "Player") 
                    {
                        canvasWorld.Children.Remove(p);
                        Player.left = Player.left -1;
                        Canvas.SetLeft(p, Player.left);
                        Canvas.SetTop(p, Player.top);
                        canvasWorld.Children.Add(p);
                    }
                }*/
            }//Canvas.SetLeft(canvasWorld, Canvas.GetLeft(canvasWorld) + 1);
            if (btnRIGHT.IsPressed || kbRight)
            {
                PlayerTemp.Top = Player.Top;
                PlayerTemp.Left = Player.Left;
                PlayerTemp.Left = PlayerTemp.Left + 2;
                bool stopRight = false;
                if (PlayerTemp.Collision(goal))
                {
                    gameTime.Stop();
                    MessageBox.Show("Goal Reached!");
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
                foreach (GameObject go in GObjs)
                {
                    if (PlayerTemp.Collision(go))
                    {
                        stopRight = true;
                        leftTemp = go.Left;
                    }
                }
                if (!stopRight) Player.Left = PlayerTemp.Left;
                else
                {
                    Player.Left = leftTemp - (int)Player.hitbox.Width;
                }



                //////////////
                canvasWorld.Children.Remove(Player.hitbox);
                //Player.left = Player.left + 2;
                Canvas.SetLeft(Player.hitbox, Player.Left);
                Canvas.SetTop(Player.hitbox, Player.Top);
                canvasWorld.Children.Add(Player.hitbox);
                /*
                foreach (System.Windows.Shapes.Rectangle p in canvasWorld.Children)
                {
                    if (p.Tag != null && p.Tag.ToString() == "Player")
                    {
                        canvasWorld.Children.Remove(p);
                        Player.left = Player.left +1;
                        Canvas.SetLeft(p, Player.left);
                        Canvas.SetTop(p, Player.top);
                        canvasWorld.Children.Add(p);
                    }
                }*/
            }//Canvas.SetLeft(canvasWorld, Canvas.GetLeft(canvasWorld) + -1);
            if (jump > 0) jump--;
            int tempHeight = 0;
            PlayerTemp.Top = Player.Top;
            PlayerTemp.Left = Player.Left;
            PlayerTemp.Top = PlayerTemp.Top + 2 - jump;
            playerNoGravity = false;
            if (PlayerTemp.Collision(goal))
            {
                gameTime.Stop();
                MessageBox.Show("Goal Reached!");
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            foreach (GameObject go in GObjs)
            {
                if (PlayerTemp.Collision(go))
                {
                    playerNoGravity = true;
                    doubleJumpAvaivable = true;
                    topTemp = go.Top;
                    tempHeight = (int)go.Height;
                }
            }
            if (!playerNoGravity) Player.Top = PlayerTemp.Top;
            else
            {
                if (jump > 1) 
                {
                    Player.Top = topTemp + tempHeight;
                    jump = 0;
                }
                else Player.Top = topTemp - (int)Player.hitbox.Height;
            }
            canvasWorld.Children.Remove(Player.hitbox);
            Canvas.SetLeft(Player.hitbox, Player.Left);
            Canvas.SetTop(Player.hitbox, Player.Top);
            canvasWorld.Children.Add(Player.hitbox);

            //GRAVITY FOR ENEMIES

            #region Enemy movement + collision
            /*
            foreach (Enemy en in Enemies)
            {
                if (en.Gravity)
                {

                    //int tempEnemyHeight = 0;
                    EnemyTemp.Top = en.Top;
                    EnemyTemp.Left = en.Left;
                    EnemyTemp.Top = EnemyTemp.Top + 1;
                    
                    enemyNoGravity = false;
                    foreach (GameObject go in GObjs)
                    {
                        if (EnemyTemp.Collision(go))
                        {
                            enemyNoGravity = true;
                            topTemp = go.Top;
                            tempHeight = (int)go.hitbox.Height;
                        }
                    }
                    if (!enemyNoGravity)
                    {
                        if (!en.OnPlatform) en.Top = EnemyTemp.Top; // Move downwards
                        else
                        {
                            en.MovementSpeed = -en.MovementSpeed;
                            en.Left = en.Left + en.MovementSpeed;
                        }
                    }
                    else
                    {
                        //en.MovementSpeed = -en.MovementSpeed;
                        en.OnPlatform = true;
                    } 
                    
                    /*
                     * ENEMY JUMP?
                    else
                    {
                        if (jump > 1)
                        {
                            Player.top = topTemp + tempHeight;
                            jump = 0;
                        }
                        else Player.top = topTemp - (int)Player.hitbox.Height;
                    }*/
                    

            /*
                }
                if (!en.Stationary && enemyNoGravity)
                {
                    EnemyTemp.Top = en.Top;
                    EnemyTemp.Left = en.Left;
                    EnemyTemp.Left = EnemyTemp.Left + en.MovementSpeed;
                    bool stopRight = false;
                    foreach (GameObject go in GObjs)
                    {
                        if (EnemyTemp.Collision(go))
                        {
                            stopRight = true;
                            if (en.MovementSpeed > 0) leftTemp = go.Left;
                            else if (en.MovementSpeed < 0) leftTemp = go.Left+(int)go.hitbox.Width;
                        }
                    }
                    if (!stopRight) en.Left = EnemyTemp.Left;
                    else
                    {
                        en.Left = leftTemp - (int)en.hitbox.Width;
                    }
                }
                canvasWorld.Children.Remove(en.hitbox);
                Canvas.SetLeft(en.hitbox, en.Left);
                Canvas.SetTop(en.hitbox, en.Top);
                canvasWorld.Children.Add(en.hitbox);
            }*/
            #endregion

            //throw new NotImplementedException();
        }

        

        private void btnA_Click(object sender, RoutedEventArgs e)
        {
            goalReached = true;
            //MessageBox.Show("A");
        }

        private void btnB_Click(object sender, RoutedEventArgs e)
        {
            if (playerNoGravity) jump = 12;
            else if (doubleJumpAvaivable)
            {
                jump = 12;
                doubleJumpAvaivable = false;
            }
            //MessageBox.Show("B");
        }

        private void btnB_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (playerNoGravity) jump = 12;
            else if (doubleJumpAvaivable)
            {
                jump = 12;
                doubleJumpAvaivable = false;
            }
        }

        private void rectB_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (playerNoGravity) jump = 12;
            else if (doubleJumpAvaivable)
            {
                jump = 12;
                doubleJumpAvaivable = false;
            }
        }
        /*
         * KEYBOARD DEBUGGING
        private void ContentPanel_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.A) kbLeft = true;
            if (e.Key == System.Windows.Input.Key.D) kbRight = true;
            if (e.Key == System.Windows.Input.Key.Space) jump = 10;
        }

        private void ContentPanel_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.A) kbLeft = false;
            if (e.Key == System.Windows.Input.Key.D) kbRight = false;
            
        }
        */
    }
}