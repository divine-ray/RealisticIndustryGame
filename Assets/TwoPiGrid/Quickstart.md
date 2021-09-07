# Two Pi Grid - Quick Start

**Two Pi Grid** is a small tool to help you create **spherical 3D grids** for your game. **The grid is logical, it does not have a physical representation in-game** (an edit mode visualizer is being developed, though).

## Table of contents

- [How to use]
- [Support]

## How to use

### Instantiate the grid

There are several ways you can instantiate the grid:

#### 1) By using a factory method:

```csharp
using TwoPiGrid;

//A typical "game manager" class, that is often a singleton and/or MonoBehaviour
public class MyGameManager
{
    //The MyGameManager object holds (and makes publicly available) the grid:
    public BaseGrid Grid { get; private set; }

    //In the constructor, we initialize the grid:
    public MyGameManager()
    {
        //We create a grid with 162 cells. The number of cells is fixed, but
        //you can change its radius and center at any time.
        Grid = BaseGrid.CreateWith162Cells(
            radius: 14,
            center: Vector3.zero); 
    }
}
```

#### 2) By passing in a settings object:

```csharp
using TwoPiGrid;

public class MyGameManager
{
    public BaseGrid Grid { get; private set; }

    public MyGameManager()
    {
        //We create a settings object to pass to the grid constructor.
        //Here we specify the grid's settings:
        var settings = new GridSettings(
            cellsAmount: GridCellsAmount._162, //This value is fixed.
            radius: 14, //You can change the radius at any time...
            center: Vector3.zero); //...and the center too.

        //Here we instantiate the grid by passing in a GridSettings object:
        Grid = new BaseGrid(settings);
    }
}
```

Alternatively, you can create a `GridSettingsSerialized` object, edit its parameters there, and have the grid load it:

```csharp
using TwoPiGrid;

public class MyGameManager
{
    public BaseGrid Grid { get; private set; }

    public MyGameManager()
    {
        //It will load the settings from a ScriptableObject named "GridSettings"
        //that is in a Resources folder in your project:
        Grid = new BaseGrid(settingsFileName: "GridSettings");
    }
}
```

To create a settings object: **Right click on the project inspector > Create > Two Pi Grid > Grid Settings**. Remember to place it in a **Resources** folder!

### Use the grid

At any time, you can query the grid for the index of the cell that is closest to a given point in world coordinates:

```csharp
public int GetIndexOfClosestVertexTo(Vector3 point)
```

You can also get the position of that cell in world coordinates:

```csharp
public Vector3 GetCellPosition(int i)
```

If you combine both methods, you can achieve a "snap to grid" effect on a spherical grid!

```csharp
using TwoPiGrid;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    //The active camera, for the ray cast:
    [SerializeField] private Camera m_Camera;

    //The grid settings object, to initialize it:
    [SerializeField] private GridSettingsSerialized gridSettings;

    //The object we are moving:
    [SerializeField] private Transform objectToMove;

    //The grid:
    private BaseGrid grid;

    //In the Start() method, we instantiate the grid:
    private void Start()
    {
        grid = new BaseGrid(gridSettings);
    }

    //In the Update() method, we move the object along the grid:
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
        	//We use a raycast to get a position in world coordinates:
            if (Physics.Raycast(
                    ray: m_Camera.ScreenPointToRay(Input.mousePosition),
                    hitInfo: out var hit,
                    maxDistance: 100,
                    layerMask: LayerMask.GetMask("Default"))) //Hit the sphere
            {
            	//We get the index of the closest cell to the hit point:
                var index = grid.GetIndexOfClosestVertexTo(hit.point);
                //And we place the object on that cell:
                objectToMove.transform.position = grid.GetCellPosition(index);
            }
        }
    }
}
```

This example comes with the asset. Play around with it!

### Customize your grid

It may be that the `BaseGrid` is too basic for you, and you need more options. Fear not!

If you want your grid to hold custom information about each of its cells, you can use the **Grid Configurator**:

Create a GridConfiguration asset: **Right click on the project inspector > Create > Two Pi Grid > Grid Configuration**. There, you can specify:

- **Namespace Name:** The namespace for the grid class.
- **Grid Prefix:** The prefix for the grid class. For `BaseGrid`, the prefix is `Base`.
- **Custom Cell Fields:** Custom properties that each cell will have.

For example, if you specify:
- "MyStudio.MyGame" as namespace name.
- "My" as grid prefix.
- A field named "TerrainType" of type "Custom Enum" named "TerrainTypes".

You will get a custom grid looking like this:

```csharp
using TwoPiGrid;

namespace MyStudio.MyGame
{
    public class MyGrid : BaseGrid
    {
        public TerrainTypes GetTerrainType(int i) => terrainTypes[i];
        private TerrainTypes[] terrainTypes = new TerrainTypes[0];
    }
}
```

This assumes that you have defined yourself a `TerrainTypes` `enum` in your own code. The idea is to let you use your own custom types with the Two Pi Grid.

**I highly recommend that whatever custom types you use are serializable and implement IComparable.**

So, once you have decided your custom grid's characteristics, you can click the "Generate custom grid" button, and it will be generated in the "TwoPiGrid Outputs" folder (if you don't have one, create it wherever you like).

#### Use your custom grid

You can use your custom grid like you would use the `BaseGrid`, except this time you get more stuff.

To instantiate your custom grid, you will need a custom grid settings object. You can create it through code, or as an asset in the inspector. For example, if you created a custom grid named `MyGrid` the settings object will be `MyGridSettings`, and the option in the "Create" menu will be "MyGrid Settings":

```csharp
using TwoPiGrid;

public class MyGameManager
{
    public MyGrid Grid { get; private set; }

    public MyGameManager()
    {
        //OPTION 1: Use a factory method:
        Grid = MyGrid.CreateWith162Cells(
            terrainTypes: new TerrainTypes[162], //<-- custom!
            radius: 14,
            center: Vector3.zero);

        //OPTION 2: Pass in a settings object:
        var settings = new MyGridSettings(
            terrainTypes: new TerrainTypes[162], //<-- custom!
            cellsAmount: GridCellsAmount._162,
            radius: 14,
            center: Vector3.zero);
        Grid = new MyGrid(settings);

        //OPTION 3: Pass in the name of a custom grid settings asset:
        Grid = new MyGrid("MyGridSettings");
    }
}
```

## Support

If you have any questions, bug reports, or feature suggestions, please send them to twopigrid@gmail.com :)
