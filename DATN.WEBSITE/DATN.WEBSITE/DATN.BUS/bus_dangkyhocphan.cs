﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.DATA;

namespace DATN.BUS
{
    public class bus_dangkyhocphan
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetDanhSachDiem(int id_sinhvien)
        {
            try
            {
                DataTable dt = null;
                var xxxx = (
                    from hkht in db.tbl_NAMHOC_HKY_HTAIs
                    join nhht in db.tbl_NAMHOC_HIENTAIs on new { ID_NAMHOC_HIENTAI = Convert.ToInt32(hkht.ID_NAMHOC_HIENTAI) } equals new { ID_NAMHOC_HIENTAI = nhht.ID_NAMHOC_HIENTAI }
                    where
                        (hkht.IS_DELETE == null || hkht.IS_DELETE != 1) && (nhht.IS_DELETE != 1 || nhht.IS_DELETE == null) &&
                        (from hp in db.tbl_LOP_HOCPHANs
                         where (hp.IS_DELETE != 1 || hp.IS_DELETE == null) && hkht.ID_NAMHOC_HKY_HTAI == hp.ID_NAMHOC_HKY_HTAI &&
                             (from diem in db.tbl_DIEM_SINHVIENs
                              where diem.IS_DELETE != 1 || diem.IS_DELETE == null
                              select new
                              {
                                  diem.ID_LOPHOCPHAN
                              }).Contains(new { ID_LOPHOCPHAN = (System.Int32?)hp.ID_LOPHOCPHAN })
                         select new
                         {
                             hp.ID_NAMHOC_HKY_HTAI
                         }).Contains(new { ID_NAMHOC_HKY_HTAI = (System.Int32?)hkht.ID_NAMHOC_HKY_HTAI })
                    select new
                    {
                        ID = ("HK" + Convert.ToString(hkht.ID_NAMHOC_HKY_HTAI)),
                        NAME = ("Học kỳ" + " " + Convert.ToString(hkht.HOCKY) + " Năm " + Convert.ToString(nhht.NAMHOC_TU) + "-" + Convert.ToString(nhht.NAMHOC_DEN)),
                        MA_MONHOC = "",
                        SO_TC = 0,
                        DIEM_BT = (double?)0,
                        DIEM_GK = (double?)0,
                        DIEM_CK = (double?)0,
                        DIEM_TONG = (double?)0,
                        DIEM_HE4 = (double?)0,
                        DIEM_CHU = "",
                        CACH_TINHDIEM = "",
                        ID_PARENT = ""
                    }).Concat
                    (
                        from diem in db.tbl_DIEM_SINHVIENs
                        join hp in db.tbl_LOP_HOCPHANs on
                            new { ID_LOPHOCPHAN = Convert.ToInt32(diem.ID_LOPHOCPHAN) } equals new { ID_LOPHOCPHAN = hp.ID_LOPHOCPHAN }

                        join mh in db.tbl_MONHOCs on
                            new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                        join hkht in db.tbl_NAMHOC_HKY_HTAIs on
                            new { ID_NAMHOC_HKY_HTAI = Convert.ToInt32(hp.ID_NAMHOC_HKY_HTAI) } equals new { ID_NAMHOC_HKY_HTAI = hkht.ID_NAMHOC_HKY_HTAI }

                        join nhht in db.tbl_NAMHOC_HIENTAIs on
                        new { ID_NAMHOC_HIENTAI = Convert.ToInt32(hkht.ID_NAMHOC_HIENTAI) } equals new { ID_NAMHOC_HIENTAI = nhht.ID_NAMHOC_HIENTAI }

                        where
                            (diem.IS_DELETE != 1 || diem.IS_DELETE == null) && diem.ID_SINHVIEN == id_sinhvien
                        select new
                        {
                            ID = ("D" + Convert.ToString(diem.ID_KETQUA)),
                            NAME = mh.TEN_MONHOC,
                            MA_MONHOC = mh.MA_MONHOC,
                            SO_TC = (int)mh.SO_TC,
                            DIEM_BT = (double?)diem.DIEM_BT,
                            DIEM_GK = (double?)diem.DIEM_GK,
                            DIEM_CK = (double?)diem.DIEM_CK,
                            DIEM_TONG = (double?)diem.DIEM_TONG,
                            DIEM_HE4 = (double?)diem.DIEM_HE4,
                            DIEM_CHU = diem.DIEM_CHU,
                            CACH_TINHDIEM = mh.CACH_TINHDIEM,
                            ID_PARENT = ("HK" + Convert.ToString(hp.ID_NAMHOC_HKY_HTAI))
                        }
                    );

                dt = TableUtil.LinqToDataTable(xxxx);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        } 

        public int GetHeDaoTao(int idsinhvien)
        {
            try
            {
                int res = 0;
                var idhedaotao = (from l in db.tbl_LOPHOCs
                                  join sv in db.tbL_SINHVIENs on new { ID_LOPHOC = l.ID_LOPHOC } equals new { ID_LOPHOC = Convert.ToInt32(sv.ID_LOPHOC) }
                                  join kn in db.tbl_KHOAHOC_NGANHs on new { ID_KHOAHOC_NGANH = Convert.ToInt32(l.ID_KHOAHOC_NGANH) } equals new { ID_KHOAHOC_NGANH = kn.ID_KHOAHOC_NGANH }
                                  join kh in db.tbl_KHOAHOCs on new { ID_KHOAHOC = Convert.ToInt32(kn.ID_KHOAHOC) } equals new { ID_KHOAHOC = kh.ID_KHOAHOC }
                                  where
                                    (kn.IS_DELETE != 1 ||
                                    kn.IS_DELETE == null) &&
                                    (kh.IS_DELETE != 1 ||
                                    kh.IS_DELETE == null) &&
                                    (l.IS_DELETE != 1 ||
                                    l.IS_DELETE == null) &&
                                    (sv.IS_DELETE != 1 ||
                                    sv.IS_DELETE == null) &&
                                    sv.ID_SINHVIEN == idsinhvien
                                  select new
                                  {
                                      kh.ID_HE_DAOTAO
                                  }).Distinct().First().ID_HE_DAOTAO;
                res = Convert.ToInt32(idhedaotao.ToString());
                return res;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetAllKhoaHoc(int pID_HEDAOTAO)
        {
            try
            {
                DataTable dt = null;
                if (pID_HEDAOTAO > 0)
                {
                    var kh = from khoc in db.tbl_KHOAHOCs
                             join hdt in db.tbl_HEDAOTAOs on khoc.ID_HE_DAOTAO equals hdt.ID_HE_DAOTAO
                             where (khoc.IS_DELETE != 1 || khoc.IS_DELETE == null) && (hdt.IS_DELETE != 1 || hdt.IS_DELETE == null) && khoc.ID_HE_DAOTAO == pID_HEDAOTAO
                             select new
                             {
                                 khoc.ID_KHOAHOC,
                                 khoc.MA_KHOAHOC,
                                 khoc.ID_HE_DAOTAO,
                                 khoc.KYHIEU,
                                 khoc.NAM_BD,
                                 khoc.NAM_KT,
                                 khoc.SO_HKY_1NAM,
                                 khoc.SO_HKY,
                                 khoc.TEN_KHOAHOC,
                                 khoc.TRANGTHAI,
                                 hdt.TEN_HE_DAOTAO

                             };
                    dt = TableUtil.LinqToDataTable(kh);
                }
                else
                {
                    var kh = from khoc in db.tbl_KHOAHOCs
                             join hdt in db.tbl_HEDAOTAOs on khoc.ID_HE_DAOTAO equals hdt.ID_HE_DAOTAO
                             where (khoc.IS_DELETE != 1 || khoc.IS_DELETE == null) && (hdt.IS_DELETE != 1 || hdt.IS_DELETE == null)
                             select new
                             {
                                 khoc.ID_KHOAHOC,
                                 khoc.MA_KHOAHOC,
                                 khoc.ID_HE_DAOTAO,
                                 khoc.KYHIEU,
                                 khoc.NAM_BD,
                                 khoc.NAM_KT,
                                 khoc.SO_HKY_1NAM,
                                 khoc.SO_HKY,
                                 khoc.TEN_KHOAHOC,
                                 khoc.TRANGTHAI,
                                 hdt.TEN_HE_DAOTAO

                             };
                    dt = TableUtil.LinqToDataTable(kh);
                }

                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetNgayHoc()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("THU", typeof(Decimal));
            dt.Columns.Add("NGAY", typeof(string));
            DataRow dr = dt.NewRow();
            dr["THU"] = 0;
            dr["NGAY"] = "---------------Tất cả-------------";
            dt.Rows.Add(dr);
            for (int i = 2; i <= 8; i++)
            {
                DataRow r = dt.NewRow();
                r["THU"] = i;
                if (i == 8)
                {
                    r["NGAY"] = "Chủ nhật";
                }
                else
                {
                    r["NGAY"] = "Thứ" + " " + i;
                }
                dt.Rows.Add(r);
                dt.AcceptChanges();
            }
            return dt;
        }

        public DataTable GetDSLopHocWhereKhoaHoc(int idKhoahoc)
        {
            try
            {
                DataTable dt = new DataTable();
                var lophoc = from lop in db.tbl_LOPHOCs
                             join khoanganh in db.tbl_KHOAHOC_NGANHs on lop.ID_KHOAHOC_NGANH equals khoanganh.ID_KHOAHOC_NGANH
                             join khoa in db.tbl_KHOAHOCs on khoanganh.ID_KHOAHOC equals khoa.ID_KHOAHOC
                             join nganh in db.tbl_NGANHs on khoanganh.ID_NGANH equals nganh.ID_NGANH
                             where (lop.IS_DELETE != 1 || lop.IS_DELETE == null) &&
                                   (khoanganh.IS_DELETE != 1 || khoanganh.IS_DELETE == null) &&
                                   (khoa.IS_DELETE != 1 || khoa.IS_DELETE == null) &&
                                   (nganh.IS_DELETE != 1 || nganh.IS_DELETE == null) &&
                                   khoanganh.ID_KHOAHOC == idKhoahoc
                             select new
                             {
                                 lop.ID_LOPHOC,
                                 TEN_LOP = "Lớp"+" - "+lop.MA_LOP.Trim()+" - " +nganh.TEN_NGANH+" - " +lop.NGAY_MOLOP.Year.ToString()
                             };
                dt = TableUtil.LinqToDataTable(lophoc);
                return dt;
            }catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetLopHPDK(int id_sinhvien)
        {
            try
            {
                DataTable dt = null;
                var hpdk = from dkhp in db.tbl_HP_DANGKies
                           join hp in db.tbl_LOP_HOCPHANs on dkhp.ID_LOPHOCPHAN equals hp.ID_LOPHOCPHAN
                           join hk in db.tbl_NAMHOC_HKY_HTAIs on hp.ID_NAMHOC_HKY_HTAI equals hk.ID_NAMHOC_HKY_HTAI
                           join nh in db.tbl_NAMHOC_HIENTAIs on hk.ID_NAMHOC_HIENTAI equals nh.ID_NAMHOC_HIENTAI
                           join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals
                               new { ID_MONHOC = mh.ID_MONHOC }
                           where
                               (dkhp.IS_DELETE != 1 || dkhp.IS_DELETE == null) &&
                               (hp.IS_DELETE != 1 || hp.IS_DELETE == null) &&
                               (mh.IS_DELETE != 1 || mh.IS_DELETE == null) &&
                                (hk.IS_DELETE != 1 || hk.IS_DELETE == null) &&
                                (nh.IS_DELETE != 1 || nh.IS_DELETE == null) &&
                                hk.IS_HIENTAI == 1 &&
                               dkhp.ID_SINHVIEN == id_sinhvien
                           select new
                           {
                               dkhp.ID_DANGKY,
                               dkhp.ID_SINHVIEN,
                               dkhp.ID_LOPHOCPHAN,
                               dkhp.ID_THAMSO,
                               mh.MA_MONHOC,
                               mh.TEN_MONHOC,
                               NGAY_DANGKY = dkhp.NGAY_DANGKY.ToString(),
                               dkhp.GIO_DANGKY,
                               mh.SO_TC,
                               dkhp.DON_GIA,
                               dkhp.THANH_TIEN
                           };
                dt = TableUtil.LinqToDataTable(hpdk);
                dt.Columns.Add("TRANGTHAI");
                foreach (DataRow r in dt.Rows)
                {
                    r["TRANGTHAI"] = "Đã lưu vào cơ sở dữ liệu";
                }
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetMonHocTheoLop(int idlophoc)
        {
            try
            {
                DataTable dt = null;
                var dsmonhoc = from mh in db.tbl_MONHOCs
                               where
                                   (mh.IS_DELETE != 1 ||
                                    mh.IS_DELETE == null) &&
                                   (from hp in db.tbl_LOP_HOCPHANs
                                    join hkht in db.tbl_NAMHOC_HKY_HTAIs on
                                        new { ID_NAMHOC_HKY_HTAI = Convert.ToInt32(hp.ID_NAMHOC_HKY_HTAI) } equals
                                        new { ID_NAMHOC_HKY_HTAI = hkht.ID_NAMHOC_HKY_HTAI }
                                    join nhht in db.tbl_NAMHOC_HIENTAIs on
                                        new { ID_NAMHOC_HIENTAI = Convert.ToInt32(hkht.ID_NAMHOC_HIENTAI) } equals
                                        new { ID_NAMHOC_HIENTAI = nhht.ID_NAMHOC_HIENTAI }
                                    where
                                        (hp.IS_DELETE != 1 ||
                                         hp.IS_DELETE == null) &&
                                        (hkht.IS_DELETE != 1 ||
                                         hkht.IS_DELETE == null) &&
                                        (nhht.IS_DELETE != 1 ||
                                         nhht.IS_DELETE == null) &&
                                        hkht.IS_HIENTAI == 1 &&
                                        nhht.IS_HIENTAI == 1 &&
                                        (from knct in db.tbl_KHOAHOC_NGANH_CTIETs
                                         where
                                             (knct.IS_DELETE != 1 ||
                                              knct.IS_DELETE == null) &&

                                             (from l in db.tbl_LOPHOCs
                                              where
                                                  (l.IS_DELETE != 1 ||
                                                   l.IS_DELETE == null) &&
                                                  l.ID_LOPHOC == idlophoc
                                              select new
                                              {
                                                  l.ID_KHOAHOC_NGANH
                                              }).Contains(new { ID_KHOAHOC_NGANH = knct.ID_KHOAHOC_NGANH })
                                         select new
                                         {
                                             knct.ID_KHOAHOC_NGANH_CTIET
                                         }).Contains(new { ID_KHOAHOC_NGANH_CTIET = (System.Int32)hp.ID_KHOAHOC_NGANH_CTIET })
                                    select new
                                    {
                                        hp.ID_MONHOC
                                    }).Contains(new { ID_MONHOC = (System.Int32?)mh.ID_MONHOC })
                               select new
                               {
                                   mh.ID_MONHOC,
                                   mh.TEN_MONHOC
                               };
                dt = TableUtil.LinqToDataTable(dsmonhoc);
                DataRow r = dt.NewRow();
                r["ID_MONHOC"] = 0;
                r["TEN_MONHOC"] = "------------------------------------------------------Tất cả-------------------------------------------------------";
                dt.Rows.Add(r);
                return dt.AsEnumerable().OrderBy(t => t.Field<int>("ID_MONHOC")).CopyToDataTable();
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetLopHP()
        {
            try
            {
                DataTable dt = null;
                var hpdky = from hp in db.tbl_LOP_HOCPHANs
                            join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals
                                new { ID_MONHOC = mh.ID_MONHOC }
                            join thu in db.tbl_LOP_HOCPHAN_CTs on hp.ID_LOPHOCPHAN equals thu.ID_LOPHOCPHAN
                            where (hp.IS_DELETE != 1 || hp.IS_DELETE == null) &&
                                (from hkht in db.tbl_NAMHOC_HKY_HTAIs
                                 where hkht.IS_HIENTAI == 1
                                 select new
                                 {
                                     hkht.ID_NAMHOC_HKY_HTAI
                                 }).Contains(new { ID_NAMHOC_HKY_HTAI = (System.Int32)hp.ID_NAMHOC_HKY_HTAI }) && (thu.IS_DELETE != 1 || thu.IS_DELETE == null)
                            select new
                            {
                                hp.ID_LOPHOCPHAN,
                                hp.ID_KHOAHOC_NGANH_CTIET,
                                hp.ID_NAMHOC_HKY_HTAI,
                                hp.ID_HEDAOTAO,
                                ID_MONHOC = (int?)hp.ID_MONHOC,
                                ISBATBUOC = ((from n in db.tbl_MONHOCs
                                              where n.ID_MONHOC == hp.ID_MONHOC
                                              select new
                                              {
                                                  n.ISBATBUOC
                                              }).First().ISBATBUOC
                                             ),
                                hp.ID_LOPHOC,hp.ID_GIANGVIEN,
                                hp.MA_LOP_HOCPHAN,
                                hp.TEN_LOP_HOCPHAN,
                                mh.MA_MONHOC,
                                mh.TEN_MONHOC,
                                mh.SO_TC,
                                hp.SOLUONG,
                                hp.TUAN_BD,
                                hp.TUAN_KT,
                                SOSVDKY = db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                                TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs
                                                 where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null)
                                                 select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString(),
                                thu.THU,
                                thu.TIET_BD,
                                thu.TIET_KT,
                                thu.SO_TIET,
                                TEN_PHONG =
                                    (from p in db.tbl_PHONGHOCs
                                     where p.ID_PHONG == thu.ID_PHONG && (p.IS_DELETE != 1 || p.IS_DELETE == null)
                                     select p).FirstOrDefault().TEN_PHONG.ToString()
                            };
                dt = TableUtil.LinqToDataTable(hpdky);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetLopHPWherMonHoc_Thu(int idmonhoc, int zthu)
        {
            DataTable dt = null;
            var hpdky = from hp in db.tbl_LOP_HOCPHANs
                        join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                        join thu in db.tbl_LOP_HOCPHAN_CTs on hp.ID_LOPHOCPHAN equals thu.ID_LOPHOCPHAN
                        where
                            (hp.IS_DELETE != 1 ||
                             hp.IS_DELETE == null) &&
                            (from hkht in db.tbl_NAMHOC_HKY_HTAIs
                             where hkht.IS_HIENTAI == 1
                             select new
                             {
                                 hkht.ID_NAMHOC_HKY_HTAI
                             }).Contains(new { ID_NAMHOC_HKY_HTAI = (System.Int32)hp.ID_NAMHOC_HKY_HTAI }) &&
                                (thu.IS_DELETE != 1 || thu.IS_DELETE == null)
                                && hp.ID_MONHOC == idmonhoc && thu.THU == zthu
                        #region Select
                        select new
                        {
                            hp.ID_LOPHOCPHAN,
                            hp.ID_KHOAHOC_NGANH_CTIET,
                            hp.ID_NAMHOC_HKY_HTAI,
                            hp.ID_HEDAOTAO,
                            ID_MONHOC = (int?)hp.ID_MONHOC,
                            hp.ID_LOPHOC,
                            hp.ID_GIANGVIEN,
                            hp.MA_LOP_HOCPHAN,
                            hp.TEN_LOP_HOCPHAN,
                            mh.MA_MONHOC,
                            mh.TEN_MONHOC,
                            mh.SO_TC,
                            hp.SOLUONG,
                            hp.TUAN_BD,
                            hp.TUAN_KT,
                            SOSVDKY = db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                            TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null) select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString(),
                            thu.THU,
                            thu.TIET_BD,
                            thu.TIET_KT,
                            thu.SO_TIET,
                            TEN_PHONG = (from p in db.tbl_PHONGHOCs where p.ID_PHONG == thu.ID_PHONG && (p.IS_DELETE != 1 || p.IS_DELETE == null) select p).FirstOrDefault().TEN_PHONG.ToString()
                        };
                        #endregion
            dt = TableUtil.LinqToDataTable(hpdky);
            return dt;
        }

        public bool Insert_HocPhanDK(DataTable iDataSoure, string pUser)
        {
            try
            {
                int count = 0;
                foreach (DataRow r in iDataSoure.Rows)
                {
                    if (r["ID_DANGKY"].ToString().Equals("0"))
                    {
                        tbl_HP_DANGKY dkhp = new tbl_HP_DANGKY();
                        dkhp.ID_THAMSO = 1;
                        dkhp.ID_SINHVIEN = Convert.ToInt32(r["ID_SINHVIEN"].ToString());
                        dkhp.ID_LOPHOCPHAN = Convert.ToInt32(r["ID_LOPHOCPHAN"].ToString());
                        dkhp.NGAY_DANGKY = Convert.ToDateTime(r["NGAY_DANGKY"]);
                        dkhp.GIO_DANGKY = DateTime.Now.ToString("HH:mm:ss");
                        dkhp.DON_GIA = Convert.ToDouble(r["DON_GIA"].ToString());
                        dkhp.THANH_TIEN = Convert.ToDouble(r["THANH_TIEN"].ToString());
                        dkhp.CREATE_USER = pUser;
                        dkhp.SO_TC = Convert.ToInt32(r["SO_TC"].ToString());
                        dkhp.CREATE_TIME = DateTime.Now;
                        dkhp.IS_DELETE = 0;

                        db.tbl_HP_DANGKies.InsertOnSubmit(dkhp);
                        db.SubmitChanges();
                        count++;
                    }

                }

                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool Insert_DangKyHuy(DataTable iDataSoure, string pUser)
        {
            try
            {
                int count = 0;
                if (iDataSoure.Rows.Count > 0)
                {
                    foreach (DataRow r in iDataSoure.Rows)
                    {
                        if (Convert.ToInt32(r["ID_DANGKY"].ToString()) > 0)
                        {
                            try
                            {
                                tbl_HP_DANGKY_HUY huydk = new tbl_HP_DANGKY_HUY();
                                huydk.ID_LOPHOCPHAN = Convert.ToInt32(r["ID_LOPHOCPHAN"].ToString());
                                huydk.ID_SINHVIEN = Convert.ToInt32(r["ID_SINHVIEN"].ToString());
                                huydk.ID_THAMSO = Convert.ToInt32(r["ID_THAMSO"].ToString());
                                huydk.NGAY_HUY = DateTime.Now;
                                huydk.GIO_HUY = DateTime.Now.ToString("HH:mm:ss");
                                huydk.DON_GIA = Convert.ToInt32(r["DON_GIA"].ToString());
                                huydk.THANH_TIEN = Convert.ToInt32(r["THANH_TIEN"].ToString());
                                huydk.IS_DELETE = 0;
                                huydk.CREATE_USER = pUser;
                                huydk.CREATE_TIME = DateTime.Now;

                                db.tbl_HP_DANGKY_HUYs.InsertOnSubmit(huydk);
                                db.SubmitChanges();
                                count++;
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                            bool res = Delete_HocPhanDK(Convert.ToInt32(r["ID_DANGKY"].ToString()), pUser);
                        }
                    }
                }
                if (count > 0)
                    return true;
                return false;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool Delete_HocPhanDK(int Id_DangKy, string pUser)
        {
            try
            {
                bool res;

                tbl_HP_DANGKY dkhp = db.tbl_HP_DANGKies.Single(t => t.ID_DANGKY == Id_DangKy);
                dkhp.IS_DELETE = 1;
                dkhp.UPDATE_TIME = DateTime.Now;
                dkhp.CREATE_USER = pUser;
                db.SubmitChanges();
                if (dkhp.ID_DANGKY != 0)
                {
                    res = true;
                }
                else
                {
                    res = false;
                }
                return res;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetHocKyNamHoc()
        {
            try
            {
                DataTable dt = null;
                var hockynamhoc = from hkht in db.tbl_NAMHOC_HKY_HTAIs
                                  join nhht in db.tbl_NAMHOC_HIENTAIs on
                                      new { ID_NAMHOC_HIENTAI = Convert.ToInt32(hkht.ID_NAMHOC_HIENTAI) } equals
                                      new { ID_NAMHOC_HIENTAI = nhht.ID_NAMHOC_HIENTAI }
                                  where
                                      nhht.IS_HIENTAI == 1
                                  select new
                                  {
                                      hkht.ID_NAMHOC_HKY_HTAI,
                                      TEN_HOKY_NH =
                                          ("Học kỳ " + "" + Convert.ToString(hkht.HOCKY) + " " + "Năm " +
                                           Convert.ToString(nhht.NAMHOC_TU) + " - " + Convert.ToString(nhht.NAMHOC_DEN))
                                  };
                dt = TableUtil.LinqToDataTable(hockynamhoc);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetNgay_SoTuan()
        {
            try
            {
                DataTable dt = null;
                var NgayTuan = from nhht in db.tbl_NAMHOC_HIENTAIs
                               where
                                   (nhht.IS_DELETE != 1 ||
                                    nhht.IS_DELETE == null) &&
                                   nhht.IS_HIENTAI == 1
                               select new
                               {
                                   nhht.NGAY_BATDAU,
                                   nhht.SO_TUAN
                               };
                dt = TableUtil.LinqToDataTable(NgayTuan);

                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public int GetHocKy(int id_hocky_htai)
        {
            try
            {
                var hocky = (from hk in db.tbl_NAMHOC_HKY_HTAIs
                             where hk.ID_NAMHOC_HKY_HTAI == id_hocky_htai
                             select new { hk.HOCKY }).First().HOCKY.ToString();
                return Convert.ToInt32(hocky.ToString());
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetTKB(int id_hocky_hientai, int id_sinhvien, int tuan)
        {
            try
            {
                DataTable dt = null;
                var tkb = from dkhp in db.tbl_HP_DANGKies
                          join hp in db.tbl_LOP_HOCPHANs on new { ID_LOPHOCPHAN = Convert.ToInt32(dkhp.ID_LOPHOCPHAN) } equals new { ID_LOPHOCPHAN = hp.ID_LOPHOCPHAN }
                          join hpct in db.tbl_LOP_HOCPHAN_CTs on new { ID_LOPHOCPHAN = hp.ID_LOPHOCPHAN } equals new { ID_LOPHOCPHAN = Convert.ToInt32(hpct.ID_LOPHOCPHAN) }
                          join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                          join p in db.tbl_PHONGHOCs on new { ID_PHONG = Convert.ToInt32(hpct.ID_PHONG) } equals new { ID_PHONG = p.ID_PHONG }
                          join gv in db.tbl_GIANGVIENs on new { ID_GIANGVIEN = Convert.ToInt32(hp.ID_GIANGVIEN) } equals new { ID_GIANGVIEN = gv.ID_GIANGVIEN }
                          where
                            (dkhp.IS_DELETE != 1 ||
                            dkhp.IS_DELETE == null) &&
                            (hp.IS_DELETE != 1 ||
                            hp.IS_DELETE == null) &&
                            (hpct.IS_DELETE != 1 ||
                            hpct.IS_DELETE == null) &&
                            (mh.IS_DELETE != 1 ||
                            mh.IS_DELETE == null) &&
                            (p.IS_DELETE != 1 ||
                            p.IS_DELETE == null) &&
                            (gv.IS_DELETE != 1 ||
                            p.IS_DELETE == null) &&
                            dkhp.ID_SINHVIEN == id_sinhvien &&
                            hp.ID_NAMHOC_HKY_HTAI == id_hocky_hientai &&
                            tuan >= hp.TUAN_BD && tuan <= hp.TUAN_KT
                          select new
                          {
                              hp.TEN_LOP_HOCPHAN,hp.ID_LOPHOCPHAN,
                              SOTIET = hpct.SO_TIET,
                              hp.MA_LOP_HOCPHAN,
                              hp.TUAN_BD,
                              hp.TUAN_KT,
                              hpct.THU,
                              hpct.TIET_BD,
                              hpct.TIET_KT,
                              p.TEN_PHONG,
                              gv.TEN_GIANGVIEN,
                              mh.TEN_MONHOC
                          };
                dt = TableUtil.LinqToDataTable(tkb);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
