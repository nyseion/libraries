/*
 * CoinSpark 2.1 - .NET C# library
 *
 * Copyright (c) Vincent M. Mele, nyseion.com
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
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