set R_Script="C:\\Program Files\\R\\R-3.1.1\\bin\\RScript.exe"
REM Use %~dp0 to get batch file directory
cd /d %~dp0
REM To trap Standard error, in the same directory as the input file resides.
set OP_PATH=%~dp1
%R_Script% dhsm.R %1 > "%OP_PATH%dhsm.batch" 2>&1
REM %R_Script% dhsm.R %1