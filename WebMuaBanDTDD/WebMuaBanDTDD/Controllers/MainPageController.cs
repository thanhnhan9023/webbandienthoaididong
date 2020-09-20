using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMuaBanDTDD.Models;
using PagedList;
using PagedList.Mvc;

namespace WebMuaBanDTDD.Controllers
{
    public class MainPageController : Controller
    {
        //
        // GET: /Home/
        dbDTDDDataContext data = new dbDTDDDataContext();

        public ActionResult Index(int ? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);

            var listHang = layHangMoi();
            return View(listHang.ToPagedList(pageNum, pageSize));
        }

        public ActionResult SlideShow()
        {
            return PartialView();
        }

        public ActionResult hienThiHang()
        {
            var hangMoi = layHangMoi();
            return View(hangMoi);
        }

        public List<HANG> layHangMoi()
        {
            return data.HANGs.ToList();
        }

        public ActionResult NhaSX()
        {
            var nsx = from sx in data.NHASXes select sx;
            return PartialView(nsx);
        }

        [HttpGet]
        public ActionResult DTTheoNSX(string maNSX)
        {
            List<HANG> hang_s = data.HANGs.Where(n => n.MANSX.Equals(maNSX)).ToList();
            return View(hang_s);
        }

        public ActionResult Details(int id)
        {
            var detailsDT = data.HANGs.Where(n => n.MADT == id).First();
            return View(detailsDT);
        }
    }
}
