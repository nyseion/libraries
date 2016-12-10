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
using System.Text;

namespace coinspark
{
/**
         * Class for managing individual asset transfer metadata
         */

    public class CoinSparkTransfer : CoinSparkBase
    {
        /**
         * Class for managing individual asset transfer metadata
         */

        public CoinSparkTransfer()
        {
            clear();
        }

        /**
         * Set all fields in transfer to their default/zero values, which are not necessarily valid.
         */


        public void clear()
        {
            assetRef = new CoinSparkAssetRef();
            inputs = new CoinSparkIORange();
            outputs = new CoinSparkIORange();
            qtyPerOutput = 0;
        }

        /**
         * Returns asset reference
         * 
         * @return asset reference
         */

        public CoinSparkAssetRef getAssetRef()
        {
            return assetRef;
        }

        /**
         * Sets asset reference
         * 
         * @param assetRef to set
         */

        public void setAssetRef(CoinSparkAssetRef assetRef)
        {
            this.assetRef = assetRef;
        }

        /**
         * Returns inputs range
         * 
         * @return inputs range
         */

        public CoinSparkIORange getInputs()
        {
            return inputs;
        }

        /**
         * Sets inputs range
         * 
         * @param inputs to set
         */

        public void setInputs(CoinSparkIORange inputs)
        {
            this.inputs = inputs;
        }

        /**
         * Returns outputs range
         * 
         * @return outputs range
         */

        public CoinSparkIORange getOutputs()
        {
            return outputs;
        }

        /**
         * Sets output range 
         * @param outputs to set
         */

        public void setOutputs(CoinSparkIORange outputs)
        {
            this.outputs = outputs;
        }

        /**
         * Returns Quantity per output
         * 
         * @return  Quantity per output
         */

        public long getQtyPerOutput()
        {
            return qtyPerOutput;
        }

        /**
         * Sest quantity per output
         * 
         * @param qtyPerOutput 
         */

        public void setQtyPerOutput(long qtyPerOutput)
        {
            this.qtyPerOutput = qtyPerOutput;
        }

      //  @Override
        public CoinSparkTransfer clone()
        {
            CoinSparkTransfer copy = new CoinSparkTransfer();
            copy.assetRef = assetRef.clone();
            copy.inputs = inputs.clone();
            copy.outputs = outputs.clone();
            copy.qtyPerOutput = qtyPerOutput;

            return copy;
        }

        /**
         * Returns true if the two CoinSparkTransfer structures are identical.
         * 
         * @param transfer2 CoinSparkTransfer to compare to 
         * @return Returns true if the two CoinSparkTransfer structures are identical.
         */

        public bool match(CoinSparkTransfer transfer2)
        {
            bool partialMatch = (inputs.first == transfer2.inputs.first) &&
                               (inputs.count == transfer2.inputs.count) &&
                               (outputs.first == transfer2.outputs.first);

            if (assetRef.getBlockNum() == COINSPARK_TRANSFER_BLOCK_NUM_DEFAULT_ROUTE)
                return (transfer2.assetRef.getBlockNum() == COINSPARK_TRANSFER_BLOCK_NUM_DEFAULT_ROUTE) && partialMatch;

            return
                assetRef.match(transfer2.assetRef) && partialMatch &&
                (getOutputs().count == transfer2.getOutputs().count) &&
                (getQtyPerOutput() == transfer2.getQtyPerOutput());
        }

        /**
         * Returns true if all values in the transfer are in their permitted ranges, false otherwise.
         * 
         * @return true if genesis structure is valid
         */

        public bool isValid()
        {
            if (!(assetRef.isValid() && inputs.isValid() && outputs.isValid()))
            {
                return false;
            }

            if ((qtyPerOutput < 0) || (qtyPerOutput > CoinSparkAssetQty.COINSPARK_ASSET_QTY_MAX))
            {
                return false;
            }

            return true;
        }

    //    @Override

        public override string ToString()
        {
            return toStringInner(this, true);
        }


// Private variables/constants/functions       


        internal static readonly int COINSPARK_TRANSFER_BLOCK_NUM_DEFAULT_ROUTE = -1;
            // magic number for a default route

