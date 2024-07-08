using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace ImageResizer
{
    public class Program
    {
        private static readonly string PATH_IMAGES = @"C:\Users\sivan\source\repos\kenny_city\kenny_city\kenney\calles\set1";
        private static readonly string FILE_ATLAS = PATH_IMAGES + @"\atlas_xxy.png";
        private static List<ImageCollection> _imagesArray = new List<ImageCollection>();


        static void Main(string[] args)
        {
            CreateAtlasPicture();
        }

        static void CreateAtlasPicture()
        {
            DeleteAtlasFiles();

            // Directorio donde se encuentran las imágenes PNG
            string imagesDirectory = PATH_IMAGES;

            // Ruta donde se guardará la imagen combinada
            string outputImagePath = FILE_ATLAS;

            string[] imageFiles = Directory.GetFiles(imagesDirectory, "*.png");

            OrderFiles(imageFiles);
            WriteAtlas();
            
           
            

            Console.WriteLine("End");
        }

        private static void WriteAtlas()
        {
            foreach (var item in _imagesArray)
            {
                (int imageWidth, int imageHeight) = GetImageDimensions(item.ImagesPath[0]);
                (int imagesPerRow, int imagesPerColumn) = FindClosestFactors(item.ImagesPath.Count());



                // Crear la imagen combinada
                using (Bitmap combinedImage = new Bitmap(imageWidth * imagesPerRow, imageHeight * imagesPerColumn))
                {
                    using (Graphics graphics = Graphics.FromImage(combinedImage))
                    {
                        graphics.Clear(Color.Transparent); // Fondo transparente

                        // Obtener todas las rutas de las imágenes PNG en el directorio




                        // Dibujar cada imagen en la posición correcta
                        for (int i = 0; i < item.ImagesPath.Count(); i++)
                        {
                            int row = i / imagesPerRow;
                            int col = i % imagesPerRow;

                            using (Image image = Image.FromFile(item.ImagesPath[i]))
                            {
                                graphics.DrawImage(image, new Rectangle(col * imageWidth, row * imageHeight, imageWidth, imageHeight));
                            }
                        }
                    }

                    // Guardar la imagen combinada
                    combinedImage.Save(item.ImageAtlas, ImageFormat.Png);
                }

            }

        }

        private static void OrderFiles(string[] imageFiles)
        {
            foreach (string imageFile in imageFiles) 
            {
                (int imageWidth, int imageHeight) = GetImageDimensions(imageFile);
                
                ImageCollection ima = new ImageCollection();
                ima.Height = imageHeight;
                ima.Width = imageWidth;
                if (ima.ImagesPath == null)
                    ima.ImagesPath = new List<string>();
                ima.ImagesPath.Add(imageFile);
                ima.ImageAtlas = FILE_ATLAS.Replace("_x", "_" + imageWidth).Replace("xy", "x" + imageHeight);


                // Usar LINQ para buscar rectángulos que coincidan con Width y Height
                var matchingImages = _imagesArray.Where(r => r.Width == imageWidth && r.Height == imageHeight).ToList();

                if (matchingImages == null || matchingImages.Count <= 0)
                {
                    _imagesArray.Add(ima);
                }
                else
                    matchingImages[0].ImagesPath.Add(imageFile);
            }
        }


        static (int, int) FindClosestFactors(int target)
        {
            if (target == 1)
                return (1, 1);

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


        private  static void DeleteAtlasFiles()
        {
            string[] imageFiles = Directory.GetFiles(PATH_IMAGES, "atla*.png");

            foreach (var item in imageFiles)
            {
                File.Delete(item);
            }

        }


        static (int, int) GetImageDimensions(string imagePath)
        {
            using (Image image = Image.FromFile(imagePath))
            {
                return (image.Width, image.Height);
            }
        }


    }


    public class ImageCollection
    {
        public int Width {  get; set; }
        public int Height { get; set; }
        public List<string>? ImagesPath { get; set; }
        public string? ImageAtlas {  get; set; }
    }




}
