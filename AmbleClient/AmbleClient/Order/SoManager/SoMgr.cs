﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AmbleClient.SO;

namespace AmbleClient.Order.SoMgr
{
   public class SoMgr
    {
       static DataClass.DataBase db = new DataClass.DataBase();


       public static List<So> SalesGetSoAccordingTofilter(int userId, bool includedSubs, string filterColumn, string filterString, List<int> states)
       {
           

           List<So> soList=new List<So>();
           if (states.Count == 0) return soList;
           List<int> salesIds = new List<int>();

           if (includedSubs)
           {
               var accountMgr = new AmbleClient.Admin.AccountMgr.AccountMgr();
               salesIds.AddRange(accountMgr.GetAllSubsId(userId,UserCombine.GetUserCanBeSales()));
           }
           else
           {
               salesIds.Add(userId);
           }

           StringBuilder sb=new StringBuilder();
           sb.Append("select soId from So where ( salesId="+salesIds[0]);
           for(int i=1;i<salesIds.Count;i++)
           {
               sb.Append(" or salesId=" + salesIds[i]);
           }
           sb.Append(" ) ");

           //append the filter
           if ((!string.IsNullOrWhiteSpace(filterColumn)) && (!string.IsNullOrWhiteSpace(filterString)))
           { 
            sb.Append(string.Format(" and {0} like '%{1}%' ",filterColumn,filterString));
           }

           sb.Append(" and (soStates="+states[0]);
           for(int i=1;i<states.Count;i++)
           {
            sb.Append(" or soStates="+states[i]);
           
           }
           sb.Append(" )");

           DataTable dt=db.GetDataTable(sb.ToString(),"soId");

           foreach(DataRow dr in dt.Rows)
           {
           soList.Add(GetSoAccordingToSoId(Convert.ToInt32(dr["soId"])));
           }
           return soList;

       }

       public static List<So> BuyerGetSoAccordingToFilter(int userId, bool includedSubs, string filterColumn, string filterString, List<int> states)

       {
           

           List<So> soList = new List<So>();
           
           if (states.Count == 0) return soList;

           List<int> buyersIds = new List<int>();

           if (includedSubs)
           {
               var accountMgr = new AmbleClient.Admin.AccountMgr.AccountMgr();
               buyersIds.AddRange(accountMgr.GetAllSubsId(userId,UserCombine.GetUserCanBeBuyers()));
           }
           else
           {
               buyersIds.Add(userId);
           }

           StringBuilder sb = new StringBuilder();
           sb.Append(string .Format("select soId from So s,rfq r where(s.rfqId=r.rfqNo) and ( (r.firstPA={0} or r.secondPa={0})",buyersIds[0]));
           for (int i = 1; i < buyersIds.Count; i++)
           {
               sb.Append(string.Format(" or (firstPA={0} or secondPA={0}) ",buyersIds[i]));
           }
           sb.Append(" ) ");

           //append the filter
           if ((!string.IsNullOrWhiteSpace(filterColumn)) && (!string.IsNullOrWhiteSpace(filterString)))
           {
               sb.Append(string.Format(" and {0} like '%{1}%' ", filterColumn, filterString));
           }

           sb.Append(" and (soStates=" + states[0]);
           for (int i = 1; i < states.Count; i++)
           {
               sb.Append(" or soStates=" + states[i]);

           }
           sb.Append(" )");

           DataTable dt = db.GetDataTable(sb.ToString(), "soId");

           foreach (DataRow dr in dt.Rows)
           {
               soList.Add(GetSoAccordingToSoId(Convert.ToInt32(dr["soId"])));
           }
           return soList; 
       }





       public static List<So> GetSoAccordingToRfqId(int rfqId)
       {
           List<So> soList = new List<So>();
         string strSql="select soId from So where rfqId="+rfqId;

         DataTable dt = db.GetDataTable(strSql,"soId");
         foreach (DataRow dr in dt.Rows)
         {
             soList.Add(GetSoAccordingToSoId(Convert.ToInt32(dr["soId"])));
         
         }

         return soList;
       }

