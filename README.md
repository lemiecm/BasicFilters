# BasicFilters
 WPF desktop app for applying functional, convulational and user-defined filters.
 
 ## Goal
The goal of this project is to implement application with graphical user interface
allowing image filtering using functional and convolution filters.
Addtionally graphical user interface should also allow adding, modifying and editing functional filters.

## Basic Functionality
• loading of a selected image file and displaying it in the application window, <br/>
• applying selected filters to the loaded image and displaying result beside <br/>
or in place of original image (displaying both images is not required, but 
recommended),<br/>
• combining multiple filters on top of each other,<br/>
• saving result image to a file,<br/>
• returning filtered image back to its original state without reloading the
file (optional, but recommended),<br/>
• implementation of following function filters - with fixed parameters easily
modifiable from the source code:<br/>
<ul>
– inversion,<br/>
– brightness correction,<br/>
– contrast enhancement,<br/>
– gamma correction.<br/>
 </ul>
• implementation of following convolution filters - with fixed coefficients and
3x3 kernel size, with both size and coefficients easily modifiable from the
source code:<br/>
<ul>
– blur,<br/>
– Gaussian blur (Gaussian smoothing),<br/>
– sharpen,<br/>
– edge detection (one, selected variant),<br/>
– emboss (one, selected variant).<br/> 
</ul>
## Editing functional filters
• separate area with size 256x256 pixels for displaying and editing functional 
filters, <br/>
• displaying function graph using polylines, <br/>
• creating new functional filters (starting as identity filter, which is a straight <br/>
line from lower left corner to upper right corner). <br/>
• adding, moving and deleting points of a graph polyline <br/>
• leftmost and rightmost points cannot be removed and can only move up
or down <br/>
• polyline represents valid function (for each color value input from 0 to 255 
there is only one output value), <br/>
• editing existing functional filters (also predefined, specified above, except
for gamma correction), <br/>
• saving created or modified filters in an application and applying them to
the image. <br/>

## Remarks
Image file loading and displaying may be handled by an external library, but
implementation of the required filters must be done using only operations on
single pixels.


