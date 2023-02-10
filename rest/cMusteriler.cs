using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace rest
{
    class cMusteriler
    {

        cGenel gnl = new cGenel();

        #region Fields
        private int _MusteriId;
        private string _MusteriAd;
        private string _MusteriSoyad;
        private string _Telefon;
        private string _Adres;
        private string _Email;
        #endregion
        #region Properties
        public int MusteriId { get => _MusteriId; set => _MusteriId = value; }
        public string MusteriAd { get => _MusteriAd; set => _MusteriAd = value; }
        public string MusteriSoyad { get => _MusteriSoyad; set => _MusteriSoyad = value; }
        public string Telefon { get => _Telefon; set => _Telefon = value; }
        public string Adres { get => _Adres; set => _Adres = value; }
        public string Email { get => _Email; set => _Email = value; } 
        #endregion

        public bool MusteriVarMi(string tlf)
        {
            bool result = false;
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText="MusteriVarMi";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Telefon", SqlDbType.VarChar).Value = tlf;
            cmd.Parameters.Add("@Sonuc", SqlDbType.Int);
            cmd.Parameters["@Sonuc"].Direction = ParameterDirection.Output;

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            try
            {
                cmd.ExecuteNonQuery();
                result = Convert.ToBoolean(cmd.Parameters["@Sonuc"].Value);
            }
            catch(SqlException ex)
            {
                string hata = ex.Message;
                throw;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
            return result;

        }

        public int MusteriEkle(cMusteriler c)
        {
            int result = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Insert Into Musteriler(Ad,Soyad,Telefon,Adres,Email)Values(@Ad,@Soyad,@Telefon,@Adres,@Email) Select SCOPE_IDENTITY()", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@Ad", SqlDbType.VarChar).Value = c._MusteriAd;
                cmd.Parameters.Add("@Soyad", SqlDbType.VarChar).Value = c._MusteriSoyad;
                cmd.Parameters.Add("@Telefon", SqlDbType.VarChar).Value = c._Telefon;
                cmd.Parameters.Add("@Adres", SqlDbType.VarChar).Value = c._Adres;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = c._Email;

                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch(SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }

            return result;


        }

        public bool MusteriBilgileriniGuncelle(cMusteriler m)
        {
            bool result = false;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Update Musteriler Set Ad=@Ad , Soyad=@Soyad , Telefon = @Telefon ,Adres = @Adres, Email=@Email where Id=@MusteriNo", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@Ad", SqlDbType.VarChar).Value = m._MusteriAd;
                cmd.Parameters.Add("@Soyad", SqlDbType.VarChar).Value = m._MusteriSoyad;
                cmd.Parameters.Add("@Telefon", SqlDbType.VarChar).Value = m._Telefon;
                cmd.Parameters.Add("@Adres", SqlDbType.VarChar).Value = m._Adres;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = m._Email;
                cmd.Parameters.Add("@MusteriNo", SqlDbType.Int).Value = m._MusteriId;

                result = Convert.ToBoolean(cmd.ExecuteNonQuery());
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }

            return result;

        }

        public void MusteriGetir(ListView lv)
        {
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select * from Musteriler", con);

            SqlDataReader dr = null;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                dr = cmd.ExecuteReader();

                int sayac = 0;

                while (dr.Read())
                {
                    lv.Items.Add(dr["Id"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Ad"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Soyad"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Telefon"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Adres"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Email"].ToString());

                    sayac++;
                }
            }
            catch(SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                dr.Close();
                con.Dispose();
                con.Close();
            }
        }

        public void MusterileriGetirId(int MusteriId,TextBox ad,TextBox soyad,TextBox tlf,TextBox adres,TextBox email)
        {
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select * From Musteriler where Id=@MusteriId", con);

            SqlDataReader dr = null;
            cmd.Parameters.Add("@MusteriId", SqlDbType.Int).Value = MusteriId;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ad.Text = dr["Ad"].ToString();
                    soyad.Text = dr["Soyad"].ToString();
                    tlf.Text = dr["Telefon"].ToString();
                    adres.Text = dr["Adres"].ToString();
                    email.Text = dr["Email"].ToString();
                }
            }
            catch(SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                dr.Close();
                con.Dispose();
                con.Close();
            }
        }

        public void MusteriGetirAd(ListView lv , string MusteriAd) 
        {
            lv.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select * From Musteriler Where Ad Like @MusteriAd + '%'", con);

            SqlDataReader dr = null;

            cmd.Parameters.Add("@MusteriAd", SqlDbType.VarChar).Value = MusteriAd;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                dr = cmd.ExecuteReader();
                int sayac = 0;

                while (dr.Read())
                {
                    lv.Items.Add(dr["Id"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Ad"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Soyad"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Telefon"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Adres"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Email"].ToString());

                    sayac++;
                }
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                dr.Close();
                con.Dispose();
                con.Close();
            }
        }

        public void MusteriGetirSoyad(ListView lv, string MusteriSoyad)
        {
            lv.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select * From Musteriler Where Soyad Like @MusteriSoyad + '%'", con);

            SqlDataReader dr = null;

            cmd.Parameters.Add("@MusteriSoyad", SqlDbType.VarChar).Value = MusteriSoyad;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                dr = cmd.ExecuteReader();
                int sayac = 0;

                while (dr.Read())
                {
                    lv.Items.Add(dr["Id"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Ad"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Soyad"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Telefon"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Adres"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Email"].ToString());

                    sayac++;
                }
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                dr.Close();
                con.Dispose();
                con.Close();
            }
        }

        public void MusteriGetirTlf(ListView lv, string tlf)
        {
            lv.Items.Clear();
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select * From Musteriler Where Telefon Like @tlf + '%'", con);

            SqlDataReader dr = null;

            cmd.Parameters.Add("@tlf", SqlDbType.VarChar).Value = tlf;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                dr = cmd.ExecuteReader();
                int sayac = 0;

                while (dr.Read())
                {
                    lv.Items.Add(dr["Id"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Ad"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Soyad"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Telefon"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Adres"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Email"].ToString());

                    sayac++;
                }
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                dr.Close();
                con.Dispose();
                con.Close();
            }
        }

    }
}
