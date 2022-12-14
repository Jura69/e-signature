using Security.Cryptography;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Security.Cryptography
{
    public class RSA
    {

        int RSA_soP, RSA_soQ, RSA_soN, RSA_soE, RSA_soD, RSA_soPhi_n;
        public int RSA_d_dau = 0;
        private int RSA_ChonSoNgauNhien()
        {
            Random rd = new Random();
            return rd.Next(101, 1001);
        }
        // Hàm kiểm tra nguyên tó

        private bool RSA_kiemTraNguyenTo(int xi)
        {
            bool kiemtra = true;
            if (xi == 2 || xi == 3)
            {
                return kiemtra; //kiem tra = true
            } else
            {
                if (xi == 1 || xi % 2 == 0 || xi % 3 == 0)
                {
                    kiemtra = false;    
                } else
                {
                    for (int i = 5; i <= Math.Sqrt(xi); i=i+6)
                    {
                        if (xi % i == 0 || xi % (i+2) == 0)
                        {
                            kiemtra = false;
                            break;
                        }
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
        // "Hàm lấy mod"
        public int RSA_mod(int mx, int ex, int nx)
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
        public void RSA_taoKhoa()
        {
            //Chọn ngẫu nhiên 2 số nguyên tố P và Q
            do
            {
                RSA_soP = RSA_ChonSoNgauNhien();
                RSA_soQ = RSA_ChonSoNgauNhien();
            }
            while (RSA_soP == RSA_soQ || !RSA_kiemTraNguyenTo(RSA_soP) || !RSA_kiemTraNguyenTo(RSA_soQ));
            //Tinh n=p*q
            RSA_soN = RSA_soP * RSA_soQ;
            //Tính Phi(n)=(p-1)*(q-1)
            RSA_soPhi_n = (RSA_soP - 1) * (RSA_soQ - 1);
            //Tính e là một số ngẫu nhiên có giá trị 0< e <phi(n) và là số nguyên tố cùng nhau với Phi(n)
            do
            {
                Random RSA_rd = new Random();
                RSA_soE = RSA_rd.Next(2, RSA_soPhi_n);
            } while (!RSA_nguyenToCungNhau(RSA_soE, RSA_soPhi_n));
            //Tính d là nghịch đảo modular của e
            RSA_soD = 0;
            int i = 2;
            while (((1 + i * RSA_soPhi_n) % RSA_soE) != 0 || RSA_soD <= 0)
            {
                i++;
                RSA_soD = (1 + i * RSA_soPhi_n) / RSA_soE;
            }
            Console.WriteLine("Public Key (E = {0},N = {1})",RSA_soE,RSA_soN);
            Console.WriteLine("Private Key (D = {0},N = {1})", RSA_soD, RSA_soN);
        }
  
        public void RSA_maHoa()
        {
            string input;
            Console.WriteLine("Nhap noi dung can ma hoa");
            input = Console.ReadLine();
            //chuyen string thanh ma unicode
            byte[] mh_1 = Encoding.Unicode.GetBytes(input);
            string base64 = Convert.ToBase64String(mh_1);
            //chuyen string thanh ma unicode
            int[] mh_2 = new int[base64.Length];
            for (int i = 0; i < base64.Length; i++)
            {
                mh_2[i] = (int)base64[i];
            }

            //Mang a chua cac ky tu da ma hoa
            int[] mh_3 = new int[mh_2.Length];
            for (int i = 0; i < mh_2.Length; i++)
            {
                mh_3[i] = RSA_mod(mh_2[i], RSA_soE, RSA_soN); //ma hoa
            }

            string str = "";
            for (int i = 0; i < mh_3.Length; i++)
            {
                str = str + (char)mh_3[i];
            }
            byte[] data = Encoding.Unicode.GetBytes(str);
            string strMaHoa = Convert.ToBase64String(data);
            Console.WriteLine("Chuoi da ma hoa");
            Console.WriteLine(strMaHoa);
            
        }

        public void RSA_giaiMa()
        {
            string input;
            Console.WriteLine("Nhap noi dung can giai ma");
            input = Console.ReadLine();
            byte[] temp2 = Convert.FromBase64String(input);
            string giaima = Encoding.Unicode.GetString(temp2);

            int[] b = new int[giaima.Length];
            for (int i = 0; i < giaima.Length; i++)
            {
                b[i] = (int)giaima[i];
            }
            //Giải mã
            int[] c = new int[b.Length];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = RSA_mod(b[i], RSA_soD, RSA_soN);// giải mã
            }

            string str = "";
            for (int i = 0; i < c.Length; i++)
            {
                str = str + (char)c[i];
            }
            Console.WriteLine("key");
            Console.WriteLine("Public Key (E = {0},N = {1})", RSA_soE, RSA_soN);
            Console.WriteLine("Private Key (D = {0},N = {1})", RSA_soD, RSA_soN);
            Console.WriteLine("Chuoi da giai ma");
            Console.WriteLine(str);

        }
    }
}