using System.Windows;
using System.Drawing;
using System.Windows.Media.Imaging;
using System;
using System.IO;

namespace Sierpinksi
{
    /// <summary>
    /// Isaac Moeckly
    /// CS 480 - Assignment 1
    /// Generate the Sierpinski Gasket in WPF-C#
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// BtnGenerate_Click(object sender, RoutedEventArgs e)
        /// Runs when 'Generate' button is clicked
        /// Generates bitmap and displays final Sierpinski fractal into window's image object
        /// </summary>

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            // Set # of steps program will take to generate pixels across the fractal, defined by user
            int steps = 0;
            if (int.TryParse(txtbox1.Text, out int v))       // Check if input is integer
            {
                steps = v;

                // Hard cap on # of steps to prevent long generation times or program crashes
                if (steps >= 2000000)
                {
                    lbl1.Content = "Input too big. Please use smaller Integer.";
                    lbl1.Foreground = System.Windows.Media.Brushes.Red;
                    lbl1.FontWeight = FontWeights.Bold;
                    return;
                }

                lbl1.Content = "Successfully generated!";
                lbl1.Foreground = System.Windows.Media.Brushes.Green;
                lbl1.FontWeight = FontWeights.Bold;
            }

            else
            {                           // If not integer, end click event early and instruct to input integer
                lbl1.Content = "Wrong Input. Please use Integer.";
                lbl1.Foreground = System.Windows.Media.Brushes.Red;
                lbl1.FontWeight = FontWeights.Bold;
                return;
            }

            // Defining bitmap
            Bitmap drawingSurface = new Bitmap(461, 461);
            Graphics GFX = Graphics.FromImage(drawingSurface);

            // Initializing bitmap's background color and Sierpinski Fractal's top-level triangle points
            GFX.FillRectangle(Brushes.White, 0, 0, drawingSurface.Width, drawingSurface.Height);    // Set background color to white
            drawingSurface.SetPixel(drawingSurface.Width / 2, 10, Color.Red);                                 //RED (A) - top triangle corner
            drawingSurface.SetPixel(10, drawingSurface.Height - 10, Color.Blue);                            //BLUE (B) - bottom left triangle corner
            drawingSurface.SetPixel(drawingSurface.Width - 10, drawingSurface.Height - 10, Color.Green);    //GREEN (C) - bottom right triangle corner
            // Initializing outside 'S' point
            int sHeight = -10;
            int sWidth = -10;
            // Initializing midpoint calculation's width and height
            int medHeight = 0;
            int medWidth = 0;

            // Initialize random number generator
            var rand = new Random();

            for (int i = 0; i < steps; i++)
            {
                // Generate random number between 1 and 3
                int randnum = rand.Next(1, 4);
                // 1 = red top(A), 2 = blue left corner(B), 3 = green right corner(C)
                if (randnum == 1)
                {
                    medWidth = (sWidth + (drawingSurface.Width / 2)) / 2;       // Calculate midpoint between 'S' and point A
                    medHeight = (sHeight + 10) / 2;
                }

                if (randnum == 2)
                {
                    medWidth = (sWidth + 10) / 2;                               // Calculate midpoint between 'S' and point B
                    medHeight = (sHeight + drawingSurface.Height - 10) / 2;
                }

                if (randnum == 3)
                {
                    medWidth = (sWidth + drawingSurface.Width - 10) / 2;        // Calculate midpoint between 'S' and point C
                    medHeight = (sHeight + drawingSurface.Height - 10) / 2;
                }

                if (medHeight < 231) { drawingSurface.SetPixel(medWidth, medHeight, Color.Red); }    // If midpoint found is in top half of the screen, color it red
                else
                {
                    if (medWidth < 231) { drawingSurface.SetPixel(medWidth, medHeight, Color.Blue); }  // If midpoint found is in bottom left quadrant, color it blue
                    else { drawingSurface.SetPixel(medWidth, medHeight, Color.Green); }               // If midpoint found is in bottom right quardrant, color it green
                }

                // Set new 'S' height and width to found midpoint's height and width
                sHeight = medHeight;
                sWidth = medWidth;
            }

            // Save bitmap to memoryStream and set imageSource to memoryStream
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();

            MemoryStream stream = new MemoryStream();
            drawingSurface.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;

            bitmap.EndInit();

            // Set window's image source to generated bitmap png
            image1.Source = bitmap;

            // Update # of steps taken
            txtBlk.Text = "Steps = " + steps + ";";

            return;
        }

        private void quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