        internal static readonly int COINSPARK_TRANSFER_QTY_FLOAT_LENGTH = 2;
        internal static readonly short COINSPARK_TRANSFER_QTY_FLOAT_MANTISSA_MAX = 1000;
        internal static readonly short COINSPARK_TRANSFER_QTY_FLOAT_EXPONENT_MAX = 11;
        internal static readonly int COINSPARK_TRANSFER_QTY_FLOAT_MASK = 0x3FFF;
        internal static readonly int COINSPARK_TRANSFER_QTY_FLOAT_EXPONENT_MULTIPLE = 1001;



        internal CoinSparkAssetRef assetRef;
        internal CoinSparkIORange inputs;
        internal CoinSparkIORange outputs;
        internal long qtyPerOutput;




        internal static string toStringInner(CoinSparkTransfer transfer, bool headers )
        {
            StringBuilder sb = new StringBuilder();
            if (headers)
                sb.Append("COINSPARK TRANSFER\n");

            bool isDefaultRoute = (transfer.assetRef.getBlockNum() == COINSPARK_TRANSFER_BLOCK_NUM_DEFAULT_ROUTE);

            if (isDefaultRoute)
            {
                sb.Append("      Default route:\n");
            }
            else
            {
                sb.Append(transfer.assetRef.toStringInner(false));

                sb.Append(string.Format("    Asset reference: {0}\n", transfer.assetRef.encode()));
            }

            if (transfer.getInputs().count > 0)
            {
                if (transfer.getInputs().count > 1)
                    sb.Append(string.Format("             Inputs: {0} - {1} (count {2})",
                        transfer.getInputs().first,
                        transfer.getInputs().first + transfer.getInputs().count - 1,
                        transfer.getInputs().count));
                else
                    sb.Append(string.Format("              Input: {0}", transfer.getInputs().first));
            }
            else
                sb.Append("           Inputs: none");

            sb.Append(string.Format(" (small endian hex: first {0} count {1})\n",
                unsignedToSmallEndianHex(transfer.getInputs().first, 2),
                unsignedToSmallEndianHex(transfer.getInputs().count, 2))); //oooo why biginteger

            if (transfer.getOutputs().count > 0)
            {
                if (transfer.getOutputs().count > 1 && !isDefaultRoute)
                    sb.Append(string.Format("            Outputs: {0} - {1} (count {2})",
                        transfer.getOutputs().first,
                        transfer.getOutputs().first + transfer.getOutputs().count - 1,
                        transfer.getOutputs().count));
                else
                    sb.Append(string.Format("             Output: {0}", transfer.getOutputs().first));
            }
            else
                sb.Append("          Outputs: none");

            sb.Append(string.Format(" (small endian hex: first {0} count {1})\n",
                unsignedToSmallEndianHex(transfer.getOutputs().first, 2),
                unsignedToSmallEndianHex(transfer.getOutputs().count, 2)));

            if (!isDefaultRoute)
            {
                sb.Append(string.Format("     Qty per output: {0} (small endian hex {1}", transfer.getQtyPerOutput(),
                    unsignedToSmallEndianHex(transfer.getQtyPerOutput(), 8)));
                CoinSparkAssetQty qtyEncodedFloat = new CoinSparkAssetQty(transfer.qtyPerOutput, 0,
                    COINSPARK_TRANSFER_QTY_FLOAT_MANTISSA_MAX, COINSPARK_TRANSFER_QTY_FLOAT_EXPONENT_MAX);
                if (qtyEncodedFloat.value == transfer.qtyPerOutput)
                {
                    long encodeQuantity = new CoinSparkAssetQty(
                    (qtyEncodedFloat.exponent*COINSPARK_TRANSFER_QTY_FLOAT_EXPONENT_MULTIPLE +
                     qtyEncodedFloat.mantissa) & COINSPARK_TRANSFER_QTY_FLOAT_MASK).value;

                    sb.Append(string.Format(", as float {0}", unsignedToSmallEndianHex(
                        encodeQuantity, COINSPARK_TRANSFER_QTY_FLOAT_LENGTH)));
                }

                sb.Append(")\n");
            }
            if (headers)
                sb.Append("END COINSPARK TRANSFER\n\n");

            return sb.ToString();
        }

