using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer
{

    internal class CreateAtlasVer1
    {

        private static readonly string PATH_IMAGES = @"C:\Users\sivan\source\repos\kenny_city\kenny_city\kenney\calles\set1";
        private static readonly string FILE_ATLAS = @"C:\Users\sivan\source\repos\kenny_city\kenny_city\kenney\calles\set1\atlas.png";


        static void Process(string[] args)
        {
            CreateAtlasPicture();
        }


        static void CreateAtlasPicture()
        {

            if (File.Exists(FILE_ATLAS))
                File.Delete(FILE_ATLAS);


            // Directorio donde se encuentran las imágenes PNG
            string imagesDirectory = PATH_IMAGES;

            // Ruta donde se guardará la imagen combinada
            string outputImagePath = FILE_ATLAS;

            string[] imageFiles = Directory.GetFiles(imagesDirectory, "*.png");

            // Tamaño de las imágenes individuales 
            (int imageWidth, int imageHeight) = GetImageDimensions(imageFiles[0]);




            (int imagesPerRow, int imagesPerColumn) = FindClosestFactors(imageFiles.Count());


            // Crear la imagen combinada
            using (Bitmap combinedImage = new Bitmap(imageWidth * imagesPerRow, imageHeight * imagesPerColumn))
            {
                using (Graphics graphics = Graphics.FromImage(combinedImage))
                {
                    graphics.Clear(Color.Transparent); // Fondo transparente

                    // Obtener todas las rutas de las imágenes PNG en el directorio


                    if (imageFiles.Length < imageFiles.Count())
                    {
                        Console.WriteLine("No hay suficientes imágenes en el directorio.");
                        return;
                    }

                    // Dibujar cada imagen en la posición correcta
                    for (int i = 0; i < imageFiles.Count(); i++)
                    {
                        int row = i / imagesPerRow;
                        int col = i % imagesPerRow;

                        (int imageWidthTmp, int imageHeightTmp) = GetImageDimensions(imageFiles[i]);
                        if (imageHeight != imageHeightTmp || imageWidth != imageWidthTmp)
                        {
                            //throw new Exception("Las imagenes no son coincidentes: " + imageFiles[i]);
                            if (imageWidthTmp == 133)
                                Console.WriteLine(imageFiles[i] + " - size: {0}x{1}", imageWidthTmp, imageHeightTmp);

                        }

                        using (Image image = Image.FromFile(imageFiles[i]))
                        {
                            graphics.DrawImage(image, new Rectangle(col * imageWidth, row * imageHeight, imageWidth, imageHeight));
                        }
                    }
                }

                // Guardar la imagen combinada
                combinedImage.Save(outputImagePath, ImageFormat.Png);
            }
        }


        static (int, int) GetImageDimensions(string imagePath)
        {
            using (Image image = Image.FromFile(imagePath))
            {
                return (image.Width, image.Height);
            }
        }




        static (int, int) FindClosestFactors(int target)
        {
            int n = target;
            var pairs = Enumerable.Range((int)Math.Sqrt(n), n - (int)Math.Sqrt(n) + 1)
                             .Where(a => n % a == 0)
                                .Select(a => (a, n / a, Math.Abs(n / a - a)));

            if (pairs.ToArray()[0].Item2 == 1)
            {
                int mid = target / 2;
                return (mid + 1, 2);
            }


            return (pairs.ToArray()[0].a, pairs.ToArray()[0].Item2);
        }



        public static void OpenWithDefaultProgram(string path)
        {
            using Process fileopener = new Process();

            fileopener.StartInfo.FileName = "explorer";
            fileopener.StartInfo.Arguments = "\"" + path + "\"";
            fileopener.Start();
        }



    }

}
