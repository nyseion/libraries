/*
 * CoinSpark 2.1 - Java library
 *
 * Copyright (c) Coin Sciences Ltd
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software ist
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

/**
 * Base class implementing static utility functions and classes/utility functions used internally.
 */

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace coinspark
{
    public class CoinSparkBase
    {

//	Public constants
        internal static readonly byte COINSPARK_GENESIS_PREFIX = Convert.ToByte('g');
        internal static readonly byte COINSPARK_TRANSFERS_PREFIX = Convert.ToByte('t');
        internal static readonly byte COINSPARK_PAYMENTREF_PREFIX = Convert.ToByte('r');
        internal static readonly byte COINSPARK_MESSAGE_PREFIX = Convert.ToByte('m');

        internal static readonly long COINSPARK_SATOSHI_QTY_MAX = 2100000000000000L;

//	General public functions for managing CoinSpark metadata and bitcoin transaction output scripts    

        /**
         * Extracts OP_RETURN metadata (not necessarily CoinSpark data) from a bitcoin tx output script.
         * 
         * @param scriptPubKey Output script as hexadecimal.
         * @return byte [] | null Raw binary embedded metadata if found, null otherwise. 
         */

        public static byte[] scriptToMetadata(string scriptPubKey)
        {
            return scriptToMetadata(hexToByte(scriptPubKey));
        }

        /**
         * Extracts OP_RETURN metadata (not necessarily CoinSpark data) from a bitcoin tx output script.
         * 
         * @param scriptPubKey Output script as raw binary data.
         * @return byte [] | null Raw binary embedded metadata if found, null otherwise. 
         */

        public static byte[] scriptToMetadata(byte[] scriptPubKey)
        {
            if (scriptPubKey == null)
                return null;

            int scriptPubKeyLen = scriptPubKey.Length;
            int metadataLength = scriptPubKeyLen - 2; // Skip the signature

            if ((scriptPubKeyLen > 2) && (scriptPubKey[0] == 0x6a) &&
                (scriptPubKey[1] > 0) && (scriptPubKey[1] <= 75) && (scriptPubKey[1] == metadataLength))
            {
                return Arrays.copyOfRange(scriptPubKey, 2, scriptPubKeyLen);
            }

            return null;
        }

        /**
         * Extracts OP_RETURN metadata (not necessarily CoinSpark data) from a bitcoin tx output scripts.
         * 
         * @param scriptPubKeys Output scripts as hexadecimal.
         * @return metadata if found, null otherwise
         */

        public static byte[] scriptsToMetadata(string[] scriptPubKeys)
        {
            byte[][] raw = new byte[scriptPubKeys.Length][];

            for (int i = 0; i < scriptPubKeys.Length; i++)
            {
                raw[i] = hexToByte(scriptPubKeys[i]);
            }

            return scriptsToMetadata(raw);
        }

        /**
         * Extracts OP_RETURN metadata (not necessarily CoinSpark data) from a bitcoin tx output scripts.
         * 
         * @param scriptPubKeys Output scripts as raw binary data.
         * @return metadata if found, null otherwise
         */

        public static byte[] scriptsToMetadata(byte[][] scriptPubKeys)
        {
            foreach (byte[] scriptPubKey in scriptPubKeys)
            {
                if (!scriptIsRegular(scriptPubKey))
                {
                    return scriptToMetadata(scriptPubKey);
                }
            }

            return null;
        }

        /**
         * Converts CoinSpark metadata (or other data) into an OP_RETURN bitcoin tx output script.
         * 
         * @param metadata metadata  Raw binary metadata to be converted.
         * @return string | null The OP_RETURN bitcoin tx output script as hexadecimal, null if we failed.
         */

        public static string metadataToScriptHex(byte[] metadata)
        {
            return byteToHex(metadataToScript(metadata));
        }

        /**
         * Converts CoinSpark metadata (or other data) into an OP_RETURN bitcoin tx output script.
         * 
         * @param metadata metadata  Raw binary metadata to be converted.
         * @return byte [] | null The OP_RETURN bitcoin tx output script as raw binary, null if we failed.
         */

        public static byte[] metadataToScript(byte[] metadata)
        {
            byte[] scriptPubKey;
            if ((metadata != null && metadata.Length <= 75)) // && (scriptPubKeyMaxLen>=scriptLength) ) {
            {
                int scriptRawLen = metadata.Length + 2;

                scriptPubKey = new byte[scriptRawLen];

                scriptPubKey[0] = 0x6a;
                scriptPubKey[1] = (byte) metadata.Length;
                Array.Copy(metadata, 0, scriptPubKey, 2, metadata.Length);
                //   System.arraycopy(metadata, 0, scriptPubKey, 2, metadata.Length);
                return scriptPubKey;
            }

            return null;
        }

        /**
         * Calculates the maximum length of CoinSpark metadata that can be added to some existing CoinSpark metadata
         * to fit into a specified number of bytes.
         * 
         * The calculation is not simply metadataMaxLen-metadata.Length because some space is saved when combining pieces of CoinSpark metadata together.
         *
         * @param metadata The existing CoinSpark metadata in raw binary form, which can itself already be
         *    a combination of more than one CoinSpark metadata element.
         * @param metadataMaxLen The total number of bytes available for the combined metadata.
         * @return integer The number of bytes which are available for the new piece of metadata to be added.
         */

        public static int metadataMaxAppendLen(byte[] metadata, int metadataMaxLen)
        {
            return Math.Max(metadataMaxLen - (metadata.Length + 1 - COINSPARK_METADATA_IDENTIFIER_LEN), 0);
        }

        /**
         * Appends one piece of CoinSpark metadata to another.
         * 
         * @param metadata The existing CoinSpark metadata in raw binary form (or null), which can itself already be
         *    a combination of more than one CoinSpark metadata element. 
         * @param metadataMaxLen The total number of bytes available for the combined metadata.
         * @param appendMetadata The new CoinSpark metadata to be appended, in raw binary form.
         * @return byte [] | null The combined CoinSpark metadata as raw binary, or null if we failed.
         */

        public static byte[] metadataAppend(byte[] metadata, int metadataMaxLen, byte[] appendMetadata)
        {
            if (metadata == null) // metadata == null case
            {
                if (metadataMaxLen < appendMetadata.Length) // check append metdata is short enough
                    return null;

                return appendMetadata;
            }

            CoinSparkBuffer oldBuffer = new CoinSparkBuffer(metadata);

            if (!oldBuffer.locateRange(0)) // check we can find last metadata
                return null;

            if (appendMetadata.Length < COINSPARK_METADATA_IDENTIFIER_LEN + 1)
                // check there is enough to check the prefix
                return null;

            if (
                memcmp(Encoding.UTF8.GetBytes(COINSPARK_METADATA_IDENTIFIER), appendMetadata,
                    COINSPARK_METADATA_IDENTIFIER_LEN) != 0) // check the prefix
                return null;

            int needLength = metadata.Length + appendMetadata.Length - COINSPARK_METADATA_IDENTIFIER_LEN + 1;
            if (metadataMaxLen < needLength) // check there is enough space
                return null;


            int lastMetadataLen = oldBuffer.availableForRead() + 1; // include prefix
            int lastMetaDataPos = oldBuffer.offsetRead - 1;

            CoinSparkBuffer newBuffer = new CoinSparkBuffer();
            newBuffer.writeBytes(metadata, lastMetaDataPos); // Data before last metadata
            newBuffer.writeByte((byte) lastMetadataLen); // Length prefix for last metadata
            newBuffer.writeByte(metadata[lastMetaDataPos]); // Last metadata prefix
            newBuffer.writeBytes(oldBuffer.readBytes(oldBuffer.availableForRead()));
                // Last metadata without identifier and prefix

            newBuffer.writeBytes(Arrays.copyOfRange(appendMetadata, COINSPARK_METADATA_IDENTIFIER_LEN,
                appendMetadata.Length)); // Appended metadata

            return newBuffer.toBytes();
        }

        /**
         * Tests whether a bitcoin tx output script is 'regular', i.e. not an OP_RETURN script.
         * 
         * This function will declare empty scripts or invalid hex scripts as 'regular' as well, since they are not OP_RETURNs.
         * Use this to build $outputsRegular arrays which are used by various other functions.
         * 
         * @param scriptPubKey Output script as hexadecimal.
         * @return true if the script is 'regular', false if it is an OP_RETURN script.
         */

        public static bool scriptIsRegular(string scriptPubKey)
        {
            return scriptIsRegular(hexToByte(scriptPubKey));
        }

        /**
         * Tests whether a bitcoin tx output script is 'regular', i.e. not an OP_RETURN script.
         * 
         * This function will declare empty scripts or invalid hex scripts as 'regular' as well, since they are not OP_RETURNs.
         * Use this to build $outputsRegular arrays which are used by various other functions.
         * 
         * @param scriptPubKey Output script as raw binary data.
         * @return true if the script is 'regular', false if it is an OP_RETURN script.
         */

        public static bool scriptIsRegular(byte[] scriptPubKey)
        {
            return (scriptPubKey.Length < 1) || (scriptPubKey[0] != 0x6a);
        }



//	Utitlity functions/classes used internally in CoinSpark Library    

        private static readonly long COINSPARK_FEE_BASIS_MAX_SATOSHIS = 1000;

        internal static long getMinFeeBasis(long[] outputsSatoshis, bool[] outputsRegular)
        {
            if (outputsSatoshis.Length != outputsRegular.Length)
            {
                return COINSPARK_SATOSHI_QTY_MAX;
            }
            int countOutputs = outputsRegular.Length;
            long smallestOutputSatoshis = COINSPARK_SATOSHI_QTY_MAX;

            for (int outputIndex = 0; outputIndex < countOutputs; outputIndex++)
            {
                if (outputsRegular[outputIndex])
                    if (smallestOutputSatoshis > outputsSatoshis[outputIndex])
                        smallestOutputSatoshis = outputsSatoshis[outputIndex];
            }

            if (smallestOutputSatoshis > COINSPARK_FEE_BASIS_MAX_SATOSHIS)
                smallestOutputSatoshis = COINSPARK_FEE_BASIS_MAX_SATOSHIS;
            return smallestOutputSatoshis;
        }

        internal static readonly string COINSPARK_METADATA_IDENTIFIER = "SPK";

        private static readonly byte[] hexCharMap =
        {
            Convert.ToByte('0'), Convert.ToByte('1'), Convert.ToByte('2'),
            Convert.ToByte('3'), Convert.ToByte('4'), Convert.ToByte('5'), Convert.ToByte('6'), Convert.ToByte('7'),
            Convert.ToByte('8'), Convert.ToByte('9'), Convert.ToByte('A'), Convert.ToByte('B'), Convert.ToByte('C'),
            Convert.ToByte('D'), Convert.ToByte('E'), Convert.ToByte('F')
        };

        private static readonly int[] base58Minus49ToInteger =
        {
            // 74 elements
            0, 1, 2, 3, 4, 5, 6, 7, 8, -1, -1, -1, -1, -1, -1, -1,
            9, 10, 11, 12, 13, 14, 15, 16, -1, 17, 18, 19, 20, 21, -1, 22,
            23, 24, 25, 26, 27, 28, 29, 30, 31, 32, -1, -1, -1, -1, -1, -1,
            33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, -1, 44, 45, 46, 47,
            48, 49, 50, 51, 52, 53, 54, 55, 56, 57
        };

        private static readonly int COINSPARK_METADATA_IDENTIFIER_LEN = 3;
        private static readonly int COINSPARK_LENGTH_PREFIX_MAX = 96;

        /**
         * Returns SHA256 hash of the input raw data
         * @param input input raw data
         * @param inputLen actual size of the data to hash (raw data may be longer)
         * C function: void CoinSparkCalcSHA256Hash(const unsigned char* input, const size_t inputLen, unsigned char hash[32]);
         * @return SHA-256 hash 
         */

        internal static byte[] coinSparkCalcSHA256Hash(byte[] input, int inputLen)
        {
            SHA256 crypt = SHA256.Create();
            byte[] hashInput = new byte[inputLen];
            Array.Copy(input, hashInput, inputLen);
            return crypt.ComputeHash(hashInput);
        }


        internal static int base58ToInteger(byte base58Character) // returns -1 if invalid
        {
            if ((base58Character < 49) || (base58Character > 122))
                return -1;

            return base58Minus49ToInteger[base58Character - 49];
        }

        internal static string byteToHex(byte[] raw)
        {
            if (raw == null)
                return null;

            byte[] hexBytes = new byte[raw.Length*2];

            for (int i = 0; i < raw.Length; i++)
            {
                short value = (short) (raw[i] & 0xFF);
                hexBytes[2*i + 0] = hexCharMap[(byte) (value >> 4)];
                hexBytes[2*i + 1] = hexCharMap[(byte) (value & 15)];
            }
            return Encoding.UTF8.GetString(hexBytes);
        }

        internal static byte[] hexToByte(string hex)
        {
            if (hex == null)
                return null;

            return Enumerable.Range(0, hex.Length/2).Select(x => Convert.ToByte(hex.Substring(x*2, 2), 16)).ToArray();

        }

        internal static byte[] unsignedToSmallEndianBytes(long value, int bytes)
        {
            if (bytes <= 0)
                return null;

            byte[] raw = new byte[bytes];

            for (int index = 0; index < bytes; index++)
            {
                raw[index] = (byte) (value & 0xFF);
                value /= 256;
            }

            return raw;
        }

        internal static string unsignedToSmallEndianHex(long value, int bytes)
        {
            return byteToHex(unsignedToSmallEndianBytes(value, bytes));
        }

        internal static long SmallEndianBytesToUnsigned(byte[] dataBuffer, int offset, int bytes)
        {
            //  if (bytes < 0 || offset < 0 || bytes+offset >dataBuffer.Length)
            //      return null;

            long value = 0;

            for (int curbyte = bytes - 1; curbyte >= 0; curbyte--)
            {
                value *= 256;
                int ttt = dataBuffer[curbyte + offset];
                if (ttt < 0) ttt &= 0xFF;
                value += ttt;
            }

            return value & 0x7FFFFFFFFFFFFFFFL;
        }

        internal static int memcmp(byte[] b1, byte[] b2, int sz)
        {
            for (int i = 0; i < sz; i++)
            {
                if (b1[i] != b2[i])
                {
                    if ((b1[i] >= 0 && b2[i] >= 0) || (b1[i] < 0 && b2[i] < 0))
                        return b1[i] - b2[i];
                    if (b1[i] < 0 && b2[i] >= 0)
                        return 1;
                    if (b2[i] < 0 && b1[i] >= 0)
                        return -1;
                }
            }
            return 0;
        }


        internal static int getLastRegularOutput(bool[] outputsRegular)
        {
            int countOutputs = outputsRegular.Length;
            int outputIndex;

            for (outputIndex = countOutputs - 1; outputIndex >= 0; outputIndex--)
            {
                if (outputsRegular[outputIndex])
                    return outputIndex;
            }

            return countOutputs; // indicates no regular ones were found
        }

        internal class CoinSparkBuffer
        {
            private static readonly int BUFFER_ALLOC_LENGTH = 40;

            private readonly bool resizable;
            private byte[] raw;
            internal int offsetRead;
            private int offsetWrite;
            private int sizeRead;


            internal CoinSparkBuffer(int size)
            {
                raw = new byte[size];
                offsetRead = 0;
                offsetWrite = 0;
                sizeRead = offsetWrite;
                resizable = false;
            }

            internal CoinSparkBuffer()
            {
                raw = new byte[BUFFER_ALLOC_LENGTH];
                offsetRead = 0;
                offsetWrite = 0;
                sizeRead = offsetWrite;
                resizable = true;
            }

            internal CoinSparkBuffer(string Source, bool IsHex)
            {
                raw = IsHex ? hexToByte(Source) : Encoding.UTF8.GetBytes(Source);
                offsetRead = 0;
                offsetWrite = raw.Length;
                sizeRead = offsetWrite;
                resizable = false;
            }

            internal CoinSparkBuffer(byte[] Source)
            {
                raw = Source;
                offsetRead = 0;
                offsetWrite = raw.Length;
                sizeRead = offsetWrite;
                resizable = false;
            }

            internal int length()
            {
                return offsetWrite;
            }

            internal int availableForRead()
            {
                return sizeRead - offsetRead;
            }

            internal void resetReadOffset()
            {
                offsetRead = 0;
                sizeRead = offsetWrite;
            }

            internal void resetWriteOffset()
            {
                offsetWrite = 0;
            }

            internal bool realloc(int bytes)
            {
                int size = raw.Length;
                while (size < offsetWrite + bytes)
                {
                    size += BUFFER_ALLOC_LENGTH;
                }
                if (size > raw.Length)
                {
                    if (!resizable)
                    {
                        return false;
                    }
                    byte[] newRaw = new byte[size];
                    Array.Copy(raw, 0, newRaw, 0, offsetWrite);
                    // System.arraycopy(raw, 0, newRaw, 0, offsetWrite);
                    raw = newRaw;
                }
                return true;
            }

            internal byte[] toBytes()
            {
                if (offsetWrite == 0)
                {
                    return null;
                }

                byte[] newBytes = new byte[offsetWrite];
                Array.Copy(raw, newBytes, offsetWrite);
                return newBytes;
                //     return Arrays.copyOf(raw, offsetWrite);
            }

            internal string toAscii()
            {
                return Encoding.UTF8.GetString(toBytes());
            }

            internal string toHex()
            {
                return byteToHex(toBytes());
            }

            internal bool writeByte(byte b)
            {
                if (!realloc(1))
                    return false;

                raw[offsetWrite] = b;
                offsetWrite++;
                return true;
            }

            internal bool writeInt(int value, int size)
            {
                return writeLong(value, size);
            }

            internal bool writeLong(long value, int size)
            {
                return writeBytes(unsignedToSmallEndianBytes(value, size));
            }

            internal bool writeBytes(byte[] b)
            {
                if (b == null)
                    return true;

                return writeBytes(b, b.Length);
            }

            internal bool writeBytes(byte[] b, int size)
            {
                if (size <= 0)
                    return true;

                if (!realloc(size))
                    return false;
                Array.Copy(b, 0, raw, offsetWrite, size);
                //System.arraycopy(b, 0, raw, offsetWrite, size);

                offsetWrite += size;
                return true;
            }

            internal bool writeString(string s)
            {
                return writeBytes(Encoding.UTF8.GetBytes(s));
            }

            internal Byte readByte()
            {
                //   if(offsetRead>=sizeRead)
                //    return null;

                offsetRead++;
                return raw[offsetRead - 1];
            }

            internal long readLong(int size)
            {
                // if(offsetRead+size>sizeRead)
                //  return null;

                offsetRead += size;

                return SmallEndianBytesToUnsigned(raw, offsetRead - size, size);
            }

            internal int readInt(int size)
            {
                //    if(size>4)
                //        return null;

                //    if(offsetRead+size>sizeRead)
                //     return null;

                return (int) readLong(size);
            }

            internal byte[] readBytes(int size)
            {
                if (offsetRead + size > sizeRead)
                    return null;

                offsetRead += size;

                return Arrays.copyOfRange(raw, offsetRead - size, offsetRead);
            }


            internal bool canRead(int size)
            {
                if (offsetRead + size > sizeRead)
                    return false;
                return true;
            }

            internal bool locateRange(byte desiredPrefix)
            {
                offsetRead = 0;
                if (!canRead(COINSPARK_METADATA_IDENTIFIER_LEN + 1))
                    return false;

                if (memcmp(Encoding.UTF8.GetBytes(COINSPARK_METADATA_IDENTIFIER), raw, COINSPARK_METADATA_IDENTIFIER_LEN) != 0) // check it starts 'SPK'
                    return false;

                offsetRead += COINSPARK_METADATA_IDENTIFIER_LEN; // skip past 'SPK'

                while (offsetRead < offsetWrite)
                {
                    byte foundPrefix = readByte(); // read the next prefix

                    if (desiredPrefix != 0 ? (foundPrefix == desiredPrefix) : (foundPrefix > COINSPARK_LENGTH_PREFIX_MAX))
                    {
                        // it's our data from here to the end (if desiredPrefix is 0, it matches the last one whichever it is)
                        sizeRead = offsetWrite;
                        return true;
                    }

                    if (foundPrefix > COINSPARK_LENGTH_PREFIX_MAX) // it's some other type of data from here to end
                        return false;

                    // if we get here it means we found a length byte

                    if (offsetRead + foundPrefix > offsetWrite) // something went wrong - length indicated is longer than that available
                        return false;

                    if (offsetRead >= offsetWrite) // something went wrong - that was the end of the input data
                        return false;

                    if (raw[offsetRead] == desiredPrefix)
                    {
                        // it's the length of our part
                        offsetRead++;
                        sizeRead = offsetRead + foundPrefix - 1;
                        return true;
                    }
                    offsetRead += foundPrefix;
                }

                return false;
            }

        }

    }
}