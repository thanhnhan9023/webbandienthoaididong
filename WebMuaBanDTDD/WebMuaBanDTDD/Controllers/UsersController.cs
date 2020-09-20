using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using WebMuaBanDTDD.Models;

namespace WebMuaBanDTDD.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /User/
        dbDTDDDataContext data = new dbDTDDDataContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection c, KHACHHANG Kh)
        {
            var hoten = c["Hoten"];
            var tendn = c["TenDN"];
            var password = c["Password"];
            var retype = c["Retype"];
            var diachi = c["Diachi"];
            var phone = c["Dienthoai"];
            var ngaysinh = string.Format("{0:dd/MM/yyyy}", c["Ngaysinh"]);
            if (string.IsNullOrEmpty(hoten))
                ViewData["Error1"] = "Họ tên không được bỏ trống";
            if (string.IsNullOrEmpty(tendn))
                ViewData["Error2"] = "Tên đăng nhập không được bỏ trống";
            if (string.IsNullOrEmpty(password))
                ViewData["Error3"] = "Password không được bỏ trống";
            if (string.IsNullOrEmpty(retype))
                ViewData["Error4"] = "Nhập lại Password không được bỏ trống";
            if (string.IsNullOrEmpty(phone))
                ViewData["Error5"] = "Điện thoại không được bỏ trống";
            else
            {
                Kh.TENKH = hoten;
                Kh.DCHI = diachi;
                Kh.DIENTHOAI = phone;
                Kh.TENDN = tendn;
                Kh.MATKHAU = password;
                data.KHACHHANGs.InsertOnSubmit(Kh);
                data.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection c)
        {
            var tendn = c["TenDN"];
            var password = c["Password"];
            if (string.IsNullOrEmpty(tendn))
                ViewData["Error1"] = "Tên đăng nhập không được bỏ trống";
            else if (string.IsNullOrEmpty(password))
                ViewData["Error2"] = "Password không được bỏ trống";
            else
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.TENDN == tendn && n.MATKHAU == password);
                if (kh != null)
                {
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index", "MainPage");
                }
                else
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}
