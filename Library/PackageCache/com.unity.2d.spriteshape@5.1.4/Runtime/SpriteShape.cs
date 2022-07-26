using System;
using System.Collections.Generic;

namespace UnityEngine.U2D
{
    public enum ShapeTangentMode
    {
        Linear = 0,
        Continuous = 1,
        Broken = 2,
    };

    public enum CornerType
    {
        OuterTopLeft,
        OuterTopRight,
        OuterBottomLeft,
        OuterBottomRight,
        InnerTopLeft,
        InnerTopRight,
        InnerBottomLeft,
        InnerBottomRight,
    };

    public enum QualityDetail
    {
        High = 16,
        Mid = 8,
        Low = 4
    }

    public enum Corner
    {
        Disable = 0,
        Automatic = 1,
        Stretched = 2,
    }

    [System.Serializable]
    public class SplineControlPoint
    {
        public Vector3 position;
        public Vector3 leftTangent;
        public Vector3 rightTangent;
        public ShapeTangentMode mode;
        public float height = 1f;
        public float bevelCutoff;
        public float bevelSize;
        public int spriteIndex;
        public bool corner;
        [SerializeField]
        Corner m_CornerMode;

        public Corner cornerMode
        {
            get => m_CornerMode;
            set => m_CornerMode = value;
        }

        public override int GetHashCode()
        {
            return  ((int)position.x).GetHashCode() ^ ((int)position.y).GetHashCode() ^ position.GetHashCode() ^
                    (leftTangent.GetHashCode() << 2) ^ (rightTangent.GetHashCode() >> 2) ^  ((int)mode).GetHashCode() ^
                    height.GetHashCode() ^ spriteIndex.GetHashCode() ^ corner.GetHashCode() ^ (m_CornerMode.GetHashCode() << 2);
        }
    }

    [System.Serializable]
    public class AngleRange : ICloneable
    {
        public float start
        {
            get { return m_Start; }
            set { m_Start = value; }
        }

        public float end
        {
            get { return m_End; }
            set { m_End = value; }
        }

        public int order
        {
            get { return m_Order; }
            set { m_Order = value; }
        }

        public List<Sprite> sprites
        {
            get { return m_Sprites; }
            set { m_Sprites = value; }
        }

        [SerializeField]
        float m_Start;
        [SerializeField]
        float m_End;
        [SerializeField]
        int m_Order;
        [SerializeField]
        List<Sprite> m_Sprites = new List<Sprite>();

        public object Clone()
        {
            AngleRange clone = this.MemberwiseClone() as AngleRange;
            clone.sprites = new List<Sprite>(clone.sprites);

            return clone;
        }

        public override bool Equals(object obj)
        {
            var other = obj as AngleRange;

            if (other == null)
                return false;

            bool equals = start.Equals(other.start) && end.Equals(other.end) && order.Equals(other.order);

            if (!equals)
                return false;

            if (sprites.Count != other.sprites.Count)
                return false;

            for (int i = 0; i < sprites.Count; ++i)
                if (sprites[i] != other.sprites[i])
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = start.GetHashCode() ^ end.GetHashCode() ^ order.GetHashCode();

            if (sprites != null)
            {
                for (int i = 0; i < sprites.Count; i++)
                {
                    Sprite sprite = sprites[i];
                    if (sprite)
                        hashCode = hashCode * 16777619 ^ (sprite.GetHashCode() + i);
                }
            }

            return hashCode;
        }
    }

    [System.Serializable]
    public class CornerSprite : ICloneable
    {
        public CornerType cornerType
        {
            get { return m_CornerType; }
            set { m_CornerType = value; }
        }

        public List<Sprite> sprites
        {
            get { return m_Sprites; }
            set { m_Sprites = value; }
        }

        [SerializeField]
        CornerType m_CornerType;               ///< Set Corner type. enum { OuterTopLeft = 0, OuterTopRight = 1, OuterBottomLeft = 2, OuterBottomRight = 3, InnerTopLeft = 4, InnerTopRight = 5, InnerBottomLeft = 6, InnerBottomRight = 7 }
        [SerializeField]
        List<Sprite> m_Sprites;

        public object Clone()
        {
            CornerSprite clone = this.MemberwiseClone() as CornerSprite;
            clone.sprites = new List<Sprite>(clone.sprites);

