How to update the FDO providers in MapGuide
-------------------------------------------

When you install MapGuide, it only has a subset of the avalible FDO providers. This will hopefully be fixed in the future, but for now you can manually upgrade you FDO provider collection. These are the steps required to update your FDO providers:

 1. Stop the MapGuide service
 2. Remove the FDO folder (found in C:\Program Files\MapGuideOpenSource2.0\Server\Bin on Windows)
 3. Download the latest FDO package for Windows or Linux
 4. Extract the archive, and rename "Bin" to "FDO"
 5. Place the "FDO" folder where you erased the other in step 2.
 6. Start the MapGuide service