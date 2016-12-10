/*
 * CoinSpark 2.1 - .NET C# library (derived from the CoinSpark 2.1 Java library)
 *
 * Copyright (c) Coin Sciences Ltd
 * Modified work Copyright (c) Vincent M. Mele, nyseion.com
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
using System;
using System.Linq;
using System.Text;

namespace coinspark
{

//import java.util.Arrays;

/**
         * CoinSparkMessage class for managing CoinSpark messages
         */


    public class CoinSparkMessage : CoinSparkBase
    {

// Public functions    

    /**
     * Returns Server Host
     * 
     * @return Server Host
     */

    public string getServerHost()
    {
        return serverHost;
    }

    /**
     * Sets DomainName
     * 
     * @param ServerHost to set
     */

    public void setServerHost(string ServerHost)
    {
        serverHost = ServerHost;
    }

    /**
     * Returns Server Path
     * 
     * @return Server Path
     */

    public string getServerPath()
    {
        return serverPath;
    }

    /**
     * Sets Server Path
     * 
     * @param Server to set
     */

    public void setServerPath(string Server)
    {
        serverPath = Server;
    }

    /**
     * Returns Use https flag
     * 
     * @return Use https flag
     */

    public bool getUseHttps()
    {
        return useHttps;
    }

    /**
     * Sets Use https flag
     * 
     * @param UseHttps to set
     */

    public void setUseHttps(bool UseHttps )
    {
        useHttps = UseHttps;
    }

    /**
     * Returns Use path prefix flag
     * 
     * @return Use path prefix flag
     */

    public bool getUsePrefix()
    {
        return usePrefix;
    }

    /**
     * Sets Use path prefix flag
     * 
     * @param UsePrefix flag to set
     */

    public void setUsePrefix(bool UsePrefix )
    {
        usePrefix = UsePrefix;
    }

    /**
     * Sets all components of the server URL
     * 
     * @param DomainPath 
     */

    public void setDomainPath(CoinSparkDomainPath DomainPath)
    {
        serverHost = DomainPath.domainName;
        serverPath = DomainPath.path;
        useHttps = DomainPath.useHttps;
        usePrefix = DomainPath.usePrefix;
    }

    /**
     * Returns is public flag
     * 
     * @return is public flag
     */

    public bool getIsPublic()
    {
        return isPublic;
    }

    /**
     * Sets is public flag
     * 
     * @param IsPublic flag to set
     */

    public void setIsPublic(bool IsPublic )
    {
        isPublic = IsPublic;
    }

/**
             * Returns Message hash
             * 
             * @return Message hash
             */

    public byte[] getHash()
    {
        return hash;
    }

    /**
     * Returns Message hash length (to be) encoded
     * 
     * @return Message hash length
     */

    public int getHashLen()
    {
        return hashLen;
    }

    /**
     * Sets message hash.
     * 
     * @param Hash to set
     */

    public void setHash(byte[] Hash)
    {
        if (Hash == null)
        {
            hash = null;
        }
        else
        {
            hash = Arrays.copyOfRange(Hash, 0, Hash.Length);
        }
    }

    /**
     * Sets message hash length.
     * 
     * @param HashLen to set
     */

    public void setHashLen(int HashLen)
    {
        hashLen = HashLen;
    }

    /**
     * Returns array of output ranges
     * 
     * @return array of output ranges
     */

    public CoinSparkIORange[] getOutputRanges()
    {
        return Arrays.copyOf(outputRanges, countOutputRanges);
    }

    /**
     * Sets array of output ranges
     * 
     * @param OutputRanges to set
     */

    public void setOutputRanges(CoinSparkIORange[] OutputRanges)
    {
        countOutputRanges = OutputRanges.Length;
        outputRanges = Arrays.copyOf(OutputRanges, countOutputRanges);
    }

    /**
     * Adds OutputRange to the list
     * 
     * @param OutputRange to add
     */

    public void addOutputs(CoinSparkIORange OutputRange)
    {
        if (COINSPARK_MESSAGE_MAX_IO_RANGES > countOutputRanges)
        {
            outputRanges[countOutputRanges] = OutputRange;
            countOutputRanges++;
        }
    }

    /**
     * CoinSparkMessage class for managing CoinSpark messages
     */

    public CoinSparkMessage()
    {
        clear();
    }

    /**
     * Set all fields in address to their default/zero values, which are not necessarily valid.
     */

    public void clear()
    {
        useHttps = false;
        serverHost = "";
        usePrefix = false;
        serverPath = "";
        isPublic = false;
        outputRanges = new CoinSparkIORange[COINSPARK_MESSAGE_MAX_IO_RANGES];
        countOutputRanges = 0;
        hash = new byte[COINSPARK_MESSAGE_HASH_MAX_LEN];
        hashLen = 0;
    }

    /**
     * Returns full URL of delivery server.
     * 
     * @return full URL of delivery server.
     */

    public string getFullURL()
    {
        return (new CoinSparkDomainPath(serverHost, serverPath, useHttps, usePrefix)).getFullURL();
    }

   // @Override

    public override string ToString()
    {
        CoinSparkBuffer assetWebPageBuffer = new CoinSparkBuffer();
        string encodedWebPage = "";

        CoinSparkDomainPath assetWebPage = new CoinSparkDomainPath(serverHost, serverPath, useHttps, usePrefix);
        if (assetWebPage.encode(assetWebPageBuffer, true))
        {
            encodedWebPage = assetWebPageBuffer.toHex();
        }

        string urlString = calcServerUrl();

        StringBuilder sb = new StringBuilder();
        sb.Append("COINSPARK MESSAGE\n");
        sb.Append(string.Format("    Server URL: {0} (length {1}+{2} encoded {3} length {4})\n",
            urlString,
            serverHost.Length,
            serverPath.Length,
            encodedWebPage,
            assetWebPage.encodedLen(true)));
        sb.Append(string.Format("Public message: {0}\n", isPublic ? "yes" : "no"));

        for (int index = 0; index < countOutputRanges; index++)
        {
            if (outputRanges[index].count > 0)
            {
                if (outputRanges[index].count > 1)
                {
                    sb.Append(string.Format("       Outputs: {0} - {1} (count {2})",
                        outputRanges[index].first,
                        outputRanges[index].first + outputRanges[index].count - 1,
                        outputRanges[index].count));
                }
                else
                {
                    sb.Append(string.Format("        Output: {0}", outputRanges[index].first));
                }
                sb.Append(string.Format(" (small endian hex: first {0} count {1})\n",
                    unsignedToSmallEndianHex(outputRanges[index].first, 2),
                    unsignedToSmallEndianHex(outputRanges[index].count, 2)));
            }
            else
            {
                sb.Append("       Outputs: none");
            }

        }

        sb.Append("  Message hash: ");
        sb.Append(byteToHex(Arrays.copyOf(hash, hashLen)));
        sb.Append(string.Format(" (length {0})\n", hashLen));

        sb.Append("END COINSPARK MESSAGE\n\n");
        return sb.ToString();
    }

    /**
     * Returns true if all values in the message are in their permitted ranges, false otherwise.
     * 
     * @return true if genesis structure is valid
     */

    public bool isValid()
    {
        if (serverHost.Length > COINSPARK_MESSAGE_SERVER_HOST_MAX_LEN)
            return false;

        if (serverPath.Length > COINSPARK_MESSAGE_SERVER_PATH_MAX_LEN)
            return false;

        if (hash.Length < hashLen) // check we have at least as much data as specified by $this->hashLen
            return false;

        if ((hashLen < COINSPARK_MESSAGE_HASH_MIN_LEN) || (hashLen > COINSPARK_MESSAGE_HASH_MAX_LEN))
            return false;

        if ((!isPublic) && (countOutputRanges == 0)) // public or aimed at some outputs at least
            return false;

        if (countOutputRanges > COINSPARK_MESSAGE_MAX_IO_RANGES)
            return false;

        for (int index = 0; index < countOutputRanges; index++)
        {
            if (!outputRanges[index].isValid())
            {
                return false;
            }
        }

        return true;
    }

    /**
     * Returns true if the two CoinSparkMessage structures are the same. If strict is true then
     * the OutputRanges must be identical.
     * If strict is false then only normalized OutputRanges must be identical.
     *
     * @param message2 CoinSparkMessage to compare with
     * @param strict Strict comparison flag
     * @return true if two CoinSparkMessage match, false otherwise
     */


    public bool match(CoinSparkMessage message2, bool strict )
    {
        int hashCompareLen = Math.Min(Math.Min(hashLen, message2.getHashLen()), COINSPARK_MESSAGE_HASH_MAX_LEN);

        CoinSparkIORange[] thisRanges;
        CoinSparkIORange[] otherRanges;


        thisRanges = getOutputRanges();
        otherRanges = message2.getOutputRanges();
        if (!strict)
        {
            thisRanges = CoinSparkIORange.normalizeIORanges(thisRanges);
            otherRanges = CoinSparkIORange.normalizeIORanges(otherRanges);
        }

        if (thisRanges.Length != otherRanges.Length)
            return false;

        for (int index = 0; index < thisRanges.Length; index++)
        {
            if (!thisRanges[index].match(otherRanges[index]))
                return false;
        }

        byte[] otherHash = message2.getHash();
        for (int index = 0; index < hashCompareLen; index++)
        {
            if (hash[index] != otherHash[index])
                return false;
        }

        return (useHttps == message2.getUseHttps()) &&
               serverHost.ToLower().SequenceEqual(message2.getServerHost().ToLower()) &&
               (usePrefix == message2.getUsePrefix()) &&
               serverPath.ToLower().SequenceEqual(message2.getServerPath().ToLower()) &&
               (isPublic == message2.getIsPublic());
    }

    /**
     * Encodes the message into metadata (maximal size is metadataMaxLen);
     * 
     * @param countOutputs number of outputs in transaction
     * @param metadataMaxLen maximal size of encoded data
     * @return string | null Encoded message as hexadecimal, null if we failed.
     */

    public string encodeToHex(int countOutputs, int metadataMaxLen)
    {
        CoinSparkBuffer buffer = new CoinSparkBuffer();
        if (!encode(buffer, countOutputs, metadataMaxLen))
        {
            return null;
        }

        return buffer.toHex();
    }

    /**
     * Encodes the message into metadata (maximal size is metadataMaxLen);
     * 
     * @param countOutputs number of outputs in transaction
     * @param metadataMaxLen maximal size of encoded data
     * @return byte [] | null Encoded message as hexadecimal, null if we failed.
     */

    public byte[] encode(int countOutputs, int metadataMaxLen)
    {
        CoinSparkBuffer buffer = new CoinSparkBuffer();

        if (!encode(buffer, countOutputs, metadataMaxLen))
        {
            return null;
        }

        return buffer.toBytes();
    }

    /**
     * Decodes the message.
     * 
     * @param countOutputs number of outputs in transaction
     * @param metadata Metadata to decode as hexadecimal
     * @return true on success, false on failure
     */

    public bool decode(string metadata, int countOutputs)
    {
        CoinSparkBuffer buffer = new CoinSparkBuffer(metadata, true);
        return decode(buffer, countOutputs);
    }

    /**
     * Decodes the message.
     * 
     * @param countOutputs number of outputs in transaction
     * @param metadata Metadata to decode as raw data
     * @return true on success, false on failure
     */

    public bool decode(byte[] metadata, int countOutputs)
    {
        CoinSparkBuffer buffer = new CoinSparkBuffer(metadata);
        return decode(buffer, countOutputs);
    }

    /**
     * Calculates the hash for the specific set of ContentParts
     * 
     * @param salt  salt parameter
     * @param messageParts content parts to hash
     * @return message hash or null on failure
     */

    public static byte[] calcMessageHash(byte[] salt, CoinSparkMessagePart[] messageParts)
    {
        if (salt == null)
        {
            salt = new byte[0];
        }

        int bufferSize = 16 + salt.Length + messageParts.Length*3;

        foreach (CoinSparkMessagePart part in messageParts)
        {
            // Avoid null pointer exception when accessing length attribute later
            if (part.content == null)
            {
                return null;
            }

            bufferSize += (part.mimeType != null) ? Encoding.UTF8.GetBytes(part.mimeType).Length : 0;
            bufferSize += (part.fileName != null) ? Encoding.UTF8.GetBytes(part.fileName).Length : 0;
            bufferSize += part.content.Length;
        }

        byte[] buffer = new byte[bufferSize];
        int offset = 0;

        if (salt.Length > 0)
        {
                Array.Copy(salt, 0, buffer, offset, salt.Length);
        }
        offset += salt.Length + 1;
        buffer[offset - 1] = 0x00;
        foreach (CoinSparkMessagePart part in messageParts)
        {
            if ((part.mimeType != null) && (part.mimeType.Length > 0))
            {
                    Array.Copy(Encoding.UTF8.GetBytes(part.mimeType), 0, buffer, offset, part.mimeType.Length);
                    offset += part.mimeType.Length;
            }
            offset += 1;
            buffer[offset - 1] = 0x00;
            if ((part.fileName != null) && (part.fileName.Length > 0))
            {
                Array.Copy(Encoding.UTF8.GetBytes(part.fileName), 0, buffer, offset, part.fileName.Length);
              //  System.arraycopy(Encoding.UTF8.GetBytes(part.fileName), 0, buffer, offset, part.fileName.Length);
                offset += part.fileName.Length;
            }
            offset += 1;
            buffer[offset - 1] = 0x00;
            if (part.content.Length > 0)
            {
                    Array.Copy(part.content, 0, buffer, offset, part.content.Length);
                   // System.arraycopy(part.content, 0, buffer, offset, part.content.Length);
            }
            offset += part.content.Length + 1;
            buffer[offset - 1] = 0x00;
        }

        return coinSparkCalcSHA256Hash(buffer, offset);
    }

    /**
     * Returns true if message has specified output in its ranges
     * 
     * @param outputIndex output index to check
     * @return true if message has specified output in its ranges, false otherwise
     */

    public bool hasOutput(int outputIndex)
    {
        for (int index = 0; index < countOutputRanges; index++)
        {
            if ((outputIndex >= outputRanges[index].first) &&
                (outputIndex < (outputRanges[index].first + outputRanges[index].count)))
                return true;
        }
        return false;
    }

    /**
     * Calculates the appropriate message hash length so that when encoded as metadata the genesis will
     * fit in metadataMaxLen bytes. For now, set metadataMaxLen to 40 (see Bitcoin's MAX_OP_RETURN_RELAY parameter).
     *
     * @param metadataMaxLen metadata maximal length
     * @return asset hash length of message
     */

    /**
     * Calculates the appropriate message hash length so that when encoded as metadata the genesis will
     * fit in metadataMaxLen bytes.For now, set metadataMaxLen to 40 (see Bitcoin's MAX_OP_RETURN_RELAY parameter).
     * 
     * @param countOutputs number of outputs in transaction
     * @param metadataMaxLen metadata maximal length
     * @return hash length of the message
     */

    public int calcHashLen(int countOutputs, int metadataMaxLen)
    {
        int len = metadataMaxLen - COINSPARK_METADATA_IDENTIFIER.Length - 1;

        CoinSparkDomainPath assetWebPage = new CoinSparkDomainPath(serverHost, serverPath, useHttps, usePrefix);
        len -= assetWebPage.encodedLen(true);

        if (isPublic)
            len--;

        for (int index = 0; index < countOutputRanges; index++)
        {
            int[] result = getOutputRangePacking(outputRanges[index], countOutputs);

            if (result != null)
            {
                len -= (1 + result[1] + result[2]);
            }
        }

        if (len > COINSPARK_MESSAGE_HASH_MAX_LEN)
            len = COINSPARK_MESSAGE_HASH_MAX_LEN;

        return len;
    }



// Private variables/constants/functions   

    private bool useHttps;
    private string serverHost;
    private bool usePrefix;
    private string serverPath;
    private bool isPublic;
    CoinSparkIORange[] outputRanges;
    private int countOutputRanges;
    private byte[] hash;
    private int hashLen;


    private static readonly int COINSPARK_MESSAGE_SERVER_HOST_MAX_LEN = 32;
    private static readonly int COINSPARK_MESSAGE_SERVER_PATH_MAX_LEN = 24;
    private static readonly int COINSPARK_MESSAGE_HASH_MIN_LEN = 12;
    private static readonly int COINSPARK_MESSAGE_HASH_MAX_LEN = 32;
    private static readonly int COINSPARK_MESSAGE_MAX_IO_RANGES = 16;


    private static readonly int COINSPARK_OUTPUTS_MORE_FLAG = 0x80;
    private static readonly int COINSPARK_OUTPUTS_RESERVED_MASK = 0x60;
    private static readonly int COINSPARK_OUTPUTS_TYPE_MASK = 0x18;
    private static readonly int COINSPARK_OUTPUTS_TYPE_SINGLE = 0x00; // one output index (0...7)
    private static readonly int COINSPARK_OUTPUTS_TYPE_FIRST = 0x08; // first (0...7) outputs
    private static readonly int COINSPARK_OUTPUTS_TYPE_UNUSED = 0x10; // for future use
    private static readonly int COINSPARK_OUTPUTS_TYPE_EXTEND = 0x18; // "extend", including public/all
    private static readonly int COINSPARK_OUTPUTS_VALUE_MASK = 0x07;
    private static readonly int COINSPARK_OUTPUTS_VALUE_MAX = 7;

    private string calcServerUrl()
    {
        string s = "";

        s += useHttps ? "https" : "http";
        s += "://" + serverHost + "/";
        s += usePrefix ? "coinspark/" : "";
        s += serverPath;
        s += (serverPath.Length > 0) ? "/" : "";

        return s.ToLower();
    }

    private int[] getOutputRangePacking(CoinSparkIORange outputRange, int countOutputs)
    {
        int[] result = new int[3];
        int packing;
        Byte packingExtend;

        bool[] packingOptions = CoinSparkPacking.getPackingOptions(null, outputRange, countOutputs, true);

        result[1] = 0;
        result[2] = 0;

        if (packingOptions[(int)CoinSparkPacking.PackingType._1_0_BYTE] &&
            (outputRange.first <= COINSPARK_OUTPUTS_VALUE_MAX)) // inline single output
            packing = COINSPARK_OUTPUTS_TYPE_SINGLE | (outputRange.first & COINSPARK_OUTPUTS_VALUE_MASK);

        else if (packingOptions[(int)CoinSparkPacking.PackingType._0_1_BYTE] &&
                 (outputRange.count <= COINSPARK_OUTPUTS_VALUE_MAX)) // inline first few outputs
            packing = COINSPARK_OUTPUTS_TYPE_FIRST | (outputRange.count & COINSPARK_OUTPUTS_VALUE_MASK);

        else
        {
            // we'll be taking additional bytes
            packingExtend = CoinSparkPacking.encodePackingExtend(packingOptions);
            if (packingExtend == null)
                return null;

            result = CoinSparkPacking.packingExtendAddByteCounts(packingExtend, result[1], result[2], true);

            packing = COINSPARK_OUTPUTS_TYPE_EXTEND | (packingExtend & COINSPARK_OUTPUTS_VALUE_MASK);
        }

        result[0] = packing;
        return result;
    }

    private bool encode(CoinSparkBuffer buffer, int countOutputs, int metadataMaxLen)
    {
        int packing, packingExtend;

        try
        {
            if (!isValid())
                throw new Exception("invalid message");

            //  4-character identifier

            buffer.writeString(COINSPARK_METADATA_IDENTIFIER);
            buffer.writeByte(COINSPARK_MESSAGE_PREFIX);

            //  Server host and path

            CoinSparkDomainPath assetWebPage = new CoinSparkDomainPath(serverHost, serverPath, useHttps, usePrefix);
            if (!assetWebPage.encode(buffer, true))
                throw new Exception("cannot write domain name/path");

            //  Output ranges

            if (isPublic)
            {
                // add public indicator first
                packing = ((countOutputRanges > 0) ? COINSPARK_OUTPUTS_MORE_FLAG : 0) |
                          COINSPARK_OUTPUTS_TYPE_EXTEND | CoinSparkPacking.COINSPARK_PACKING_EXTEND_PUBLIC;
                buffer.writeInt(packing, 1);
            }

            for (int index = 0; index < countOutputRanges; index++)
            {
                int firstBytes, countBytes;

                int[] result = getOutputRangePacking(outputRanges[index], countOutputs);
                if (result == null)
                    throw new Exception("invalid range");

                packing = result[0];
                firstBytes = result[1];
                countBytes = result[2];

                //  The packing byte

                if ((index + 1) < countOutputRanges)
                    packing |= COINSPARK_OUTPUTS_MORE_FLAG;

                buffer.writeInt(packing, 1);

                buffer.writeInt(outputRanges[index].first, firstBytes);
                buffer.writeInt(outputRanges[index].count, countBytes);
            }

            //  Message hash

            buffer.writeBytes(hash, hashLen);

            if (buffer.toBytes().Length> metadataMaxLen)
                throw new Exception("total length above limit");

        }
        catch (Exception ex)
        {
            //System.out.print(ex.getMessage());
            return false;
        }

        return true;
    }

    private bool decode(CoinSparkBuffer buffer, int countOutputs)
    {
        if (!buffer.locateRange(COINSPARK_MESSAGE_PREFIX))
            return false;

        try
        {
            //  Server host and path
            CoinSparkDomainPath assetWebPage = new CoinSparkDomainPath(serverHost, serverPath, useHttps, usePrefix);
            if (!assetWebPage.decode(buffer, true))
                throw new Exception("cannot decode server host");

            serverHost = assetWebPage.domainName;
            serverPath = assetWebPage.path;
            useHttps = assetWebPage.useHttps;
            usePrefix = assetWebPage.usePrefix;

            //  Output ranges

            isPublic = false;
            outputRanges = new CoinSparkIORange[COINSPARK_MESSAGE_MAX_IO_RANGES];
            countOutputRanges = 0;

            int packing = COINSPARK_OUTPUTS_MORE_FLAG;

            while ((packing & COINSPARK_OUTPUTS_MORE_FLAG) > 0)
            {
                if (buffer.canRead(1))
                {
                    packing = buffer.readInt(1); //  Read the next packing byte and check reserved bits are zero
                }
                else
                    throw new Exception("Cannot read packing");

                if ((packing & COINSPARK_OUTPUTS_RESERVED_MASK) > 0)
                    throw new Exception("reserved bits used in packing");

                int packingType = packing & COINSPARK_OUTPUTS_TYPE_MASK;
                int packingValue = packing & COINSPARK_OUTPUTS_VALUE_MASK;

                if ((packingType == COINSPARK_OUTPUTS_TYPE_EXTEND) &&
                    (packingValue == CoinSparkPacking.COINSPARK_PACKING_EXTEND_PUBLIC))
                {
                    isPublic = true; // special case for public messages
                }
                else
                {
                    //  Create a new output range			
                    if (countOutputRanges >= COINSPARK_MESSAGE_MAX_IO_RANGES) // too many output ranges
                        throw new Exception("too many output ranges");

                    int firstBytes = 0;
                    int countBytes = 0;
                    CoinSparkIORange outputRange;
                    //  Decode packing byte			
                    if (packingType == COINSPARK_OUTPUTS_TYPE_SINGLE) // inline single input
                    {
                        outputRange = new CoinSparkIORange();
                        outputRange.first = packingValue;
                        outputRange.count = 1;
                    }
                    else if (packingType == COINSPARK_OUTPUTS_TYPE_FIRST) // inline first few outputs
                    {
                        outputRange = new CoinSparkIORange();
                        outputRange.first = 0;
                        outputRange.count = packingValue;
                    }
                    else if (packingType == COINSPARK_OUTPUTS_TYPE_EXTEND) // we'll be taking additional bytes
                    {
                        CoinSparkPacking.PackingType extendPackingType;
                        extendPackingType = CoinSparkPacking.decodePackingExtend((byte) packingValue, true);
                        if (extendPackingType == CoinSparkPacking.PackingType._NONE)
                            throw new Exception("Wrong packing type");

                        outputRange = CoinSparkPacking.packingTypeToValues(extendPackingType, null, countOutputs);

                        int[] result =
                        CoinSparkPacking.packingExtendAddByteCounts(packingValue, firstBytes, countBytes, true);
                        firstBytes = result[1];
                        countBytes = result[2];
                    }
                    else
                        throw new Exception("unused packing type");


                    //  The index of the first output and number of outputs, if necessary

                    if (firstBytes > 0)
                    {
                        if (buffer.canRead(firstBytes))
                            outputRange.first = buffer.readInt(firstBytes);
                        else
                            throw new Exception("Cannot read first");
                    }

                    if (countBytes > 0)
                    {
                        if (buffer.canRead(countBytes))
                            outputRange.count = buffer.readInt(countBytes);
                        else
                            throw new Exception("Cannot read count");
                    }

                    outputRanges[countOutputRanges] = outputRange; //	Add on the new output range

                    countOutputRanges++;
                }

            }

            //  Message hash

            hashLen = buffer.availableForRead(); //TBD loss
            hashLen = Math.Min(hashLen, COINSPARK_MESSAGE_HASH_MAX_LEN); // apply maximum

            if (hashLen < COINSPARK_MESSAGE_HASH_MIN_LEN) // not enough hash data                
                throw new Exception("has data out of range");

            hash = buffer.readBytes(hashLen);
        }
        catch (Exception ex)
        {
           // System.out.print(ex.getMessage());
            return false;
        }

        return isValid();



    }


    }
}