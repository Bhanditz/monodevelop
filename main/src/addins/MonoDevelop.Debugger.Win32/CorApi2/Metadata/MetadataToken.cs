namespace CorApi2.Metadata
{
    public struct MetadataToken
    {
        public MetadataToken(uint value)
        {
            this.value = value;
        }

        public uint Value
        {
            get
            {
                return value;
            }
        }

        public MetadataTokenType Type
        {
            get
            {
                return (MetadataTokenType)(value & 0xFF000000);
            }
        }

        public uint Index
        {
            get
            {
                return value & 0x00FFFFFF;
            }
        }
        
        public static implicit operator uint(MetadataToken token) { return token.value; }
        public static bool operator==(MetadataToken v1, MetadataToken v2) { return (v1.value == v2.value);}
        public static bool operator!=(MetadataToken v1, MetadataToken v2) { return !(v1 == v2);}

        public static bool IsTokenOfType(uint token, params MetadataTokenType[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                if ((int)(token & 0xFF000000) == (int)types[i])
                    return true;
            }

            return false;
        }

        public bool IsOfType(params MetadataTokenType[] types) { return IsTokenOfType(Value, types); }

        public override bool Equals(object other)
        {
            if (other is MetadataToken)
            {
                MetadataToken oToken = (MetadataToken)other;
                return (value == oToken.value);
            }
            return false;
        }

        public override int GetHashCode() { return value.GetHashCode();}

        private uint value;
    }
}