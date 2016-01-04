using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Phone.Tasks;
//using Microsoft.Xna.Framework;
using System.Drawing;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Threading.Tasks;
using Windows.Storage;
using System.Runtime.Serialization.Json;
using Windows.Storage.Streams;
using Newtonsoft.Json;






namespace PhoneApp4
{
    public partial class CreateLevel : PhoneApplicationPage
    {
        private CameraCaptureTask cameraTask;
        public List<GameObject> gos;
        private string fileName;
        public CreateLevel()
        {
            InitializeComponent();

            cameraTask = new CameraCaptureTask();
            cameraTask.Completed += new EventHandler<PhotoResult>(cameraTask_Completed);
        }

        private void cameraTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                bmp.ImageOpened += ImageOpenedTrue;
                //imgPreview.Source = bmp;
                //bmp.DecodePixelHeight = 460;
                //Height="460" Margin="10,10,0,0" VerticalAlignment="Top" Width="684">
                WriteableBitmap wbmp = new WriteableBitmap(bmp);
                //txtImage.Text = "Korkeus: " + wbmp.PixelHeight + " Leveys: " + wbmp.PixelWidth;
                Bitmap image = (Bitmap)wbmp;
                //AForge.Imaging.Filters.Grayscale
                //Grayscale gs = new Grayscale(

                // create filter
                ResizeNearestNeighbor filter = new ResizeNearestNeighbor(700, 480);
                // apply the filter
                Bitmap resizeImage = filter.Apply(image);
                txtImage.Text = "Korkeus: " + resizeImage.Height + " Leveys: " + resizeImage.Width;
                // create grayscale filter (BT709)
                Grayscale GSfilter = new Grayscale(0.2125, 0.7154, 0.0721);
                // apply the filter
                Bitmap grayImage = GSfilter.Apply(resizeImage);

                // create filter
                Threshold BIfilter = new Threshold(70);
                // apply the filter
                BIfilter.ApplyInPlace(grayImage);

                // create filter
                Invert Invertfilter = new Invert();
                // apply the filter
                Invertfilter.ApplyInPlace(grayImage);

                AForge.Imaging.Filters.ConnectedComponentsLabeling Blobfilter = new AForge.Imaging.Filters.ConnectedComponentsLabeling();
                Bitmap newImage = Blobfilter.Apply(grayImage);
                BlobCounter blobCounter = new BlobCounter(newImage);
                blobCounter.FilterBlobs = true;
                blobCounter.MinWidth = 5;
                blobCounter.MinHeight = 5;
                Blob[] blobs = blobCounter.GetObjectsInformation();
                gos = new List<GameObject>();
                List<Blob> blobList = blobs.ToList();
                PointCollection extremePoints = new PointCollection();
                System.Windows.Shapes.Rectangle r = new System.Windows.Shapes.Rectangle();
                PointCollection edges;
                List<AForge.IntPoint> lst;
                List<int[]> intlist = new List<int[]>();
                GameObject go = new GameObject();
                foreach (Blob b in blobs)
                {
                    lst = blobCounter.GetBlobsEdgePoints(b);
                    edges = new PointCollection();
                    foreach (AForge.IntPoint p in lst)
                    {
                        edges.Add(new System.Windows.Point(p.X, p.Y));
                    }
                    /*
                    extremePoints.Add(new System.Windows.Point(b.Rectangle.X, b.Rectangle.Y));
                    extremePoints.Add(new System.Windows.Point(b.Rectangle.X+b.Rectangle.Width, b.Rectangle.Y));
                    extremePoints.Add(new System.Windows.Point(b.Rectangle.X, b.Rectangle.Y+b.Rectangle.Height));
                    extremePoints.Add(new System.Windows.Point(b.Rectangle.X + b.Rectangle.Width, b.Rectangle.Y + b.Rectangle.Height));
                     * */
                    gos.Add(new GameObject(b.Rectangle.Y,b.Rectangle.X,b.Rectangle.Width,b.Rectangle.Height,edges)); // x ja y käännettynä, jotta lopullisessa mallissa suunnat oikein.
                    //GameObject go = new GameObject(
                    //extremePoints.Clear();
                    //edges.Clear();
                }
                //Serialize("LevelName.bin", gos);
                //SaveToLocalFolderAsync(e.ChosenPhoto, "LevelNameBG.jpg");
                //writeJsonAsync();
                fileName = txtBoxLevelName.Text;
                jsonWrite(fileName);
                WriteableBitmap wb;
                wb = (WriteableBitmap)resizeImage;

