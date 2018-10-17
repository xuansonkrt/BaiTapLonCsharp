using DirectShowLib;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Formmain
{
    public partial class frmDiemDanh : Form
    {
        DsDevice[] multiCam;
        HaarCascade haar;
        Image<Bgr, Byte> ImageFrame;
        Capture capture;
        Image<Gray, Byte> objFace;
        public frmDiemDanh()
        {
            InitializeComponent();
            multiCam = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            int i = 1;
            string name;
            foreach(DsDevice cam in multiCam)
            {
                name = i + ":" + cam.Name;
                cbCamIndex.Items.Add(name);
                i++;
            }
            haar = new HaarCascade("haarcascade_frontalface_default.xml");
        }
        private void ProcessFrame(object sender, EventArgs arg)
        {
            ImageFrame = capture.QueryFrame();
            imgCamera.Image = ImageFrame;
            FaceRecognition();

        }

        private void FaceRecognition()
        {
            if(ImageFrame!=null)
            {
                Image<Gray, Byte> grayFrame = ImageFrame.Convert<Gray, Byte>();
                var faces = grayFrame.DetectHaarCascade(haar, 1.3, 4,
                                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                                new Size(25, 25))[0];


                foreach (var f in faces)
                {


                    imgTrain.Image = ImageFrame.Copy(f.rect).Convert<Bgr, Byte>().Resize(148,
                                                        161, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC); // hien thi 1 khuon mat len imgTrain => nhap thong tin
                    objFace = ImageFrame.Copy(f.rect).Convert<Gray, Byte>().Resize(148,
                                                       161, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    ImageFrame.Draw(f.rect, new Bgr(Color.Red), 3); // danh dau khuon mat phat hien
                    

                }
            }
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(capture!=null)
            {
                capture = null;
            }

            try
            {
                capture = new Capture(cbCamIndex.SelectedIndex);
                Application.Idle += ProcessFrame;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmDiemDanh_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (capture != null)
                capture.Dispose();
            ImageFrame = null;
        }
    }
}
