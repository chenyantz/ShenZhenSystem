﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbleClient.Order.SoMgr
{
   public class So
    {
        public int soId;
        public int rfqId;
        public string customerName;
        public string contact;
        public int salesId;
        public int? approverId;
        public DateTime? approveDate;
        public string salesOrderNo;
        public DateTime orderDate;
        public string customerPo;
        public string paymentTerm;
        public string freightTerm;
        public string customerAccount;
        public string specialInstructions;
        public string billTo;
        public string shipTo;
        public int soStates;
    
    }

    public class SoItems:ICloneable
    {
        public int soItemsId;
        public int soId;
        public int saleType;
        public string partNo;
        public string mfg;
        public int rohs;
        public string dc;
        public string intPartNo;
        public string shipFrom;
        public string shipMethod;
        public string trackingNo;
        public int qty;
        public int qtyshipped;
        public int currencyType;
        public float unitPrice;
        public DateTime dockDate;
        public DateTime? shippedDate;
        public string shippingInstruction;
        public string packingInstruction;



        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }


    public enum SoStateEnum
    { 
      New=0,
      Approved=1,
      Rejected=2,
      Closed=3
    };


}
