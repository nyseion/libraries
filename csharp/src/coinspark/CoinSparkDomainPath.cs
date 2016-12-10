/*
 * CoinSpark 2.1 - Java library
 *
 * Copyright (c) Coin Sciences Ltd
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

namespace coinspark { 
/**
 * CoinSpark class for managing Asset web pages structures
 */

public class CoinSparkDomainPath : CoinSparkBase{

    /**
     * Doiman name (without scheme).
     */
    
    internal string domainName;
    
    /**
     * Path used in asset web page URL construction, may be null. 
     * If null Spent txID and Vout are used.
     */
    
    internal string path;
    
    /**
     * Use Https flag.
     */
    
    internal bool useHttps;
    
    /**
     * Use "coinspark/" prefix in path flag.
     */
    
    internal bool usePrefix;

    /**
     * CoinSpark class for managing Asset web pages structures
     */
    
    internal CoinSparkDomainPath()
    {
        
    }
    
    /**
     * CoinSpark class for managing Asset web pages structures
     * 
     * @param DomainName domain name to set
     * @param Path path to set (may be null)
     * @param UseHttps use https
     * @param UsePrefix use "coinspark/" prefix
     */
    
    public CoinSparkDomainPath(string DomainName,string Path, bool UseHttps, bool UsePrefix)
    {
        domainName=DomainName;
        path=Path;
        useHttps=UseHttps;
        usePrefix=UsePrefix;
    }

    /**
     * Calculates the URL.
     * 
     * @return string FUll Domain/path URL 
     */
    
    public string getFullURL()
    {
        return string.Format("{0}://{1}/{2}{3}/", useHttps ? "https" : "http",domainName,usePrefix ? "coinspark/" : "",path).ToLower();
    }
    
    /**
     * Returns true if all values in the asset web page are in their permitted ranges, false otherwise.
     * 
     * @return true if asset web page structure is valid
     */
    
    internal bool isValid()
    {
        if ((domainName.Length > CoinSparkGenesis.COINSPARK_GENESIS_DOMAIN_NAME_MAX_LEN) || (path.Length > CoinSparkGenesis.COINSPARK_GENESIS_PAGE_PATH_MAX_LEN))
            return false;
                
        return true;
    }
    
    /**
     * Compares two CoinSpark asset web page structures references
     *  
     * @param page2 Asset web page structure to compare with
     * @return Returns true if the two Asset web page structures are identical.
     */
    
    internal bool match(CoinSparkDomainPath page2)
    {
        return 
                domainName.ToLower().SequenceEqual(page2.domainName.ToLower()) && 
                path.ToLower().SequenceEqual(page2.path.ToLower()) &&
                (useHttps == page2.useHttps) && 
                (usePrefix == page2.usePrefix);
    }
    
    /**
     * Calculates the URL for the asset web page of genesis.
     * 
     * @param FirstSpentTxID if path=null or path.Length=0 pass the previous txid whose output was spent by the first input of the genesis
     * @param FirstSpentVout if path=null or path.Length=0 pass the output index of firstSpentTxID spent by the first input of the genesis
     * @return string | null URL of the Asset web page, null on failure
     */
    
    internal string getAssetURL(string FirstSpentTxID,long FirstSpentVout)
    {
        if(domainName == null)
        {
            return null;
        }
        
        string suffix=path;

        
        if((suffix == null) || (suffix.Length == 0))
        {
            if(FirstSpentTxID.Length != 64)
            {
                return null;
            }
            int start= (int)(FirstSpentVout % 64);
            int charsToCopy=Math.Min(16, 64-start);
            suffix=FirstSpentTxID.Substring(start, charsToCopy);
            if(charsToCopy<16)
            {
                suffix+=FirstSpentTxID.Substring(0,16-charsToCopy); 
            }
        }
        
        return string.Format("{0}://{1}/{2}{3}/", useHttps ? "https" : "http",domainName,usePrefix ? "coinspark/" : "",suffix).ToLower();
    }
            