                using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (iso.FileExists(fileName+"_bg.jpg"))
                    {
                        iso.DeleteFile(fileName+"_bg.jpg");
                    }
                    using (IsolatedStorageFileStream isostream = iso.CreateFile(fileName+"_bg.jpg"))
                    {
                        Extensions.SaveJpeg(wb, isostream, wb.PixelWidth, wb.PixelHeight, 0, 85);
                        isostream.Close(); 
                    }
                }

                //Rectangle[] rects = blobCounter.GetObjectsRectangles();
                //txtImage.Text = txtImage.Text + "Number of rectangles:+ " + rects.Length;

                wbmp = (WriteableBitmap)newImage;

                imgImage.Source = wbmp;
                // BY PIXEL
                /*
                foreach (int pix in wbmp.Pixels)
                {
                    int a = pix >> 24;
                    int r = (pix & 0x00ff0000) >> 16;
                    int g = (pix & 0x0000ff00) >> 8;
                    int b = (pix & 0x000000ff);
                    Color c = new Color(r, g, b, a);
                    int intensity = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                    if (intensity > 127) ; // BLACK / SOLID
                    else ; // WHITE / NON-SOLID
                }*/
                //byte[] baLevel = CustomConvertToBytes(bmp);
                /*
                WriteableBitmap wbmp = new WriteableBitmap(bmp as BitmapSource);
                imgPreview.Source = wbmp;*/
            }
        }

        #region JSON

        private async void jsonWrite(string fileName) 
        {

            // Serialize our Product class into a string
            // Changed to serialze the List
            string jsonContents = JsonConvert.SerializeObject(gos);

            // Get the app data folder and create or replace the file we are storing the JSON in.
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile textFile = await localFolder.CreateFileAsync(fileName + ".json", CreationCollisionOption.ReplaceExisting);

            // Open the file...
            using (IRandomAccessStream textStream = await textFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                // write the JSON string!
                using (DataWriter textWriter = new DataWriter(textStream))
                {
                    textWriter.WriteString(jsonContents);
                    await textWriter.StoreAsync();
                }
            }
            
        }
#endregion
        