            return clone;
        }

        public override bool Equals(object obj)
        {
            var other = obj as CornerSprite;

            if (other == null)
                return false;

            if (!cornerType.Equals(other.cornerType))
                return false;

            if (sprites.Count != other.sprites.Count)
                return false;

            for (int i = 0; i < sprites.Count; ++i)
                if (sprites[i] != other.sprites[i])
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = cornerType.GetHashCode();

            if (sprites != null)
            {
                for (int i = 0; i < sprites.Count; i++)
                {
                    Sprite sprite = sprites[i];
                    if (sprite)
                    {
                        hashCode ^= (i + 1);
                        hashCode ^= sprite.GetHashCode();
                    }
                }
            }

            return hashCode;
        }
    }

    [HelpURLAttribute("https://docs.unity3d.com/Packages/com.unity.2d.spriteshape@latest/index.html?subfolder=/manual/SSProfile.html")]
    public class SpriteShape : ScriptableObject
    {
        public List<AngleRange> angleRanges
        {
            get { return m_Angles; }
            set { m_Angles = value; }
        }

        public Texture2D fillTexture
        {
            get { return m_FillTexture; }
            set { m_FillTexture = value; }
        }

        public List<CornerSprite> cornerSprites
        {
            get { return m_CornerSprites; }
            set { m_CornerSprites = value; }
        }

        public float fillOffset
        {
            get { return m_FillOffset; }
            set { m_FillOffset = value; }
        }

        public bool useSpriteBorders
        {
            get { return m_UseSpriteBorders; }
            set { m_UseSpriteBorders = value; }
        }

        [SerializeField]
        List<AngleRange> m_Angles = new List<AngleRange>();
        [SerializeField]
        Texture2D m_FillTexture;
        [SerializeField]
        List<CornerSprite> m_CornerSprites = new List<CornerSprite>();
        [SerializeField]
        float m_FillOffset;

        [SerializeField]
        bool m_UseSpriteBorders = true;

        private CornerSprite GetCornerSprite(CornerType cornerType)
        {
            var cornerSprite = new CornerSprite();
            cornerSprite.cornerType = cornerType;
            cornerSprite.sprites = new List<Sprite>();
            cornerSprite.sprites.Insert(0, null);
            return cornerSprite;
        }

        void ResetCornerList()
        {
            m_CornerSprites.Clear();
            m_CornerSprites.Insert(0, GetCornerSprite(CornerType.OuterTopLeft));
            m_CornerSprites.Insert(1, GetCornerSprite(CornerType.OuterTopRight));
            m_CornerSprites.Insert(2, GetCornerSprite(CornerType.OuterBottomLeft));
            m_CornerSprites.Insert(3, GetCornerSprite(CornerType.OuterBottomRight));
            m_CornerSprites.Insert(4, GetCornerSprite(CornerType.InnerTopLeft));
            m_CornerSprites.Insert(5, GetCornerSprite(CornerType.InnerTopRight));
            m_CornerSprites.Insert(6, GetCornerSprite(CornerType.InnerBottomLeft));
            m_CornerSprites.Insert(7, GetCornerSprite(CornerType.InnerBottomRight));
        }

        void OnValidate()
        {
            if (m_CornerSprites.Count != 8)
                ResetCornerList();
        }

        void Reset()
        {
            m_Angles.Clear();
            ResetCornerList();
        }

        internal static int GetSpriteShapeHashCode(SpriteShape spriteShape)
        {
            // useSpriteBorders, fillOffset and fillTexture are hashChecked elsewhere.

            unchecked
            {
                int hashCode = (int)2166136261;

                hashCode = hashCode * 16777619 ^ spriteShape.angleRanges.Count;

                for (int i = 0; i < spriteShape.angleRanges.Count; ++i)
                {
                    hashCode = hashCode * 16777619 ^ (spriteShape.angleRanges[i].GetHashCode() + i);
                }

                hashCode = hashCode * 16777619 ^ spriteShape.cornerSprites.Count;

                for (int i = 0; i < spriteShape.cornerSprites.Count; ++i)
                {
                    hashCode = hashCode * 16777619 ^ (spriteShape.cornerSprites[i].GetHashCode() + i);
                }

                return hashCode;
            }
        }

    }
}