       public static So GetSoAccordingToSoId(int soId)
       {
           string strSql = "select * from So where soId=" + soId;

           DataTable dt = db.GetDataTable(strSql,"So");
           if (dt.Rows.Count != 1)
           {
               return null;
           }
           DataRow dr = dt.Rows[0];

           int? tmpApproverId=null;
           if(dr["approverId"]!=DBNull.Value)
           {
            tmpApproverId=Convert.ToInt32(dr["approverId"]);
           }
           DateTime? tmpApproveDate=null;
           if(dr["approveDate"]!=DBNull.Value)
           {
            tmpApproveDate=Convert.ToDateTime(dr["approveDate"]);
           }

           return new So
           {
               soId = Convert.ToInt32(dr["soId"]),
               rfqId = Convert.ToInt32(dr["rfqId"]),
               customerName = dr["customerName"].ToString(),
               contact = dr["contact"].ToString(),
               salesId = Convert.ToInt32(dr["salesId"]),
               approverId = tmpApproverId,
               approveDate = tmpApproveDate,
               salesOrderNo = dr["salesOrderNo"].ToString(),
               orderDate = Convert.ToDateTime(dr["orderDate"]),
               customerPo = dr["customerPo"].ToString(),
               paymentTerm = dr["paymentTerm"].ToString(),
               freightTerm = dr["freightTerm"].ToString(),
               customerAccount = dr["customerAccount"].ToString(),
               specialInstructions = dr["specialInstructions"].ToString(),
               billTo = dr["billTo"].ToString(),
               shipTo = dr["shipTo"].ToString(),
               soStates = Convert.ToInt32(dr["soStates"])
           };

       }

       public static List<SoItems> GetSoItemsAccordingToSoId(int soId)
       {
           List<SoItems> soItemsList = new List<SoItems>();

           string strSql = "select * from SoItems where soId="+soId;
           DataTable dt = db.GetDataTable(strSql, "soitems");
           foreach (DataRow dr in dt.Rows)
           {
               soItemsList.Add(
                   new SoItems
                   {
                       soItemsId = Convert.ToInt32(dr["soItemsId"]),
                       soId = Convert.ToInt32(dr["soId"]),
                       saleType = Convert.ToInt32(dr["saleType"]),
                       partNo = dr["partNo"].ToString(),
                       mfg = dr["mfg"].ToString(),
                       rohs = Convert.ToInt32(dr["rohs"]),
                       dc = dr["dc"].ToString(),
                       intPartNo = dr["intPartNo"].ToString(),
                       shipFrom = dr["shipFrom"].ToString(),
                       shipMethod=dr["shipMethod"].ToString(),
                       trackingNo = dr["trackingNo"].ToString(),
                       qty = Convert.ToInt32(dr["qty"]),
                       qtyshipped = Convert.ToInt32(dr["qtyShipped"]),
                       currencyType = Convert.ToInt32(dr["currency"]),
                       unitPrice = Convert.ToSingle(dr["unitPrice"]),
                      
                       dockDate = Convert.ToDateTime(dr["dockDate"]),
                       shippedDate = Convert.ToDateTime(dr["shippedDate"]),
                       shippingInstruction = dr["shippingInstruction"].ToString(),
                       packingInstruction = dr["packingInstruction"].ToString()

                   });
          }
           return soItemsList; 
    
       }

       
       public static bool SaveSoMain(So so)
       {
           string strSql = "insert into So(rfqId,customerName,contact,salesId,salesOrderNo,orderDate,customerPo,paymentTerm,freightTerm,customerAccount,specialInstructions,billTo,shipTo,soStates) " +
               string.Format(" values({0},'{1}','{2}',{3},'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',0)", so.rfqId, so.customerName, so.contact, so.salesId, so.salesOrderNo, so.orderDate.ToShortDateString(), so.customerPo,
               so.paymentTerm, so.freightTerm, so.customerAccount, so.specialInstructions, so.billTo, so.shipTo);

           if (db.ExecDataBySql(strSql) == 1)
               return true;
           
           return false;
       }


       public static int GetTheInsertId(int salesId)
       {
           string strSql = "select max(soId) from So where salesId=" + salesId;
           return Convert.ToInt32(db.GetSingleObject(strSql));
       }

       public static bool SaveSoItems(int soId, List<SoItems> soitems)
       {
           List<string> strSqls = new List<string>();
           foreach (SoItems soItem in soitems)
           {

               string strsql = "insert into SoItems(soId,saleType,partNo,mfg,rohs,dc,intPartNo,shipFrom,shipMethod,trackingNo,qty,qtyShipped,currency,unitPrice,dockDate,shippedDate,shippingInstruction,packingInstruction) " +
                   string.Format(" values({0},{1},'{2}','{3}',{4},'{5}','{6}','{7}','{8}','{9}',{10},{11},{12},{13},'{14}','{15}','{16}','{17}')", soId, soItem.saleType, soItem.partNo, soItem.mfg, soItem.rohs, soItem.dc,
                   soItem.intPartNo, soItem.shipFrom,soItem.shipMethod,soItem.trackingNo, soItem.qty, soItem.qtyshipped, soItem.currencyType, soItem.unitPrice, soItem.dockDate.ToShortDateString(),soItem.shippedDate.HasValue?soItem.shippedDate.Value.ToShortDateString():"null",
                   soItem.shippingInstruction, soItem.packingInstruction);
               strSqls.Add(strsql);
           
           }

           return db.ExecDataBySqls(strSqls);

       }