        internal bool encode(CoinSparkBuffer buffer, CoinSparkTransfer previousTransfer, int countInputs,
            int countOutputs)
        {
            bool[] inputPackingOptions;
            bool[] outputPackingOptions;
            byte packing, packingExtend;
            byte packingExtendInput, packingExtendOutput;


            CoinSparkGenesis tempGenesis = new CoinSparkGenesis();
            long encodeQuantity;

            try
            {
                if (!isValid())
                {
                    throw new Exception("Invalid transfer");
                }

                bool isDefaultRoute = (assetRef.getBlockNum() == COINSPARK_TRANSFER_BLOCK_NUM_DEFAULT_ROUTE);

                packing = 0;
                packingExtend = 0;

                //  Packing for genesis reference

                if (isDefaultRoute)
                {
                    if (previousTransfer != null &&
                        (previousTransfer.assetRef.getBlockNum() != COINSPARK_TRANSFER_BLOCK_NUM_DEFAULT_ROUTE))
                        throw new Exception("No default route");
                    // default route transfers have to come at the start

                    packing |= CoinSparkPacking.COINSPARK_PACKING_GENESIS_PREV;

                }
                else
                {
                    if (previousTransfer != null && assetRef.match(previousTransfer.assetRef))
                        packing |= CoinSparkPacking.COINSPARK_PACKING_GENESIS_PREV;

                    else if (assetRef.getBlockNum() <= CoinSparkPacking.COINSPARK_UNSIGNED_3_BYTES_MAX)
                    {
                        if (assetRef.getTxOffset() <= CoinSparkPacking.COINSPARK_UNSIGNED_3_BYTES_MAX)
                            packing |= CoinSparkPacking.COINSPARK_PACKING_GENESIS_3_3_BYTES;
                        else if (assetRef.getTxOffset() <= CoinSparkPacking.COINSPARK_UNSIGNED_4_BYTES_MAX)
                            packing |= CoinSparkPacking.COINSPARK_PACKING_GENESIS_3_4_BYTES;
                        else
                            throw new Exception("Wrong block number");
                    }

                    else if ((assetRef.getBlockNum() <= CoinSparkPacking.COINSPARK_UNSIGNED_4_BYTES_MAX) &&
                             (assetRef.getTxOffset() <= CoinSparkPacking.COINSPARK_UNSIGNED_4_BYTES_MAX))
                        packing |= CoinSparkPacking.COINSPARK_PACKING_GENESIS_4_4_BYTES;

                    else
                        throw new Exception("Block number out of range");
                }

                //  Packing for input and output indices
                inputPackingOptions = CoinSparkPacking.getPackingOptions(previousTransfer != null
                    ? previousTransfer.inputs
                    : null, inputs, countInputs, false);
                outputPackingOptions = CoinSparkPacking.getPackingOptions(previousTransfer != null
                    ? previousTransfer.outputs
                    : null, outputs, countOutputs, false);

                if (inputPackingOptions[(int)CoinSparkPacking.PackingType._0P] &&
                    outputPackingOptions[(int)CoinSparkPacking.PackingType._0P])
                    packing |= CoinSparkPacking.COINSPARK_PACKING_INDICES_0P_0P;

                else if (inputPackingOptions[(int)CoinSparkPacking.PackingType._0P] &&
                         outputPackingOptions[(int)CoinSparkPacking.PackingType._1S])
                    packing |= CoinSparkPacking.COINSPARK_PACKING_INDICES_0P_1S;

                else if (inputPackingOptions[(int)CoinSparkPacking.PackingType._0P] &&
                         outputPackingOptions[(int)CoinSparkPacking.PackingType._ALL])
                    packing |= CoinSparkPacking.COINSPARK_PACKING_INDICES_0P_ALL;

                else if (inputPackingOptions[(int)CoinSparkPacking.PackingType._1S] &&
                         outputPackingOptions[(int)CoinSparkPacking.PackingType._0P])
                    packing |= CoinSparkPacking.COINSPARK_PACKING_INDICES_1S_0P;

                else if (inputPackingOptions[(int)CoinSparkPacking.PackingType._ALL] &&
                         outputPackingOptions[(int)CoinSparkPacking.PackingType._0P])
                    packing |= CoinSparkPacking.COINSPARK_PACKING_INDICES_ALL_0P;

                else if (inputPackingOptions[(int)CoinSparkPacking.PackingType._ALL] &&
                         outputPackingOptions[(int)CoinSparkPacking.PackingType._1S])
                    packing |= CoinSparkPacking.COINSPARK_PACKING_INDICES_ALL_1S;

                else if (inputPackingOptions[(int)CoinSparkPacking.PackingType._ALL] &&
                         outputPackingOptions[(int)CoinSparkPacking.PackingType._ALL])
                    packing |= CoinSparkPacking.COINSPARK_PACKING_INDICES_ALL_ALL;

                else
                {
                    // we need the second (extended) packing byte
                    packing |= CoinSparkPacking.COINSPARK_PACKING_INDICES_EXTEND;

                    if ((packingExtendInput = CoinSparkPacking.encodePackingExtend(inputPackingOptions)) == null)
                        throw new Exception("Input packing error");

                    if ((packingExtendOutput = CoinSparkPacking.encodePackingExtend(outputPackingOptions)) == null)
                        throw new Exception("Input packing error");

                    packingExtend =
                        (byte) ((packingExtendInput << CoinSparkPacking.COINSPARK_PACKING_EXTEND_INPUTS_SHIFT) |
                                (packingExtendOutput << CoinSparkPacking.COINSPARK_PACKING_EXTEND_OUTPUTS_SHIFT));
                }

                //  Packing for quantity

                encodeQuantity = qtyPerOutput;
                CoinSparkAssetQty qtyEncodedFloat = new CoinSparkAssetQty(qtyPerOutput, 0,
                    COINSPARK_TRANSFER_QTY_FLOAT_MANTISSA_MAX, COINSPARK_TRANSFER_QTY_FLOAT_EXPONENT_MAX);

                if (qtyPerOutput == (previousTransfer != null ? previousTransfer.qtyPerOutput : 1))
                    packing |= CoinSparkPacking.COINSPARK_PACKING_QUANTITY_1P;

                else if (qtyPerOutput >= CoinSparkAssetQty.COINSPARK_ASSET_QTY_MAX)
                    packing |= CoinSparkPacking.COINSPARK_PACKING_QUANTITY_MAX;

                else if (qtyPerOutput <= CoinSparkPacking.COINSPARK_UNSIGNED_BYTE_MAX)
                    packing |= CoinSparkPacking.COINSPARK_PACKING_QUANTITY_1_BYTE;

                else if (qtyPerOutput <= CoinSparkPacking.COINSPARK_UNSIGNED_2_BYTES_MAX)
                    packing |= CoinSparkPacking.COINSPARK_PACKING_QUANTITY_2_BYTES;

                else if (qtyEncodedFloat.value == qtyPerOutput)
                {
                    packing |= CoinSparkPacking.COINSPARK_PACKING_QUANTITY_FLOAT;
                    encodeQuantity = (qtyEncodedFloat.exponent*COINSPARK_TRANSFER_QTY_FLOAT_EXPONENT_MULTIPLE +
                                      qtyEncodedFloat.mantissa) & COINSPARK_TRANSFER_QTY_FLOAT_MASK;
                }

                else if (qtyPerOutput <= CoinSparkPacking.COINSPARK_UNSIGNED_3_BYTES_MAX)
                    packing |= CoinSparkPacking.COINSPARK_PACKING_QUANTITY_3_BYTES;

                else if (qtyPerOutput <= CoinSparkPacking.COINSPARK_UNSIGNED_4_BYTES_MAX)
                    packing |= CoinSparkPacking.COINSPARK_PACKING_QUANTITY_4_BYTES;

                else
                    packing |= CoinSparkPacking.COINSPARK_PACKING_QUANTITY_6_BYTES;


                //  Write out the actual data

                CoinSparkPacking.PackingByteCounts counts = CoinSparkPacking.transferPackingToByteCounts(packing,
                    packingExtend);

                buffer.writeByte(packing);

                if ((packing & CoinSparkPacking.COINSPARK_PACKING_INDICES_MASK) ==
                    CoinSparkPacking.COINSPARK_PACKING_INDICES_EXTEND)
                {
                    buffer.writeByte(packingExtend);
                }

                buffer.writeLong(assetRef.getBlockNum(), counts.blockNumBytes);
                buffer.writeLong(assetRef.getTxOffset(), counts.txOffsetBytes);
                buffer.writeBytes(assetRef.getTxIDPrefix(), counts.txIDPrefixBytes);
                buffer.writeInt(inputs.first, counts.firstInputBytes);
                buffer.writeInt(inputs.count, counts.countInputsBytes);
                buffer.writeInt(outputs.first, counts.firstOutputBytes);
                buffer.writeInt(outputs.count, counts.countOutputsBytes);
                buffer.writeLong(encodeQuantity, counts.quantityBytes);


            }
            catch (Exception ex)
            {
            //    System.out.print(ex.getMessage());
                return false;
            }

            return true;
        }