#region old JSON
        /*
        private const string JSONFILENAME = "levelname.json";
        
        private async Task writeJsonAsync()
        {
        // Notice that the write is ALMOST identical ... except for the serializer.

        //var myCars = buildObjectGraph();

        var serializer = new DataContractJsonSerializer(typeof(List<GameObject>));
        using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(
                JSONFILENAME,
                CreationCollisionOption.ReplaceExisting))
        {
        serializer.WriteObject(stream, gos);
        }
        MessageBox.Show("levelname.json kirjoitettu");
        //resultTextBlock.Text = "Write succeeded";
        }

        private const string JSONFILENAMEIMAGE = "levelnamebg.json";

        private async Task writeJsonAsyncImage()
        {
            // Notice that the write is ALMOST identical ... except for the serializer.

            //var myCars = buildObjectGraph();

            var serializer = new DataContractJsonSerializer(typeof(List<GameObject>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(
                    JSONFILENAMEIMAGE,
                    CreationCollisionOption.ReplaceExisting))
            {
                serializer.WriteObject(stream, gos);
            }

            //resultTextBlock.Text = "Write succeeded";
        }

        private async Task readJsonAsync()
        {
        // Notice that the write **IS** identical ... except for the serializer.

        string content = String.Empty;

        var myStream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(JSONFILENAME);
        using (StreamReader reader = new StreamReader(myStream))
        {
            content = await reader.ReadToEndAsync();
        }

         //resultTextBlock.Text = content;
        }

       

        public async Task SaveToLocalFolderAsync(Stream file, string fileName)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using (Stream outputStream = await storageFile.OpenStreamForWriteAsync())
            {
                await file.CopyToAsync(outputStream);
            }
        }

        private static void Serialize(string fileName, object source)
        {
            var userStore = IsolatedStorageFile.GetUserStoreForApplication();

            using (var stream = new IsolatedStorageFileStream(fileName, FileMode.Create, userStore))
            {
                XmlSerializer serializer = new XmlSerializer(source.GetType());
                serializer.Serialize(stream, source);
            }
        }

        public static void Deserialize<T>(ObservableCollection<T> list, string filename)
        {

            list = new ObservableCollection<T>();

            var userStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (userStore.FileExists(filename))
            {
                using (var stream = new IsolatedStorageFileStream(filename, FileMode.Open, userStore))
                {
                    XmlSerializer serializer = new XmlSerializer(list.GetType());
                    var items = (ObservableCollection<T>)serializer.Deserialize(stream);

                    foreach (T item in items)
                    {
                        list.Add(item);
                    }
                }
            }
        }
        */
#endregion
        /*
        private bool ColorToGray(int color)
        {
            int gray = 0;

            int a = color >> 24;
            int r = (color & 0x00ff0000) >> 16;
            int g = (color & 0x0000ff00) >> 8;
            int b = (color & 0x000000ff);
            
            if ((r == g) && (g == b))
            {
                return false;
            }
            else
            {
                // Calculate for the illumination.
                // I =(int)(0.109375*R + 0.59375*G + 0.296875*B + 0.5)
                int i = (7 * r + 38 * g + 19 * b + 32) >> 6;

                gray = ((a & 0xFF) << 24) | ((i & 0xFF) << 16) | ((i & 0xFF) << 8) | (i & 0xFF);
            }
            return true;
        }*/

        private void btnPicture_Click(object sender, RoutedEventArgs e)
        {
            // TODO: start camera app to take the picture for the level
            /*
            BitmapImage bmLevel = new BitmapImage(new Uri(@"D:/Koulu/DIYPlatformer/Level.png"));
            byte[] baLevel;
            baLevel = CustomConvertToBytes(bmLevel);
            */
            cameraTask.Show();
        }

        private void ImageOpenedTrue(object sender, RoutedEventArgs e)
        {
            BitmapImage bImage = (BitmapImage)sender;

            WriteableBitmap wbImage = new WriteableBitmap(bImage);

            //use writeable bitmap forwhat ever purpose
        }

        public static byte[] CustomConvertToBytes(BitmapImage bitmapImage)
        {
            byte[] data = null;
            using (MemoryStream stream = new MemoryStream())
            {
                WriteableBitmap wBitmap = new WriteableBitmap(bitmapImage);
                wBitmap.SaveJpeg(stream, wBitmap.PixelWidth, wBitmap.PixelHeight, 0, 100);
                stream.Seek(0, SeekOrigin.Begin);
                data = stream.GetBuffer();
            }

            return data;
        }
        
        private void btnResolution_Click(object sender, RoutedEventArgs e)
        {
            // TODO: open downscaled picture with a rectangle on it and + and - buttons to scale the rectangle -> selected size will decide resolution
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            string msg = "Create: Level";
            NavigationService.Navigate(new Uri("/HelpPage.xaml?msg=" + msg, UriKind.Relative));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (fileName != null)
            {
                NavigationService.Navigate(new Uri("/PlayLevel.xaml?levelName=" + fileName, UriKind.Relative));
            }
            
        }

        private void btnPremade_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCharacterPicture_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPremadeEnemy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEnemyPicture_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPremadeBackground_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBackgroundPicture_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}