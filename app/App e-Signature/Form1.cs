using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Net.Security;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;


namespace App_e_Signature
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        int RSA_soP, RSA_soQ, RSA_soN, RSA_soE, RSA_soD, RSA_soPhi_n;
        public int RSA_d_dau = 0;
        private int RSA_ChonSoNgauNhien()
        {
            Random rd = new Random();
            return rd.Next(11, 101);
        }
        private bool RSA_kiemTraNguyenTo(int xi)
        {
            bool kiemtra = true;
            if (xi == 2 || xi == 3)
            {
                // kiemtra = true;
                return kiemtra;
            }
            else
            {
                if (xi == 1 || xi % 2 == 0 || xi % 3 == 0)
                {
                    kiemtra = false;
                }
                else
                {
                    for (int i = 5; i <= Math.Sqrt(xi); i = i + 6)
                        if (xi % i == 0 || xi % (i + 2) == 0)
                        {
                            kiemtra = false;
                            break;
                        }
                }
            }
            return kiemtra;
        }
        // "Hàm kiểm tra hai số nguyên tố cùng nhau"
        private bool RSA_nguyenToCungNhau(int ai, int bi)
        {
            bool ktx_;
            // giải thuật Euclid;
            int temp;
            while (bi != 0)
            {
                temp = ai % bi;
                ai = bi;
                bi = temp;
            }
            if (ai == 1) { ktx_ = true; }
            else ktx_ = false;
            return ktx_;
        }
        private int RSA_mod(int mx, int ex, int nx)
        {

            //Sử dụng thuật toán "bình phương nhân"
            //Chuyển e sang hệ nhị phân
            int[] a = new int[100];
            int k = 0;
            do
            {
                a[k] = ex % 2;
                k++;
                ex = ex / 2;
            }
            while (ex != 0);
            //Quá trình lấy dư
            int kq = 1;
            for (int i = k - 1; i >= 0; i--)
            {
                kq = (kq * kq) % nx;
                if (a[i] == 1)
                    kq = (kq * mx) % nx;
            }
            return kq;
        }

        private void RSA_taoKhoa()
        {
            //Tinh n=p*q
            RSA_soN = RSA_soP * RSA_soQ;

            //Tính Phi(n)=(p-1)*(q-1)
            RSA_soPhi_n = (RSA_soP - 1) * (RSA_soQ - 1);
            //Tính e là một số ngẫu nhiên có giá trị 0< e <phi(n) và là số nguyên tố cùng nhau với Phi(n)
            do
            {
                Random RSA_rd = new Random();
                RSA_soE = RSA_rd.Next(2, RSA_soPhi_n);
            }
            while (!RSA_nguyenToCungNhau(RSA_soE, RSA_soPhi_n));


            //Tính d là nghịch đảo modular của e
            RSA_soD = 0;
            int i = 2;
            while (((1 + i * RSA_soPhi_n) % RSA_soE) != 0 || RSA_soD <= 0)
            {
                i++;
                RSA_soD = (1 + i * RSA_soPhi_n) / RSA_soE;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = file.FileName;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            RSA_soP = RSA_soQ = 0;
            do
            {
                RSA_soP = RSA_ChonSoNgauNhien();
                RSA_soQ = RSA_ChonSoNgauNhien();
            }
            while (RSA_soP == RSA_soQ || !RSA_kiemTraNguyenTo(RSA_soP) || !RSA_kiemTraNguyenTo(RSA_soQ));
            RSA_taoKhoa();
            String E = Convert.ToString(RSA_soE);
            String N = Convert.ToString(RSA_soN);
            String D = Convert.ToString(RSA_soD);
            using (StreamWriter writer = File.CreateText("D:/Key/PublicKey.txt"))
            {
                writer.WriteAsync(N);
                writer.WriteAsync(",");
                writer.WriteAsync(E);
            }
            using (StreamWriter writer = File.CreateText("D:/Key/PrivateKey.txt"))
            {
                writer.WriteAsync(N);
                writer.WriteAsync(",");
                writer.WriteAsync(D);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = file.FileName;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = file.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string PrivateKey = File.ReadAllText(textBox4.Text);
            string s = PrivateKey;
            char c = char.Parse(",");
            int answer = -1;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == c)
                {
                    answer = i;
                    break;
                }
            }
            int temp = answer;
            string D = s.Substring(0, temp);
            string N = s.Substring(temp+1);

            Int32.TryParse(D, out int RSA_soD);
            Int32.TryParse(N, out int RSA_soN);

            string plaintext = File.ReadAllText(textBox1.Text);
            string chuoivao = ComputeSha256Hash(plaintext);
            byte[] mh_temp1 = Encoding.Unicode.GetBytes(chuoivao);
            string base64 = Convert.ToBase64String(mh_temp1);

            // Chuyen xau thanh ma Unicode
            int[] mh_temp2 = new int[base64.Length];
            for (int i = 0; i < base64.Length; i++)
            {
                mh_temp2[i] = (int)base64[i];
            }

            //Mảng a chứa các kí tự đã mã hóa
            int[] mh_temp3 = new int[mh_temp2.Length];
            for (int i = 0; i < mh_temp2.Length; i++)
            {
                mh_temp3[i] = RSA_mod(mh_temp2[i], RSA_soD, RSA_soN); // mã hóa
            }

            //Chuyển sang kiểu kí tự trong bảng mã Unicode
            string str = "";
            for (int i = 0; i < mh_temp3.Length; i++)
            {
                str = str + (char)mh_temp3[i];
            }
            byte[] data = Encoding.Unicode.GetBytes(str);
            string a = Convert.ToBase64String(data);
 
            using (StreamWriter writer = File.CreateText("D:/FileMaHoa/FileMaHoa.txt"))
            {
                writer.WriteAsync(ComputeSha256Hash(plaintext));
                writer.WriteAsync(a);
            }

        }
      


        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = file.FileName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                textBox5.Text = file.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string Send_file = File.ReadAllText(textBox5.Text);
            string Hash_Send = Send_file.Substring(0, 64);
            string MaHoa = Send_file.Substring(65);
   
            string PublicKey = File.ReadAllText(textBox3.Text);
            string s = PublicKey;
            char c = char.Parse(",");
            int answer = -1;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == c)
                {
                    answer = i;
                    break;}
            }

            int temp = answer;
            string E = s.Substring(0, temp);
            string N = s.Substring(temp + 1);

            Int32.TryParse(E, out int RSA_soE);
            Int32.TryParse(N, out int RSA_soN);

            String ChuoiVao = MaHoa;
            byte[] temp2 = Convert.FromBase64String(ChuoiVao);
            string giaima = Encoding.Unicode.GetString(temp2);

            int[] b = new int[giaima.Length];
            for (int i = 0; i < giaima.Length; i++)
            {
                b[i] = (int)giaima[i];
            }
            //Giải mã
            int[] t = new int[b.Length];
            for (int i = 0; i < t.Length; i++)
            {
                t[i] = RSA_mod(b[i], RSA_soD, RSA_soN);// giải mã
            }

            string str = "";
            for (int i = 0; i < t.Length; i++)
            {
                str = str + (char)t[i];
            }
            byte[] data2 = Convert.FromBase64String(str);
            string Hash_re = Encoding.Unicode.GetString(data2);
            textBox6.Text = Hash_re;
        }              
    }
        
        

     
}