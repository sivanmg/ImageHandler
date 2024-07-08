using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ImageResizer
{
    internal class Resizing
    {



        static void ResizingBatch(string[] args)
        {
            string inputPath = "";
            string outputPath = "";



            string rootPath = @"C:\Users\sivan\Escritorio\kenney\tmp\";
            string resizedPath = @"C:\Users\sivan\Escritorio\kenney\resized\";
            List<string> pngFiles = GetAllPngFiles(rootPath);



            // Cantidad de píxeles a mover hacia arriba
            int pixelsToMove = 6;




            foreach (string file in pngFiles)
            {
                inputPath = file;
                if (!new DirectoryInfo(resizedPath).Exists)
                    Directory.CreateDirectory(resizedPath);

                outputPath = resizedPath + new FileInfo(inputPath).Name;
                using (var originalImage = new Bitmap(inputPath))
                {
                    int newWidth = GetNearestMultipleOfEight(originalImage.Width);
                    int newHeight = GetNearestMultipleOfEight(originalImage.Height);

                    using (var resizedImage = ResizeImage(originalImage, newWidth, newHeight))
                    {
                        resizedImage.Save(outputPath, ImageFormat.Png);
                        // Llama al método para mover la imagen hacia arriba

                    }
                }


                foreach (var item in moveUp)
                {
                    if (outputPath.Contains(item))
                    {
                        string up_outputPath = outputPath.Replace(".png", "-up.png");
                        MoveImageUp(outputPath, up_outputPath, pixelsToMove);
                        new FileInfo(outputPath).Delete();
                    }
                }




                Console.WriteLine("Imagen redimensionada guardada en: " + outputPath);


            }

        }
        static int GetNearestMultipleOfEight(int value)
        {
            return value;
            //return (int)Math.Round(value / 32.0) * 32;
        }

        static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }




        static List<string> GetAllPngFiles(string rootPath)
        {
            // Usa Directory.GetFiles para obtener todos los archivos .png en la carpeta y subcarpetas
            List<string> files = new List<string>();
            try
            {
                files.AddRange(Directory.GetFiles(rootPath, "*.png", SearchOption.AllDirectories));
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"Acceso no autorizado a la carpeta: {e.Message}");
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine($"Directorio no encontrado: {e.Message}");
            }

            return files;
        }



        static void MoveImageUp(string originalImagePath, string modifiedImagePath, int pixelsToMove)
        {
            using (Image originalImage = Image.FromFile(originalImagePath))
            {
                int width = originalImage.Width;
                int height = originalImage.Height;

                Bitmap modifiedImage = new Bitmap(width, height);

                using (Graphics graphics = Graphics.FromImage(modifiedImage))
                {
                    // Establecer color de fondo transparente
                    graphics.Clear(Color.Transparent);

                    // Dibujar la imagen original desplazada hacia arriba
                    graphics.DrawImage(originalImage, new Rectangle(0, -pixelsToMove, width, height));

                    // Guardar la imagen modificada
                    modifiedImage.Save(modifiedImagePath, ImageFormat.Png);
                }
            }
        }

        static string[] moveUp = { "buildingTiles_106.png",
"buildingTiles_107.png",
"buildingTiles_108.png",
"buildingTiles_109.png",
"buildingTiles_113.png",
"buildingTiles_114.png",
"buildingTiles_115.png",
"buildingTiles_116.png",
"buildingTiles_117.png",
"buildingTiles_122.png",
"buildingTiles_123.png",
"buildingTiles_124.png",
"buildingTiles_125.png",
"buildingTiles_001.png",
"buildingTiles_002.png",
"buildingTiles_003.png",
"buildingTiles_004.png",
"buildingTiles_009.png",
"buildingTiles_010.png",
"buildingTiles_011.png",
"buildingTiles_012.png",
"buildingTiles_014.png",
"buildingTiles_017.png",
"buildingTiles_018.png",
"buildingTiles_019.png",
"buildingTiles_020.png",
"buildingTiles_021.png",
"buildingTiles_022.png",
"buildingTiles_025.png",
"buildingTiles_026.png",
"buildingTiles_027.png",
"buildingTiles_028.png",
"buildingTiles_029.png",
"buildingTiles_030.png",
"buildingTiles_033.png",
"buildingTiles_034.png",
"buildingTiles_035.png",
"buildingTiles_036.png",
"buildingTiles_037.png",
"buildingTiles_040.png",
"buildingTiles_041.png",
"buildingTiles_042.png",
"buildingTiles_046.png",
"buildingTiles_092.png",
"buildingTiles_093.png",
"buildingTiles_099.png",
"buildingTiles_100.png",
"buildingTiles_101.png"};


    }
}