    /**
     * Calculates the URL for the home page, based on domain and useHttps flag
     * 
     * @return string | null URL of the domain home page, null on failure
     */
    
    internal string getDomainURL()
    {        
        if(domainName == null)
        {
            return null;
        }
        
        return string.Format("{0}://{1}", useHttps ? "https" : "http",domainName);
    }
        
// Private variables/constants/functions       
    
    
    private static readonly int COINSPARK_DOMAIN_PACKING_PREFIX_MASK     = 0xC0;
    private static readonly int COINSPARK_DOMAIN_PACKING_PREFIX_SHIFT    = 6;
    private static readonly byte COINSPARK_DOMAIN_PACKING_SUFFIX_MASK     = 0x3F;
    private static readonly int COINSPARK_DOMAIN_PACKING_SUFFIX_MAX      = 61;
    private static readonly int COINSPARK_DOMAIN_PACKING_SUFFIX_IPv4_NO_PATH  = 62; // messages only
    private static readonly int COINSPARK_DOMAIN_PACKING_SUFFIX_IPv4     = 63;
    private static readonly int COINSPARK_DOMAIN_PACKING_IPv4_HTTPS      = 0x40;
    private static readonly int COINSPARK_DOMAIN_PACKING_IPv4_NO_PATH_PREFIX   = 0x80;
    private static readonly int COINSPARK_DOMAIN_PATH_ENCODE_BASE        = 40;
    private static readonly int COINSPARK_DOMAIN_PATH_FALSE_MARKER       = 38;
    private static readonly int COINSPARK_DOMAIN_PATH_TRUE_MARKER        = 39;

    private static readonly string[] domainNamePrefixes = {
            "",
            "www."
    };
    private static readonly string[] domainNameSuffixes={      // 60, // leave space for 3 more in future
            "",

            // most common suffixes based on Alexa's top 1M sites as of 10 June 2014, with some manual adjustments

            ".at",
            ".au",
            ".be",
            ".biz",
            ".br",
            ".ca",
            ".ch",
            ".cn",
            ".co.jp",
            ".co.kr",
            ".co.uk",
            ".co.za",
            ".co",
            ".com.ar",
            ".com.au",
            ".com.br",
            ".com.cn",
            ".com.mx",
            ".com.tr",
            ".com.tw",
            ".com.ua",
            ".com",
            ".cz",
            ".de",
            ".dk",
            ".edu",
            ".es",
            ".eu",
            ".fr",
            ".gov",
            ".gr",
            ".hk",
            ".hu",
            ".il",
            ".in",
            ".info",
            ".ir",
            ".it",
            ".jp",
            ".kr",
            ".me",
            ".mx",
            ".net",
            ".nl",
            ".no",
            ".org",
            ".pl",
            ".ps",
            ".ro",
            ".ru",
            ".se",
            ".sg",
            ".tr",
            ".tv",
            ".tw",
            ".ua",
            ".uk",
            ".us",
            ".vn"
    };

    private static readonly string domainNameChars = "0123456789abcdefghijklmnopqrstuvwxyz-.<>"; 
   
