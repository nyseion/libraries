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
//import java.util.Arrays;


/**
 * CoinSparkAssetRef class for managing asset references.
 */

public class CoinSparkAssetRef : CoinSparkBase {
    
// Public constants 
    
    /**
     * Number of the first bytes of genesis transaction id stored in CoinSpark asset reference.
     */
    
    public static readonly int COINSPARK_ASSETREF_TXID_PREFIX_LEN = 2;

    
// Public functions    

    /**
     * Returns block in which genesis transaction is confirmed.
     * 
     * @return block number
     */
    
    public long getBlockNum() {
        return blockNum;
    }

    /**
     * Sets block in which genesis transaction is confirmed.
     * 
     * @param BlockNum block number to set
     */
    
    public void setBlockNum(long BlockNum) {
        blockNum = BlockNum;
    }

    /**
     * Returns byte offset within that block 
     * 
     * @return byte offset
     */
    
    public long getTxOffset() {
        return txOffset;
    }
    
    /**
     * Sets byte offset within that block. 
     * 
     * @param TxOffset offset to set
     */
    
    public void setTxOffset(long TxOffset) {
        txOffset = TxOffset;
    }

    
    /**
     * Returns first COINSPARK_ASSETREF_TXID_PREFIX_LEN bytes of genesis transaction id. 
     * 
     * @return transaction id prefix
     */
    
    public byte[] getTxIDPrefix() {
        return txIDPrefix;
    }

    /**
     * Sets first COINSPARK_ASSETREF_TXID_PREFIX_LEN bytes of genesis transaction id. 
     * 
     * @param TxIDPrefix transaction id prefix to set
     */
    
    public void setTxIDPrefix(byte[] TxIDPrefix) {
        txIDPrefix = new byte[TxIDPrefix.Length];
        TxIDPrefix.CopyTo(txIDPrefix,0);
    }
    
    /**
     * CoinSparkAssetRef class for managing asset references.
     */
    
    public CoinSparkAssetRef() {
        clear();
    }
    
    /**
     * Set all fields in address to their default/zero values, which are not necessarily valid.
     */
    
    public void clear() {
        blockNum = 0;
        txOffset = 0;
        txIDPrefix = new byte[COINSPARK_ASSETREF_TXID_PREFIX_LEN];
        for(int i=0;i<COINSPARK_ASSETREF_TXID_PREFIX_LEN;i++)
        {
            txIDPrefix[i]=0;
        }
    }

    /**
     * CoinSparkAssetRef class for managing asset references.
     * 
     * @param BlockNum Block in which genesis transaction is confirmed
     * @param TxOffset Byte offset within that block
     * @param TxIDPrefix First COINSPARK_ASSETREF_TXID_PREFIX_LEN bytes of genesis transaction id
     */
    
    public CoinSparkAssetRef(long BlockNum, long TxOffset, byte[] TxIDPrefix) {
        clear();
        setBlockNum(BlockNum);
        setTxOffset(TxOffset);
        setTxIDPrefix(TxIDPrefix);
    }

   // @Override
    public CoinSparkAssetRef clone()// throws CloneNotSupportedException
    {
        CoinSparkAssetRef copy= new CoinSparkAssetRef();
        copy.blockNum=blockNum;
        copy.txOffset=txOffset;
        copy.txIDPrefix = Arrays.copyOfRange(txIDPrefix, 0, txIDPrefix.Length);
        
        return copy;
    }

   // @Override
    public override string ToString()
    {
        return toStringInner(true);
    }

    /**
     * Returns true if all values in the asset reference are in their permitted ranges, false otherwise.
     * 
     * @return true if asset reference is valid
     */

    public bool isValid()
    {
        if(blockNum != CoinSparkTransfer.COINSPARK_TRANSFER_BLOCK_NUM_DEFAULT_ROUTE)
        {
            if( (blockNum < 0) || (blockNum > COINSPARK_ASSETREF_BLOCK_NUM_MAX) )
                return false;
            
            if( (txOffset < 0) || (txOffset > COINSPARK_ASSETREF_TX_OFFSET_MAX))
                return false;            
        }
        
        return true;
    }

