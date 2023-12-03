using DatabaseMongoMusicApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DatabaseMongoMusicApp;
using System.Diagnostics;

namespace DatabaseSQLMusicApp
{
    public partial class Form1 : Form
    {
        BindingSource albumBindingSource = new BindingSource();
        BindingSource tracksBindingSource = new BindingSource();

        List<Album> albums = new List<Album>();
        AlbumDataBaseService albumDataBaseService = new AlbumDataBaseService(); // Assuming you have an AlbumDataService class

        public Form1()
        {
            InitializeComponent();
        }

        private void updateAlbumGrid()
        {
            albumBindingSource.DataSource = albumDataBaseService.getAll();

            dataGridView1.DataSource = albumBindingSource;
        }

        private void updateTrackGrid()
        {
            dataGridView2.DataSource = tracksBindingSource;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updateAlbumGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String searchTerm = textBox1.Text;

            List<Album> searchResults = albumDataBaseService.search(searchTerm);

            dataGridView1.DataSource = albumBindingSource;
            albumBindingSource.DataSource = searchResults;

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            DataGridView dataGridView = (DataGridView)sender;

            // Row number refers to the position in the table
            int rowNumber = dataGridView.CurrentCell.RowIndex;

            // Item number is the id value of the Album object
            string itemNumber = dataGridView.Rows[rowNumber].Cells[0].Value.ToString();

            // Uncomment to test
            MessageBox.Show(rowNumber.ToString() + " " + itemNumber);

            updateTrackGridForAlbum(itemNumber);

            // Load picture
            try
            {
                pictureBox1.Load(dataGridView.Rows[rowNumber].Cells[4].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a new album using the contents of the text inputs
                Album a = new Album
                {
                    AlbumName = tbAlbumName.Text,
                    ArtistName = tbArtist.Text,
                    Year = Int32.Parse(tbYear.Text),
                    ImageURL = tbImageURL.Text,
                    Description = tbDescription.Text,
                    Tracks = new List<Track>() // Assuming Track is the type you want here
                };

                // Add to the database
                albumDataBaseService.addOne(a);

                // Refresh the grid display
                updateAlbumGrid();
            }
            catch (Exception ex)
            {
                // Display any errors when the previous code fails
                MessageBox.Show(ex.Message);
            }

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
          /*  int rowClicked = dataGridView2.CurrentRow.Index;
            MessageBox.Show("You clicked row " + rowClicked);
            int trackID = (int)dataGridView2.Rows[rowClicked].Cells[0].Value;
            MessageBox.Show("ID of track " + trackID);

            AlbumsDAO albumsDao = new AlbumsDAO();

            int result = albumsDao.deleteTrack(trackID);

            MessageBox.Show("Result " + result);

            dataGridView2.DataSource = null;
            albums = albumsDao.getAllAlbums();*/
        }

        private void updateTrackGridForAlbum(string itemNumber)
        {
            Album album = albumDataBaseService.getOne(itemNumber);

            tracksBindingSource.DataSource = album.Tracks;

            dataGridView2.DataSource = tracksBindingSource;

        }
    }
}
