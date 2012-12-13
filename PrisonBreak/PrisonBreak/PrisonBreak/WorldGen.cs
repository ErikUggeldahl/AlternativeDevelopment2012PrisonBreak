using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
		private static Body CreateStatic(Texture2D image)
		{
			// Shorthand
			const float MInPx = RigidBody.MInPx;

			// Dimensions
			int width = image.Width;
			int height = image.Height;
			const int tileSize = 1;

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

			// The body to return
			Body combinedBody = BodyFactory.CreateBody(RigidBody.World);
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

					// Create the fixture dimensions
					float bodyWidth = 16f * currentX / MInPx;
					float bodyHeight = 16f * tileSize / MInPx;

					// Create the transform offsets
					float xOffset = (bodyWidth - 16f * width / MInPx) * 0.5f;
					float yOffset = (bodyHeight - 16f * height / MInPx) * 0.5f;
					Vector2 transform = new Vector2(xOffset + x * 16f / MInPx, yOffset + y * 16f / MInPx);

					// Create the fixture using the above and attach it to combinedBody
					Fixture f = FixtureFactory.AttachRectangle(bodyWidth, bodyHeight, 1f, new Vector2(xOffset + x * 16f / MInPx, yOffset + y * 16f / MInPx), combinedBody);

					// Adjust x to skip all consecutive tiles as they've been handled already
					x += currentX;
				}
			}
			return combinedBody;
		}

		public static GameObject CreateWorldGO(GraphicsDevice gd, ContentManager content, string worldSprite, string collisionSprite)
		{
			GameObject worldGO = new GameObject();
			worldGO.AddTransform();
			worldGO.AddStaticSprite(content.Load<Texture2D>(worldSprite));
			worldGO.AddRenderer(gd, SpriteTransparency.Transparent);
			worldGO.AddStaticRigidBody(CreateStatic(content.Load<Texture2D>(collisionSprite)));
			worldGO.RigidBody.CollisionCategory = CollisionCats.WorldCategory;
			return worldGO;
		}
	}
}












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