using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace PrisonBreak.QuadTree
{
    class QuadTreeNode<T> where T : IBounded
    {
        public static int maxDepth;

        Rectangle bounds;
        int depth;

        QuadTreeNode<T> nw, ne, sw, se;
        private Rectangle rNW, rNE, rSW, rSE;

        List<T> elements;
        int numEContained;

        public Rectangle AABounds
        {
            get { return bounds; }
        }

        public List<T> Elements
        {
            get { return elements; }
        }

        public int NumEContained
        {
            get { return numEContained; }
        }

        public QuadTreeNode(Rectangle bounds, int depth)
        {
            this.bounds = bounds;
            this.depth = depth;

            int northHeight = bounds.Height / 2;
            int southHeight = bounds.Height - northHeight;
            int westWidth = bounds.Width / 2;
            int eastWidth = bounds.Width - westWidth;

            rNW = new Rectangle(bounds.X, bounds.Y, westWidth, northHeight);
            rNE = new Rectangle(bounds.X + westWidth, bounds.Y, eastWidth, northHeight);
            rSW = new Rectangle(bounds.X, bounds.Y + northHeight, westWidth, southHeight);
            rSE = new Rectangle(bounds.X + westWidth, bounds.Y + northHeight, eastWidth, southHeight);

            elements = new List<T>();
            numEContained = 0;
        }

        public void Insert(T toInsert)
        {
            if (depth == maxDepth)
            {
                elements.Add(toInsert);
            }
            else
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
            numEContained++;
        }

        private void CreateSubDivisions()
        {
            if (nw != null)
                return;

            nw = new QuadTreeNode<T>(rNW, depth + 1);
            ne = new QuadTreeNode<T>(rNE, depth + 1);
            sw = new QuadTreeNode<T>(rSW, depth + 1);
            se = new QuadTreeNode<T>(rSE, depth + 1);
        }

        public void Remove(T toRemove)
        {
            if (depth == maxDepth)
            {
                if (!elements.Remove(toRemove))
                {
                    System.Diagnostics.Debug.WriteLine("Could not remove element.");
                }
            }
            else
            {
                if (rNW.Contains(toRemove.AABounds))
                {
                    nw.Remove(toRemove);
                    if (nw.NumEContained == 0 && ne.NumEContained == 0 && sw.NumEContained == 0 && se.NumEContained == 0)
                    {
                        DestroySubDivisions();
                    }
                }
                else if (rNE.Contains(toRemove.AABounds))
                {
                    ne.Remove(toRemove);
                    if (ne.NumEContained == 0 && nw.NumEContained == 0 && sw.NumEContained == 0 && se.NumEContained == 0)
                    {
                        DestroySubDivisions();
                    }
                }
                else if (rSW.Contains(toRemove.AABounds))
                {
                    sw.Remove(toRemove);
                    if (sw.NumEContained == 0 && ne.NumEContained == 0 && nw.NumEContained == 0 && se.NumEContained == 0)
                    {
                        DestroySubDivisions();
                    }
                }
                else if (rSE.Contains(toRemove.AABounds))
                {
                    se.Remove(toRemove);
                    if (se.NumEContained == 0 && ne.NumEContained == 0 && sw.NumEContained == 0 && nw.NumEContained == 0)
                    {
                        DestroySubDivisions();
                    }
                }
                else
                {
                    if (!elements.Remove(toRemove))
                    {
                        System.Diagnostics.Debug.WriteLine("Could not remove element.");
                    }
                }
            }
            numEContained--;
        }

        private void DestroySubDivisions()
        {
            nw = null;
            ne = null;
            sw = null;
            se = null;
        }

        public List<T> Query(T queryArea)
        {
            List<T> intersectingElements = new List<T>(elements.Count);
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].AABounds.Intersects(queryArea.AABounds))
                {
                    intersectingElements.Add(elements[i]);
                }
            }

            if (nw == null)
            {
                return intersectingElements;
            }

            List<T>[] lChildren = new List<T>[4];
            lChildren[0] = rNW.Intersects(queryArea.AABounds) ? nw.Query(queryArea) : null;
            lChildren[1] = rNE.Intersects(queryArea.AABounds) ? ne.Query(queryArea) : null;
            lChildren[2] = rSW.Intersects(queryArea.AABounds) ? sw.Query(queryArea) : null;
            lChildren[3] = rSE.Intersects(queryArea.AABounds) ? se.Query(queryArea) : null;

            int childrenCount = 0;
            for (int i = 0; i < 4; i++)
            {
                if (lChildren[i] != null)
                {
                    childrenCount += lChildren[i].Count;
                }
            }

            List<T> all = new List<T>(childrenCount + intersectingElements.Count);
            all.AddRange(intersectingElements);
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
