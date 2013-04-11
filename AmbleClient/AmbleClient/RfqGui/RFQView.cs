﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AmbleClient.RfqGui.RfqManager;

namespace AmbleClient.RfqGui
{
    public partial class RFQView : Form
    {
        int rfqId;
        RfqMgr rfqMgr;
        public RFQView(int rfqId)
        {
            InitializeComponent();
            this.rfqId = rfqId;
            rfqMgr = new RfqMgr();
        
        }

        private void RFQView_Load(object sender, EventArgs e)
        {
            Rfq rfq = rfqMgr.GetRfqAccordingToRfqId(rfqId);
            rfqItems1.FillTheTable(rfq);
            GuiOpAccordingToRfqState((RfqStatesEnum)rfq.rfqStates);
        }

        private void GuiOpAccordingToRfqState(RfqStatesEnum rfqState)
        {
            switch (rfqState)
            { 
                case RfqStatesEnum.New:
                      tsbQuote.Enabled = false;
                      tsbSo.Enabled = false;
                      tsbViewSo.Enabled = false;    
                       break;
                case RfqStatesEnum.Routed:
                       tsbRoute.Enabled = false;
                       tsbQuote.Enabled=false;
                       tsbSo.Enabled = false;
                       tsbViewSo.Enabled = false;
                       break;
                case RfqStatesEnum.Offered:
                       tsbRoute.Enabled = false;
                       tsbSo.Enabled = false;
                       tsbViewSo.Enabled = false;
                       break;
                case RfqStatesEnum.Quoted:
                       tsbRoute.Enabled = false;
                       tsbQuote.Enabled = false;
                       tsbViewSo.Enabled = false;
                       break;
                case RfqStatesEnum.HasSO:
                       tsbRoute.Enabled = false;
                       tsbQuote.Enabled = false;
                       break;
                case RfqStatesEnum.Closed:
                       tsbRoute.Enabled = false;
                       tsbQuote.Enabled = false;
                       tsbSo.Enabled = false;
                       tsbCloseRfq.Enabled = false;
                       break;
            }

        }

        private void tsbQuote_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Set The RFQ Status to Quoted?","", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {

                if (rfqMgr.ChangeRfqState(RfqStatesEnum.Quoted, rfqId))
                {
                    rfqMgr.AddRfqHistory(rfqId, UserInfo.UserId, "Quoted the RFQ");
                    GuiOpAccordingToRfqState(RfqStatesEnum.Quoted);
                }
                else
                {
                    MessageBox.Show("Quote the RFQ Fail");
                }
            }
        }

        private void tsbRoute_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Set The RFQ Status to Routed?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                if (rfqMgr.ChangeRfqState(RfqStatesEnum.Routed, rfqId))
                {
                    rfqMgr.AddRfqHistory(rfqId, UserInfo.UserId, "Routed the RFQ");
                    GuiOpAccordingToRfqState(RfqStatesEnum.Routed);

                }
                else
                {
                    MessageBox.Show("Route the RFQ Fail");
                }

            }
        }

        private void tsbUpdate_Click(object sender, EventArgs e)
        {
            if (rfqItems1.UpdateInfo(rfqId))
            {
                rfqMgr.AddRfqHistory(rfqId, UserInfo.UserId,"Updated the RFQ");
                MessageBox.Show("Update the RFQ successfully");
            }
            else
            {
                MessageBox.Show("Failed to Update the RFQ");
            }
        }

        private void tsbCopy_Click(object sender, EventArgs e)
        {
            rfqMgr.CopyRfq(rfqId, UserInfo.UserId);
        }

        private void tsbSo_Click(object sender, EventArgs e)
        {
            SO.NewSo newSo = new SO.NewSo(rfqId);
            newSo.FillCustomerAndContact(this.rfqItems1.tbCustomer.Text, this.rfqItems1.tbContact.Text);
            newSo.ShowDialog();
        }

        private void tsbViewSo_Click(object sender, EventArgs e)
        {
            SO.SoView soView = new SO.SoView(rfqId);
            soView.ShowDialog();
        }

        private void tsbCloseRfq_Click(object sender, EventArgs e)
        {
            if (rfqItems1.cbCloseReason.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select a Reason for Closing the RFQ");
                rfqItems1.cbCloseReason.Focus();
            }
            else
            {
                if (MessageBox.Show("Set The RFQ Status to Closed?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    rfqItems1.UpdateInfo(rfqId);
                    rfqMgr.ChangeRfqState(RfqStatesEnum.Closed, rfqId);
                    rfqMgr.AddRfqHistory(rfqId, UserInfo.UserId, "Closed the RFQ");
                    GuiOpAccordingToRfqState(RfqStatesEnum.Closed);
                }
            }
            
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
