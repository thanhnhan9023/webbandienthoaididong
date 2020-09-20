using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMuaBanDTDD.Models;
using System.IO;
using System.Web.Security;
using PagedList;
using PagedList.Mvc;

namespace WebMuaBanDTDD.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        dbDTDDDataContext data = new dbDTDDDataContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dienthoai(int ? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            return View(data.HANGs.ToList().ToPagedList(pageNum, pageSize));
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection c)
        {
            var tenDN = c["TenDN"];
            var password = c["Password"];
            if (string.IsNullOrEmpty(tenDN))
                ViewData["Error1"] = "Tên đăng nhập không được để trống";
            if (string.IsNullOrEmpty(password))
                ViewData["Error2"] = "Mật khẩu không được để trống";
            else
            {
                ADMINISTRATOR ad = data.ADMINISTRATORs.SingleOrDefault(n => n.TENDN_ADMIN == tenDN && n.PASSWORD_ADMIN == password);
                if (ad != null)
                {
                    Session["TaiKhoanAd"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.ThongBao("Tên đăng nhập hoặc mật khẩu không đúng");
            }
            return View();
        }

        [HttpGet]
        public ActionResult themDienThoai()
        {
            ViewBag.MANSX = new SelectList(data.NHASXes.ToList().OrderBy(n => n.TENNHASX), "MANSX", "TENNHASX");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult themDienThoai(HANG dt, HttpPostedFileBase fileUp)
        {
            ViewBag.MANSX = new SelectList(data.NHASXes.ToList().OrderBy(n => n.TENNHASX), "MANSX", "TENNHASX");
            if (fileUp == null)
            {
                ViewBag.ThongBao = "Hãy chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUp.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images/sanpham"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    else
                        fileUp.SaveAs(path);
                    dt.HINHANH = fileName;
                    data.HANGs.InsertOnSubmit(dt);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("Dienthoai", "Admin");
        }

        public ActionResult chiTietDT(int id)
        {
            HANG dt = data.HANGs.SingleOrDefault(n => n.MADT == id);
            ViewBag.MADT = dt.MADT;
            if (dt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dt);
        }

        [HttpGet]
        public ActionResult xoaDT(int id)
        {
            HANG dt = data.HANGs.SingleOrDefault(n => n.MADT == id);
            ViewBag.MADT = dt.MADT;
            if (dt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dt);
        }

        [HttpPost, ActionName("xoaDT")]
        public ActionResult XacNhanXoa(int id)
        {
            HANG dt = data.HANGs.SingleOrDefault(n => n.MADT == id);
            ViewBag.MADT = dt.MADT;
            if (dt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.HANGs.DeleteOnSubmit(dt);
            data.SubmitChanges();
            return RedirectToAction("chiTietDT", "Admin");
        }

        [HttpGet]        
        public ActionResult suaDT(int id)
        {
            HANG dt = data.HANGs.SingleOrDefault(n => n.MADT == id);
            if (dt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MANSX = new SelectList(data.NHASXes.ToList().OrderBy(n => n.TENNHASX), "MANSX", "TENNHASX", dt.MANSX);
            return View(dt);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult suaDT(HANG dt, HttpPostedFileBase fileUp)
        {
            ViewBag.MANSX = new SelectList(data.NHASXes.ToList().OrderBy(n => n.TENNHASX), "MANSX", "TENNHASX");
            if (fileUp == null)
            {
                ViewBag.ThongBao = "Hãy chọn ảnh cho sản phẩm";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUp.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images/sanpham"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    else
                        fileUp.SaveAs(path);
                    dt.HINHANH = fileName;
                    UpdateModel(dt);
                    data.SubmitChanges();
                }
                return RedirectToAction("chiTietDT", "Admin");
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Admin");
        }
    }
}
