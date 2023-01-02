using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Net.Security;
using System.IO;

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

        #region RSA
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
            //Tính e là một số ngẫu nhiên có giá trị 1< e <phi(n) và là số nguyên tố cùng nhau với Phi(n)
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
            if (!Directory.Exists("D:/Key"))
            {
                Directory.CreateDirectory("D:/Key");
            }
            if (!File.Exists("D:/Key/RSA_PublicKey.txt"))
            {
                File.Create("D:/Key/RSA_PublicKey.txt");
            }
            else
            {
                if (!File.Exists("D:/Key/RSA_PrivateKey.txt"))
                {
                    File.Create("D:/Key/RSA_PrivateKey.txt");
                }
            }

            using (StreamWriter writer = File.CreateText("D:/Key/RSA_PrivateKey.txt"))
            {
                writer.WriteAsync(N);
                writer.WriteAsync(",");
                writer.WriteAsync(E);
            }
          
            using (StreamWriter writer = File.CreateText("D:/Key/RSA_PublicKey.txt"))
            {
                writer.WriteAsync(N);
                writer.WriteAsync(",");
                writer.WriteAsync(D);
            }
            MessageBox.Show("Tạo khóa thành công!", "Thông báo");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = file.FileName;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                textBox12.Text = file.FileName;
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
            if (textBox1.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Chưa Upload đầy đủ file cần thiết", "Lỗi");
            }
            else
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
                string N = s.Substring(0, temp);
                string E = s.Substring(temp + 1);

                Int32.TryParse(E, out int RSA_soE);
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
                    mh_temp3[i] = RSA_mod(mh_temp2[i], RSA_soE, RSA_soN); // mã hóa
                }

                //Chuyển sang kiểu kí tự trong bảng mã Unicode
                string str = "";
                for (int i = 0; i < mh_temp3.Length; i++)
                {
                    str = str + (char)mh_temp3[i];
                }
                byte[] data = Encoding.Unicode.GetBytes(str);
                string a = Convert.ToBase64String(data);

                if (!Directory.Exists("D:/FileKy"))
                {
                    Directory.CreateDirectory("D:/FileKy");

                }
                if (!File.Exists("D:/FileKy/RSA_FileKy.txt"))
                {
                    File.Create("D:/FileKy/RSA_FileKy.txt");
                }

                using (StreamWriter writer = File.CreateText("D:/FileKy/RSA_FileKy.txt"))
                {
                    writer.WriteAsync(ComputeSha256Hash(plaintext));
                    writer.WriteAsync(a);
                }
                MessageBox.Show("Ký file thành công!", "Thông báo");
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
            if (textBox2.Text == "" || textBox3.Text == "" || textBox5.Text == "" )
            {
                MessageBox.Show("Chưa Upload đầy đủ file cần thiết", "Lỗi");
            }
            else
            {
                string Send_file = File.ReadAllText(textBox5.Text);
                string Hash_Send = Send_file.Substring(0, 64);
                string MaHoa = Send_file.Substring(64);

                string PublicKey = File.ReadAllText(textBox3.Text);
                string s = PublicKey;
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
                string N = s.Substring(0, temp);
                string D = s.Substring(temp + 1);

                Int32.TryParse(D, out int RSA_soD);
                Int32.TryParse(N, out int RSA_soN);

                string ChuoiVao = MaHoa;
                byte[] temp2 = Convert.FromBase64String(ChuoiVao);
                string giaima = Encoding.Unicode.GetString(temp2);

                int[] b = new int[giaima.Length];
                for (int i = 0; i < giaima.Length; i++)
                {
                    b[i] = (int)giaima[i];
                }
                //Giải mã
                int[] d = new int[b.Length];
                for (int i = 0; i < d.Length; i++)
                {
                    d[i] = RSA_mod(b[i], RSA_soD, RSA_soN);// giải mã
                }

                string str = "";
                for (int i = 0; i < d.Length; i++)
                {
                    str = str + (char)d[i];
                }
                byte[] data2 = Convert.FromBase64String(str);
                string RSA_giaima = Encoding.Unicode.GetString(data2);
                if (String.Compare(RSA_giaima, Hash_Send) == 0)
                {
                    MessageBox.Show("Chữ ký chính xác, file xác thức", "Xác thực thành công!");
                }
                else
                {
                    MessageBox.Show("Chữ ký không chính xác, file không xác thực", "Xác thực thất bại!");
                }
            }

        }
        #endregion


        public int EsoP, EsoQ, E_So_G_A, EsoA, EsoX, EsoD, EsoK, EsoY;

        private void button15_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                textBox7.Text = file.FileName;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                textBox9.Text = file.FileName;
            }
        }

        private int E_ChonSoNgauNhien()
        {
            Random rdE = new Random();
            return rdE.Next(1000, 3000);
        }
        private bool E_kiemTraNguyenTo(int so_kt)
        {
            bool kiemtra = true;
            if (so_kt == 2 || so_kt == 3)
            {
                // kiemtra = true;
                return kiemtra;
            }
            else
            {
                if (so_kt == 1 || so_kt % 2 == 0 || so_kt % 3 == 0)
                {
                    kiemtra = false;
                }
                else
                {
                    for (int i = 5; i <= Math.Sqrt(so_kt); i = i + 6)
                        if (so_kt % i == 0 || so_kt % (i + 2) == 0)
                        {
                            kiemtra = false;
                            break;
                        }
                }
            }
            return kiemtra;
        }  //"Hàm kiểm tra nguyên tố"

        private void button11_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                textBox10.Text = file.FileName;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                textBox11.Text = file.FileName;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
           
        }

        private bool E_kiemTraUocCuaSoP(int so_P, int so_Q)
        {
            bool kt_Okie = true;
            if ((so_P - 1) % so_Q == 0)
            {
                kt_Okie = true;
            }
            else
                kt_Okie = false;
            return kt_Okie;
        }
        public int E_LuyThuaModulo_(int CoSo_, int SoMu_, int soModulo_)
        {

            //Sử dụng thuật toán "bình phương nhân"
            //Chuyển e sang hệ nhị phân
            int[] a = new int[100];
            int k = 0;
            do
            {
                a[k] = SoMu_ % 2;
                k++;
                SoMu_ = SoMu_ / 2;
            }
            while (SoMu_ != 0);
            //Quá trình lấy dư
            int kq = 1;

            for (int i = k - 1; i >= 0; i--)
            {
                kq = (kq * kq) % soModulo_;
                if (a[i] == 1)
                    kq = (kq * CoSo_) % soModulo_;
            }
            return kq;
        }
        private bool E_kiemTraPTSinh(int so_kt, int E_SoP_, int E_soQ_)// kiem tra phan tu sinh
        {
            bool ktOkie = true;
            int soMu = E_SoP_ - 1 / E_soQ_;
            int ketQuaKT = E_LuyThuaModulo_(so_kt, soMu, E_SoP_);

            if (ketQuaKT != 1)
            {
                ktOkie = true;
            }
            else
            {
                if (ketQuaKT == 1) ktOkie = false;
            }
            return ktOkie;
        }
        private bool nguyenToCungNhau(int ai, int bi)// "Hàm kiểm tra hai số nguyên tố cùng nhau"
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
        private string TapP_1(int soDauVao)
        {
            string ChuoiDauRa = string.Empty;
            for (int i = 1; i < soDauVao; i++)
            {
                if (nguyenToCungNhau(soDauVao, i) == true)
                { ChuoiDauRa += i.ToString() + "#"; }
            }
            return ChuoiDauRa;
        }
        // Find the all factors of Ø  {f1,f2,….,fn} – { 1 }
        private string Tap_Qi(int soDauvao) // tìm các số khi phân tích ra thừa số của số P
        {
            string ChuoiDauRa = string.Empty;
            int soix = 2;
            while (soDauvao != 1)
            {
                if (soDauvao % soix == 0)
                {
                    ChuoiDauRa += soix.ToString() + "#";
                    soDauvao = soDauvao / soix;
                }
                else soix++;
            }
            return ChuoiDauRa;
        }
        private int E_tinhModulo_nghichdao(int SoNCNDn, int SoMdlm)
        {
            int kd = SoMdlm;
            int r = 1, q, y0 = 0, y1 = 1, y = 0;
            while (SoNCNDn != 0)
            {
                r = SoMdlm % SoNCNDn;
                if (r == 0)
                    break;
                else
                {
                    q = SoMdlm / SoNCNDn;
                    y = y0 - y1 * q;
                    SoMdlm = SoNCNDn;
                    SoNCNDn = r;
                    y0 = y1;
                    y1 = y;
                }
            }
            if (y >= 0)
                return y;
            else
            {
                y = kd + y;
                return y;
            }
        }
        private int E_TinhC1muxModP(int SoC1, int SomuX, int somDLP)
        {
            int kq_E_TinhC1muxModP = 1;
            for (int i = 0; i <= SomuX; i++)
            {
                kq_E_TinhC1muxModP = kq_E_TinhC1muxModP * E_tinhModulo_nghichdao(SoC1, somDLP);
            }
            return kq_E_TinhC1muxModP;
        }

        public string E_MaHoa(string ChuoiVao)
        {
            //Chuyen xau thanh ma Unicode         

            byte[] mhE_temp1 = Encoding.Unicode.GetBytes(ChuoiVao);
            string base64 = Convert.ToBase64String(mhE_temp1);

            // Chuyển xâu thành mã Unicode dạng số          
            int[] mh_temp2 = new int[base64.Length];
            for (int i = 0; i < base64.Length; i++)
            {
                mh_temp2[i] = (int)base64[i];

            }


            //Mảng a chứa các kí tự sẽ  mã hóa
            int[] mh_temp3 = new int[mh_temp2.Length];
            // thực hiện mã hóa: z = (d^k * m ) mod p

            for (int i = 0; i < mh_temp2.Length; i++)
            {
                mh_temp3[i] = ((mh_temp2[i] % EsoP) * (E_LuyThuaModulo_(EsoD, EsoK, EsoP))) % EsoP;

            }
            //Chuyển sang kiểu kí tự trong bảng mã Unicode
            string str = "";
            for (int i = 0; i < mh_temp3.Length; i++)
            {
                str = str + (char)mh_temp3[i];

            }
            byte[] E_data1 = Encoding.Unicode.GetBytes(str);
            string BanMaHoa = Convert.ToBase64String(E_data1);
            return BanMaHoa;

        }
        public string E_GiaiMa(string ChuoiVao)
        {
            //Chuyen xau thanh ma Unicode       
            string BanGiaiMa = "";

            byte[] Egm_temp1 = Convert.FromBase64String(ChuoiVao);
            string Egm_giaima = Encoding.Unicode.GetString(Egm_temp1);

            int[] Eb = new int[Egm_giaima.Length];
            for (int i = 0; i < Egm_giaima.Length; i++)
            {
                Eb[i] = (int)Egm_giaima[i];

            }
            //Giải mã
            //   m = ( r * z ) mod p =((r mod p) * (z mod p))mod p  with r = y^(p-1-x) mod p

            int[] Ec = new int[Eb.Length];
            int sor = E_LuyThuaModulo_(EsoY, (EsoP - (1 + EsoX)), EsoP);

            for (int i = 0; i < Ec.Length; i++)
            {
                Ec[i] = (Eb[i] * sor) % EsoP;// giải mã

            }
            string str = "";
            for (int i = 0; i < Ec.Length; i++)
            {
                str = str + (char)Ec[i];
            }
            byte[] data2 = Convert.FromBase64String(str);
            BanGiaiMa = Encoding.Unicode.GetString(data2);
            return BanGiaiMa;
        }

        private void TaoKhoa_click()
        {
            EsoQ = E_So_G_A = EsoA = EsoX = EsoD = EsoK = 0;

            // chọn số nguyên tố ngẫu nhiên Q thỏa mãn Q là ước của P - 1;
            do
            {
                Random rdQ = new Random();
                EsoQ = rdQ.Next(2, EsoP - 1);
            }
            while (!E_kiemTraNguyenTo(EsoP) || !E_kiemTraUocCuaSoP(EsoP, EsoQ));
            // tìm số G để tìm số A (A là phần tử sinh): 
            do
            {
                Random rdE_So_G_A = new Random();
                E_So_G_A = rdE_So_G_A.Next(2, EsoP - 1);
            }
            while (!E_kiemTraPTSinh(E_So_G_A, EsoP, EsoQ));

            EsoA = E_LuyThuaModulo_(E_So_G_A, EsoP - 1 / EsoQ, EsoP); // phần tử sinh

            do
            {
                Random rdEsoX = new Random();
                EsoX = rdEsoX.Next(2, EsoP - 2);
            }
            while (EsoX == EsoQ || EsoX == E_So_G_A);
            // d= a^x mod P
            EsoD = E_LuyThuaModulo_(EsoA, EsoX, EsoP);// beta; d          
            do
            {
                Random rdEsoK = new Random();
                EsoK = rdEsoK.Next(2, EsoP - 2);
            }
            while (EsoK == EsoX || EsoK == EsoA || EsoK == EsoQ || EsoK == E_So_G_A || !nguyenToCungNhau(EsoK, EsoP - 1));
            // Tính Y = A^k mod p - Khóa công khai
            EsoY = E_LuyThuaModulo_(EsoA, EsoK, EsoP);
        }

        

        private void button9_Click(object sender, EventArgs e)
        {
            string e_X = Convert.ToString(EsoX);
            string e_P = Convert.ToString(EsoP);
            string e_A = Convert.ToString(EsoA);
            string e_D = Convert.ToString(EsoD);
            string e_K = Convert.ToString(EsoK);
            string e_Y = Convert.ToString(EsoY);
            if (EsoP == 0) 
            {
                do
                {
                    EsoP = E_ChonSoNgauNhien();
                }
                while (E_kiemTraNguyenTo(EsoP) == false);
                TaoKhoa_click();
                e_X = Convert.ToString(EsoX);
                e_P = Convert.ToString(EsoP);
                e_A = Convert.ToString(EsoA);
                e_D = Convert.ToString(EsoD);
                e_K = Convert.ToString(EsoK);
                e_Y = Convert.ToString(EsoY);
                {
                    EsoP = int.Parse(e_P);
                    if (E_kiemTraNguyenTo(EsoP) == false)
                    {
                        MessageBox.Show("Phải chọn P là số nguyên tố ", "Thông Báo ");
                    }
                    else
                        if (EsoP < 1000)
                    {
                        MessageBox.Show("Số P quá nhỏ! Nhập số khác ", "Thông Báo ");
                    }
                    else
                    {
                        TaoKhoa_click();
                        e_X = Convert.ToString(EsoX);
                        e_P = Convert.ToString(EsoP);
                        e_A = Convert.ToString(EsoA);
                        e_D = Convert.ToString(EsoD);
                        e_K = Convert.ToString(EsoK);
                        e_Y = Convert.ToString(EsoY);
                    }
                }
            }
       

            if (!Directory.Exists("D:/Key"))
            {
                Directory.CreateDirectory("D:/Key");
            }
            if (!File.Exists("D:/Key/E_PublicKey.txt"))
            {
                File.Create("D:/Key/E_PublicKey.txt");
            }
            else
            {
                if (!File.Exists("D:/Key/E_PrivateKey.txt"))
                {
                    File.Create("D:/Key/E_PrivateKey.txt");
                }
            }

            using (StreamWriter writer = File.CreateText("D:/Key/E_PrivateKey.txt"))
            {
                writer.WriteAsync(e_X);
            }

            using (StreamWriter writer = File.CreateText("D:/Key/E_PublicKey.txt"))
            {
                writer.WriteAsync(e_P);
                writer.WriteAsync(",");
                writer.WriteAsync(e_A);
                writer.WriteAsync(",");
                writer.WriteAsync(e_D);
            }
            MessageBox.Show("Tạo khóa thành công!", "Thông báo");

        }

 



    }
}