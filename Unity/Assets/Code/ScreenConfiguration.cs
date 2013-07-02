
using UnityEngine;
namespace Assets.Code
{
	public class ScreenConfiguration
	{
        public Vector2 Resolution { get; private set; }
        public Vector2 GridSize { get; private set; }
        public Vector2 GridCellSize { get; private set; }
        public float HeightWhiteSpace { get; private set; }

        public ScreenConfiguration(uint gridWidth)
        {
            Resolution = new Vector2(Screen.width, Screen.height);
            var cellSizeX = Resolution.x / gridWidth;

            GridSize = new Vector2(gridWidth, Mathf.Floor(Resolution.y / cellSizeX));
            GridCellSize = new Vector2(cellSizeX, cellSizeX);

            HeightWhiteSpace = Resolution.y - (GridSize.y * GridCellSize.y);

            Camera.main.orthographic = true;
            Camera.main.orthographicSize = Resolution.y / 2;
            Camera.main.transform.position = new Vector3(Resolution.x / 2, (Resolution.y / 2), -200);
        }
	}
}
