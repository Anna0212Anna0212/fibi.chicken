using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 小飛雞
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Random newpic_top = new Random();  //柱子的位置移動亂數

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.Controls.Clear();   //這我也不太知道，將柱子建立的分身存入之類的
            timer2.Stop();   //柱子開始的timer
            pictureBox1.Location = new Point(125, 401);   //小雞定位
            timer1.Start();   //小雞的位置開始下降timer
            pictureBox2.Visible = false;   //柱子的範本，因為有出錯所以timer停止、顯示隱藏

            // 設置窗體能夠捕捉鍵盤事件 
            this.KeyPreview = true;
            // 綁定 KeyDown 事件 
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);


            timer2.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Top += 10;    //小雞下降
            if (pictureBox1.Top >= 700)    //如果碰倒邊界停止遊戲
            {
                timer1.Stop();
                MessageBox.Show("game over");
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.Top -= 50;   //點擊後小雞會飛起
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //柱子定位參考
            //left:900
            //x=1 高:-350~-150
            //x=0 低:450~250
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            // 建立 PictureBox 分身
            PictureBox newPictureBox = new PictureBox();  
            newPictureBox.Size = pictureBox2.Size;    // 以 pictureBox2 為模板複製
            int x = newpic_top.Next(0, 2);   //利用亂數來決定上還是下的柱子
            switch (x)
            {
                case 0:
                    newPictureBox.Image =new Bitmap("Flappy Bird down.png");   //newPictureBox開啟圖片向上的
                    //newPictureBox.Image = imageList1.Images[x];
                    newPictureBox.Location = new Point(918, newpic_top.Next(250, 450));  // 設定位置
                    break;
                case 1:
                    newPictureBox.Image = new Bitmap("Flappy Bird up.png");
                    //newPictureBox.Image = imageList1.Images[x];
                    newPictureBox.Location = new Point(918, newpic_top.Next(-350, -150));  // 設定位置
                    break;
            }
            newPictureBox.Visible = true;  // 顯示分身
            this.Controls.Add(newPictureBox);  // 將分身加入到表單的控制項集合中

            // 使用 Timer 來移動 newPictureBox
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();   
            //因為newPictureBox沒法在建立的timer3物件中顯示，只能在此建立一個timer來用
            timer.Interval = 10; // 設定移動的時間間隔 (20 毫秒)
            timer.Tick += (obj, y) =>
            {
                newPictureBox.Left -= 15;   //newPictureBox移動

                Rectangle rect1 = pictureBox1.Bounds;   //設定picturebox的界線
                Rectangle rect2 = newPictureBox.Bounds;

                if (newPictureBox.Left <= -10)   //如果newPictureBox碰到底部後消失
                {
                    timer.Stop();
                    this.Controls.Remove(newPictureBox);
                    newPictureBox.Dispose();
                    timer.Dispose();
                }
                else if (rect1.IntersectsWith(rect2))   //如果newPictureBox碰到小雞後消失
                {
                    timer.Stop();
                    this.Controls.Remove(newPictureBox);
                    newPictureBox.Dispose();
                    timer.Dispose();
                    timer1.Stop();
                    timer2.Stop();
                    MessageBox.Show("game over");
                }
            };

            // 啟動 Timer
            timer.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // 檢查是否按下了空白鍵 
            if (e.KeyCode == Keys.Space)  //其他按鍵：Keys.W
            {
                pictureBox1.Top -= 50;   //點擊後小雞會飛起
            }

        }
    }
}
