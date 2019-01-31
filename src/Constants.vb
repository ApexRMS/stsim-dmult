'*********************************************************************************************
' Dynamic Multipliers: A SyncroSim Package of dynamic multipliers for ST-Sim
'
' Copyright © 2007-2019 Apex Resource Management Solution Ltd. (ApexRMS). All rights reserved.
'
'*********************************************************************************************

Module Constants

    Public Const DATAFEED_STATE_ATTRIBUTE_TYPE_NAME As String = "STSim_StateAttributeType"
    Public Const SPATIAL_MAP_STATE_ATTRIBUTE_VARIABLE_PREFIX As String = "sa"

    'Dynamic Habitat Suitability Multipliers
    Public Const DATAFEED_DHSM_NAME As String = "DM_DHSMultipliers"
    Public Const DATASHEET_DHSM_ENABLED_COLUMN_NAME As String = "Enabled"
    Public Const DATASHEET_DHSM_FREQUENCY_COLUMN_NAME As String = "Frequency"
    Public Const DATASHEET_DHSM_STATE_ATTRIBUTE_TYPE_COLUMN_NAME As String = "StateAttributeTypeID"
    Public Const DATASHEET_DHSM_SCRIPT_COLUMN_NAME As String = "Script"

    ' File naming conventions
    Public Const DHSM_FOLDER_NAME = "DHSM"
    Public Const DHSM_TRANSITION_GROUP_CSV_FILENAME = "transgroup.csv" ' The name of the Transition Group export file. NOTE: R likes TXT instead of CSV
    Public Const DHSM_SA_FILENAME = "stateattr.tif"           ' The name of the DHSM State Attribute raster export file 
    Public Const DHSM_DHSM_TG_FILENAME = "dhsm-tg-{0}.tif"      ' The name of the Dynamic Habitat Suitability Spatial Multipliers raster import files

End Module
