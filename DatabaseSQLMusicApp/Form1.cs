using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseSQLMusicApp
{
    public partial class Form1 : Form
    {

        BindingSource albumBindingSource = new BindingSource();
        BindingSource tracksBindingSource = new BindingSource();

        List<Album> albums = new List<Album>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AlbumsDAO albumsDAO = new AlbumsDAO();

            albumBindingSource.DataSource = albumsDAO.getAllAlbums();

            dataGridView1.DataSource = albumBindingSource;

            pictureBox1.Load("https://m.media-amazon.com/images/I/81-SFiRVerL._UF1000,1000_QL80_.jpg");

            // Ensure that the sender is a DataGridView
            if (sender is DataGridView dataGridView)
            {
                int rowClicked = dataGridView.CurrentRow.Index;

                String imageURl = dataGridView1.Rows[rowClicked].Cells[4].Value.ToString();

                pictureBox1.Load(imageURl);

                tracksBindingSource.DataSource = albums[rowClicked].Tracks;

                dataGridView2.DataSource = tracksBindingSource;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            AlbumsDAO albumsDAO = new AlbumsDAO();

            albumBindingSource.DataSource = albumsDAO.searchTitles(textBox1.Text);

            dataGridView1.DataSource = albumBindingSource;

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            AlbumsDAO albumsDAO = new AlbumsDAO();

            int rowClicked = e.RowIndex;

            if (rowClicked >= 0 && rowClicked < dataGridView1.Rows.Count)
            {
                // Load album image
                String imageURl = dataGridView1.Rows[rowClicked].Cells[4].Value.ToString();
                pictureBox1.Load(imageURl);

                // Load tracks for the selected album
                int albumID = (int)dataGridView1.Rows[rowClicked].Cells[0].Value;
                tracksBindingSource.DataSource = albumsDAO.getTracksForAlbum(albumID);
                dataGridView2.DataSource = tracksBindingSource;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Album album = new Album
            {
                AlbumName = tbAlbumName.Text,
                ArtistName = tbArtist.Text,
                Year = Int32.Parse(tbYear.Text),
                ImageURL = tbImageURL.Text,
                Description = tbDescription.Text,
            };

            AlbumsDAO albums = new AlbumsDAO();
            int result = AlbumsDAO.addOneAlbum(album);
            MessageBox.Show(result + "new row(s) inserted");
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            int rowClicked = dataGridView.CurrentRow.Index;

            String videoURL = dataGridView.Rows[rowClicked].Cells[3].Value.ToString();

            webView21.Source = new Uri(videoURL);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int rowClicked = dataGridView2.CurrentRow.Index;
            MessageBox.Show("You clicked row " + rowClicked);
            int trackID = (int)dataGridView2.Rows[rowClicked].Cells[0].Value;
            MessageBox.Show("ID of track " + trackID);

            AlbumsDAO albumsDao = new AlbumsDAO();

            int result = albumsDao.deleteTrack(trackID);

            MessageBox.Show("Result " + result);

            dataGridView2.DataSource = null;
            albums = albumsDao.getAllAlbums();
        }
    }
}
