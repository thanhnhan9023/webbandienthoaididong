using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMuaBanDTDD.Models;

namespace WebMuaBanDTDD.Controllers
{
    public class GioHangController : Controller
    {
        //
        // GET: /GioHang/
        dbDTDDDataContext data = new dbDTDDDataContext();

        public ActionResult Index()
        {
            return View();
        }

        public List<GioHang> layGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult themGioHang(int iMaDT, string strURL)
        {
            List<GioHang> lstGioHang = layGioHang();
            GioHang sp = lstGioHang.Find(n => n.iMaDT == iMaDT);
            if (sp == null)
            {
                sp = new GioHang(iMaDT);
                lstGioHang.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.iSoLuong++;
                return Redirect(strURL);
            }
        }

        private int tongSoLuong()
        {
            int iTongSL = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
                iTongSL = lstGioHang.Sum(n => n.iSoLuong);
            return iTongSL;
        }

        private Double tongTien()
        {
            Double tong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
                tong = lstGioHang.Sum(n => n.dThanhTien);
            return tong;
        }

        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = layGioHang();
            if (lstGioHang.Count == 0)
                return RedirectToAction("Index", "MainPage");
            ViewBag.TongSl = tongSoLuong();
            ViewBag.TongTien = tongTien();
            return View(lstGioHang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSl = tongSoLuong();
            ViewBag.TongTien = tongTien();
            return PartialView();
        }

        public ActionResult xoaGioHang(int iMaSP)
        {
            List<GioHang> lstGioHang = layGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaDT == iMaSP);
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.iMaDT == iMaSP);
                return RedirectToAction("GioHang");
            }
            if (lstGioHang.Count == 0)
                return RedirectToAction("Index", "MainPage");
            return RedirectToAction("GioHang");
        }

        public ActionResult xoaAll()
        {
            List<GioHang> lstGioHang = layGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "MainPage");
        }

        public ActionResult capNhat(int iMaSP, FormCollection c)
        {
            List<GioHang> lstGioHang = layGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaDT == iMaSP);
            if (sp != null)
                sp.iSoLuong = int.Parse(c["txtSL"].ToString());
            return RedirectToAction("GioHang");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
                return RedirectToAction("DangNhap", "Users");
            if (Session["GioHang"] == null)
                return RedirectToAction("Index", "MainPage");
            List<GioHang> lstGioHang = layGioHang();
            ViewBag.TongSl = tongSoLuong();
            ViewBag.TongTien = tongTien();
            return View(lstGioHang);
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection c)
        {
            HOADON hd = new HOADON();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> gh = layGioHang();

            hd.MAKH = kh.MAKH;
            var ngayBan = string.Format("{0:MM/dd/yyyy}", c["NgayBan"]);
            hd.NGAYBAN = DateTime.Now;
            hd.THANHTIEN = tongTien();
            data.HOADONs.InsertOnSubmit(hd);
            data.SubmitChanges();

            foreach (var item in gh)
            {
                CHITIETHD ct = new CHITIETHD();
                ct.MAHD = hd.MAHD;
                ct.MADT = item.iMaDT;
                ct.SOLUONG = item.iSoLuong;
                ct.GIABAN = item.dDonGia;
                data.CHITIETHDs.InsertOnSubmit(ct);
            }

            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhan", "GioHang");
        }

        public ActionResult XacNhan()
        {
            return View();
        }
    }
}
