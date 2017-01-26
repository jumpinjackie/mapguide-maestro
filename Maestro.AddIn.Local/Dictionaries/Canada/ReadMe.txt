Datum Shift Issues for Canadian Users

This text file discusses issues concerning geodetic grid interpolation
data files which are supported by CS-MAP, but which are not distributed
with CS-MAP.   In the absence of a any of the actions described in this
file, the datum shift portion of the coordinate conversion process of
Canadian geographic features will be processed as indicated in the 
Geodetic Transformation Dictionary definition.

In those cases where a fallback method has been specified, and the
fallback method was used successfully, a non-fatal, non-normal status
value (+2) will be returned.  In the absence of a fallback specification,
for failure thereof to produce a valid result, the datum shift portion of
coordinate conversions will be suppressed and a non-fatal non-normal
status return (+1) will be returned.

The coordinate system software automatically performs a datum shift if 
the source and destination coordinate systems use different datums. 
Within North America, this is most often a shift between the NAD27 and 
NAD83 datums.  For U.S. users, CS-MAP uses the freely distributable
NADCON data files supplied by USGS.

Both  versions of the Canadian National Transformation are supported,
but use of version 1 is strongly discouraged by Geomatics Canada and
should be avoided.  Neither of these files are included in the CS-MAP
distribution as the publisher, Geomatics Canada, requires that you register
with them before granting permission to use the file.  There is no fee, only
registration as a user is required.  You will not be able to obtain a copy of
the version 1 file from Geomatics Canada under any circumstances.

To use version 2 of the Canadian National Transformation, you need to
perform the following steps:

1.  Obtain a copy of the data file.  Contact:

	Information Services
	Geodetic Survey Division, Geomatics Canada
	615 Booth Street
	Ottawa, Ontario, K1A 0E9
	(613) 995-4410
	information@geod.nrcan.gc.ca
	http://www.geod.nrcan.gc.ca.

2.  Once you have the file, copy it into the CS-MAP Dictionaries
folder hierarchy, preferably at Canada (this is the same directory where
this ReadMe.txt file was installed) and name it “Ntv2_0.gsb”.

3. You will need to modify the definition of the Geodetic Transformation
named "NAD27_to_NAD83" as defined in the dictionary source file named
"GeodeticTransform.asc".  Essentially you will need to edit that definition to
include a reference to the newly installed Ntv2_0.gsb file.  Note that all file
references should start with the "dot" notation which indicates a reference
to the folder in which the Geodetic Transformation Dictionary will ultimately
reside (essentially, CS-MAP's "Dictionaries" folder).  Use the CS-MAP
dictionary compiler tool to compile all dictionaries.  The newly created
"GeodeticTransform.csd" file will contain the updated  definition of
"NAD27_to_NAD83" which will include a reference to the newly installed
Ntv2_0.gsb file.

The NAD27 to NAD83 transformation data files overlap.  In the region of
overlap, the various files will not produce exactly the same results.
You may indicate which data file is to take precedence by sequencing the 
file references in the order of precedence.  Overlap in this specific case
occurs between the NTV2_0.gsb file and the CONUS.L?S files (especially in
and around the Detroit area) and between the NTV2_0.gsb file and the
ALASKA.L?S files along the common border between Canada and Alaska.

The procedure to use datum shift files provided by a provincial government
is similar to the above.  You must first obtain the file, copy it into a logical
location in the dictionary folder hierarchy, and then make CS-MAP aware of
the existence and location of the file by editing and then compiling, 
the “GeodeticTransform.asc” data file as described above.
