using System.Security.Principal;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace Dolgozat0528
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        internal static Connect Conn = new Connect();
        public void lekerdez()
        {
            Conn.Connection.Open();

            string sql = "SELECT * FROM `filmek` WHERE 1";

            MySqlCommand cmd = new MySqlCommand(sql, Conn.Connection);

            MySqlDataReader dr = cmd.ExecuteReader();


            dr.Read();

            do
            {
                var felhasznalo = new
                {
                    filmazon = dr.GetInt32(0),
                    cim = dr.GetString(1),
                    ev = dr.GetInt32(2),
                    szines = dr.GetString(3),
                    mufaj = dr.GetString(4),
                    hossz = dr.GetInt32(5),
                };

                lbAdatok.Items.Add(felhasznalo.filmazon + ";" + felhasznalo.cim + ";" + felhasznalo.ev + ";" + felhasznalo.szines + ";" + felhasznalo.mufaj + ";" + felhasznalo.hossz);

            }
            while (dr.Read());

            dr.Close();
            Conn.Connection.Close();
        }

        private void lekerdez_Click(object sender, RoutedEventArgs e)
        {
            lekerdez();
        }

        private void lbAdatok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbAdatok.SelectedItem == null)
            {
                return;

            }
            else
            {
            object sor = lbAdatok.SelectedItem;
            string sorString = sor.ToString();
            string[] felvag = sorString.Split(';');
            lbFilmAzon.Content = felvag[0];
            tb1.Text = felvag[1];
            tb2.Text = felvag[2];
            tb3.Text = felvag[3];
            tb4.Text = felvag[4];
            tb5.Text = felvag[5];
            }
        }
        private void modosit_Click(object sender, RoutedEventArgs e)
        {
            Conn.Connection.Open();
            string sql = "UPDATE `filmek` SET `cim`=@cim, `ev`=@ev, `szines`=@szines, `mufaj`=@mufaj, `hossz`=@hossz WHERE filmazon=@filmazon";

            MySqlCommand cmd = new MySqlCommand(sql, Conn.Connection);
            cmd.Parameters.AddWithValue("@cim", tb1.Text);
            cmd.Parameters.AddWithValue("@ev", tb2.Text);
            cmd.Parameters.AddWithValue("@szines", tb3.Text);
            cmd.Parameters.AddWithValue("@mufaj", tb4.Text);
            cmd.Parameters.AddWithValue("@hossz", tb5.Text);
            cmd.Parameters.AddWithValue("@filmazon", lbFilmAzon.Content.ToString());

            cmd.ExecuteNonQuery();
            Conn.Connection.Close();

            MessageBox.Show("Sikeres módosítás!");
            lbAdatok.Items.Clear();
            lekerdez();
        }

    }
}