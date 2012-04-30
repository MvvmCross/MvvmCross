Description
°°°°°°°°°°°

The WriteableBitmapEx library is a collection of extension methods for Silverlight's WriteableBitmap. The WriteableBitmap class that was added in Silverlight 3 and is also available on Windows Phone, allows the direct manipulation of a bitmap and could be used to generate fast procedural images by drawing directly to a bitmap. The WriteableBitmap API is very minimalistic and there's only the raw Pixels array for such operations. The WriteableBitmapEx library tries to compensate that with extensions methods that are easy to use like built in methods and offer GDI+ like functionality. The library extends the WriteableBitmap class with elementary and fast (2D drawing) functionality, conversion methods and functions to combine (blit) WriteableBitmaps. 
The extension methods are grouped into different CS files with a partial class. It is possible to include just a few methods by using the specific source CS files directly or all extension methods through the built library assembly.

See http://writeablebitmapex.codeplex.com


Project structure
°°°°°°°°°°°°°°°°°

It is possible to use the built assembly that contains all extension methods or just specific methods by using the CS files directly. The extension methods are grouped into multiple CS files.

Solution: The Visual Studio solutions for the library itself and the samples.
Source:   The Visual Studio projects for the samples and the library (WriteableBitmapEx) with the WriteableBitmap*Extensions.cs files.


License
°°°°°°°

The library is released under the Microsoft Public License (Ms-PL). Please read the License.txt for details.