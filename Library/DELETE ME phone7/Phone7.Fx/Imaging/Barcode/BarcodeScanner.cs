using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;
using System.Linq;
using Microsoft.Phone.Reactive;

namespace Phone7.Fx.Imaging.Barcode
{
    public class BarcodeScanner
    {
        private CameraCaptureTask _task;
        //private PhotoChooserTask _task;

        public event EventHandler<BarcodeEventArgs> ScanCompleted;

        public BarcodeScanner()
        {
            _task = new CameraCaptureTask();
            //_task = new PhotoChooserTask();
            _task.Completed += task_Completed;
        }

        public void TakePictureAndScan()
        {
            _task.Show();
        }

        void task_Completed(object sender, PhotoResult e)
        {
            if (ScanCompleted != null && e.TaskResult == TaskResult.OK)
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(e.ChosenPhoto);

                WriteableBitmap bitmap = new WriteableBitmap(image);

                var results = FullScanBitmap(bitmap, 100);

                ScanCompleted(this, new BarcodeEventArgs { Found = results });
            }
        }

        public List<string> FullScanBitmap(WriteableBitmap bmp, int numscans)
        {
            List<string> results = new List<string>();

            DateTime t1 = DateTime.Now;

            var o = Observable.ForkJoin(
                Observable.Start(() => ScanBitmap(bmp, numscans, ScanDirection.Vertical, BarcodeType.All)),
                Observable.Start(() => ScanBitmap(bmp, numscans, ScanDirection.Horizontal, BarcodeType.All)));


            //ScanBitmap(bmp, numscans, ScanDirection.Vertical, BarcodeType.All);
            //ScanBitmap(bmp, numscans, ScanDirection.Horizontal, BarcodeType.All);
            


            foreach (var res  in o.First())
                results.AddRange(res);

            return results.Distinct().ToList();
        }

        public List<string> ScanBitmap(WriteableBitmap bmp, int numscans, ScanDirection direction, BarcodeType types)
        {
            List<string> result = new List<string>();

            Parser parser = new Parser();

            int height = direction == ScanDirection.Horizontal ? bmp.PixelWidth : bmp.PixelHeight;

            if (numscans > height)
                numscans = height; // fix for doing full scan on small images
            for (int i = 0; i < numscans; i++)
            {
                int y1 = (i * height) / numscans;
                int y2 = ((i + 1) * height) / numscans;

                string codesRead = parser.ReadBarcodes(bmp, y1, y2, direction, types);

                if (!string.IsNullOrEmpty(codesRead))
                {
                    result.AddRange(codesRead.Split('|'));
                }
            }
            return result;
        }
    }
}