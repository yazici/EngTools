using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pamux.GameDev.UserControls
{
    public class BitmapManipulationContext
    {
        public int Height = 1024;
        public int Width = 1024;
        public PixelFormat PixelFormat = PixelFormats.Gray8;


        public int BytesPerPixel => (PixelFormat.BitsPerPixel + 7) / 8;

        public int Stride => (Width * PixelFormat.BitsPerPixel + 7) / 8;
        public int BufferSize => Height * Stride;
        public byte[] Pixels;
        private Random random;

        public BitmapManipulationContext(int seed = 0)
        {
            Pixels = new byte[BufferSize];
            if (seed == 0)
            {
                random = new Random();
            }
            else
            {
                random = new Random(seed);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetXY(int x, int y, byte value)
        {
            Pixels[y * Stride + x * BytesPerPixel] = value;
        }

        internal void PerlinNoise()
        {
            var perlin = new PerlinNoise(1);


            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    //SetXY(x, y, (byte) perlin.Noise(x,y,0));
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetXY(int x, int y)
        {
            return Pixels[y * Stride + x * BytesPerPixel];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetXYRandom(int x, int y)
        {
            Pixels[y * Stride + x * BytesPerPixel] = RandomByte;
        }

        public byte RandomByte => (byte)(random.Next() % 256);


        public void FillRandomly()
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    SetXYRandom(x, y);
                }
            }
        }

        public void FillRandomly(int blockWidth, int blockHeight)
        {
            byte value = 0;

            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    if (x % blockWidth == 0)
                    {
                        if (y % blockHeight == 0)
                        { 
                            value = RandomByte;
                        }
                        else
                        {
                            value = GetXY(x, y - 1);
                        }
                    }
                    SetXY(x, y, value);
                }
            }
        }

        public void FillRandomGrid(int resolutionX, int resolutionY, int h, int w)
        {
            int stepY = Height / resolutionY;
            int stepX = Width / resolutionX;

            for (int y = 1; y < resolutionY; ++y)
            {
                for (int x = 1; x < resolutionX;  ++x)
                {
                    SetXYSquare(x * stepX, y * stepY, h, w, RandomByte);
                }
            }
        }

        private void SetXYSquare(int cx, int cy, int h, int w, byte value)
        {
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    SetXY(cx + x, cy + y, value);
                }
            }
        }

        public BitmapSource Create()
        {
            return BitmapSource.Create(Width, Height, 96, 96, PixelFormats.Gray8, null, Pixels, Stride);
        }
    }
}
