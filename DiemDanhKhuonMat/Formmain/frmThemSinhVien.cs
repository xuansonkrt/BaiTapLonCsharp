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
    public partial class frmThemSinhVien : Form
    {
        DsDevice[] multiCam;
        HaarCascade haar;
        Capture capture; //camera input
        bool captureInProcess = false;
        Image<Gray, Byte> objFace;
        Image<Bgr, Byte> ImageFrame;
        int FaceNum;
        Image<Bgr, Byte>[] EXFace; // mang chua cac khuon mat phat hien duoc
        bool picProcess = false;
        public frmThemSinhVien()
        {
            InitializeComponent();
            try
            {
                haar = new HaarCascade("haarcascade_frontalface_default.xml");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            multiCam = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            int i = 1;
            string name;
            foreach (DsDevice cam in multiCam)
            {
                name = i + ": " + cam.Name;
                cbCamIndex.Items.Add(name);
                i++;
            }

        }

        public void ProcessFrame(object sender, EventArgs arg)
        {
            if (!picProcess)
            {
                ImageFrame = capture.QueryFrame();
                imgCamera.Image = ImageFrame;
                DetectFaces_cam();
            }

        }
        private void DetectFaces_pic()
        {
            picProcess = true;
            btnChup.Enabled = false;
            if (ImageFrame != null)
            {
                Image<Gray, Byte> grayFrame = ImageFrame.Convert<Gray, Byte>();
                var faces = grayFrame.DetectHaarCascade(haar, 1.3, 4,
                                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                                new Size(25, 25))[0];   // mang chua cac khuon mat phat hien duoc
                                                        /*
                                                            haar: trained data
                                                            1.1: Scale Increase Rate (1.1,1.2,1.3,1.4) càng nhỏ: phát hiện được nhiều khuôn mặt -> chậm hơn. 
                                                            4:  Minimum Neighbors Threshold 0->4  giá trị cao phát hiện chặt chẽ hơn
                                                            CANNY_PRUNING:  (0)
                                                            new Size(25, 25): size of the smallest face to search for. mặc định 25x25
                                                         */


                if (faces.Length > 0)
                {
                    MessageBox.Show("Số khuôn mặt được phát hiện: " + faces.Length);


                    EXFace = new Image<Bgr, Byte>[faces.Length];
                    int i = 0;
                    foreach (var face in faces)
                    {
                        EXFace[i] = ImageFrame.Copy(face.rect).Convert<Bgr, Byte>().Resize(148,
                               161, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                        i++;
                        ImageFrame.Draw(face.rect, new Bgr(Color.Blue), 3); // danh dau khuon mat phat hien
                    }
                    imgCamera.Image = ImageFrame;

                    imgTrain.Image = EXFace[FaceNum];
                    objFace = EXFace[FaceNum].Convert<Gray, Byte>();
                    // FaceNum++;
                    if ((faces.Length > 1))
                    {
                        btnNext.Enabled = true;
                        btnPrev.Enabled = false;
                    }
                    else
                    {
                        btnNext.Enabled = false;
                        btnPrev.Enabled = false;
                    }

                }
                else
                {
                    MessageBox.Show("Không có khuôn mặt!");
                }

                /*   ImageFrame.Draw(faces[0].rect, new Bgr(Color.Red), 3);
                   imgTrain.Image = ImageFrame.Copy(faces[0].rect).Convert<Bgr, Byte>().Resize(148,
                                                                 161, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);*/

                txtMaSV.Focus();
            }
        }
        private void DetectFaces_cam()
        {
            btnNext.Enabled = false;
            btnPrev.Enabled = false;
            if (ImageFrame != null)
            {
                Image<Gray, Byte> grayFrame = ImageFrame.Convert<Gray, Byte>();
                var faces = grayFrame.DetectHaarCascade(haar, 1.3, 4,
                                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                                new Size(25, 25))[0];   // mang chua cac khuon mat phat hien duoc
                                                        /*
                                                            haar: trained data
                                                            1.1: Scale Increase Rate (1.1,1.2,1.3,1.4) càng nhỏ: phát hiện được nhiều khuôn mặt -> chậm hơn. 
                                                            4:  Minimum Neighbors Threshold 0->4  giá trị cao phát hiện chặt chẽ hơn
                                                            CANNY_PRUNING:  (0)
                                                            new Size(25, 25): size of the smallest face to search for. mặc định 25x25
                                                         */


                if (faces.Length != 0)
                {

                    btnChup.Enabled = true;
                    if (captureInProcess == true)
                        btnChup.Enabled = false;
                    //  if(face.rect!=null)
                    if ((captureInProcess == false))
                    {
                        imgTrain.Image = ImageFrame.Copy(faces[0].rect).Convert<Bgr, Byte>().Resize(148,
                                                        161, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC); // hien thi 1 khuon mat len imgTrain => nhap thong tin
                        objFace = ImageFrame.Copy(faces[0].rect).Convert<Gray, Byte>().Resize(148,
                                                                161, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    }
                    ImageFrame.Draw(faces[0].rect, new Bgr(Color.Red), 3); // danh dau khuon mat phat hien

                    //nhap(objFace);
                }
                else
                {
                    if (captureInProcess == false)
                    {
                        imgTrain.Image = null;
                        btnChup.Enabled = false;
                    }
                }
            }


        }
        private void frmThemSinhVien_Load(object sender, EventArgs e)
        {

        }

        private void btnBatDau_Click(object sender, EventArgs e)
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
            catch( Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnChup_Click(object sender, EventArgs e)
        {
            captureInProcess = true;
            txtMaSV.Focus();
            btnChup.Enabled = false;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            captureInProcess = false;
            txtMaSV.Text = "";
            txtHoTen.Text = "";
            txtLop.Text = "";
            txtNgaySinh.Text = "";
            radNam.Checked = false;
            radNu.Checked = false;
            btnChup.Enabled = true;
            imgTrain = null;
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            btnNext.Enabled = true;
            if (FaceNum > 0)
            {
                FaceNum--;
                imgTrain.Image = EXFace[FaceNum];
            }
            else
            {
                btnPrev.Enabled = false;
            }
            if (FaceNum == 0)
                btnPrev.Enabled = false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            btnPrev.Enabled = true;
            if (FaceNum < EXFace.Length - 1)
            {
                FaceNum++;
                imgTrain.Image = EXFace[FaceNum];
            }
            if (FaceNum == EXFace.Length - 1)
            {
                btnNext.Enabled = false;
            }
        }

        private void btnMoFile_Click(object sender, EventArgs e)
        {
            ofdMoFile.ShowDialog();
            string file = ofdMoFile.FileName;
            try
            {
                Image imgInput = Image.FromFile(ofdMoFile.FileName);
                ImageFrame = new Image<Bgr, byte>(new Bitmap(imgInput));
                imgCamera.Image = ImageFrame;
                DetectFaces_pic();
            }

            catch (Exception ex)
            {
                // to do some thing
            }
        }

        private void frmThemSinhVien_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void frmThemSinhVien_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (capture != null)
                capture.Dispose();
            ImageFrame = null;
        }
    }
}