       public static int GetSoStateAccordingToSoId(int soId)
       {
           string strSql = " select soStates from so where soId=" + soId.ToString();
           return Convert.ToInt32(db.GetSingleObject(strSql));
      
       }




       public static bool UpdateSoState(int soId,int userid, int state)
       {
           string strSql;

           if (state == new SoApprove().GetStateValue())
           {
               strSql = string.Format("update so set soStates={0},approverId={1},approveDate='{2}' where soId={3}", state, userid, DateTime.Now.ToShortDateString(), soId);
           }
           else if (state == new SoWaitingForShip().GetStateValue())
           {
               if (GetSoStateAccordingToSoId(soId) == new SoApprove().GetStateValue())
               {
                   strSql = string.Format("update so set soStates={0} where soId={1}", state, soId);

               }
               else
               {
                   return false;
               }
            
           }
           else
           {
               strSql = string.Format("update so set soStates={0} where soId={1}", state, soId);
           }
           
           if (db.ExecDataBySql(strSql) == 1)
           {
               return true;
           }
           else
           {
               return false;
           }
       }


       public static bool UpdateSoMain(So so)
       {
           string strSql = string.Format("update So set customerName='{0}',contact='{1}',salesId={2},salesOrderNo='{3}',customerPo='{4}',paymentTerm='{5}',freightTerm='{6}',customerAccount='{7}',specialInstructions='{8}',billTo='{9}',shipTo='{10}' where soId={11}",
        so.customerName, so.contact, so.salesId, so.salesOrderNo, so.customerPo,so.paymentTerm, so.freightTerm, so.customerAccount, so.specialInstructions, so.billTo, so.shipTo,so.soId);

           if (db.ExecDataBySql(strSql) == 1)
               return true;

           return false;
       }


       public static string GetUpDateSoItemString(SoItems soItem)
       {

          return string.Format("update SoItems set saleType={0},partNo='{1}',mfg='{2}',rohs={3},dc='{4}',intPartNo='{5}',shipFrom='{6}',shipMethod='{7}',trackingNo='{8}',qty={9},qtyShipped={10},currency={11},unitPrice={12},dockDate='{13}',shippedDate='{14}',shippingInstruction='{15}',packingInstruction='{16}' where soItemsId={17} ",
       soItem.saleType, soItem.partNo, soItem.mfg, soItem.rohs, soItem.dc,soItem.intPartNo, soItem.shipFrom, soItem.shipMethod, soItem.trackingNo, soItem.qty, soItem.qtyshipped, soItem.currencyType, soItem.unitPrice, soItem.dockDate.ToShortDateString(), soItem.shippedDate.HasValue ? soItem.shippedDate.Value.ToShortDateString() : "null",
    soItem.shippingInstruction, soItem.packingInstruction,soItem.soItemsId);

       }

       public static string GetSaveNewSoItemString(SoItems soItem)
       {
           string strsql = "insert into SoItems(soId,saleType,partNo,mfg,rohs,dc,intPartNo,shipFrom,shipMethod,trackingNo,qty,qtyShipped,currency,unitPrice,dockDate,shippedDate,shippingInstruction,packingInstruction) " +
        string.Format(" values({0},{1},'{2}','{3}',{4},'{5}','{6}','{7}','{8}','{9}',{10},{11},{12},{13},'{14}','{15}','{16}','{17}')", soItem.soId, soItem.saleType, soItem.partNo, soItem.mfg, soItem.rohs, soItem.dc,
        soItem.intPartNo, soItem.shipFrom, soItem.shipMethod, soItem.trackingNo, soItem.qty, soItem.qtyshipped, soItem.currencyType, soItem.unitPrice, soItem.dockDate.ToShortDateString(), soItem.shippedDate.HasValue ? soItem.shippedDate.Value.ToShortDateString() : "null",
        soItem.shippingInstruction, soItem.packingInstruction);
           return strsql;
       }

       public static void DeleteSoItembySoItemId(int soItemsId)
       {
           string strSql= string.Format("delete from SoItems where soItemsId={0}",soItemsId);
           db.ExecDataBySql(strSql);


       }


       public static void UpdateSoItems(List<SoItemsContentAndState> soItemStateList)
       {
           List<string> strSqls = new List<string>();
           
           foreach (SoItemsContentAndState sics in soItemStateList)
           {
               switch (sics.state)
               {
                   case OrderItemsState.Normal:
                       break;

                   case OrderItemsState.New:
                       strSqls.Add(GetSaveNewSoItemString(sics.soitem));
                       break;

                   case OrderItemsState.Modified:
                      strSqls.Add(GetUpDateSoItemString(sics.soitem));
                       break;

               }
           }
           if (strSqls.Count == 0)
           {
               return;
           }
           
           db.ExecDataBySqls(strSqls);

       }
    }
}
