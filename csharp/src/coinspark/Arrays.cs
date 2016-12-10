namespace coinspark
{
    public class Arrays
    {
        public static int[] copyOfRange(int[] src, int start, int end)
        {
            int len = end - start;
            int[] dest = new int[len];
            // note i is always from 0
            for (int i = 0; i < len; i++)
            {
                dest[i] = src[start + i]; // so 0..n = 0+x..n+x
            }
            return dest;
        }
        public static byte[] copyOfRange(byte[] src, int start, int end)
        {
            int len = end - start;
            byte[] dest = new byte[len];
            // note i is always from 0
            for (int i = 0; i < len; i++)
            {
                dest[i] = src[start + i]; // so 0..n = 0+x..n+x
            }
            return dest;
        }
        public static CoinSparkIORange[] copyOfRange(CoinSparkIORange[] src, int start, int end)
        {
            int len = end - start;
            CoinSparkIORange[] dest = new CoinSparkIORange[len];
            // note i is always from 0
            for (int i = 0; i < len; i++)
            {
                dest[i] = src[start + i]; // so 0..n = 0+x..n+x
            }
            return dest;
        }

        public static byte[] copyOf(byte[] src, int len = -1)
        {
            if (len == -1)
                len = src.Length;

            return copyOfRange(src, 0, len);
        }

        public static CoinSparkIORange[] copyOf(CoinSparkIORange[] src, int len = -1)
        {
            if (len == -1)
                len = src.Length;

            return copyOfRange(src, 0, len);

        }

        public static byte[] fill(byte[] fill, byte excess)
        {
            
            return new byte[4];
        }
    }
}