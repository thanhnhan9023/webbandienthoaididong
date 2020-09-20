using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMuaBanDTDD.Models
{
    public class GioHang
    {
        dbDTDDDataContext data = new dbDTDDDataContext();
        public int iMaDT { set; get; }
        public string sTenDT { set; get; }
        public string sHinhAnh { set; get; }
        public string sMaNSX { set; get; }
        public Double dDonGia { set; get; }
        public int iSoLuong { set; get; }
        public Double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }

        public GioHang(int MaDT)
        {
            iMaDT = MaDT;
            HANG dt = data.HANGs.Single(n => n.MADT == iMaDT);
            sTenDT = dt.TENDT;
            sHinhAnh = dt.HINHANH;
            sMaNSX = dt.MANSX;
            dDonGia = Double.Parse(dt.GIA.ToString());
            iSoLuong = 1;
        }
    }
}