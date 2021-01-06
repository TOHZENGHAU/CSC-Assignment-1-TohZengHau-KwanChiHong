using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TestingSub.Models
{
    public class CustomerModel
    {


        public string customer_id { get; set; }
        public string email { get; set; }
        public string subscription_status { get; set; }
        public string charge_status { get; set; }
        public string basic { get; set; }
        public string premium { get; set; }

        public CustomerModel() { }
        public CustomerModel(string customer_id, string email, string subscription_status, string basic, string premium)
        {
            this.customer_id = customer_id;
            this.email = email;
            this.subscription_status = subscription_status;
            this.basic = basic;
            this.premium = premium;
        }
        public int SaveDetails()
        {
            SqlConnection con = new SqlConnection(GetConStr.ConString());
            string query = "INSERT INTO Customer(customer_id, email, subscription_status, basic, premium) values ('" + customer_id + "','" + email + "','" + subscription_status + "','" + basic + "','" + premium + "')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }
        public int RecordChargeStatus(string cs, string customer_id)
        {
            SqlConnection con = new SqlConnection(GetConStr.ConString());
            string query = "UPDATE Customer set charge_status = '" + cs + "' WHERE customer_id = '" + customer_id + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }
        public string getDetailsWithId(string customer_id)
        {
            SqlConnection con = new SqlConnection(GetConStr.ConString());
            string query = "SELECT * FROM Customer WHERE customer_id = '" + customer_id + "'";
            string result = "";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                result = dr["basic"].ToString();
                
            }
            con.Close();
            return result;
        }
        public int PlanChange(string cond, string customer_id)
        {
            SqlConnection con = new SqlConnection(GetConStr.ConString());
            string query = "";
            if (cond == "basic")
            {
                query = "UPDATE Customer set subscription_status = 'active', basic = 'True', premium = 'False' WHERE customer_id = '" + customer_id + "'";
            }
            else
            {
                query = "UPDATE Customer set subscription_status = 'active', basic = 'False', premium = 'True' WHERE customer_id = '" + customer_id + "'";
            }

            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }
        public int PlanRemove(string customer_id)
        {
            SqlConnection con = new SqlConnection(GetConStr.ConString());
            string query = "UPDATE Customer set subscription_status = 'inactive' WHERE customer_id = '" + customer_id + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }


        //public List<CustomerModel> getDetails()
        //{
        //    List<CustomerModel> list = new List<CustomerModel>();

        //    SqlConnection con = new SqlConnection(GetConStr.ConString());
        //    string query = "SELECT * FROM Customer";
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    con.Open();
        //    cmd.ExecuteNonQuery();
        //    SqlDataReader dr = cmd.ExecuteReader();
        //    while (dr.Read())
        //    {
        //        string customer_id = dr.GetString(1);
        //        string email = dr.GetString(2);
        //        string subscription_status = dr.GetString(3);

        //        CustomerModel cm = new CustomerModel(customer_id, email,subscription_status);
        //        list.Add(cm);

        //    }
        //    con.Close();
        //    return list;
        //}
    }
}
