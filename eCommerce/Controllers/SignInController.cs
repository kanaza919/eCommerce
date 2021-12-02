﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using eCommerce.Extensions;
using eCommerce.Models;
using eCommerce.Models.ViewModels;
using eCommerce.Areas;
using System.IO;

namespace eCommerce.Controllers
{
    public class SignInController : Controller
    {
        // GET: SignIn
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckkSignIn(NguoiDungViewModel nguoiDungViewModel)
        {
            DauGiaEntities db = new DauGiaEntities();
            if (ModelState.IsValid)
            {
                var taiKhoanHopLe = db.NguoiDungs.Where(s => s.Email == nguoiDungViewModel.Email && s.Password == nguoiDungViewModel.Password).SingleOrDefault();
                if (taiKhoanHopLe == null)
                {
                    this.AddNotification("Sai email hoặc mật khẩu!", NotificationType.ERROR);
                    return View("SignIn");
                }

                Session["HoTen"] = taiKhoanHopLe.HoTen.ToString();
                Session["MaNguoiDung"] = taiKhoanHopLe.MaNguoiDung.ToString();
                if (taiKhoanHopLe.IsAdmin == false)
                    return View("SignIn");
                return RedirectToAction("Index", "Admin/HomeAdmin");
            }
            else
            {
                return View("SignIn");
            }
        }
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(NguoiDungViewModel nguoiDungViewModel)
        {
            if (!ModelState.IsValid)
                return View(nguoiDungViewModel);

            DauGiaEntities db = new DauGiaEntities();
            var EmailHopLe = db.NguoiDungs.Where(s => s.Email == nguoiDungViewModel.Email).SingleOrDefault();
            if (EmailHopLe != null)
            {
                this.AddNotification("Email này đã bị trùng. Vui lòng sử dụng email khác!", NotificationType.ERROR);
                return View(nguoiDungViewModel);
            }

            //thêm hình ảnh vào thư mục UserImages và lưu đường dẫn vào database
            string fileName = Path.GetFileNameWithoutExtension(nguoiDungViewModel.ImageFile.FileName);
            string extension = Path.GetExtension(nguoiDungViewModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            nguoiDungViewModel.HinhAnh = "~/UserImages/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/UserImages/"), fileName);
            nguoiDungViewModel.ImageFile.SaveAs(fileName);

            //gán các giá trị IsAdmin, IsApprove, SoDuVi là giá trị mặc định
            nguoiDungViewModel.NgayDangKy = DateTime.Now;
            nguoiDungViewModel.IsAdmin = false;
            nguoiDungViewModel.IsApproved = false;
            nguoiDungViewModel.SoDuVi = 0;
            nguoiDungViewModel.IsRequesting = false;

            NguoiDung nguoiDung = nguoiDungViewModel;
            db.NguoiDungs.Add(nguoiDung);
            db.SaveChanges();

            TempData["toastr-success"] = "Đăng kí thành công";
            return RedirectToAction("SignIn");
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult CheckForgotPassword(NguoiDungViewModel nguoiDungViewModel, string command)
        {
            DauGiaEntities db = new DauGiaEntities();
            if (command == "Gửi mail")
            {
                var emailHopLe = db.NguoiDungs.Where(s => s.Email == nguoiDungViewModel.Email).SingleOrDefault();
                if (emailHopLe == null)
                {
                    this.AddNotification("Email không tồn tại. Vui lòng nhập lại!", NotificationType.WARNING);
                    return View("ForgotPassword");
                }

                string resetCode = Guid.NewGuid().ToString();
                TempData["resetCode"] = resetCode;
                TempData["email"] = nguoiDungViewModel.Email;
                SendResetPasswordLinkEmail(emailHopLe.Email, resetCode);
                this.AddNotification("Đã gửi mail. Vui lòng kiểm tra email!", NotificationType.SUCCESS);
                return View("ForgotPassword");
            }
            else
                return RedirectToAction("SignIn", "SignIn");
        }


        [NonAction]
        public void SendResetPasswordLinkEmail(string email, string resetCode)
        {
            var verifyUrl = "/SignIn/ResetPassword/" + resetCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("democnpmnc@gmail.com", "Long Nguyen");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "democnpmnc123";
            string subject = "Reset password GoodDeals account";
            string body = "Chào. Nhấp vào link kế bên để cài lại mật khẩu. <a href=" + link + ">Link đây</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            }) smtp.Send(message);
        }

        public ActionResult ResetPassword(string id)
        {
            TempData.Keep();
            if (TempData["resetCode"] != null)
                if (id == TempData["resetCode"].ToString())
                    return View();
            return HttpNotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model, string command)
        {
            if (command == "Xác nhận")
            {
                if (ModelState.IsValid)
                {
                    if (TempData["resetCode"] != null)
                    {
                        DauGiaEntities db = new DauGiaEntities();
                        string email = TempData["email"].ToString();
                        var taiKhoan = db.NguoiDungs.Where(s => s.Email == email).SingleOrDefault();
                        if (taiKhoan == null)
                            return View();
                        taiKhoan.Password = model.MatKhauMoi;
                        db.SaveChanges();
                        TempData.Clear();
                        this.AddNotification("Mật khẩu được thay đổi thành công.", NotificationType.SUCCESS);
                    }
                    else
                    {
                        this.AddNotification("Phiên đặt lại mật khẩu đã hết hạn. Vui lòng thử lại!", NotificationType.ERROR);
                    }
                }
            }
            else
            {
                TempData.Clear();
                return RedirectToAction("SignIn", "SignIn");
            }
            return View(model);
        }

        public ActionResult SignOut()
        {
            Session.Abandon();
            return RedirectToAction("SignIn", "SignIn");
        }
    }
}