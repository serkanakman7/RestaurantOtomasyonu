using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace rest
{
    class cPersonelGorev
    {

        cGenel gnl = new cGenel();

        private int _PersonelGorevId;
        private string _Tanim;

        public int PersonelGorevId { get => _PersonelGorevId; set => _PersonelGorevId = value; }
        public string Tanim { get => _Tanim; set => _Tanim = value; }


        public void PersonelGorevGetir(ComboBox cb)
        {
            cb.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select * From PersonelGorevleri", con);

            SqlDataReader dr = null;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    cPersonelGorev c = new cPersonelGorev();
                    c._PersonelGorevId = Convert.ToInt32(dr["Id"]);
                    c._Tanim = dr["Gorev"].ToString();
                    cb.Items.Add(c);
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

        public string PersonelGorevTanim(int per)
        {
            string result = "";

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Gorev From PersonelGorevleri Where Id=@PerId ", con );

            cmd.Parameters.Add("@PerId", SqlDbType.Int).Value = per;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                result = cmd.ExecuteScalar().ToString();
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

        public override string ToString()
        {
            return _Tanim;
        }

    }
}
