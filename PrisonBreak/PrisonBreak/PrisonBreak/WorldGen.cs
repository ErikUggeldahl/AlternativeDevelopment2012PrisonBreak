using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

// Used if using decomposition
//using FarseerPhysics.Collision;
//using FarseerPhysics.Common;
//using FarseerPhysics.Common.Decomposition;

using PrisonBreak.Components;

namespace PrisonBreak
{
	public class WorldGen
	{
		public static Body CreateStatic(Texture2D image)
		{
			// Shorthand
			const float MInPx = RigidBody.MInPx;

			// Dimensions
			int width = image.Width;
			int height = image.Height;
			const int tileSize = 10;

			// Pixel data in two formats
			sbyte[,] bytes = new sbyte[width, height];	// Note: Should be +1 if using decomposition
			uint[] texData = new uint[width * height];
			image.GetData(texData);

			// Initialize pixel data
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					// Image must be flipped due to the way it's processed
					int flippedY = height - 1 - y;
					if (texData[y * width + x] == 0)
						bytes[x, flippedY] = 1;		// 1 represents empty alpha (due to Marching Squares convention)
					else
						bytes[x, flippedY] = -1;	// -1 represents occupied alpha (due to Marching Squares convention)
				}
			}

			// The return combined body
			Body combinedBodies = BodyFactory.CreateBody(RigidBody.World);
			for (int y = 0; y < height; y += tileSize)
			{
				for (int x = 0; x < width; x += tileSize)
				{
					// currentX crawls horizontal pixels to make the largest possible horizontal bodies
					int currentX = x;
					while (currentX < width && bytes[currentX, y] == -1)
					{
						currentX++;
					}

					// Abort if no pixels were found
					if (currentX == x)
						continue;

					// Adjust currentX to be the number of consecutive tiles found
					currentX -= x;

					// Create the body
					float bodyWidth = currentX / MInPx;
					float bodyHeight = tileSize / MInPx;
					Body tileBody = BodyFactory.CreateRectangle(RigidBody.World, bodyWidth, bodyHeight, 1f);
					combinedBodies.FixtureList.AddRange(tileBody.FixtureList);
					

					// Transform the body to center it
					float xOffset = (bodyWidth - width / MInPx) * 0.5f;
					float yOffset = (tileSize - height) * 0.5f / MInPx;
					Vector2 transform = new Vector2(xOffset + x / MInPx, yOffset + y / MInPx);
					tileBody.SetTransform(transform, 0f);

					// Adjust x to skip all consecutive tiles as they've been handled already
					x += currentX;
				}
			}

			return combinedBodies;

			// Unused decomposition code. Had some issues with cut corners and was inefficient

			//int cellSize = 10;
			//float subCellSize = 1f;
			//int nXCells = width / cellSize;
			//int nYCells = height / cellSize;

			//List<Body>[,] bodyGrid = new List<Body>[nXCells, nYCells];

			//for (int gridY = 0; gridY < nYCells; gridY++)
			//{
			//    for (int gridX = 0; gridX < nXCells; gridX++)
			//    {
			//        float adjustedX = gridX * cellSize;
			//        float adjustedY = gridY * cellSize;
			//        List<Vertices> polys = MarchingSquares.DetectSquares(new AABB(new Vector2(adjustedX, adjustedY), new Vector2(adjustedX + cellSize, adjustedY + cellSize)), subCellSize, subCellSize, bytes, 2, true);
			//        bodyGrid[gridX, gridY] = new List<Body>();

			//        Vector2 scale = new Vector2(1f / RigidBody.MInPx, -1f / RigidBody.MInPx);
			//        Vector2 translate = new Vector2(((float)width / -2f) / RigidBody.MInPx, ((float)150f / 2f) / RigidBody.MInPx);
			//        for (int i = 0; i < polys.Count; i++)
			//        {
			//            polys[i].Scale(ref scale);
			//            polys[i].Translate(ref translate);
			//            polys[i].ForceCounterClockWise();
			//            Vertices verts = FarseerPhysics.Common.PolygonManipulation.SimplifyTools.CollinearSimplify(polys[i]);
			//            List<Vertices> decomposedPolys = EarclipDecomposer.ConvexPartition(verts);

			//            for (int j = 0; j < decomposedPolys.Count; j++)
			//            {
			//                if (decomposedPolys[i].Count > 2)
			//                    bodyGrid[gridX, gridY].Add(BodyFactory.CreatePolygon(RigidBody.World, decomposedPolys[i], 1));
			//            }
			//        }
			//    }
			//}
		}
	}
}
