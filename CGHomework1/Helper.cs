using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using static CGHomework1.MainWindow;

namespace CGHomework1
{
    class Helper
    {
        
        public BitmapSource MyLittleHelp(BitmapSource source, int[] functionFig)
        {
            // Calculate stride of source
            int stride = (source.PixelWidth * source.Format.BitsPerPixel + 7) / 8;

            // Create data array to hold source pixel data
            int length = stride * source.PixelHeight;
            byte[] data = new byte[length];

            // Copy source image pixels to the data array
            source.CopyPixels(data, stride, 0);

            // Loop for inversion(alpha is skipped)
            for (int i = 0; i < length; i += 4)
            {
                data[i] = (byte)functionFig[data[i]];
                data[i+1] = (byte)functionFig[data[i+1]];
                data[i+2] = (byte)functionFig[data[i+2]];
                
            }

            // Create a new BitmapSource from the inverted pixel buffer
            return BitmapSource.Create(
                source.PixelWidth, source.PixelHeight,
                source.DpiX, source.DpiY, source.Format,
                null, data, stride);
        }

        // Function Filters' Helper methods

        public  BitmapSource Invert(BitmapSource source, int[] functionFig, ViewModel myViewModel )
        {
            // Calculate stride of source
            int stride = (source.PixelWidth * source.Format.BitsPerPixel + 7) / 8;

            // Create data array to hold source pixel data
            int length = stride * source.PixelHeight;
            byte[] data = new byte[length];

            // Copy source image pixels to the data array
            source.CopyPixels(data, stride, 0);

           
            // Loop for inversion(alpha is skipped)
            for (int i = 0; i < length; i+=4)
            {
                data[i] = (byte)(255 - data[i]); //R & G & B
                data[i+1] = (byte)(255 - data[i+1]);
                data[i+2] = (byte)(255 - data[i+2]); 
            }

            // Loop for inversion(alpha is skipped)
            for (int i = 0; i < 256; i++)
            {
               
                functionFig[i] = (255 - functionFig[i]); 
            }
            // Loop for inversion(alpha is skipped)
            for (int i = 0; i < myViewModel.Vertices.Count; i++)
            {
                myViewModel.Vertices[i].Point = new  Point(myViewModel.Vertices[i].Point.X, 255 - myViewModel.Vertices[i].Point.Y+44);
            }
            
            // Create a new BitmapSource from the inverted pixel buffer
            return BitmapSource.Create(
                source.PixelWidth, source.PixelHeight,
                source.DpiX, source.DpiY, source.Format,
                null, data, stride);
        }
        public BitmapSource ChangeBrightness(BitmapSource source, int change_value, int[] functionFig, ViewModel myViewModel)
        {
            // Calculate stride of source
            int stride = (source.PixelWidth * source.Format.BitsPerPixel + 7) / 8;

            // Create data array to hold source pixel data
            int length = stride * source.PixelHeight;
            byte[] data = new byte[length];

            // Copy source image pixels to the data array
            source.CopyPixels(data, stride, 0);

            
            // Loop for inversion(alpha is skipped)
            for (int i = 0; i < length; i+=4)
            {
                // R
                if ((int)data[i] + change_value > 255)
                    data[i] = (byte)255;
                else if ((int)data[i] + change_value < 0)
                    data[i] = (byte)0;
                else 
                    data[i] = (byte)(data[i] + change_value);
                // G
                if ((int)data[i+1] + change_value > 255)
                    data[i+1] = (byte)255;
                else if ((int)data[i+1] + change_value < 0)
                    data[i+1] = (byte)0;
                else
                    data[i+1] = (byte)(data[i+1] + change_value);
                // B 
                if ((int)data[i+2] + change_value > 255)
                    data[i+2] = (byte)255;
                else if ((int)data[i+2] + change_value < 0)
                    data[i+2] = (byte)0;
                else
                    data[i+2] = (byte)(data[i+2] + change_value);




            }
            for (int i = 0; i < 256; i++)
            {
                
                if (functionFig[i] + change_value > 255)
                    functionFig[i] = 255;
                else
                    functionFig[i] = (functionFig[i] + change_value);
               

            }
            for (int i = 0; i < myViewModel.Vertices.Count; i++)
            {
                if (myViewModel.Vertices[i].Point.Y - change_value < 22)
                    myViewModel.Vertices[i].Point = new Point(myViewModel.Vertices[i].Point.X, 22);
                else if (myViewModel.Vertices[i].Point.Y - change_value > 277)
                    myViewModel.Vertices[i].Point = new Point(myViewModel.Vertices[i].Point.X, 277);
                else
                    myViewModel.Vertices[i].Point = new Point(myViewModel.Vertices[i].Point.X, myViewModel.Vertices[i].Point.Y - change_value);

               // myViewModel.Vertices[i].Point = new Point(myViewModel.Vertices[i].Point.X,myViewModel.Vertices[i].Point.Y - change_value);

            }
            // Create a new BitmapSource from the inverted pixel buffer
            return BitmapSource.Create(
                source.PixelWidth, source.PixelHeight,
                source.DpiX, source.DpiY, source.Format,
                null, data, stride);
        }
        public BitmapSource ChangeContrast(BitmapSource source, int[] functionFig, ViewModel myViewModel)
        {
            // Calculate stride of source
            int stride = (source.PixelWidth * source.Format.BitsPerPixel + 7) / 8;

            // Create data array to hold source pixel data
            int length = stride * source.PixelHeight;
            byte[] data = new byte[length];

            // Copy source image pixels to the data array
            source.CopyPixels(data, stride, 0);

            int threshold = 50;
            double contrastLevel = Math.Pow((100.0 + threshold) / 100.0, 2);
            // Loop for contrast
            for (int i = 0; i < length; i+=4)
            {
                var tmpByte_R = ((((data[i] / 255.0) - 0.5)
                         * contrastLevel) + 0.5) * 255.0;

                if (tmpByte_R > 255)
                {
                    tmpByte_R = 255;
                }
                else if (tmpByte_R < 0)
                {
                    tmpByte_R = 0;
                }
                data[i] = (byte)tmpByte_R;

                var tmpByte_G = ((((data[i+1] / 255.0) - 0.5)
                        * contrastLevel) + 0.5) * 255.0;

                if (tmpByte_G > 255)
                {
                    tmpByte_G = 255;
                }
                else if (tmpByte_G < 0)
                {
                    tmpByte_G = 0;
                }
                data[i+1] = (byte)tmpByte_G;

                var tmpByte_B = ((((data[i+2] / 255.0) - 0.5)
                        * contrastLevel) + 0.5) * 255.0;

                if (tmpByte_B > 255)
                {
                    tmpByte_B = 255;
                }
                else if (tmpByte_B < 0)
                {
                    tmpByte_B = 0;
                }
                data[i+2] = (byte)tmpByte_B;

            }
            for(int i =0; i < 256; i++)
            {
                var tmpByte_F = ((((functionFig[i] / 255.0) - 0.5)
                         * contrastLevel) + 0.5) * 255.0;

                if (tmpByte_F > 255)
                {
                    tmpByte_F = 255;
                }
                else if (tmpByte_F < 0)
                {
                    tmpByte_F = 0;
                }
                functionFig[i] = (int)tmpByte_F;
                
            }
            for(int i =0; i< myViewModel.Vertices.Count;i++)
            {
                var tmpByte_VM= ((((Math.Abs(myViewModel.Vertices[i].Point.Y-277) / 255.0) - 0.5)
                        * contrastLevel) + 0.5) * 255.0;

                if (tmpByte_VM > 255)
                {
                    tmpByte_VM = 255;
                }
                else if (tmpByte_VM < 0)
                {
                    tmpByte_VM = 0;
                }
                myViewModel.Vertices[i].Point = new Point(myViewModel.Vertices[i].Point.X, 277-tmpByte_VM );
            }
            // Create a new BitmapSource from the inverted pixel buffer
            return BitmapSource.Create(
                source.PixelWidth, source.PixelHeight,
                source.DpiX, source.DpiY, source.Format,
                null, data, stride);
        }
        public BitmapSource ChangeGamma(BitmapSource source, int[] functionFig, ViewModel myViewModel)
        {
            // Calculate stride of source
            int stride = (source.PixelWidth * source.Format.BitsPerPixel + 7) / 8;

            // Create data array to hold source pixel data
            int length = stride * source.PixelHeight;
            byte[] data = new byte[length];

            // Copy source image pixels to the data array
            source.CopyPixels(data, stride, 0);

            int constantC = 1;
            double gamma = 2;
           
            // Loop for contrast
            for (int i = 0; i < length; i+=4)
            {
                double range = (double)data[i] / 255;
                double correction = constantC * Math.Pow(range, 1/gamma);
                data[i] = (byte)(correction * 255);

                range = (double)data[i + 1] / 255;
                correction = constantC * Math.Pow(range, 1/gamma);
                data[i + 1] = (byte)(correction * 255);

                range = (double)data[i + 2] / 255;
                correction = constantC * Math.Pow(range, 1/gamma);
                data[i + 2] = (byte)(correction * 255);

            }
            for(int i =0; i< 256;i++)
            {
                double range = (double)functionFig[i] / 255;
                double correction = constantC * Math.Pow(range, 1/gamma);
                functionFig[i] =(int)(correction * 255);
            }
            for (int i = 0;i < myViewModel.Vertices.Count; i++)
            {
                double range = (double)Math.Abs(myViewModel.Vertices[i].Point.Y-277)/ 255;
                double correction = constantC * Math.Pow(range, 1/gamma);
                myViewModel.Vertices[i].Point = new Point(myViewModel.Vertices[i].Point.X,277-(int)(correction * 255));
            }
            // Create a new BitmapSource from the inverted pixel buffer
            return BitmapSource.Create(
                source.PixelWidth, source.PixelHeight,
                source.DpiX, source.DpiY, source.Format,
                null, data, stride);
        }
        public static double TestMethod(int[,] matrix)
        {
            double sum = 0;
            for (int column = 0; column < 3; column++)
                for (int row = 0; row < 3; row++)
                    sum += matrix[row,column];
            return sum;
        }
        //Convolutional
        public BitmapSource ChangeBlur(BitmapSource source, int[,] kernelMatrix, int bias)
        {
            // Calculate stride of source
            int stride = (source.PixelWidth * source.Format.BitsPerPixel + 7) / 8;

            // Create data array to hold source pixel data
            int length = stride * source.PixelHeight;
            byte[] data = new byte[length];
            byte[] resultData = new byte[length];
            // Copy source image pixels to the data array
            source.CopyPixels(data, stride, 0);

            double coeff = TestMethod(kernelMatrix);
            if (coeff <= 0)
                coeff = 1;
            // Loop for contrast
            for (int i = 0; i < length; i+=4)
            {
                double kernel_sum_R ;
                double kernel_sum_G;
                double kernel_sum_B;

                
                // Upper side only
                if (i >= 0 && i <= stride-4)
                {

                    resultData[i] = data[i];
                    resultData[i + 1] = data[i + 1];
                    resultData[i + 2] = data[i + 2];
                    resultData[i + 3] = data[i + 3];
                }
                // Left side only 
                else if (i % (stride) == 0 && i != (source.PixelHeight - 1) * stride)
                {

                    resultData[i] = data[i];
                    resultData[i + 1] = data[i + 1];
                    resultData[i + 2] = data[i + 2];
                    resultData[i + 3] = data[i + 3];

                }
                // Right side only 
                else if ((i+4) % stride == 0 && i != length - 4)
                {


                    resultData[i] = data[i];
                    resultData[i + 1] = data[i + 1];
                    resultData[i + 2] = data[i + 2];
                    resultData[i + 3] = data[i + 3];
                }
                // Down side 
                else if (i >= (source.PixelHeight - 1) * stride && i <= length - 4)
                {

                    resultData[i] = data[i];
                    resultData[i + 1] = data[i + 1];
                    resultData[i + 2] = data[i + 2];
                    resultData[i + 3] = data[i + 3];
                }
                // Middle part
                else
                {
                    // R, G and B values are averaged separately
                    

                    kernel_sum_R = (data[i - 4 - stride]) * kernelMatrix[0, 0] + // Top left
                         (data[i - stride ] ) * kernelMatrix[0, 1] + // Top center
                         (data[i + 4 - stride]) * kernelMatrix[0, 2] + // Top right
                         (data[i - 4 ] ) * kernelMatrix[1, 0] + // Mid left
                         (data[i ] ) * kernelMatrix[1, 1] + // Current pixel
                        ( data[i + 4 ] ) * kernelMatrix[1, 2] + // Mid right
                         (data[i - 4 + stride ] ) * kernelMatrix[2, 0] + // Low left
                         (data[i + stride ]) * kernelMatrix[2, 1] + // Low center
                         (data[i + 4 + stride ] ) * kernelMatrix[2, 2];  // Low right


                    double res = kernel_sum_R / coeff + bias;
                    if (res <= 0)
                        res = 0;
                    else if (res >= 255)
                        res = 255;
                    resultData[i]= (byte)res;
                   // resultData[i] = (byte)Math.Min(Math.Max((int)(byte)(kernel_sum_R / coeff+bias), 0), 255);

                    kernel_sum_G = (data[i - 4 - stride+1]) * kernelMatrix[0, 0] + // Top left
                         ( data[i - stride+1] ) * kernelMatrix[0, 1]+ // Top center
                          (data[i + 4 - stride+1]) * kernelMatrix[0, 2] + // Top right
                          (data[i - 4+1] ) * kernelMatrix[1, 0] + // Mid left
                          (data[i +1] ) * kernelMatrix[1, 1] + // Current pixel
                         ( data[i + 4+1] ) * kernelMatrix[1, 2] + // Mid right
                         (data[i - 4 + stride+1])* kernelMatrix[2, 0] + // Low left
                         ( data[i + stride+1] ) * kernelMatrix[2, 1] + // Low center
                         ( data[i + 4 + stride+1] ) * kernelMatrix[2, 2] ;  // Low right

                    double res_G = kernel_sum_G/ coeff + bias;
                    if (res_G <= 0)
                        res_G= 0;
                    else if (res_G >= 255)
                        res_G = 255;
                    resultData[i+1] = (byte)res_G;
                    //resultData[i + 1] = (byte)Math.Min(Math.Max((int)(kernel_sum_G/ coeff+bias), 0), 255);

                    kernel_sum_B = (data[i - 4 - stride+2]) * kernelMatrix[0,0] + // Top left
                        ( data[i - stride+2] ) * kernelMatrix[0,1 ] + // Top center
                         (data[i + 4 - stride+2]) * kernelMatrix[0, 2] + // Top right
                         (data[i - 4+2]) * kernelMatrix[1, 0] + // Mid left
                        ( data[i +2])  * kernelMatrix[1, 1] + // Current pixel
                        ( data[i + 4+2] ) * kernelMatrix[1, 2] + // Mid right
                        ( data[i - 4 + stride+2] ) * kernelMatrix[2, 0] + // Low left
                        ( data[i + stride+2] ) * kernelMatrix[2, 1] + // Low center
                        ( data[i + 4 + stride+2] ) * kernelMatrix[2, 2] ;  // Low right

                   double res_B = kernel_sum_B / coeff + bias;
                    if (res_B <= 0)
                        res_B = 0;
                    else if (res_B >= 255)
                        res_B = 255;
                    resultData[i+2] = (byte)res_B;
                   // resultData[i+2] = (byte)Math.Min(Math.Max((int)(kernel_sum_B / coeff+bias), 0), 255);

                    resultData[i+3] = data[i+3];

                }
               
 
            }

            // Create a new BitmapSource from the inverted pixel buffer
            return BitmapSource.Create(
                source.PixelWidth, source.PixelHeight,
                source.DpiX, source.DpiY, source.Format,
                null, resultData, stride);
        }

    }
}