        internal bool decode(CoinSparkBuffer buffer, CoinSparkTransfer previousTransfer, int countInputs,
            int countOutputs)
        {
            byte packing, packingExtend = 0;
            CoinSparkPacking.PackingType inputPackingType = CoinSparkPacking.PackingType._NONE;
            CoinSparkPacking.PackingType outputPackingType = CoinSparkPacking.PackingType._NONE;

            try
            {
                //  Extract packing

                if (buffer.canRead(1))
                {
                    packing = buffer.readByte();
                }
                else
                    throw new Exception("Cannot read packing");


                // Packing for genesis reference

                switch (packing & CoinSparkPacking.COINSPARK_PACKING_GENESIS_MASK)
                {
                    case CoinSparkPacking.COINSPARK_PACKING_GENESIS_PREV:
                        if (previousTransfer != null)
                            assetRef = previousTransfer.assetRef;

                        else
                        {
                            // it's for a default route
                            assetRef.setBlockNum(COINSPARK_TRANSFER_BLOCK_NUM_DEFAULT_ROUTE);
                            assetRef.setTxOffset(0);
                            Arrays.fill(assetRef.getTxIDPrefix(), 0);
                        }
                        break;
                }

                //  Packing for input and output indices

                if ((packing & CoinSparkPacking.COINSPARK_PACKING_INDICES_MASK) ==
                    CoinSparkPacking.COINSPARK_PACKING_INDICES_EXTEND)
                {
                    // we're using second packing metadata byte

                    if (buffer.canRead(1))
                    {
                        packingExtend = buffer.readByte();
                    }
                    else
                        throw new Exception("Cannot read packing extend");

                    inputPackingType =
                        CoinSparkPacking.decodePackingExtend(
                            (byte) ((packingExtend >> CoinSparkPacking.COINSPARK_PACKING_EXTEND_INPUTS_SHIFT) &
                                    CoinSparkPacking.COINSPARK_PACKING_EXTEND_MASK), false);

                    if (inputPackingType == CoinSparkPacking.PackingType._NONE)
                        throw new Exception("Wrong packing type");

                    outputPackingType =
                        CoinSparkPacking.decodePackingExtend(
                            (byte) ((packingExtend >> CoinSparkPacking.COINSPARK_PACKING_EXTEND_OUTPUTS_SHIFT) &
                                    CoinSparkPacking.COINSPARK_PACKING_EXTEND_MASK), false);
                    if (outputPackingType == CoinSparkPacking.PackingType._NONE)
                        throw new Exception("No packing type");


                }
                else
                {
                    // not using second packing metadata byte

                    switch (packing & CoinSparkPacking.COINSPARK_PACKING_INDICES_MASK) // input packing
                    {
                        case CoinSparkPacking.COINSPARK_PACKING_INDICES_0P_0P:
                        case CoinSparkPacking.COINSPARK_PACKING_INDICES_0P_1S:
                        case CoinSparkPacking.COINSPARK_PACKING_INDICES_0P_ALL:
                            inputPackingType = CoinSparkPacking.PackingType._0P;
                            break;

                        case CoinSparkPacking.COINSPARK_PACKING_INDICES_1S_0P:
                            inputPackingType = CoinSparkPacking.PackingType._1S;
                            break;

                        case CoinSparkPacking.COINSPARK_PACKING_INDICES_ALL_0P:
                        case CoinSparkPacking.COINSPARK_PACKING_INDICES_ALL_1S:
                        case CoinSparkPacking.COINSPARK_PACKING_INDICES_ALL_ALL:
                            inputPackingType = CoinSparkPacking.PackingType._ALL;
                            break;
                    }

                    switch (packing & CoinSparkPacking.COINSPARK_PACKING_INDICES_MASK) // output packing
                    {
                        case CoinSparkPacking.COINSPARK_PACKING_INDICES_0P_0P:
                        case CoinSparkPacking.COINSPARK_PACKING_INDICES_1S_0P:
                        case CoinSparkPacking.COINSPARK_PACKING_INDICES_ALL_0P:
                            outputPackingType = CoinSparkPacking.PackingType._0P;
                            break;

                        case CoinSparkPacking.COINSPARK_PACKING_INDICES_0P_1S:
                        case CoinSparkPacking.COINSPARK_PACKING_INDICES_ALL_1S:
                            outputPackingType = CoinSparkPacking.PackingType._1S;
                            break;

                        case CoinSparkPacking.COINSPARK_PACKING_INDICES_0P_ALL:
                        case CoinSparkPacking.COINSPARK_PACKING_INDICES_ALL_ALL:
                            outputPackingType = CoinSparkPacking.PackingType._ALL;
                            break;
                    }
                }

                inputs = CoinSparkPacking.packingTypeToValues(inputPackingType,
                    previousTransfer != null ? previousTransfer.inputs : null, countInputs);
                outputs = CoinSparkPacking.packingTypeToValues(outputPackingType,
                    previousTransfer != null ? previousTransfer.outputs : null, countOutputs);

                //  Read in the fields as appropriate

                CoinSparkPacking.PackingByteCounts counts = CoinSparkPacking.transferPackingToByteCounts(packing,
                    packingExtend);

                long[] resLong = new long[1];
                if (counts.blockNumBytes > 0)
                {
                    if (buffer.canRead(counts.blockNumBytes))
                        assetRef.setBlockNum(buffer.readLong(counts.blockNumBytes));
                    else
                        throw new Exception("Cannot read block number");
                }

                if (counts.txOffsetBytes > 0)
                {
                    if (buffer.canRead(counts.txOffsetBytes))
                        assetRef.setTxOffset(buffer.readLong(counts.txOffsetBytes));
                    else
                        throw new Exception("Cannot read txn offset");
                }

                if (counts.txIDPrefixBytes > 0)
                {
                    if (buffer.canRead(counts.txIDPrefixBytes))
                        assetRef.setTxIDPrefix(buffer.readBytes(counts.txIDPrefixBytes));
                    else
                        throw new Exception("Cannot read txn prefix");
                }

                if (counts.firstInputBytes > 0)
                {
                    if (buffer.canRead(counts.firstInputBytes))
                        inputs.first = buffer.readInt(counts.firstInputBytes);
                    else
                        throw new Exception("Cannot read input first");
                }

                if (counts.countInputsBytes > 0)
                {
                    if (buffer.canRead(counts.countInputsBytes))
                        inputs.count = buffer.readInt(counts.countInputsBytes);
                    else
                        throw new Exception("Cannot read input count");
                }

                if (counts.firstOutputBytes > 0)
                {
                    if (buffer.canRead(counts.firstOutputBytes))
                        outputs.first = buffer.readInt(counts.firstOutputBytes);
                    else
                        throw new Exception("Cannot read output first");
                }

                if (counts.countOutputsBytes > 0)
                {
                    if (buffer.canRead(counts.countOutputsBytes))
                        outputs.count = buffer.readInt(counts.countOutputsBytes);
                    else
                        throw new Exception("Cannot read output count");
                }

                long decodeQuantity = 0;
                if (counts.quantityBytes > 0)
                {
                    if (buffer.canRead(counts.quantityBytes))
                        decodeQuantity = buffer.readLong(counts.quantityBytes);
                    else
                        throw new Exception("Cannot read quantity");
                }


                //  Finish up reading in quantity

                switch (packing & CoinSparkPacking.COINSPARK_PACKING_QUANTITY_MASK)
                {
                    case CoinSparkPacking.COINSPARK_PACKING_QUANTITY_1P:
                        if (previousTransfer != null)
                            qtyPerOutput = previousTransfer.qtyPerOutput;
                        else
                            qtyPerOutput = 1;
                        break;

                    case CoinSparkPacking.COINSPARK_PACKING_QUANTITY_MAX:
                        qtyPerOutput = CoinSparkAssetQty.COINSPARK_ASSET_QTY_MAX;
                        break;

                    case CoinSparkPacking.COINSPARK_PACKING_QUANTITY_FLOAT:
                        decodeQuantity &= COINSPARK_TRANSFER_QTY_FLOAT_MASK;

                        qtyPerOutput = new CoinSparkAssetQty(
                            (int) (decodeQuantity%COINSPARK_TRANSFER_QTY_FLOAT_EXPONENT_MULTIPLE),
                            (int) (decodeQuantity/COINSPARK_TRANSFER_QTY_FLOAT_EXPONENT_MULTIPLE)).value;
                        break;

                    default:
                        qtyPerOutput = decodeQuantity;
                        break;
                }
            }
            catch (Exception ex)
            {
           //     System.out.print(ex.getMessage());
                return false;
            }

            return true;
        }

    }

}