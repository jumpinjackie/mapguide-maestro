Datum Shift Issues for Japan JGD2011 Users (PatchJGD) 

This text file discusses issues concerning geodetic grid interpolation
data files which are supported by CS-MAP, but which are not distributed
with CS-MAP. In the absence of any of the actions described in this
file, the datum shift portion of the coordinate conversion process of
Japan JGD2011 geographic features will be processed but without the patched 
JGD corrections.

The coordinate system software automatically performs a datum shift if 
the source and destination coordinate systems use different datum's. 
For Japan JGD2011 datum coordinate reference system users it is 
recommended to use the Japan grid shift file JGD2011V100.par which is 
used in the geodetic datum transformation from JGD2000 to JGD2011 datum 
or inverse.
This Japan JGD2000 to JGD2011 grid shift file is not included in the CS-MAP
distribution as OSGeo do not have the distribution permission of the publisher, 
"Geospatial Information Authority of Japan". There is no fee, only download 
the file.


To use Japan JGD2000 - JGD2011 (PatchJGD) transformation, you need to
perform the following steps:

1.  Download the parameter file "PatchJGD" from "Geospatial Information 
    Authority of Japan".

	Url: http://psgsv.gsi.go.jp/koukyou/public/jishin/index.html
        Filename: "PatchJGD"(JGD2011V100.par)

    Note: Information is not in English. You may use an online translation 
    service.

2.  Once you have the file, copy it into the coordinate system dictionary
folder hierarchy, preferably at Japan (this is the same directory where
this ReadMe.txt file was installed) and name it “JGD2011V100.par”.


3.  For CS-MAP users only.
Open the "GeodeticTransformation.asc" text file in a text editor and search 
for the transformation "JGD2000_to_JGD2011" definition.
Delete the initial "#" symbol from the line:
#    GRID_FILE: JPPAR,Fwd,.\Japan\JGD2011V100.par
Save the file.
Compile new the csd files from the modified asc text files.

