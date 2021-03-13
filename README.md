# BasicFilters #
 WPF desktop app for applying functional, convulational and user-defined filters.
 
## Goal ## 
The goal of this project is to implement application with graphical user interface
allowing image filtering using functional and convolution filters.
Addtionally graphical user interface should also allow adding, modifying and editing functional filters.

## Basic Functionality ##
<ul>
 <li>loading of a selected image file and displaying it in the application window</li>
 <li>applying selected filters to the loaded image and displaying result beside or in place of original image(displaying both images is not required, but 
recommended)</li>
 <li>combining multiple filters on top of each other</li>
 <li>saving result image to a file</li>
 <li>returning filtered image back to its original state without reloading the
file (optional, but recommended</li>
 <li>implementation of following function filters - with fixed parameters easily
modifiable from the source code:
 <ul>
   <li>inversion</li>
  <li>brightness correction</li>
  <li>contrast enhancement</li>
  <li>gamma correction</li>
 </ul>
 </li>
 <li>implementation of following convolution filters - with fixed coefficients and
3x3 kernel size, with both size and coefficients easily modifiable from the
source code:
  <ul>
     <li> blur</li>
     <li>Gaussian blur</li>
     <li>sharpen</li>
     <li>edge detection (one, selected variant)</li>
     <li>emboss (one, selected variant)</li>
 </ul>
 </li>
</ul>

## Editing functional filters ##
<ul>
 <li>separate area with size 256x256 pixels for displaying and editing functional 
 filters</li>
 <li>displaying function graph using polylines</li>
 <li>creating new functional filters (starting as identity filter, which is a straight <br/>
 line from lower left corner to upper right corner)</li>
 <li>adding, moving and deleting points of a graph polyline </li>
 <li>leftmost and rightmost points cannot be removed and can only move up
 or down</li>
 <li>polyline represents valid function (for each color value input from 0 to 255 
 there is only one output value)</li>
 <li>editing existing functional filters (also predefined, specified above, except
 for gamma correction)</li>
 <li>saving created or modified filters in an application and applying them to
 the image</li>
</ul>

## Remarks
Image file loading and displaying may be handled by an external library, but
implementation of the required filters must be done using only operations on
single pixels.


