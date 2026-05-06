using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Lop
    {
        private string _malop;
        private string _tenlop;
        public string Malop
        {
            get { return _malop; }
            set { _malop = value; }
        }
        public string Tenlop
        {
            get { return _tenlop; }
            set { _tenlop = value; }
        }
    }
    public class Sinhvien
    {
        private string _masv;
        private string _hoten;
        private bool _gioitinh;
        private DateTime _ngaysinh;
        private string _diachi;
        private string _malop;

        public string Masv
        {
            get
            {
                return _masv;
            }

            set
            {
                _masv = value;
            }
        }

        public string Hoten
        {
            get
            {
                return _hoten;
            }

            set
            {
                _hoten = value;
            }
        }

        public DateTime Ngaysinh
        {
            get
            {
                return _ngaysinh;
            }

            set
            {
                _ngaysinh = value;
            }
        }

        public string Diachi
        {
            get
            {
                return _diachi;
            }

            set
            {
                _diachi = value;
            }
        }

        public string Malop
        {
            get
            {
                return _malop;
            }

            set
            {
                _malop = value;
            }
        }
        public bool Gioitinh
        {
            get
            {
                return _gioitinh;
            }

            set
            {
                _gioitinh = value;
            }
        }
    }
} 
    
