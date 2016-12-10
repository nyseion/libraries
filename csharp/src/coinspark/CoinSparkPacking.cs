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


// package org.coinspark.protocol;

using System;

namespace coinspark { 
/**
 * Internal class with static functions needed for packing/unpacking transfer metadata
 */

public class CoinSparkPacking {
 
    internal const int COINSPARK_UNSIGNED_BYTE_MAX       = 0xFF;
    internal const int COINSPARK_UNSIGNED_2_BYTES_MAX    = 0xFFFF;
    internal const int COINSPARK_UNSIGNED_3_BYTES_MAX    = 0xFFFFFF;
    internal const long COINSPARK_UNSIGNED_4_BYTES_MAX   = 0xFFFFFFFFL;
    
    internal const int COINSPARK_PACKING_GENESIS_MASK  = 0xC0;
    internal const int COINSPARK_PACKING_GENESIS_PREV  = 0x00;

    internal const int COINSPARK_PACKING_GENESIS_3_3_BYTES  = 0x40; // 3 bytes for block index, 3 for txn offset
    internal const int COINSPARK_PACKING_GENESIS_3_4_BYTES  = 0x80; // 3 bytes for block index, 4 for txn offset
    internal const int COINSPARK_PACKING_GENESIS_4_4_BYTES  = 0xC0; // 4 bytes for block index, 4 for txn offset

    internal const byte COINSPARK_PACKING_INDICES_MASK       = 0x38;
    internal const byte COINSPARK_PACKING_INDICES_0P_0P      = 0x00; // input 0 only or previous, output 0 only or previous
    internal const byte COINSPARK_PACKING_INDICES_0P_1S      = 0x08; // input 0 only or previous, output 1 only or subsequent single
    internal const byte COINSPARK_PACKING_INDICES_0P_ALL     = 0x10; // input 0 only or previous, all outputs
    internal const byte COINSPARK_PACKING_INDICES_1S_0P      = 0x18; // input 1 only or subsequent single, output 0 only or previous
    internal const byte COINSPARK_PACKING_INDICES_ALL_0P     = 0x20; // all inputs, output 0 only or previous
    internal const byte COINSPARK_PACKING_INDICES_ALL_1S     = 0x28; // all inputs, output 1 only or subsequent single
    internal const byte COINSPARK_PACKING_INDICES_ALL_ALL    = 0x30; // all inputs, all outputs
    internal const byte COINSPARK_PACKING_INDICES_EXTEND     = 0x38; // use second byte for more extensive information

    internal const byte COINSPARK_PACKING_EXTEND_INPUTS_SHIFT= 3;
    internal const byte COINSPARK_PACKING_EXTEND_OUTPUTS_SHIFT=0;

    internal const byte COINSPARK_PACKING_EXTEND_MASK        = 0x07;
    internal const byte COINSPARK_PACKING_EXTEND_0P          = 0x00; // index 0 only or previous(transfers only)
    internal const int  COINSPARK_PACKING_EXTEND_PUBLIC      = 0x00; // this is public (messages only)
    internal const byte COINSPARK_PACKING_EXTEND_1S          = 0x01; // index 1 only or subsequent single (transfers only)
    internal const byte COINSPARK_PACKING_EXTEND_0_1_BYTE    = 0x01; // // starting at 0, 1 byte for count (messages only)
    internal const byte COINSPARK_PACKING_EXTEND_1_0_BYTE    = 0x02; // 1 byte for single index
    internal const byte COINSPARK_PACKING_EXTEND_2_BYTES     = 0x03; // 2 bytes for single index
    internal const byte COINSPARK_PACKING_EXTEND_1_1_BYTES   = 0x04; // 1 byte for first index, 1 byte for count
    internal const byte COINSPARK_PACKING_EXTEND_2_1_BYTES   = 0x05; // 2 bytes for first index, 1 byte for count
    internal const byte COINSPARK_PACKING_EXTEND_2_2_BYTES   = 0x06; // 2 bytes for first index, 2 bytes for count
    internal const byte COINSPARK_PACKING_EXTEND_ALL         = 0x07; // all inputs|outputs

