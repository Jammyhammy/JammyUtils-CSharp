using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JammyUtils.ScaleReader.Classes;

namespace JammyUtils.ScaleReaderDemo
{
    public partial class TCPDemo : Form
    {
        public TCPDemo()
        {
            InitializeComponent();
            scaleTimer.Tick += ScaleTimer_Tick;
            
        }

        private async void ScaleTimer_Tick(object sender, EventArgs e)
        {
            var listresponses = new List<Task<string>>();

            // 
            Enumerable.Range(1, 10).ToList<int>().ForEach(x =>
            {
                listresponses.Add(TCPScaleReader.SendWeighRequest("192.168.1.50", 8000, length: 72));

                textBox1.AppendText($"Send Request{Environment.NewLine}");
            });

            while(listresponses.Count > 0)
            {
                var finishedtask = await Task.WhenAny(listresponses.ToArray());
                listresponses.Remove(finishedtask);
                textBox1.AppendText(finishedtask.Result);
            }
            
        }
        
        //private Task<string> GetWeighRequest(string address, int port, char? sendchar = null, int length = 128)
        //{
        //    return Task.Run(() =>
        //    {
        //        var result = "";
        //        //    var tsresponse = TCPScaleReader.SendWeighRequest("192.168.1.50", 8000, length: 72);
        //        //    textBox1.AppendText($"Send Request{Environment.NewLine}");
        //        //    await tsresponse;
        //        return result;
        //    });
        //}

    }
}
