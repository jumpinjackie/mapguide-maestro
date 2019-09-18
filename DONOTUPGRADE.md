# Packages which cannot be upgraded until further notice

## NetTopologySuite

Current version used: `1.15.3`

Reason for staying at this version: This package introduces breakging changes that have flow-on effects to the Maestro API. In particular we expose geometry interfaces from this library that have been removed in 2.0.0

## mg-desktop

Current version used: `3.0.0.8701`

Reason for staying at this version: This would require us updating our coordinate system dictionaries which would blow up the repository size to an unacceptable level