    internal const byte COINSPARK_PACKING_QUANTITY_MASK      = 0x07;
    internal const byte COINSPARK_PACKING_QUANTITY_1P        = 0x00; // quantity=1 or previous
    internal const byte COINSPARK_PACKING_QUANTITY_1_BYTE    = 0x01;
    internal const byte COINSPARK_PACKING_QUANTITY_2_BYTES   = 0x02;
    internal const byte COINSPARK_PACKING_QUANTITY_3_BYTES   = 0x03;
    internal const byte COINSPARK_PACKING_QUANTITY_4_BYTES   = 0x04;
    internal const byte COINSPARK_PACKING_QUANTITY_6_BYTES   = 0x05;
    internal const byte COINSPARK_PACKING_QUANTITY_FLOAT     = 0x06;
    internal const byte COINSPARK_PACKING_QUANTITY_MAX       = 0x07; // transfer all quantity across
    
    private const int maxPackingTypes = 9; //TBD
    internal enum PackingType {      // do not want to use ordinal. see Joshua Bloch, Effective Java (2nd ed), item 31
        _NONE = -1,
        _0P = 0,
        _1S = 1,
        _ALL = 2,
        _1_0_BYTE = 3,
        _0_1_BYTE = 4,
        _2_BYTES =5,
        _1_1_BYTES = 6,
        _2_1_BYTES = 7,
        _2_2_BYTES = 8,
        countPackingTypes = maxPackingTypes //TBD

     //   private readonly int value;
     //   private PackingType(int value) {
     //       this.value = value;
        }

  //      internal int getValue() {
  //          return value;
  //      }
    
    
    internal readonly static byte[] packingExtendMap = { // same order as above
            COINSPARK_PACKING_EXTEND_0P,
            COINSPARK_PACKING_EXTEND_1S,
            COINSPARK_PACKING_EXTEND_ALL,
            COINSPARK_PACKING_EXTEND_1_0_BYTE,
            COINSPARK_PACKING_EXTEND_0_1_BYTE,
            COINSPARK_PACKING_EXTEND_2_BYTES,
            COINSPARK_PACKING_EXTEND_1_1_BYTES,
            COINSPARK_PACKING_EXTEND_2_1_BYTES,
            COINSPARK_PACKING_EXTEND_2_2_BYTES
    };

    internal static byte encodePackingExtend(bool[] packingOptions)
    {
        foreach (PackingType option in Enum.GetValues(typeof(PackingType)))       
        {
            if ((option != PackingType._NONE && option != PackingType.countPackingTypes && packingOptions[(int)option])) {
                return packingExtendMap[(int)option];
            }
        }
        return 0;
    }
    
    
    internal static PackingType decodePackingExtend(byte packingExtend, bool ForMessages)
    {
        PackingType packingType = PackingType._NONE;
            foreach (PackingType option in Enum.GetValues(typeof(PackingType)))
         { 
                if (option != PackingType._NONE && option != PackingType.countPackingTypes)
                if(option!=(ForMessages ? PackingType._1S : PackingType._0_1_BYTE))// no _1S for messages, no _0_1_BYTE for transfers
                    if (packingExtend==packingExtendMap[(int)option]) {                    
                        packingType=option;
                        return packingType;
                    }
        }

        return packingType;
    }
    
    
    internal class PackingByteCounts{
        internal int blockNumBytes;
        internal int txOffsetBytes;
        internal int txIDPrefixBytes;
        internal int firstInputBytes;
        internal int countInputsBytes;
        internal int firstOutputBytes;
        internal int countOutputsBytes;
        internal int quantityBytes;

        public PackingByteCounts()
        {
            //  Set default values for bytes for all fields to zero
            blockNumBytes = 0;
            txOffsetBytes = 0;
            txIDPrefixBytes = 0;
            firstInputBytes = 0;
            countInputsBytes = 0;
            firstOutputBytes = 0;
            countOutputsBytes = 0;
            quantityBytes = 0;
        }

        //@Override
        public override string ToString()
        {
            return string.Format(
                    "blockIndex {0}, txnOffset {1}, txnIDPrefix {2}, firstInput {3}, countInputs {4}, firstOutput {5}, countOutputs {6}, quantity {7}",
                    blockNumBytes, txOffsetBytes, txIDPrefixBytes,
                    firstInputBytes, countInputsBytes, firstOutputBytes, countOutputsBytes, quantityBytes);
        }
    }
    
    
    internal static CoinSparkIORange packingTypeToValues(PackingType packingType, CoinSparkIORange previousRange, int countInputOutputs)
    {
        CoinSparkIORange range=new CoinSparkIORange();
        switch (packingType)
        {
            case PackingType._0P:
                if (previousRange!= null) {
                    range.first=previousRange.first;
                    range.count=previousRange.count;
                } else {
                    range.first=0;
                    range.count=1;                    
                }
                break;

            case PackingType._1S:
                if (previousRange != null)
                    range.first=previousRange.first+previousRange.count;
                else
                    range.first=1;

                range.count=1;
                break;

            case PackingType._0_1_BYTE:
                range.first=0;
                break;
                
            case PackingType._1_0_BYTE:
            case PackingType._2_BYTES:
                range.count=1;
                break;

            case PackingType._ALL:
                range.first=0;
                range.count=countInputOutputs;
                break;

            default:                                                            // other packing types need to be read in
                break;
        }
        
        return range;
    }
    
