# *******************************************************************
# dhsm.R
#
# Developed by ApexRMS
# Last edited: 2014-12-12
# *******************************************************************
#
# This script should be called by the Syncrosim application, using a Sysem.Diagnostics.Process call ( same as gdal_translate). It can also
# be called from the Command line. The following assumptions are made:
#   - The 1st command line argument is the absolute filename of the input State Attribute Raster. Use "'s if the filename contains spaces.
#   - The Transistion Group CSV file is located in the same directory as the input file
#   - Output files will be created in the same directory as the input file
# Example usage:
# CMD>RScript habSuit.R "D:\ApexRMS\Dynamic Habitat Suitability\Testdata\It0001-Ts0001-sa-446.tif"


library(raster)

# Expecting a command line argument specifying the full path of input raster file.
args <- commandArgs(trailingOnly = TRUE)

if (length(args) == 0) {
  stop('Expected command line value specifying full input filename.') 
}

inpFilename = args[1]
if (!(file.exists(inpFilename))){
  stop(sprintf("Specified input filename '%s' does not exist",inpFilename)) 
}

#Test File: 
#inpFilename = "D:/ApexRMS/Dynamic Habitat Suitability/Testdata/It0001-Ts0001-sa-446.tif"

# Figure out the input files directory. For now we're going to expect all files interations take place there.
baseName = dirname(inpFilename)

# Load the specified State Attribute raster file
sa <- raster(inpFilename)

# Load the Transition Group definition file ( exported by SSIM)
tgFilename<-file.path(baseName,"transgroup.csv")
if(!(file.exists(tgFilename))){
  stop('The expected Transition Group file does not exist in the same directory as the input file')   
}
tg <- read.csv( tgFilename)

# Loop through the transition groups and create a raster for each
for(id in tg$ID){
  
  # Clone the input State Attribute raster, to get basic raster structure
  transMult <- sa
  
  # For test purposes, set values less that mean to False, True otherwise
  #TEST CODE START
  vals = sa[]
  tmMean = mean(vals,na.rm = TRUE)
  transMult[] = vals > tmMean
  #TEST CODE END

  # INSERT UNR Code here, replacing TEST CODE above 
  
  # Save to new raster.
  #DEVNOTE: For some reason, the creation time is a little off in the resultant tif files.
  outputFname = sprintf("dhsm-tg-%d.tif", id)
  fullName = file.path(baseName,outputFname)
  rf<-writeRaster(transMult, fullName, format="GTiff", overwrite=TRUE)
  
}

print(sprintf("Successfully completed processing State Attribute file '%s' !",inpFilename))

