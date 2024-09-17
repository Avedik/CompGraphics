using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FastBmap
{
    public unsafe class FastBitmap : IDisposable
    {
        public readonly int Width;

        public readonly int Height;

        public int Count => Height * Width;

        /// <summary>
        /// Bitmap, �� �������� ��� ������ ���� ��������� FastBitmap.
        /// ��������, ����� ����� ����� ���� ��������������.
        /// </summary>
        private readonly Bitmap _source;

        /// <summary>
        /// ������ � Bitmap.
        /// ��������, ����� ����� ����� ���� ��������������.
        /// </summary>
        private readonly BitmapData _bitmapData;

        /// <summary>
        /// ���������� ����, ��������� �� ���� ������� � ������� �����������.
        /// </summary>
        private readonly int _bytesPerPixel;

        /// <summary>
        /// ���������� ����, ��������� �� ���� ������ � ������� �����������.
        /// </summary>
        private readonly int _stride;

        /// <summary>
        /// ��������� �� ������ ��������� ������, ������� ������������ ��������.
        /// </summary>
        private readonly byte* _scan0;

        /// <summary>
        /// PixelFormat, � �������� �� ��������� ����� Bitmap.
        /// </summary>
        private const PixelFormat TargetPixelFormat = PixelFormat.Format32bppArgb;

        public FastBitmap(Bitmap bitmap)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;
            _source = bitmap;
            // ��������� ������ � Bitmap � ����������� ������
            _bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite,
                TargetPixelFormat
            );
            _stride = _bitmapData.Stride;
            _bytesPerPixel = Image.GetPixelFormatSize(TargetPixelFormat) / 8;
            _scan0 = (byte*)_bitmapData.Scan0.ToPointer();
        }

        /// <summary>
        /// ���������� ��������� �� ������������ ������� � ������
        /// </summary>
        /// <param name="point">���������� �������</param>
        private byte* PixelOffset(Point point)
            =>
                _scan0 // ��������� �� ������ ������ ������ Bitmap
                + point.Y * _stride // ����� Y-��� ������
                + point.X * _bytesPerPixel; // ����� X-���� �������

        public void SetPixel(Point point, Color color)
        {
            var data = PixelOffset(point);
            data[Channel.A] = color.A;
            data[Channel.R] = color.R;
            data[Channel.G] = color.G;
            data[Channel.B] = color.B;
        }

        public Color GetPixel(Point point)
        {
            var data = PixelOffset(point);
            return Color.FromArgb(
                data[Channel.A],
                data[Channel.R],
                data[Channel.G],
                data[Channel.B]
            );
        }

        public Color this[int x, int y]
        {
            get => GetPixel(new Point(x, y));
            set => SetPixel(new Point(x, y), value);
        }

        /// <summary>
        /// ���������� ������ Bitmap ������� � �����������.
        /// ��� ������������� using ���������� �������������.
        /// </summary>
        public void Dispose()
        {
            _source.UnlockBits(_bitmapData);
        }

        private static class Channel
        {
            public const int A = 3;
            public const int R = 2;
            public const int G = 1;
            public const int B = 0;
        }
    }

    public static class FastBitmapTools
    {
        /// <summary>
        /// ��������� �����-�� �������� � ������ �������� ��������� Bitmap.
        /// </summary>
        /// <param name="source">Bitmap, ������� �������� ���������.</param>
        /// <param name="action">��������, ����������� ��� ���������.</param>
        public static void ForEach(this Bitmap source, Action<Color> action)
        {
            using (var fastSource = new FastBitmap(source))
                fastSource.ForEach(action);
        }

        /// <summary>
        /// ��������� �����-�� �������� � ������ �������� ��������� FastBitmap.
        /// </summary>
        /// <param name="source">Bitmap, ������� �������� ���������.</param>
        /// <param name="action">��������, ����������� ��� ���������.</param>
        public static void ForEach(this FastBitmap source, Action<Color> action)
        {
            for (var y = 0; y < source.Height; y += 1)
                for (var x = 0; x < source.Width; x += 1)
                    action(source[x, y]);
        }

        /// <summary>
        /// ���������� ����� Bitmap, ��� ������� ������� �������� ��������������� ��������
        /// ��������� Bitmap ����������� �������.
        /// </summary>
        /// <param name="source">�������� Bitmap.</param>
        /// <param name="transform">��������������, ����������� � �������� ��������� �������.</param>
        public static Bitmap Select(this Bitmap source, Func<Color, Color> transform)
        {
            using (var fastSource = new FastBitmap(source))
                return fastSource.Select(transform);
        }

        /// <summary>
        /// ���������� ����� Bitmap, ��� ������� ������� �������� ��������������� ��������
        /// ��������� FastBitmap ����������� �������.
        /// </summary>
        /// <param name="source">�������� Bitmap.</param>
        /// <param name="transform">��������������, ����������� � �������� ��������� �������.</param>
        public static Bitmap Select(this FastBitmap source, Func<Color, Color> transform)
        {
            var result = new Bitmap(source.Width, source.Height);

            using (var fastResult = new FastBitmap(result))
            {
                for (var y = 0; y < fastResult.Height; y += 1)
                    for (var x = 0; x < fastResult.Width; x += 1)
                        fastResult[x, y] = transform(source[x, y]);
            }

            return result;
        }
    }
}