    internal static int [] packingExtendAddByteCounts(int packingExtend,int firstBytes, int countBytes, bool forMessages)
    {
        int [] result=new int[3];
        
        result[0]=0;                                                            // Reserved for packing
        result[1]=firstBytes;
        result[2]=countBytes;
        
        switch (packingExtend)
        {
            case COINSPARK_PACKING_EXTEND_0_1_BYTE:
                if (forMessages) // otherwise it's really COINSPARK_PACKING_EXTEND_1S
	                result[2]=1;
                break;
		
            case COINSPARK_PACKING_EXTEND_1_0_BYTE:
                result[1] = 1;
                break;

            case COINSPARK_PACKING_EXTEND_2_BYTES:
                result[1] = 2;
                break;

            case COINSPARK_PACKING_EXTEND_1_1_BYTES:
                result[1] = 1;
                result[2] = 1;
                break;

            case COINSPARK_PACKING_EXTEND_2_1_BYTES:
                result[1] = 2;
                result[2] = 1;
                break;

            case COINSPARK_PACKING_EXTEND_2_2_BYTES:
                result[1] = 2;
                result[2] = 2;
                break;
        }
        
        return result;
    }
    
    internal static PackingByteCounts transferPackingToByteCounts(byte packing, byte packingExtend)
    {
        //  Set default values for bytes for all fields to zero
        
        PackingByteCounts counts = new PackingByteCounts();
        
        //  Packing for genesis reference
        
        switch (packing & COINSPARK_PACKING_GENESIS_MASK)
        {
            case COINSPARK_PACKING_GENESIS_3_3_BYTES:
                counts.blockNumBytes = 3;
                counts.txOffsetBytes = 3;
                counts.txIDPrefixBytes = CoinSparkAssetRef.COINSPARK_ASSETREF_TXID_PREFIX_LEN;
                break;

            case COINSPARK_PACKING_GENESIS_3_4_BYTES:
                counts.blockNumBytes = 3;
                counts.txOffsetBytes = 4;
                counts.txIDPrefixBytes = CoinSparkAssetRef.COINSPARK_ASSETREF_TXID_PREFIX_LEN;
                break;

            case COINSPARK_PACKING_GENESIS_4_4_BYTES:
                counts.blockNumBytes = 4;
                counts.txOffsetBytes = 4;
                counts.txIDPrefixBytes = CoinSparkAssetRef.COINSPARK_ASSETREF_TXID_PREFIX_LEN;
                break;
        }

        //  Packing for input and output indices  (relevant for extended indices only)

        if ((packing & COINSPARK_PACKING_INDICES_MASK) == COINSPARK_PACKING_INDICES_EXTEND) { // we're using extended indices

            int[] countsBytes;
            
            //  Input indices
            countsBytes=packingExtendAddByteCounts(((packingExtend >> COINSPARK_PACKING_EXTEND_INPUTS_SHIFT) & COINSPARK_PACKING_EXTEND_MASK),
                counts.firstInputBytes,counts.countInputsBytes, false);
            counts.firstInputBytes=countsBytes[1];
            counts.countInputsBytes=countsBytes[2];
            
            //  Output indices
            countsBytes=packingExtendAddByteCounts(((packingExtend >> COINSPARK_PACKING_EXTEND_OUTPUTS_SHIFT) & COINSPARK_PACKING_EXTEND_MASK),
                counts.firstOutputBytes,counts.countOutputsBytes, false);
            counts.firstOutputBytes=countsBytes[1];
            counts.countOutputsBytes=countsBytes[2];
            
/*                
            switch ((packingExtend >> COINSPARK_PACKING_EXTEND_INPUTS_SHIFT) & COINSPARK_PACKING_EXTEND_MASK)
            {
                case COINSPARK_PACKING_EXTEND_1_0_BYTE:
                    counts.firstInputBytes = 1;
                    break;

                case COINSPARK_PACKING_EXTEND_2_BYTES:
                    counts.firstInputBytes = 2;
                    break;

                case COINSPARK_PACKING_EXTEND_1_1_BYTES:
                    counts.firstInputBytes = 1;
                    counts.countInputsBytes = 1;
                    break;

                case COINSPARK_PACKING_EXTEND_2_1_BYTES:
                    counts.firstInputBytes = 2;
                    counts.countInputsBytes = 1;
                    break;

                case COINSPARK_PACKING_EXTEND_2_2_BYTES:
                    counts.firstInputBytes = 2;
                    counts.countInputsBytes = 2;
                    break;
            }

            //  Output indices

            switch ((packingExtend >> COINSPARK_PACKING_EXTEND_OUTPUTS_SHIFT) & COINSPARK_PACKING_EXTEND_MASK)
            {
                case COINSPARK_PACKING_EXTEND_1_0_BYTE:
                    counts.firstOutputBytes = 1;
                    break;

                case COINSPARK_PACKING_EXTEND_2_BYTES:
                    counts.firstOutputBytes = 2;
                    break;

                case COINSPARK_PACKING_EXTEND_1_1_BYTES:
                    counts.firstOutputBytes = 1;
                    counts.countOutputsBytes = 1;
                    break;

                case COINSPARK_PACKING_EXTEND_2_1_BYTES:
                    counts.firstOutputBytes = 2;
                    counts.countOutputsBytes = 1;
                    break;

                case COINSPARK_PACKING_EXTEND_2_2_BYTES:
                    counts.firstOutputBytes = 2;
                    counts.countOutputsBytes = 2;
                    break;
            }

*/
        }
        //  Packing for quantity

        switch (packing & COINSPARK_PACKING_QUANTITY_MASK)
        {
            case COINSPARK_PACKING_QUANTITY_1_BYTE:
                counts.quantityBytes = 1;
                break;

            case COINSPARK_PACKING_QUANTITY_2_BYTES:
                counts.quantityBytes = 2;
                break;

            case COINSPARK_PACKING_QUANTITY_3_BYTES:
                counts.quantityBytes = 3;
                break;

            case COINSPARK_PACKING_QUANTITY_4_BYTES:
                counts.quantityBytes = 4;
                break;

            case COINSPARK_PACKING_QUANTITY_6_BYTES:
                counts.quantityBytes = 6;
                break;

            case COINSPARK_PACKING_QUANTITY_FLOAT:
                counts.quantityBytes = CoinSparkTransfer.COINSPARK_TRANSFER_QTY_FLOAT_LENGTH;
                break;
        }
        
        return counts;
    }
    
