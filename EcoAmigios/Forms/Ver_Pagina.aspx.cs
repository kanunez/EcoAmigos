using EcoAmigios.Class;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EcoAmigios.Forms
{
    public partial class Ver_Pagina : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=EcoAmigos;Integrated Security=True");
        SqlDataReader dr = null;
        SqlCommand cmd = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            conn.Open();
            cmd = new SqlCommand("SELECT * FROM Pagina WHERE Nombre_Pagina = '" + Session["Nombre_Pag"].ToString() + "'",conn);

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    ImageP.ImageUrl = "~/Imagenes/" + dr["Img_pag"].ToString();
                    TbTipoPag.Text = dr["Tipo_Pagina"].ToString();
                    TbNombreP.Text = dr["Nombre_Pagina"].ToString();
                    TbDescP.Text = dr["Descripcion_Pagina"].ToString();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mnesaje Alerta!", "alert('No existe esta pagina!!')", true);
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mnesaje Alerta!", "alert('" + ex + "')", true);
            }
            finally
            {
                conn.Close();
            }

            if (Session["Id_Usuario"] == null)
            {
                IBMensaje.Enabled = false;
                Label49.Enabled = false;
            }

        }

        protected void IBRegresar1_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["Id_Usuario"] == null)
            {
                Response.Redirect("Editar_Pagina.aspx");
            }
            else
            {
                Response.Redirect("Inicio_Usu.aspx");
            }
               
        }

        public static string Encriptar(string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }
        public static string DesEncriptar(string _cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }
        protected void IBMensaje_Click(object sender, ImageClickEventArgs e)
        {
            conn.Open();
            cmd = new SqlCommand("SELECT * FROM Pagina WHERE Nombre_Pagina = '" + Session["Nombre_Pag"].ToString() + "'", conn);

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string ID = Encriptar(dr["Identificacion"].ToString());
                    conn.Close();
                    conn.Open();
                    cmd = new SqlCommand("SELECT * FROM GruAmbiental WHERE Identificacion = '" + ID + "'", conn);
                    try
                    {
                        dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            Int64 telefono = Int64.Parse(DesEncriptar(dr["Telefono"].ToString()));
                            Response.Redirect("https://wa.me/57" + telefono);
                        }
                    }
                    catch(Exception ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mnesaje Alerta!", "alert('" + ex + "!!')", true);

                        Label49.Text = "" + ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mnesaje Alerta!", "alert('No existe la pagina!!')", true);
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mnesaje Alerta!", "alert('" + ex + "')", true);
            }
            finally
            {
                conn.Close();
            }
            
        }
    }
}