    private string domainNameShort;
    private int domainNamePacking;    
    
        
    private bool shrink()
    {
        int bestPrefixLen=0;
        int bestPrefix=0;
        int bestSuffixLen=0;
        int bestSuffix=0;
        
        string source=domainName.ToLower();
        
        for (int prefixIndex=1; prefixIndex < domainNamePrefixes.Length; prefixIndex++)
        {
            int prefixLen = domainNamePrefixes[prefixIndex].Length;

            if(source.Length>prefixLen)
            {
                if (prefixLen>bestPrefixLen && domainNamePrefixes[prefixIndex].SequenceEqual(source.Substring(0, prefixLen)))
                {
                    bestPrefix=prefixIndex;
                    bestPrefixLen=prefixLen;
                }
            }
        }
        
        domainNameShort=source.Substring(bestPrefixLen);
        
        for (int suffixIndex=1; suffixIndex < domainNameSuffixes.Length; suffixIndex++)
        {
            int suffixLen = domainNameSuffixes[suffixIndex].Length;

            if(domainNameShort.Length>suffixLen)
            {
                if (suffixLen>bestSuffixLen && domainNameSuffixes[suffixIndex].SequenceEqual(domainNameShort.Substring(domainNameShort.Length-suffixLen)))
                {
                    bestSuffix=suffixIndex;
                    bestSuffixLen=suffixLen;
                }
            }
        }
        
        domainNameShort=domainNameShort.Substring(0, domainNameShort.Length-bestSuffixLen);
            
        domainNamePacking=((bestPrefix << COINSPARK_DOMAIN_PACKING_PREFIX_SHIFT)&COINSPARK_DOMAIN_PACKING_PREFIX_MASK)|
                 (bestSuffix & COINSPARK_DOMAIN_PACKING_SUFFIX_MASK);
        
        return true;
    }

    private bool expand()
    {
        int prefixIndex = (domainNamePacking & COINSPARK_DOMAIN_PACKING_PREFIX_MASK)>>COINSPARK_DOMAIN_PACKING_PREFIX_SHIFT;
        int suffixIndex = domainNamePacking & COINSPARK_DOMAIN_PACKING_SUFFIX_MASK;
        
        domainName = domainNamePrefixes[prefixIndex] + domainNameShort +  domainNameSuffixes[suffixIndex];
        
        return true;        
    }
    
    private int[] octetsIPV4()
    {
        int [] octets=new int[4];
        int octetNum, octetValue;
        char stringChar = '0';
        int i = 0;

        for (octetNum=0; octetNum<4; octetNum++)
        {
            octetValue=0;

            while (i < domainName.Length)
            {
                stringChar = domainName[(i++)];

                if ((stringChar>='0') && (stringChar<='9')) {
                    octetValue=octetValue*10+(stringChar-'0');
                    if (octetValue>255)
                        return null;

                }
                else if ((stringChar=='.') || (i >= domainName.Length))
                {
                    break;
                }
                else
                    return null;
            }

            octets[octetNum] = octetValue;
            if (octetNum < 3 && stringChar != '.')
                return null;

            if (octetNum == 3 && i != domainName.Length)
                return null;
        }

        return octets;        
    }
    
    private bool packString(CoinSparkBuffer buffer,string Source)
    {
        int strPos, strTriplet, strChar, strLen;
        
        strLen=Source.Length;
        try
        {        
            strTriplet=0;
            for (strPos = 0; strPos<Source.Length; strPos++) {

                int foundChar = domainNameChars.IndexOf(char.ToLower(Source[strPos]));
                if (foundChar == -1)
                    throw new Exception("Invalid character in packing source");
                strChar = foundChar;

                switch (strPos%3)
                {
                    case 0:
                        strTriplet=strChar;
                        break;

                    case 1:
                        strTriplet+=strChar*COINSPARK_DOMAIN_PATH_ENCODE_BASE;
                        break;

                    case 2:
                        strTriplet+=strChar*COINSPARK_DOMAIN_PATH_ENCODE_BASE*COINSPARK_DOMAIN_PATH_ENCODE_BASE;
                        break;
                }

                if ( ((strPos%3)==2) || (strPos==Source.Length-1) ) // write out 2 bytes if we've collected 3 chars, or if we're finishing
                { 
                    buffer.writeBytes(unsignedToSmallEndianBytes(strTriplet, 2));
                }
            }
        }
        catch (Exception cne)
        {
            Console.WriteLine(cne.Message);
            return false;
        }
        return true;
    }
    
