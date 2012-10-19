using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace PrisonBreak.QuadTree
{
    class QuadTree<T> : IBounded where T : IBounded
    {
        Rectangle worldBounds;
        int maxDepth = 6;

        QuadTreeNode<T> rootNode;

        public Rectangle AABounds
        {
            get { return worldBounds; }
        }

        public QuadTree(Rectangle worldBounds)
        {
            QuadTreeNode<T>.maxDepth = maxDepth;
            this.worldBounds = worldBounds;
            rootNode = new QuadTreeNode<T>(worldBounds, 0);
        }

        public void Insert(T toInsert)
        {
            if (!worldBounds.Contains(toInsert.AABounds))
            {
                System.Diagnostics.Debug.WriteLine("Invalid rectangle to insert.");
            }

            rootNode.Insert(toInsert);
        }

        public void Remove(T toRemove)
        {
            rootNode.Remove(toRemove);
        }

        public void Clear()
        {
            rootNode = new QuadTreeNode<T>(worldBounds, 0);
        }

        public List<T> Query(T queryArea)
        {
            return rootNode.Query(queryArea);
        }

        public List<Rectangle> Print()
        {
            return rootNode.GetRectangles();
        }
    }
}