    internal static bool[] getPackingOptions(CoinSparkIORange previousRange, CoinSparkIORange range, int countInputOutputs,bool ForMessages)
    {
        bool[] packingOptions=new bool[maxPackingTypes];
        
        bool firstZero, firstByte, first2Bytes, countOne, countByte;

        firstZero=(range.first == 0);
        firstByte=(range.first <= COINSPARK_UNSIGNED_BYTE_MAX);
        first2Bytes=(range.first <= COINSPARK_UNSIGNED_2_BYTES_MAX);
        countOne=(range.count == 1);
        countByte=(range.count <= COINSPARK_UNSIGNED_BYTE_MAX);

        if(ForMessages)
        {
            packingOptions[(int)PackingType._0P]=false;
            packingOptions[(int)PackingType._1S]=false;
            packingOptions[(int)PackingType._0_1_BYTE]=firstZero && countByte;

        }
        else
        {
            if (previousRange != null) {
                packingOptions[(int)PackingType._0P]=(range.first==previousRange.first) &&
                        (range.count == previousRange.count);
                packingOptions[(int)PackingType._1S]=(range.first == (previousRange.first + previousRange.count)) && countOne;

            } else {
                packingOptions[(int)PackingType._0P]=firstZero && countOne;
                packingOptions[(int)PackingType._1S]=(range.first==1) && countOne;
            }
            packingOptions[(int)PackingType._0_1_BYTE]=false;
        }

        packingOptions[(int)PackingType._1_0_BYTE]=firstByte && countOne;
        packingOptions[(int)PackingType._2_BYTES]=first2Bytes && countOne;
        packingOptions[(int)PackingType._1_1_BYTES]=firstByte && countByte;
        packingOptions[(int)PackingType._2_1_BYTES]=first2Bytes && countByte;
        packingOptions[(int)PackingType._2_2_BYTES]=first2Bytes && (range.count <= COINSPARK_UNSIGNED_2_BYTES_MAX);
        packingOptions[(int)PackingType._ALL]=firstZero && (range.count >= countInputOutputs);
        
        return packingOptions;
    }
    
}
}