# Changelog - Two Pi Grid

## [1.1.1] - 2020-11-13
### Added
- Added syntax checks to custom grid name, namespace, and cell fields.

## [1.1.0] - 2020-11-03
### Added
- Added custom grid settings inspector for easier cell property editing.

## [1.0.1] - 2020-11-02
### Fixed
- Fixed bug where it would bypass safety checks when attempting to generate custom grid.

## [1.0.0] - 2020-10-31
### Added
- Added assembly definition file to Example so that it will work.

### Removed
- Removed grid visualizer. Will be added back at a later time, but it needs to be reworked first.

## [1.0.0-beta] - 2020-10-29
### Fixed
- Fixed crash when creating custom grid in Mac OS.

## [1.0.0-alpha] - 2020-10-25

### Added
- `GridConfiguration`: configure the settings of your custom gid (name, namespace, cell attributes).
- Automatic generation of a custom grid based on `GridConfiguration` parameters.
- Added comments to public classes and methods.
- Added example scene.

### Removed
- Removed mesh visual settings. Will be added back at a later time, but they need to be reworked first.
- Removed cellTypesX and cellTypesY arrays from `BaseGrid`, as now we rely on procedurally generating subclasses of `BaseGrid` with custom "cell types" arrays.

## [0.0.1] - 2020-10-22

### Added
- Icosphere grid generation (icosahedron subdivided 0-3 times, truncated icosahedron, chamfered dodecahedron).
- Base grid settings (radius, sphere type)
- Icosphere mesh generation (icosahedron subdivided 0-3 times, truncated icosahedron, chamfered dodecahedron).
- Base grid visualizer: create a `GameObject` with the generated grid mesh, modify its properties in the Inspector.
- Mesh visual settings (ramp texture, UVs assigned to each cell according to the value of cellTypesX and cellTypesY arrays).