    private byte [] unpackString(CoinSparkBuffer buffer,int parts)
    {
        int strPos, strTriplet, strChar;
        byte [] result=new byte[256];
        
        strTriplet=0;
        strChar=0;
        strPos=0;
        try
        {
            while(parts>0)    
            {
                if(strPos>=result.Length)
                {
                    throw new Exception("Domain/Path too long");                    
                }
                if ((strPos%3)==0)
                {
                    if(buffer.canRead(2))
                    {
                        strTriplet=buffer.readInt(2);
                        if (strTriplet >=
                                (COINSPARK_DOMAIN_PATH_ENCODE_BASE*COINSPARK_DOMAIN_PATH_ENCODE_BASE*COINSPARK_DOMAIN_PATH_ENCODE_BASE))
                            throw new Exception("Invalid value in Domain/Path");
                    }
                    else
                        throw new Exception("Ran out of characters");
                }

                switch (strPos%3)
                {
                    case 0:
                        strChar=strTriplet%COINSPARK_DOMAIN_PATH_ENCODE_BASE;
                        break;

                    case 1:
                        strChar=(strTriplet/COINSPARK_DOMAIN_PATH_ENCODE_BASE)%COINSPARK_DOMAIN_PATH_ENCODE_BASE;
                        break;

                    case 2:
                        strChar=strTriplet/(COINSPARK_DOMAIN_PATH_ENCODE_BASE*COINSPARK_DOMAIN_PATH_ENCODE_BASE);
                        break;
                }

                if ((strChar==COINSPARK_DOMAIN_PATH_FALSE_MARKER) || (strChar == COINSPARK_DOMAIN_PATH_TRUE_MARKER)) 
                {
                    parts--;
                }

                result[strPos] = (byte)domainNameChars[(strChar)];
                strPos++;
            }
        }
        catch (Exception cne)
        {
           Console.WriteLine(cne.Message);
            return null;
        }
        
        return  Arrays.copyOfRange(result, 0, strPos);
    }
    
    
    internal int encodedLen(bool forMessages)
    {
        int encodedLen=0;                
        int decodedLen=path.Length+1;
        
        if(octetsIPV4() != null)
        {
            encodedLen+=5;
            if(forMessages)
            {
                if(decodedLen==1)// will skip server path in this case
                {
                    decodedLen=0;
                }
            }
        }
        else
        {
            encodedLen+=1;
            shrink();
            decodedLen+=domainNameShort.Length+1;            
        }
        
        if(decodedLen>0)
        {
            encodedLen+=2*((decodedLen-1)/3+1);
        }
        
        return encodedLen;
    }
    
