using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.U2D
{

    namespace UTess
    {

        enum TessEventType
        {
            EVENT_POINT = 0,
            EVENT_END = 1,
            EVENT_START = 2,
        };

        struct TessEdge
        {
            public int a;
            public int b;
        };

        struct TessEvent
        {
            public float2 a;
            public float2 b;
            public int idx;
            public int type;
        };

        struct TessHull
        {
            public float2 a;
            public float2 b;
            public int idx;

            public ArraySlice<int> ilarray;
            public int ilcount;
            public ArraySlice<int> iuarray;
            public int iucount;
        };

        struct TessCell
        {
            public int a;
            public int b;
            public int c;
        };

        struct TessStar
        {
            public ArraySlice<int> points;
            public int pointCount;
        };

        internal struct TessUtils
        {

            // From https://www.cs.cmu.edu/afs/cs/project/quake/public/code/predicates.c and is public domain. Can't find one within Unity.
            public static float OrientFast(float2 a, float2 b, float2 c)
            {
                float epsilon = 1.1102230246251565e-16f;
                float errbound3 = (3.0f + 16.0f * epsilon) * epsilon;
                float l = (a.y - c.y) * (b.x - c.x);
                float r = (a.x - c.x) * (b.y - c.y);
                float det = l - r;
                float s = 0;
                if (l > 0)
                {
                    if (r <= 0)
                    {
                        return det;
                    }
                    else
                    {
                        s = l + r;
                    }
                }
                else if (l < 0)
                {
                    if (r >= 0)
                    {
                        return det;
                    }
                    else
                    {
                        s = -(l + r);
                    }
                }
                else
                {
                    return det;
                }

                float tol = errbound3 * s;
                if (det >= tol || det <= -tol)
                {
                    return det;
                }

                return epsilon;
            }

            public static float Norm(float2 cV)
            {
                return cV.x * cV.x + cV.y * cV.y;
            }

            public static float Dist(float2 cV1, float2 cV2)
            {
                return (cV1.x - cV2.x) * (cV1.x - cV2.x) + (cV1.y - cV2.y) * (cV1.y - cV2.y);
            }            
            public static bool IsInsideCircle(float2 a, float2 b, float2 c, float2 p)
            {
                float ab = Norm(a);
                float cd = Norm(b);
                float ef = Norm(c);

                float ax = a.x;
                float ay = a.y;
                float bx = b.x;
                float by = b.y;
                float cx = c.x;
                float cy = c.y;

                float circum_x = (ab * (cy - by) + cd * (ay - cy) + ef * (by - ay)) /
                                 (ax * (cy - by) + bx * (ay - cy) + cx * (by - ay));
                float circum_y = (ab * (cx - bx) + cd * (ax - cx) + ef * (bx - ax)) /
                                 (ay * (cx - bx) + by * (ax - cx) + cy * (bx - ax));

                float2 circum = new float2();
                circum.x = circum_x / 2;
                circum.y = circum_y / 2;
                float circum_radius = Dist(a, circum);
                float dist = Dist(p, circum);
                return circum_radius - dist > 0.00001f;
            }

            public unsafe static void InsertionSort<T, U>(void* array, int lo, int hi, U comp)
                where T : struct where U : IComparer<T>
            {
                int i, j;
                T t;
                for (i = lo; i < hi; i++)
                {
                    j = i;
                    t = UnsafeUtility.ReadArrayElement<T>(array, i + 1);
                    while (j >= lo && comp.Compare(t, UnsafeUtility.ReadArrayElement<T>(array, j)) < 0)
                    {
                        UnsafeUtility.WriteArrayElement<T>(array, j + 1, UnsafeUtility.ReadArrayElement<T>(array, j));
                        j--;
                    }

                    UnsafeUtility.WriteArrayElement<T>(array, j + 1, t);
                }
            }

        }

        struct TessEventCompare : IComparer<TessEvent>
        {
            public int Compare(TessEvent a, TessEvent b)
            {
                float f = (a.a.x - b.a.x);
                if (0 != f)
                    return (f > 0) ? 1 : -1;

                f = (a.a.y - b.a.y);
                if (0 != f)
                    return (f > 0) ? 1 : -1;

                int i = a.type - b.type;
                if (0 != i)
                    return i;

                if (a.type != (int) TessEventType.EVENT_POINT)
                {
                    float o = TessUtils.OrientFast(a.a, a.b, b.b);
                    if (0 != o)
                    {
                        return (o > 0) ? 1 : -1;
                    }
                }

                return a.idx - b.idx;
            }
        }

        struct TessEdgeCompare : IComparer<TessEdge>
        {
            public int Compare(TessEdge a, TessEdge b)
            {
                int i = a.a - b.a;
                if (0 != i)
                    return i;
                i = a.b - b.b;
                return i;
            }
        }

        struct TessCellCompare : IComparer<TessCell>
        {
            public int Compare(TessCell a, TessCell b)
            {
                int i = a.a - b.a;
                if (0 != i)
                    return i;
                i = a.b - b.b;
                if (0 != i)
                    return i;
                i = a.c - b.c;
                return i;
            }
        }

        internal struct Tessellator
        {
            // For Processing.
            NativeArray<TessEdge> m_Edges;
            NativeArray<TessStar> m_Stars;
            NativeArray<TessCell> m_Cells;
            int m_CellCount;

            // For Storage.
            NativeArray<int> m_ILArray;
            NativeArray<int> m_IUArray;
            NativeArray<int> m_SPArray;
            int m_NumPoints;
            int m_StarCount;            

            // Intermediates.
            NativeArray<int> m_Flags;
            NativeArray<int> m_Neighbors;
            NativeArray<int> m_Constraints;

            static float TestPoint(TessHull hull, float2 point)
            {
                return TessUtils.OrientFast(hull.a, hull.b, point);
            }

            static int GetLowerHullForVector(NativeArray<TessHull> hulls, int hullCount, float2 p)
            {
                int i;
                int l = 0;
                int h = hullCount - 1;
                i = l - 1;
                while (l <= h)
                {
                    int m;
                    m = ((int) (l + h)) >> 1;
                    if (TestPoint(hulls[m], p) < 0)
                    {
                        i = m;
                        l = m + 1;
                    }
                    else
                        h = m - 1;
                }

                return i;
            }

            static int GetUpperHullForVector(NativeArray<TessHull> hulls, int hullCount, float2 p)
            {
                int i;
                int l = 0;
                int h = hullCount - 1;
                i = h + 1;
                while (l <= h)
                {
                    int m;
                    m = ((int) (l + h)) >> 1;
                    if (TestPoint(hulls[m], p) > 0)
                    {
                        i = m;
                        h = m - 1;
                    }
                    else
                        l = m + 1;
                }

                return i;
            }

            static float FindSplit(TessHull hull, TessEvent edge)
            {
                float d = 0;
                if (hull.a.x < edge.a.x)
                {
                    d = TessUtils.OrientFast(hull.a, hull.b, edge.a);
                }
                else
                {
                    d = TessUtils.OrientFast(edge.b, edge.a, hull.a);
                }

                if (0 != d)
                {
                    return d;
                }

                if (edge.b.x < hull.b.x)
                {
                    d = TessUtils.OrientFast(hull.a, hull.b, edge.b);
                }
                else
                {
                    d = TessUtils.OrientFast(edge.b, edge.a, hull.b);
                }

                if (0 != d)
                {
                    return d;
                }

                return hull.idx - edge.idx;
            }

            static int GetLowerEqualHullForEvent(NativeArray<TessHull> hulls, int hullCount, TessEvent p)
            {
                int i;
                int l = 0;
                int h = hullCount - 1;
                i = l - 1;
                while (l <= h)
                {
                    int m;
                    m = ((int) (l + h)) >> 1;
                    if (FindSplit(hulls[m], p) <= 0)
                    {
                        i = m;
                        l = m + 1;
                    }
                    else
                        h = m - 1;
                }

                return i;
            }

            static int GetEqualHullForEvent(NativeArray<TessHull> hulls, int hullCount, TessEvent p)
            {
                int l = 0;
                int h = hullCount - 1;
                while (l <= h)
                {
                    int m;
                    m = ((int) (l + h)) >> 1;
                    float f = FindSplit(hulls[m], p);
                    if (f == 0)
                    {
                        return m;
                    }
                    else if (f <= 0)
                    {
                        l = m + 1;
                    }
                    else
                        h = m - 1;
                }

                return -1;
            }

            void AddPoint(NativeArray<TessHull> hulls, int hullCount, NativeArray<float2> points, float2 p,
                int idx)
            {
                int l = GetLowerHullForVector(hulls, hullCount, p);
                int u = GetUpperHullForVector(hulls, hullCount, p);
                for (int i = l; i < u; ++i)
                {
                    TessHull hull = hulls[i];

                    int m = hull.ilcount;
                    while (m > 1 && TessUtils.OrientFast(points[hull.ilarray[m - 2]], points[hull.ilarray[m - 1]], p) >
                        0)
                    {
                        TessCell c = new TessCell();
                        c.a = hull.ilarray[m - 1];
                        c.b = hull.ilarray[m - 2];
                        c.c = idx;
                        m_Cells[m_CellCount++] = c;
                        m -= 1;
                    }

                    hull.ilcount = m + 1;
                    hull.ilarray[m] = idx;

                    m = hull.iucount;
                    while (m > 1 && TessUtils.OrientFast(points[hull.iuarray[m - 2]], points[hull.iuarray[m - 1]], p) <
                        0)
                    {
                        TessCell c = new TessCell();
                        c.a = hull.iuarray[m - 2];
                        c.b = hull.iuarray[m - 1];
                        c.c = idx;
                        m_Cells[m_CellCount++] = c;
                        m -= 1;
                    }

                    hull.iucount = m + 1;
                    hull.iuarray[m] = idx;

                    hulls[i] = hull;
                }
            }

            static void InsertHull(NativeArray<TessHull> Hulls, int Pos, ref int Count, TessHull Value)
            {
                if (Count < Hulls.Length - 1)
                {
                    for (int i = Count; i > Pos; --i)
                        Hulls[i] = Hulls[i - 1];
                    Hulls[Pos] = Value;
                    Count++;
                }
            }

            static void EraseHull(NativeArray<TessHull> Hulls, int Pos, ref int Count)
            {
                if (Count < Hulls.Length)
                {
                    for (int i = Pos; i < Count - 1; ++i)
                        Hulls[i] = Hulls[i + 1];
                    Count--;
                }
            }

            void SplitHulls(NativeArray<TessHull> hulls, ref int hullCount, NativeArray<float2> points,
                TessEvent evt)
            {
                int index = GetLowerEqualHullForEvent(hulls, hullCount, evt);
                TessHull hull = hulls[index];

                TessHull newHull;
                newHull.a = evt.a;
                newHull.b = evt.b;
                newHull.idx = evt.idx;

                int y = hull.iuarray[hull.iucount - 1];
                newHull.iuarray = new ArraySlice<int>(m_IUArray, newHull.idx * m_NumPoints, m_NumPoints);
                newHull.iucount = hull.iucount;
                for (int i = 0; i < newHull.iucount; ++i)
                    newHull.iuarray[i] = hull.iuarray[i];
                hull.iuarray[0] = y;
                hull.iucount = 1;
                hulls[index] = hull;

                newHull.ilarray = new ArraySlice<int>(m_ILArray, newHull.idx * m_NumPoints, m_NumPoints);
                newHull.ilarray[0] = y;
                newHull.ilcount = 1;

                InsertHull(hulls, index + 1, ref hullCount, newHull);
            }

            void MergeHulls(NativeArray<TessHull> hulls, ref int hullCount, NativeArray<float2> points,
                TessEvent evt)
            {
                float2 temp = evt.a;
                evt.a = evt.b;
                evt.b = temp;
                int index = GetEqualHullForEvent(hulls, hullCount, evt);

                TessHull upper = hulls[index];
                TessHull lower = hulls[index - 1];

                lower.iucount = upper.iucount;
                for (int i = 0; i < lower.iucount; ++i)
                    lower.iuarray[i] = upper.iuarray[i];

                hulls[index - 1] = lower;
                EraseHull(hulls, index, ref hullCount);
            }

            internal void Triangulate(NativeArray<float2> points, NativeArray<TessEdge> edgesIn)
            {
                int numEdges = edgesIn.Length;
                const int kStarEdges = 16;

                m_NumPoints = points.Length;
                m_StarCount = m_NumPoints > kStarEdges ? m_NumPoints : kStarEdges;
                m_StarCount = m_StarCount * 2;
                m_CellCount = 0;
                m_Cells = new NativeArray<TessCell>(m_NumPoints * (m_NumPoints + 1), Allocator.Temp);
                m_ILArray = new NativeArray<int>(m_NumPoints * (m_NumPoints + 1), Allocator.Temp); // Make room for -1 node.
                m_IUArray = new NativeArray<int>(m_NumPoints * (m_NumPoints + 1), Allocator.Temp); // Make room for -1 node.
                m_SPArray = new NativeArray<int>(m_NumPoints * (m_StarCount), Allocator.Temp); // Make room for -1 node.

                NativeArray<TessHull> hulls = new NativeArray<TessHull>(m_NumPoints * 8, Allocator.Temp);
                int hullCount = 0;

                NativeArray<TessEvent> events = new NativeArray<TessEvent>(m_NumPoints + (numEdges * 2), Allocator.Temp);
                int eventCount = 0;

                for (int i = 0; i < m_NumPoints; ++i)
                {
                    TessEvent evt = new TessEvent();
                    evt.a = points[i];
                    evt.b = new float2();
                    evt.idx = i;
                    evt.type = (int) TessEventType.EVENT_POINT;
                    events[eventCount++] = evt;
                }

                for (int i = 0; i < numEdges; ++i)
                {
                    TessEdge e = edgesIn[i];
                    float2 a = points[e.a];
                    float2 b = points[e.b];
                    if (a.x < b.x)
                    {
                        TessEvent _s = new TessEvent();
                        _s.a = a;
                        _s.b = b;
                        _s.idx = i;
                        _s.type = (int) TessEventType.EVENT_START;

                        TessEvent _e = new TessEvent();
                        _e.a = b;
                        _e.b = a;
                        _e.idx = i;
                        _e.type = (int) TessEventType.EVENT_END;

                        events[eventCount++] = _s;
                        events[eventCount++] = _e;
                    }
                    else if (a.x > b.x)
                    {
                        TessEvent _s = new TessEvent();
                        _s.a = b;
                        _s.b = a;
                        _s.idx = i;
                        _s.type = (int) TessEventType.EVENT_START;

                        TessEvent _e = new TessEvent();
                        _e.a = a;
                        _e.b = b;
                        _e.idx = i;
                        _e.type = (int) TessEventType.EVENT_END;

                        events[eventCount++] = _s;
                        events[eventCount++] = _e;
                    }
                }

                unsafe
                {
                    TessUtils.InsertionSort<TessEvent, TessEventCompare>(
                        NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(events), 0, eventCount - 1,
                        new TessEventCompare());
                    ;
                }

                float minX = events[0].a.x - (1 + math.abs(events[0].a.x)) * math.pow(2.0f, -16.0f);
                TessHull hull;
                hull.a.x = minX;
                hull.a.y = 1;
                hull.b.x = minX;
                hull.b.y = 0;
                hull.idx = -1;
                hull.ilarray = new ArraySlice<int>(m_ILArray, m_NumPoints * m_NumPoints, m_NumPoints); // Last element
                hull.iuarray = new ArraySlice<int>(m_IUArray, m_NumPoints * m_NumPoints, m_NumPoints);
                hull.ilcount = 0;
                hull.iucount = 0;
                hulls[hullCount++] = hull;

                for (int i = 0, numEvents = eventCount; i < numEvents; ++i)
                {
                    switch (events[i].type)
                    {
                        case (int) TessEventType.EVENT_POINT:
                        {
                            AddPoint(hulls, hullCount, points, events[i].a, events[i].idx);
                        }
                            break;

                        case (int) TessEventType.EVENT_START:
                        {
                            SplitHulls(hulls, ref hullCount, points, events[i]);
                        }
                            break;

                        default:
                        {
                            MergeHulls(hulls, ref hullCount, points, events[i]);
                        }
                            break;
                    }
                }

                hulls.Dispose();
                events.Dispose();
            }


            void Prepare(NativeArray<TessEdge> edgesIn)
            {
                m_Stars = new NativeArray<TessStar>(edgesIn.Length, Allocator.Temp);

                for (int i = 0; i < edgesIn.Length; ++i)
                {
                    TessEdge e = edgesIn[i];
                    e.a = (edgesIn[i].a < edgesIn[i].b) ? edgesIn[i].a : edgesIn[i].b;
                    e.b = (edgesIn[i].a > edgesIn[i].b) ? edgesIn[i].a : edgesIn[i].b;
                    edgesIn[i] = e;
                    TessStar s = m_Stars[i];
                    s.points = new ArraySlice<int>(m_SPArray, i * m_StarCount, m_StarCount);
                    s.pointCount = 0;
                    m_Stars[i] = s;
                }

                unsafe
                {
                    TessUtils.InsertionSort<TessEdge, TessEdgeCompare>(
                        NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(edgesIn), 0, edgesIn.Length - 1,
                        new TessEdgeCompare());
                }

                m_Edges = new NativeArray<TessEdge>(edgesIn.Length, Allocator.Temp);
                m_Edges.CopyFrom(edgesIn);

                // Fill stars.
                for (int i = 0; i < m_CellCount; ++i)
                {
                    int a = m_Cells[i].a;
                    int b = m_Cells[i].b;
                    int c = m_Cells[i].c;
                    TessStar sa = m_Stars[a];
                    TessStar sb = m_Stars[b];
                    TessStar sc = m_Stars[c];
                    sa.points[sa.pointCount++] = b;
                    sa.points[sa.pointCount++] = c;
                    sb.points[sb.pointCount++] = c;
                    sb.points[sb.pointCount++] = a;
                    sc.points[sc.pointCount++] = a;
                    sc.points[sc.pointCount++] = b;
                    m_Stars[a] = sa;
                    m_Stars[b] = sb;
                    m_Stars[c] = sc;
                }
            }

            int OppositeOf(int a, int b)
            {
                ArraySlice<int> points = m_Stars[b].points;
                for (int k = 1, n = m_Stars[b].pointCount; k < n; k += 2)
                {
                    if (points[k] == a)
                    {
                        return points[k - 1];
                    }
                }

                return -1;
            }

            static int GetEqualHullForEdges(NativeArray<TessEdge> edges, TessEdge p)
            {
                int l = 0;
                int h = edges.Length - 1;
                TessEdgeCompare tec = new TessEdgeCompare();
                while (l <= h)
                {
                    int m;
                    m = ((int) (l + h)) >> 1;
                    int f = tec.Compare(edges[m], p);
                    if (f == 0)
                    {
                        return m;
                    }
                    else if (f <= 0)
                    {
                        l = m + 1;
                    }
                    else
                        h = m - 1;
                }

                return -1;
            }

            int FindConstraint(int a, int b)
            {
                TessEdge e;
                e.a = a < b ? a : b;
                e.b = a > b ? a : b;
                return GetEqualHullForEdges(m_Edges, e);
            }

            void AddTriangle(int i, int j, int k)
            {
                TessStar si = m_Stars[i];
                TessStar sj = m_Stars[j];
                TessStar sk = m_Stars[k];
                si.points[si.pointCount++] = j;
                si.points[si.pointCount++] = k;
                sj.points[sj.pointCount++] = k;
                sj.points[sj.pointCount++] = i;
                sk.points[sk.pointCount++] = i;
                sk.points[sk.pointCount++] = j;
                m_Stars[i] = si;
                m_Stars[j] = sj;
                m_Stars[k] = sk;
            }

            void RemovePair(int r, int j, int k)
            {
                TessStar s = m_Stars[r];
                ArraySlice<int> points = s.points;
                for (int i = 1, n = s.pointCount; i < n; i += 2)
                {
                    if (points[i - 1] == j && points[i] == k)
                    {
                        points[i - 1] = points[n - 2];
                        points[i] = points[n - 1];
                        s.points = points;
                        s.pointCount = s.pointCount - 2;
                        m_Stars[r] = s;
                        return;
                    }
                }
            }

            void RemoveTriangle(int i, int j, int k)
            {
                RemovePair(i, j, k);
                RemovePair(j, k, i);
                RemovePair(k, i, j);
            }

            void EdgeFlip(int i, int j)
            {
                int a = OppositeOf(i, j);
                int b = OppositeOf(j, i);
                RemoveTriangle(i, j, a);
                RemoveTriangle(j, i, b);
                AddTriangle(i, b, a);
                AddTriangle(j, a, b);
            }

            void Flip(NativeArray<float2> points, ref NativeArray<int> stack, ref int stackCount,
                int a, int b, int x)
            {
                int y = OppositeOf(a, b);

                if (y < 0)
                {
                    return;
                }

                if (b < a)
                {
                    int tmp = a;
                    a = b;
                    b = tmp;
                    tmp = x;
                    x = y;
                    y = tmp;
                }

                if (FindConstraint(a, b) != -1)
                {
                    return;
                }

                if (TessUtils.IsInsideCircle(points[a], points[b], points[x], points[y]))
                {
                    stack[stackCount++] = a;
                    stack[stackCount++] = b;
                }
            }

            NativeArray<TessCell> GetCells(ref int count)
            {
                NativeArray<TessCell> cellsOut = new NativeArray<TessCell>(m_NumPoints * (m_NumPoints + 1), Allocator.Temp);
                count = 0;
                for (int i = 0, n = m_Stars.Length; i < n; ++i)
                {
                    ArraySlice<int> points = m_Stars[i].points;
                    for (int j = 0, m = m_Stars[i].pointCount; j < m; j += 2)
                    {
                        int s = points[j];
                        int t = points[j + 1];
                        if (i < math.min(s, t))
                        {
                            TessCell c = new TessCell();
                            c.a = i;
                            c.b = s;
                            c.c = t;
                            cellsOut[count++] = c;
                        }
                    }
                }

                return cellsOut;
            }

            internal void ApplyDelaunay(NativeArray<float2> points, NativeArray<TessEdge> edgesIn)
            {

                NativeArray<int> stack = new NativeArray<int>(m_NumPoints * (m_NumPoints + 1), Allocator.Temp);
                int stackCount = 0;

                Prepare(edgesIn);
                for (int a = 0; a < m_NumPoints; ++a)
                {
                    TessStar star = m_Stars[a];
                    for (int j = 1; j < star.pointCount; j += 2)
                    {
                        int b = star.points[j];

                        if (b < a)
                        {
                            continue;
                        }

                        if (FindConstraint(a, b) >= 0)
                        {
                            continue;
                        }

                        int x = star.points[j - 1], y = -1;
                        for (int k = 1; k < star.pointCount; k += 2)
                        {
                            if (star.points[k - 1] == b)
                            {
                                y = star.points[k];
                                break;
                            }
                        }

                        if (y < 0)
                        {
                            continue;
                        }

                        if (TessUtils.IsInsideCircle(points[a], points[b], points[x], points[y]))
                        {
                            stack[stackCount++] = a;
                            stack[stackCount++] = b;
                        }
                    }
                }

                while (stackCount > 0)
                {
                    int b = stack[stackCount - 1];
                    stackCount--;
                    int a = stack[stackCount - 1];
                    stackCount--;

                    int x = -1, y = -1;
                    TessStar star = m_Stars[a];
                    for (int i = 1; i < star.pointCount; i += 2)
                    {
                        int s = star.points[i - 1];
                        int t = star.points[i];
                        if (s == b)
                        {
                            y = t;
                        }
                        else if (t == b)
                        {
                            x = s;
                        }
                    }

                    if (x < 0 || y < 0)
                    {
                        continue;
                    }

                    if (!TessUtils.IsInsideCircle(points[a], points[b], points[x], points[y]))
                    {
                        continue;
                    }

                    EdgeFlip(a, b);

                    Flip(points, ref stack, ref stackCount, x, a, y);
                    Flip(points, ref stack, ref stackCount, a, y, x);
                    Flip(points, ref stack, ref stackCount, y, b, x);
                    Flip(points, ref stack, ref stackCount, b, x, y);
                }

                stack.Dispose();
            }

            int GetEqualCellForCells(NativeArray<TessCell> cells, int count, TessCell p)
            {
                int l = 0;
                int h = count - 1;
                TessCellCompare tcc = new TessCellCompare();
                while (l <= h)
                {
                    int m;
                    m = ((int) (l + h)) >> 1;
                    int f = tcc.Compare(cells[m], p);
                    if (f == 0)
                    {
                        return m;
                    }
                    else if (f <= 0)
                    {
                        l = m + 1;
                    }
                    else
                        h = m - 1;
                }

                return -1;
            }

            int FindNeighbor(NativeArray<TessCell> cells, int count, int a, int b, int c)
            {
                int x = a, y = b, z = c;
                if (b < c)
                {
                    if (b < a)
                    {
                        x = b;
                        y = c;
                        z = a;
                    }
                }
                else if (c < a)
                {
                    x = c;
                    y = a;
                    z = b;
                }

                if (x < 0)
                {
                    return -1;
                }

                TessCell key;
                key.a = x;
                key.b = y;
                key.c = z;
                return GetEqualCellForCells(cells, count, key);
            }

            NativeArray<TessCell> Constrain(ref int count)
            {
                var cells = GetCells(ref count);
                int nc = count;
                for (int i = 0; i < nc; ++i)
                {
                    TessCell c = cells[i];
                    int x = c.a, y = c.b, z = c.c;
                    if (y < z)
                    {
                        if (y < x)
                        {
                            c.a = y;
                            c.b = z;
                            c.c = x;
                        }
                    }
                    else if (z < x)
                    {
                        c.a = z;
                        c.b = x;
                        c.c = y;
                    }

                    cells[i] = c;
                }

                unsafe
                {
                    TessUtils.InsertionSort<TessCell, TessCellCompare>(
                        NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(cells), 0, m_CellCount - 1,
                        new TessCellCompare());
                }

                // Out
                m_Flags = new NativeArray<int>(nc, Allocator.Temp);
                m_Neighbors = new NativeArray<int>(nc * 3, Allocator.Temp);
                m_Constraints = new NativeArray<int>(nc * 3, Allocator.Temp);
                var next = new NativeArray<int>(nc * 3, Allocator.Temp);
                var active = new NativeArray<int>(nc * 3, Allocator.Temp);

                int side = 1, nextCount = 0, activeCount = 0;

                for (int i = 0; i < nc; ++i)
                {
                    TessCell c = cells[i];
                    for (int j = 0; j < 3; ++j)
                    {
                        int x = j, y = (j + 1) % 3;
                        x = (x == 0) ? c.a : (j == 1) ? c.b : c.c;
                        y = (y == 0) ? c.a : (y == 1) ? c.b : c.c;

                        int o = OppositeOf(y, x);
                        int a = m_Neighbors[3 * i + j] = FindNeighbor(cells, count, y, x, o);
                        int b = m_Constraints[3 * i + j] = (-1 != FindConstraint(x, y)) ? 1 : 0;
                        if (a < 0)
                        {
                            if (0 != b)
                            {
                                next[nextCount++] = i;
                            }
                            else
                            {
                                active[activeCount++] = i;
                                m_Flags[i] = 1;
                            }
                        }
                    }
                }

                while (activeCount > 0 || nextCount > 0)
                {
                    while (activeCount > 0)
                    {
                        int t = active[activeCount - 1];
                        activeCount--;
                        if (m_Flags[t] == -side)
                        {
                            continue;
                        }

                        m_Flags[t] = side;
                        TessCell c = cells[t];
                        for (int j = 0; j < 3; ++j)
                        {
                            int f = m_Neighbors[3 * t + j];
                            if (f >= 0 && m_Flags[f] == 0)
                            {
                                if (0 != m_Constraints[3 * t + j])
                                {
                                    next[nextCount++] = f;
                                }
                                else
                                {
                                    active[activeCount++] = f;
                                    m_Flags[f] = side;
                                }
                            }
                        }
                    }

                    for (int e = 0; e < nextCount; e++)
                        active[e] = next[e];
                    activeCount = nextCount;
                    nextCount = 0;
                    side = -side;
                }

                active.Dispose();
                next.Dispose();
                return cells;
            }

            internal NativeArray<TessCell> RemoveExterior(ref int cellCount)
            {
                int constrainedCount = 0;
                NativeArray<TessCell> constrained = Constrain(ref constrainedCount);

                NativeArray<TessCell> cellsOut = new NativeArray<TessCell>(constrainedCount, Allocator.Temp);
                cellCount = 0;
                for (int i = 0; i < constrainedCount; ++i)
                {
                    if (m_Flags[i] == -1)
                    {
                        cellsOut[cellCount++] = constrained[i];
                    }
                }

                constrained.Dispose();
                return cellsOut;
            }

            internal NativeArray<TessCell> RemoveInterior(int cellCount)
            {
                int constrainedCount = 0;
                NativeArray<TessCell> constrained = Constrain(ref constrainedCount);

                NativeArray<TessCell> cellsOut = new NativeArray<TessCell>(constrainedCount, Allocator.Temp);
                cellCount = 0;
                for (int i = 0; i < constrainedCount; ++i)
                {
                    if (m_Flags[i] == 1)
                    {
                        cellsOut[cellCount++] = constrained[i];
                    }
                }

                constrained.Dispose();
                return cellsOut;
            }

            internal void Cleanup()
            {
                m_Edges.Dispose();
                m_Stars.Dispose();
                m_ILArray.Dispose();
                m_IUArray.Dispose();
                m_SPArray.Dispose();
                m_Cells.Dispose();

                m_Flags.Dispose();
                m_Neighbors.Dispose();
                m_Constraints.Dispose();
            }

        }

    }
    
}
