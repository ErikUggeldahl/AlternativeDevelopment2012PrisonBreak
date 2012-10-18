using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace PrisonBreak.QuadTree
{
    class QuadTreeNode<T> where T : IBounded
    {
        Rectangle bounds;

        QuadTreeNode<T> nw, ne, sw, se;
        Rectangle rNW, rNE, rSW, rSE;

        List<T> elements;

        public Rectangle AABounds
        {
            get { return bounds; }
        }

        public List<T> Elements
        {
            get { return elements; }
        }

        public QuadTreeNode(Rectangle bounds)
        {
            this.bounds = bounds;

            int northHeight = bounds.Height / 2;
            int southHeight = bounds.Height - northHeight;
            int westWidth = bounds.Width / 2;
            int eastWidth = bounds.Width - westWidth;

            rNW = new Rectangle(bounds.X, bounds.Y, westWidth, northHeight);
            rNE = new Rectangle(bounds.X + westWidth, bounds.Y, eastWidth, northHeight);
            rSW = new Rectangle(bounds.X, bounds.Y + northHeight, westWidth, southHeight);
            rSE = new Rectangle(bounds.X + westWidth, bounds.Y + northHeight, eastWidth, southHeight);

            elements = new List<T>();
        }

        public void Insert(T toInsert)
        {
            if (rNW.Contains(toInsert.AABounds))
            {
                CreateSubDivisions();
                nw.Insert(toInsert);
            }
            else if (rNE.Contains(toInsert.AABounds))
            {
                CreateSubDivisions();
                ne.Insert(toInsert);
            }
            else if (rSW.Contains(toInsert.AABounds))
            {
                CreateSubDivisions();
                sw.Insert(toInsert);
            }
            else if (rSE.Contains(toInsert.AABounds))
            {
                CreateSubDivisions();
                se.Insert(toInsert);
            }
            else
            {
                elements.Add(toInsert);
            }
        }

        private void CreateSubDivisions()
        {
            nw = new QuadTreeNode<T>(rNW);
            ne = new QuadTreeNode<T>(rNE);
            sw = new QuadTreeNode<T>(rSW);
            se = new QuadTreeNode<T>(rSE);
        }

        public List<T> Query(T queryArea)
        {
            if (nw == null)
                return elements;

            List<T>[] lChildren = new List<T>[4];
            lChildren[0] = rNW.Intersects(queryArea.AABounds) ? nw.Query(queryArea) : null;
            lChildren[1] = rNE.Intersects(queryArea.AABounds) ? ne.Query(queryArea) : null;
            lChildren[2] = rSW.Intersects(queryArea.AABounds) ? sw.Query(queryArea) : null;
            lChildren[3] = rSE.Intersects(queryArea.AABounds) ? se.Query(queryArea) : null;

            Console.WriteLine(rNW.Intersects(queryArea.AABounds));
            Console.WriteLine(rNE.Intersects(queryArea.AABounds));
            Console.WriteLine(rSW.Intersects(queryArea.AABounds));
            Console.WriteLine(rSE.Intersects(queryArea.AABounds));

            int childrenCount = 0;
            for (int i = 0; i < 4; i++)
            {
                if (lChildren[i] != null)
                {
                    childrenCount += lChildren[i].Count;
                }
            }

            List<T> all = new List<T>(childrenCount);
            all.AddRange(elements);
            for (int i = 0; i < 4; i++)
            {
                if (lChildren[i] != null)
                {
                    all.AddRange(lChildren[i]);
                }
            }

            return all;
        }

        public List<Rectangle> GetRectangles()
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            rectangles.Add(bounds);

            if (nw == null)
            {
                return rectangles;
            }

            rectangles.AddRange(nw.GetRectangles());
            rectangles.AddRange(ne.GetRectangles());
            rectangles.AddRange(sw.GetRectangles());
            rectangles.AddRange(se.GetRectangles());

            return rectangles;
        }
    }
}
