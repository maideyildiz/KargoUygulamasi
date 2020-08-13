using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OdbcConnection baglanti()
        {
            string giris = "Dsn=PostgreSQL35W;uid=postgres; Password=081518maidemin";
            OdbcConnection connection = new OdbcConnection(giris);

            return connection;
        }
        void butonKontrol(bool x)
        {
            btnGonderIleri.Enabled = x;
            btnAliciIlerle.Enabled = x;
            btnKayit.Enabled = x;
            btnGuncelle.Enabled = x;
            btnSil.Enabled = x;
            btnKargoAra.Enabled = x;
            btnTumu.Enabled = x;
            btnGuncelListe.Enabled = x;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            butonKontrol(false);
        }
        private void btnGirisYap_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text;
            int sifre = Convert.ToInt32(txtSifre.Text);
            OdbcConnection connection = baglanti();
            connection.Open();
            OdbcCommand command = new OdbcCommand("Select * from \"KullaniciGiris\" where \"kullaniciAdi\"= '" + kullaniciAdi + "' and \"sifre\"= '" + sifre + "'", connection);
            OdbcDataReader reader = command.ExecuteReader();
            //reader = command.ExecuteReader();
            if (reader.Read())
            {
                butonKontrol(true);
                btnAliciIlerle.Enabled = false;
                btnKayit.Enabled = false;
                btnGuncelle.Enabled = false;
                btnGirisYap.Enabled = false;
            }
            else
            {
                butonKontrol(false);
            }
            connection.Close();
            tabControl1.SelectedTab = tabPage1Kayit;
            tabControl2.SelectedTab = tab2PageGonderici;
        }
        private void btnGonderIleri_Click(object sender, EventArgs e)
        {
            string gondericiIl = txtGonderIl.Text;
            string gondericiIlce = txtGonderIlce.Text;
            string gonderAdres = txtGonderAdres.Text;
            string gonderIsim = txtGonderIsim.Text;
            string gonderSoy = txtGonderSoy.Text;
            int gonderTel = Convert.ToInt32(txtGonderTel.Text);
            string gonderMail = txtGonderMail.Text;

            OdbcConnection connection = baglanti();
            connection.Open();
            OdbcCommand command = new OdbcCommand("INSERT INTO \"IlAdi\" (\"ilAdi\") VALUES ('" + gondericiIl + "') ON CONFLICT (\"ilAdi\") DO NOTHING", connection);
            OdbcDataReader reader = command.ExecuteReader();
            command = new OdbcCommand("INSERT INTO \"Ilce\" (\"ilceAdi\",\"ilNo\") VALUES ('" + gondericiIlce + "',(SELECT (\"ilID\") FROM \"IlAdi\" where (\"ilAdi\")= '" + gondericiIl + "')) ON CONFLICT (\"ilceAdi\") DO NOTHING", connection);
            reader = command.ExecuteReader();
            command = new OdbcCommand("INSERT INTO \"Adres\" (\"acikAdres\",\"ilceNo\") VALUES ('" + gonderAdres + "',(SELECT (\"ilceID\") FROM \"Ilce\" where (\"ilceAdi\")= '" + gondericiIlce + "')) ON CONFLICT (\"acikAdres\") DO NOTHING", connection);
            reader = command.ExecuteReader();
            command = new OdbcCommand("INSERT INTO \"IletisimBilgileri\" (\"eposta\", \"tel\", \"adresNo\") VALUES ('" + gonderMail + "','" + gonderTel + "',(SELECT (\"adresID\") FROM \"Adres\" where (\"acikAdres\")= '" + gonderAdres + "')) ON CONFLICT (\"tel\") DO NOTHING", connection);
            reader = command.ExecuteReader();
            command = new OdbcCommand("INSERT INTO \"Kisi\" (\"ad\", \"soyad\",\"gonderici\",\"iletisimNo\") VALUES ('" + gonderIsim + "','" + gonderSoy + "',TRUE,(SELECT (\"iletisimID\") FROM \"IletisimBilgileri\" where (\"tel\")= '" + gonderTel + "')) ON CONFLICT (\"ad\",\"soyad\",\"iletisimNo\") DO NOTHING", connection);
            reader = command.ExecuteReader();
            command = new OdbcCommand("INSERT INTO \"GonderenKisi\" (\"kargoVerisTarihi\",\"kisiID\") VALUES ('" + DateTime.Today + "',(SELECT (\"kisiID\") FROM \"Kisi\" where (\"ad\")= '" + gonderIsim + "' and \"soyad\"= '" + gonderSoy + "'and \"gonderici\"=TRUE )) ", connection);
            reader = command.ExecuteReader();
            command = new OdbcCommand("INSERT INTO \"Kargo\" (\"gonderenNo\") VALUES ((SELECT (\"kisiID\") FROM \"Kisi\" where (\"ad\")= '" + gonderIsim + "' and \"soyad\"= '" + gonderSoy + "'and \"gonderici\"=TRUE )) ", connection);
            reader = command.ExecuteReader();

            connection.Close();
            reader.Close();
            tabControl2.SelectedTab = tab2PageAlici;
            btnGonderIleri.Enabled = false;
            btnAliciIlerle.Enabled = true;
        }

        private void btnAliciIlerle_Click(object sender, EventArgs e)
        {
            string aliciIl = txtAliciIl.Text;
            string aliciIlce = txtAliciIlce.Text;
            string aliciAdres = txtAliciAcikAdres.Text;
            string aliciAd = txtAliciAd.Text;
            string aliciSoy = txtAliciSoy.Text;
            int aliciTel = Convert.ToInt32(txtAliciTel.Text);
            string aliciMail = txtAliciMail.Text;

            OdbcConnection connection = baglanti();
            connection.Open();
            OdbcCommand command = new OdbcCommand("INSERT INTO \"IlAdi\" (\"ilAdi\") VALUES ('" + aliciIl + "') ON CONFLICT (\"ilAdi\") DO NOTHING", connection);
            OdbcDataReader reader = command.ExecuteReader();
            command = new OdbcCommand("INSERT INTO \"Ilce\" (\"ilceAdi\",\"ilNo\") VALUES ('" + aliciIlce + "',(SELECT (\"ilID\") FROM \"IlAdi\" where (\"ilAdi\")= '" + aliciIl + "')) ON CONFLICT (\"ilceAdi\") DO NOTHING", connection);
            reader = command.ExecuteReader();
            command = new OdbcCommand("INSERT INTO \"Adres\" (\"acikAdres\",\"ilceNo\") VALUES ('" + aliciAdres + "',(SELECT (\"ilceID\") FROM \"Ilce\" where (\"ilceAdi\")= '" + aliciIlce + "')) ON CONFLICT (\"acikAdres\") DO NOTHING", connection);
            reader = command.ExecuteReader();
            command = new OdbcCommand("INSERT INTO \"IletisimBilgileri\" (\"eposta\", \"tel\", \"adresNo\") VALUES ('" + aliciMail + "','" + aliciTel + "',(SELECT (\"adresID\") FROM \"Adres\" where (\"acikAdres\")= '" + aliciAdres + "')) ON CONFLICT (\"tel\") DO NOTHING", connection);
            reader = command.ExecuteReader();
            command = new OdbcCommand("INSERT INTO \"Kisi\" (\"ad\", \"soyad\",\"alici\",\"iletisimNo\") VALUES ('" + aliciAd + "','" + aliciSoy + "',TRUE,(SELECT (\"iletisimID\") FROM \"IletisimBilgileri\" where (\"tel\")= '" + aliciTel + "')) ON CONFLICT (\"ad\",\"soyad\",\"iletisimNo\") DO NOTHING", connection);
            reader = command.ExecuteReader();
            command = new OdbcCommand("INSERT INTO \"AliciKisi\" (\"kisiID\") VALUES ((SELECT (\"kisiID\") FROM \"Kisi\" where (\"ad\")= '" + aliciAd + "' and \"soyad\"= '" + aliciSoy + "'and \"alici\"=TRUE )) ", connection);
            reader = command.ExecuteReader();
            command = new OdbcCommand("UPDATE \"Kargo\" SET (\"alanKisiNo\") = ((SELECT (\"kisiID\") FROM \"Kisi\" where (\"ad\")= '" + aliciAd + "' and \"soyad\"= '" + aliciSoy + "'and \"alici\"=TRUE)) WHERE (\"gonderenNo\")= (SELECT (\"kisiID\") FROM \"Kisi\" where (\"ad\")= '" + txtGonderIsim.Text + "' and \"soyad\"= '" + txtGonderSoy.Text + "'and \"gonderici\"=TRUE )", connection);
            reader = command.ExecuteReader();


            connection.Close();
            reader.Close();
            tabControl2.SelectedTab = tab2PageKargo;
            btnAliciIlerle.Enabled = false;
            btnKayit.Enabled = true;
        }

        private void btnKayit_Click(object sender, EventArgs e)
        {
            int kayitEn = Convert.ToInt32(txtKayitEn.Text);
            int kayitBoy = Convert.ToInt32(txtKayitBoy.Text);
            int kayitKg = Convert.ToInt32(txtKayitKg.Text);
            int kayitMesafe = cmbKayitMesafe.SelectedIndex + 1;
            int kayitTur = cmbKayitTur.SelectedIndex + 1;
            int kayitFatura;
            if (rbKayitTicari.Checked) kayitFatura = 1;
            else kayitFatura = 2;
            int odemeSekli;
            if (rbKayitGonderOde.Checked) odemeSekli = 1;
            else odemeSekli = 2;
            int kayitDurum = cmbKayitDurum.SelectedIndex + 1;
            var kayitOlusum = dateKayitOlusum.Value.AddDays(1);
            var kayitTeslim = dateKayitTeslim.Value.AddDays(3);


            OdbcConnection connection = baglanti();
            connection.Open();
            OdbcCommand command = new OdbcCommand("INSERT INTO \"KargoOlcu\" (\"en\", \"boy\", \"kg\") VALUES (" + kayitEn + "," + kayitBoy + "," + kayitKg + ")", connection);
            OdbcDataReader reader = command.ExecuteReader();
            command = new OdbcCommand("UPDATE \"Kargo\" SET \"kargoCikis\"= '" + kayitOlusum + "',\"mesafeNo\"="+ kayitMesafe+ ", \"turNo\"=" + kayitTur+ ", \"odemeNo\"="+ odemeSekli +",\"kargoTeslim\"= '" + kayitTeslim +"',\"faturaSekli\"= " + kayitFatura + ",\"kargoNerede\"= " + kayitDurum + ",\"olcuNo\"= (SELECT (\"olculerID\") FROM \"KargoOlcu\" WHERE (\"en\")=" + kayitEn + " and (\"boy\")=" + kayitBoy + " and (\"kg\")=" + kayitKg + ") WHERE (\"gonderenNo\")= (SELECT (\"kisiID\") FROM \"Kisi\" where (\"ad\")= '" + txtGonderIsim.Text + "' and \"soyad\"= '" + txtGonderSoy.Text + "'and \"gonderici\"=TRUE )", connection);
            reader = command.ExecuteReader();

            connection.Close();
            reader.Close();
            btnGonderIleri.Enabled = true;
            btnAliciIlerle.Enabled = true;
            btnKayit.Enabled = true;
            MessageBox.Show("Kargo Kaydı Alınmıştır");
        }
        private void btnKargoAra_Click(object sender, EventArgs e)
        {
            int kargoNo = Convert.ToInt32(txtAraKargo.Text);
            OdbcConnection connection = baglanti();
            connection.Open();
            OdbcDataAdapter command = new OdbcDataAdapter("SELECT \"Kargo\".\"barkodNo\",\"KargoOlcu\".\"en\", \"KargoOlcu\".\"boy\",\"KargoOlcu\".\"kg\",\"Mesafe\".\"mesafeTuru\",\"KargoTuru\".\"turAdi\",\"FaturaTur\".\"faturaTipi\",\"OdemeTuru\".\"odemeTip\",\"KargoDurum\".\"kargoYeri\",\"Kargo\".\"kargoCikis\", \"Kargo\".\"kargoTeslim\" FROM \"Kargo\" INNER JOIN \"KargoOlcu\" ON \"Kargo\".\"olcuNo\" = \"KargoOlcu\".\"olculerID\" INNER JOIN \"KargoDurum\"  ON \"Kargo\".\"kargoNerede\" = \"KargoDurum\".\"durumID\" INNER JOIN \"KargoTuru\"  ON \"Kargo\".\"turNo\" = \"KargoTuru\".\"turID\" INNER JOIN \"Mesafe\"  ON \"Kargo\".\"mesafeNo\" = \"Mesafe\".\"mesafeID\" INNER JOIN \"FaturaTur\"  ON \"Kargo\".\"faturaSekli\" = \"FaturaTur\".\"faturaID\" INNER JOIN \"OdemeTuru\"  ON \"Kargo\".\"odemeNo\" = \"OdemeTuru\".\"odemeID\" WHERE \"barkodNo\"=" + kargoNo, connection);
            DataSet data = new DataSet();
            command.Fill(data);
            dataKargo.DataSource = data.Tables[0];

            connection.Close();
            dataKargo.Columns[0].HeaderText = "Barkod No";
            dataKargo.Columns[1].HeaderText = "En";
            dataKargo.Columns[2].HeaderText = "Boy";
            dataKargo.Columns[3].HeaderText = "Kg";
            dataKargo.Columns[4].HeaderText = "Mesafe";
            dataKargo.Columns[5].HeaderText = "Tür";
            dataKargo.Columns[6].HeaderText = "Fatura Tipi";
            dataKargo.Columns[7].HeaderText = "Ödeme Şekli";
            dataKargo.Columns[8].HeaderText = "Kargo Nerede";
            dataKargo.Columns[9].HeaderText = "Kargo Çıkış Tarihi";
            dataKargo.Columns[10].HeaderText = "Kargo Teslim Tarihi";

        }
        private void btnTumu_Click(object sender, EventArgs e)
        {
            OdbcConnection connection = baglanti();
            connection.Open();
            OdbcDataAdapter command = new OdbcDataAdapter("SELECT \"Kargo\".\"barkodNo\",\"KargoOlcu\".\"en\", \"KargoOlcu\".\"boy\",\"KargoOlcu\".\"kg\",\"Mesafe\".\"mesafeTuru\",\"KargoTuru\".\"turAdi\",\"FaturaTur\".\"faturaTipi\",\"OdemeTuru\".\"odemeTip\",\"KargoDurum\".\"kargoYeri\",\"Kargo\".\"kargoCikis\", \"Kargo\".\"kargoTeslim\" FROM \"Kargo\" INNER JOIN \"KargoOlcu\" ON \"Kargo\".\"olcuNo\" = \"KargoOlcu\".\"olculerID\" INNER JOIN \"KargoDurum\"  ON \"Kargo\".\"kargoNerede\" = \"KargoDurum\".\"durumID\" INNER JOIN \"KargoTuru\"  ON \"Kargo\".\"turNo\" = \"KargoTuru\".\"turID\" INNER JOIN \"Mesafe\"  ON \"Kargo\".\"mesafeNo\" = \"Mesafe\".\"mesafeID\" INNER JOIN \"FaturaTur\"  ON \"Kargo\".\"faturaSekli\" = \"FaturaTur\".\"faturaID\" INNER JOIN \"OdemeTuru\"  ON \"Kargo\".\"odemeNo\" = \"OdemeTuru\".\"odemeID\"", connection);
            DataSet data = new DataSet();
            command.Fill(data);
            dataTumu.DataSource = data.Tables[0];

            connection.Close();
            dataTumu.Columns[0].HeaderText = "Barkod No";
            dataTumu.Columns[1].HeaderText = "En";
            dataTumu.Columns[2].HeaderText = "Boy";
            dataTumu.Columns[3].HeaderText = "Kg";
            dataTumu.Columns[4].HeaderText = "Mesafe";
            dataTumu.Columns[5].HeaderText = "Tür";
            dataTumu.Columns[6].HeaderText = "Fatura Tipi";
            dataTumu.Columns[7].HeaderText = "Ödeme Şekli";
            dataTumu.Columns[8].HeaderText = "Kargo Nerede";
            dataTumu.Columns[9].HeaderText = "Kargo Çıkış Tarihi";
            dataTumu.Columns[10].HeaderText = "Kargo Teslim Tarihi";
        }
        private void btnGuncelListe_Click(object sender, EventArgs e)
        {
            OdbcConnection connection = baglanti();
            connection.Open();
            OdbcDataAdapter command = new OdbcDataAdapter("SELECT \"Kargo\".\"barkodNo\",\"KargoOlcu\".\"en\", \"KargoOlcu\".\"boy\",\"KargoOlcu\".\"kg\",\"Mesafe\".\"mesafeTuru\",\"KargoTuru\".\"turAdi\",\"FaturaTur\".\"faturaTipi\",\"OdemeTuru\".\"odemeTip\",\"KargoDurum\".\"kargoYeri\",\"Kargo\".\"kargoCikis\", \"Kargo\".\"kargoTeslim\" FROM \"Kargo\" INNER JOIN \"KargoOlcu\" ON \"Kargo\".\"olcuNo\" = \"KargoOlcu\".\"olculerID\" INNER JOIN \"KargoDurum\"  ON \"Kargo\".\"kargoNerede\" = \"KargoDurum\".\"durumID\" INNER JOIN \"KargoTuru\"  ON \"Kargo\".\"turNo\" = \"KargoTuru\".\"turID\" INNER JOIN \"Mesafe\"  ON \"Kargo\".\"mesafeNo\" = \"Mesafe\".\"mesafeID\" INNER JOIN \"FaturaTur\"  ON \"Kargo\".\"faturaSekli\" = \"FaturaTur\".\"faturaID\" INNER JOIN \"OdemeTuru\"  ON \"Kargo\".\"odemeNo\" = \"OdemeTuru\".\"odemeID\"", connection);
            DataSet data = new DataSet();
            command.Fill(data);
            dataGuncel.DataSource = data.Tables[0];

            connection.Close();

            dataGuncel.Columns[0].HeaderText = "Barkod No";
            dataGuncel.Columns[1].HeaderText = "En";
            dataGuncel.Columns[2].HeaderText = "Boy";
            dataGuncel.Columns[3].HeaderText = "Kg";
            dataGuncel.Columns[4].HeaderText = "Mesafe";
            dataGuncel.Columns[5].HeaderText = "Tür";
            dataGuncel.Columns[6].HeaderText = "Fatura Tipi";
            dataGuncel.Columns[7].HeaderText = "Ödeme Şekli";
            dataGuncel.Columns[8].HeaderText = "Kargo Nerede";
            dataGuncel.Columns[9].HeaderText = "Kargo Çıkış Tarihi";
            dataGuncel.Columns[10].HeaderText = "Kargo Teslim Tarihi";
        }
        private void dataGuncel_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtSorEn.Text = dataGuncel.CurrentRow.Cells[1].Value.ToString();
            txtSorBoy.Text = dataGuncel.CurrentRow.Cells[2].Value.ToString();
            txtSorKg.Text = dataGuncel.CurrentRow.Cells[3].Value.ToString();
            cmbSorMesafe.SelectedItem = dataGuncel.CurrentRow.Cells[4].Value.ToString();
            cmbSorTur.SelectedItem = dataGuncel.CurrentRow.Cells[5].Value.ToString();
            if (dataGuncel.CurrentRow.Cells[6].Value.Equals("Bireysel"))
            {
                rbSorBireysel.Checked = true;
            }
            else
                rbSorTicari.Checked = true;
            if (dataGuncel.CurrentRow.Cells[7].Value.Equals("Gönderici Ödemeli"))
            {
                rbSorGonder.Checked = true;
            }
            else
                rbSorAlici.Checked = true;
            cmbSorDurum.SelectedItem = dataGuncel.CurrentRow.Cells[8].Value.ToString();
            dateSorOlusum.Text = dataGuncel.CurrentRow.Cells[9].Value.ToString();
            dateSorTeslim.Text = dataGuncel.CurrentRow.Cells[10].Value.ToString();

            btnGuncelle.Enabled = true;
        }
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            int barkod= Convert.ToInt32(dataGuncel.CurrentRow.Cells[0].Value);
            int guncelEn = Convert.ToInt32(txtSorEn.Text);
            int guncelBoy = Convert.ToInt32(txtSorBoy.Text);
            int guncelKg = Convert.ToInt32(txtSorKg.Text);
            int guncelMesafe = cmbSorMesafe.SelectedIndex + 1;
            int guncelTur = cmbSorTur.SelectedIndex + 1;
            int guncelFatura;
            if (rbSorTicari.Checked) guncelFatura = 1;
            else guncelFatura = 2;
            int guncelOdeme;
            if (rbSorGonder.Checked) guncelOdeme = 1;
            else guncelOdeme = 2;
            int guncelDurum = cmbSorDurum.SelectedIndex + 1;
            var guncelOlusum = dateSorOlusum.Value;
            var guncelTeslim = dateSorTeslim.Value;


            OdbcConnection connection = baglanti();
            connection.Open();
            OdbcCommand command = new OdbcCommand("INSERT INTO \"KargoOlcu\" (\"en\", \"boy\", \"kg\") VALUES (" + guncelEn + "," + guncelBoy + "," + guncelKg + ")", connection);
            OdbcDataReader reader = command.ExecuteReader();
            command = new OdbcCommand("UPDATE \"Kargo\" SET \"kargoCikis\"= '" + guncelOlusum + "',\"mesafeNo\"=" + guncelMesafe + ", \"turNo\"=" + guncelTur + ", \"odemeNo\"=" + guncelOdeme + ",\"kargoTeslim\"= '" + guncelTeslim + "',\"faturaSekli\"= " + guncelFatura + ",\"kargoNerede\"= " + guncelDurum + ",\"olcuNo\"= (SELECT (\"olculerID\") FROM \"KargoOlcu\" WHERE (\"en\")=" + guncelEn + " and (\"boy\")=" + guncelBoy + " and (\"kg\")=" + guncelKg + ") WHERE (\"barkodNo\")= "+ barkod, connection);
            reader = command.ExecuteReader();

            connection.Close();
            reader.Close();
            MessageBox.Show("Kargo Güncellenmiştir");
        }
        private void btnSil_Click(object sender, EventArgs e)
        {
            int silinecek = Convert.ToInt32(txtSilKargo.Text);
            OdbcConnection connection = baglanti();
            connection.Open();
            OdbcCommand command = new OdbcCommand("DELETE FROM \"Kargo\" WHERE (\"barkodNo\") = '" + silinecek + "'", connection);
            OdbcDataReader reader = command.ExecuteReader();
            reader.Close();
            connection.Close();
            MessageBox.Show("Kargo Silinmiştir");
        }
    }
}