    internal bool encode(CoinSparkBuffer buffer,bool forMessages)
    {
        int [] octets;
        int parts=0;
        bool takePathPart=true;
        string stringToPack="";
        
        try
        {
            if( (octets=octetsIPV4()) != null)
            {
                byte temp = (byte)COINSPARK_DOMAIN_PACKING_SUFFIX_IPv4;
                if(forMessages  && (path.Length == 0))
                {
                    temp = (byte)COINSPARK_DOMAIN_PACKING_SUFFIX_IPv4_NO_PATH;
                    temp += (usePrefix ? (byte)COINSPARK_DOMAIN_PACKING_IPv4_NO_PATH_PREFIX : (byte)0);
                    takePathPart=false;
                }
                
                temp += (useHttps ? (byte)COINSPARK_DOMAIN_PACKING_IPv4_HTTPS : (byte)0);
                
                buffer.writeByte(temp);

                buffer.writeByte((byte)octets[0]);
                buffer.writeByte((byte)octets[1]);
                buffer.writeByte((byte)octets[2]);
                buffer.writeByte((byte)octets[3]);
            }
            else
            {
                if(!shrink())
                    throw new Exception("Cannot shrink domain");
                        
                if (domainNameShort.Length == 0)
                    throw new Exception("Zero-length packing source string");

                buffer.writeByte((byte)domainNamePacking);

                stringToPack+=domainNameShort + 
                        (useHttps ? domainNameChars[(COINSPARK_DOMAIN_PATH_TRUE_MARKER)] : domainNameChars[(COINSPARK_DOMAIN_PATH_FALSE_MARKER)]);
                parts++;
            }
            
            if(takePathPart)
            {
                stringToPack+=path.ToLower() + 
                        (usePrefix ? domainNameChars[(COINSPARK_DOMAIN_PATH_TRUE_MARKER)] : domainNameChars[(COINSPARK_DOMAIN_PATH_FALSE_MARKER)]);
                parts++;
            }           
            if(parts > 0)
            {
                if(!packString(buffer, stringToPack))
                {
                    throw new Exception("Cannot write domain and path");                    
                }
            }
        }
        catch (Exception cne)
        {
            Console.WriteLine(cne.Message);
            return false;
        }
        return true;
    }
            
    
    internal bool decode(CoinSparkBuffer buffer,bool forMessages)
    {
        byte packing, packingSuffix;
        int[] octets = new int[4];
        byte[] result = new byte[1];
        int parts=0;
        int pathPart;
        
        try
        {
            if (buffer.canRead(1))
            {
                packing=buffer.readByte();
            }
            else
                throw new Exception("Buffer is empty");

            packingSuffix=(byte)(packing & COINSPARK_DOMAIN_PACKING_SUFFIX_MASK);
            if ((packingSuffix==COINSPARK_DOMAIN_PACKING_SUFFIX_IPv4) ||
                (forMessages && (packingSuffix==COINSPARK_DOMAIN_PACKING_SUFFIX_IPv4_NO_PATH)))// check for IPv4 address
            {
                domainNamePacking=-1;
                useHttps = ((packing & COINSPARK_DOMAIN_PACKING_IPv4_HTTPS) != 0);

                if(buffer.canRead(4))
                {
                    for(int i=0;i<4;i++)
                    {
                        octets[i]=buffer.readByte();
                        if(octets[i]<0)
                            octets[i]+=256;
                    }
                }
                else
                    throw new Exception("Cannot read octets");

                domainName = string.Format("{0}.{1}.{2}.{3}", octets[0], octets[1], octets[2], octets[3]);

                if (domainName.Length >= 256) // allow for null terminator
                    throw new Exception("Domain name too long");
                
                if(forMessages && (packingSuffix==COINSPARK_DOMAIN_PACKING_SUFFIX_IPv4_NO_PATH))
                {
                    path="";
                    usePrefix = ((packing & COINSPARK_DOMAIN_PACKING_IPv4_NO_PATH_PREFIX) != 0);
                    parts--;
                }
            }
            else
            {
                domainNamePacking=packing;
                parts++;
            }
            
            parts++;
            pathPart=parts;
            
            byte [] unpacked=unpackString(buffer,parts);
            if(unpacked == null)
            {
                throw new Exception("Cannot unpack path");                    
            }
            
            byte charTrue=(byte)domainNameChars[(COINSPARK_DOMAIN_PATH_TRUE_MARKER)];
            byte charFalse=(byte)domainNameChars[(COINSPARK_DOMAIN_PATH_FALSE_MARKER)];
            
            int start=0;
            if(parts>0)
            {
                parts=1;
                for(int i=0;i<unpacked.Length;i++)
                {
                    if((unpacked[i] == charTrue) || (unpacked[i] == charFalse))
                    {
                        string decodedString="";
                        bool decodeFlag=(unpacked[i] == charTrue);
                        if(i>start)
                        {
                            decodedString= Encoding.UTF8.GetString(Arrays.copyOfRange(unpacked, start, i));
                        }
                        if(parts == pathPart)
                        {
                            path=decodedString;
                            usePrefix=decodeFlag;
                        }
                        else
                        {
                            domainNameShort=decodedString;
                            useHttps=decodeFlag;
                            expand();                        
                        }
                        start=i+1;
                        parts++;
                    }
                }            
            }
        }
        catch (Exception ex)
        {
       //    System.out.print(ex.getMessage());
           return false;
        }

        return true;
    }
}
}