    /**
     * Compares two CoinSpark asset references
     *  
     * @param reference2 Asset reference to compare with
     * @return Returns true if the two Asset references are identical.
     */
    
    public bool match(CoinSparkAssetRef reference2)
    {
        return (txIDPrefix.SequenceEqual(reference2.txIDPrefix) &&
               (txOffset == reference2.txOffset) &&
               (blockNum == reference2.blockNum));
    }
    
    /**
     * Encodes the assetRef to string.
     * 
     * @return string | null, Encoded CoinSpark asset reference, null if we failed.
     */
    
    public string encode()
    {
        if(!isValid())
            return null;
        
        int b1 = txIDPrefix[1] > 0 ? txIDPrefix[1] : txIDPrefix[1] &0xFF;
        int b2 = txIDPrefix[0] > 0 ? txIDPrefix[0] : txIDPrefix[0] &0xFF;
        int txIDPrefixInteger = 256*b1 + b2;

        return string.Format("{0}-{1}-{2}", blockNum, txOffset, txIDPrefixInteger);

    }
    
    /**
     * Decodes the CoinSpark asset reference
     * 
     * @param EncodedAssetRef Encoded asset reference to decode
     * @return true on success, false on failure
     */
    
    public bool decode(string EncodedAssetRef)
    {
        if(EncodedAssetRef.Contains("+"))
            return false;
        
        try{
            
            String[] parts = EncodedAssetRef.Split('-');
            if (parts.Length != 3)
                return false;

            blockNum = long.Parse(parts[0]);
            txOffset = long.Parse(parts[1]);

            int txIDPrefixInteger = int.Parse(parts[2]);
            if ( (txIDPrefixInteger<0) || (txIDPrefixInteger>0xFFFF) )
                throw new Exception("Invalid transaction ID prefix");
            
            txIDPrefix[0] = (byte)(txIDPrefixInteger%256);
            txIDPrefix[1] = (byte)(txIDPrefixInteger/256);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

        return isValid();
    }    
    
    
// Private variables/constants/functions   
    
    private static readonly long COINSPARK_ASSETREF_BLOCK_NUM_MAX  = 4294967295L;
    private static readonly long COINSPARK_ASSETREF_TX_OFFSET_MAX  = 4294967295L;

    private long blockNum;
    private long txOffset;
    private byte[] txIDPrefix;

    internal string toStringInner(bool headers)
    {
        StringBuilder sb = new StringBuilder();

        if (headers)
            sb.Append("COINSPARK ASSET REFERENCE\n");
        sb.Append(
                string.Format("Genesis block index: {0} (small endian hex {1})\n", blockNum,
                        unsignedToSmallEndianHex(blockNum, 4)))
                .Append(
                        string.Format(" Genesis txn offset: {0} (small endian hex {1})\n", txOffset,
                                unsignedToSmallEndianHex(txOffset, 4)))
                .Append(
                        string.Format("Genesis txid prefix: {0}\n", byteToHex(txIDPrefix)));

        if (headers)
            sb.Append("END COINSPARK ASSET REFERENCE\n\n");

        return sb.ToString();
    }
    

    internal int compare(CoinSparkAssetRef assetRef2)
    {
        // -1 if assetRef1<assetRef2, 1 if assetRef2>assetRef1, 0 otherwise

        if (blockNum != assetRef2.blockNum)
            return blockNum < assetRef2.blockNum ? -1 : 1;

        if (blockNum == CoinSparkTransfer.COINSPARK_TRANSFER_BLOCK_NUM_DEFAULT_ROUTE) // in this case don't compare other fields
            return 0;

        if (txOffset != assetRef2.txOffset)
            return txOffset < assetRef2.txOffset ? -1 : 1;
        return memcmp(txIDPrefix, assetRef2.txIDPrefix, COINSPARK_ASSETREF_TXID_PREFIX_LEN);
    }
    
}
}